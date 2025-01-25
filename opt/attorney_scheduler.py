from pyomo.environ import *
from datetime import datetime, timedelta
import holidays
import sys
import json

# Function to get the working days and holidays for a given month and year
def get_working_days_and_holidays(year, month):
    us_holidays = holidays.US(years=year)
    days = []
    holiday_indices = []
    
    current_date = datetime(year, month, 1)
    while current_date.month == month:
        if (current_date.weekday() < 5) & (current_date not in us_holidays):  # Monday to Friday are considered working days
            if current_date in us_holidays:
                holiday_indices.append(len(days))
            days.append(current_date)
        current_date += timedelta(days=1)
    return days, holiday_indices

unavailability_data = {
    # structure for tuple list below:
    # (attorney_number, unavailability_day_of_month): 1=unavailable
    (0, 1): 1,
    (1, 5): 1,
    (2, 10): 1,
    # ... more unavailability data
}


# Example usage
year = int(sys.argv[1])
month = int(sys.argv[2])
slots = range(int(sys.argv[3]))
attorneys = list(map(int, sys.argv[4].split(',')))
junior_attorneys = list(map(int, sys.argv[5].split(',')))

# JSON response
schedule = {
    "Schedule": {
        "ScheduleYear": year,
        "ScheduleMonth": month,
        "NumSlots": len(slots),
        "ScheduleAssignments": []
    }
}

working_days, holiday_indices = get_working_days_and_holidays(year, month)

# Initialize the model
model = ConcreteModel()

# Sets
courtrooms = range(3) # we should make this a input param too once it's setup in BE

model.DAYS = Set(initialize=range(len(working_days)))
model.COURTROOMS = Set(initialize=courtrooms)
model.SLOTS = Set(initialize=slots)
model.ATTORNEYS = Set(initialize=attorneys)

# Parameters
unavailability = {
    (i, d): 1 if (i, d) in unavailability_data else 0
    for i in attorneys for d in range(len(working_days))
}
is_junior = {
    i: 1 if i in junior_attorneys else 0
    for i in attorneys
}

model.unavailability = Param(model.ATTORNEYS, model.DAYS, initialize=unavailability, default=0)
model.is_junior = Param(model.ATTORNEYS, initialize=is_junior, default=0)

# Decision Variables
model.x = Var(model.ATTORNEYS, model.COURTROOMS, model.SLOTS, model.DAYS, within=Binary)
model.num_assignments = Var(model.ATTORNEYS, within=NonNegativeIntegers)

# Constraints
# 1. Each slot in each courtroom on each day must be filled by exactly one attorney
def slot_filled_rule(model, j, k, d):
    return sum(model.x[i, j, k, d] for i in model.ATTORNEYS) == 1
model.slot_filled = Constraint(model.COURTROOMS, model.SLOTS, model.DAYS, rule=slot_filled_rule)

# 2. No attorney can be scheduled more than 3 days in a week (5 working days in a week)
def max_days_per_week_rule(model, i, week):
    # the below return statement is equivalent to:
    # total = 0
    # for j in model.COURTROOMS:
    #     for k in model.SLOTS:
    #         for d in range(week * 5, (week + 1) * 5):
    #             if d in model.DAYS:
    #                 total += model.x[i, j, k, d]
    # return total <= 3
    return sum(model.x[i, j, k, d] for j in model.COURTROOMS for k in model.SLOTS for d in range(week * 5, (week + 1) * 5) if d in model.DAYS) <= 3
model.max_days_per_week = Constraint(model.ATTORNEYS, range((len(working_days) + 4) // 5), rule=max_days_per_week_rule)

# 3. No attorney can have 3 days back-to-back in the same week
def no_three_days_back_to_back_rule(model, i, d):
    if d <= 1 or d >= len(working_days) - 1:
        return Constraint.Skip
    return sum(model.x[i, j, k, d-2] for j in model.COURTROOMS for k in model.SLOTS) + \
           sum(model.x[i, j, k, d-1] for j in model.COURTROOMS for k in model.SLOTS) + \
           sum(model.x[i, j, k, d] for j in model.COURTROOMS for k in model.SLOTS) <= 2
model.no_three_days_back_to_back = Constraint(model.ATTORNEYS, model.DAYS, rule=no_three_days_back_to_back_rule)

# 4. Respect attorney unavailability and holidays
def respect_unavailability_rule(model, i, j, k, d):
    return model.x[i, j, k, d] <= 1 - model.unavailability[i, d]
model.respect_unavailability = Constraint(model.ATTORNEYS, model.COURTROOMS, model.SLOTS, model.DAYS, rule=respect_unavailability_rule)

# 5. Junior attorney must be paired with a full attorney
def junior_full_attorney_pair_rule(model, j, d):
    return sum(model.x[i, j, k, d] for k in model.SLOTS for i in model.ATTORNEYS if model.is_junior[i]) <= \
           sum(model.x[i, j, k, d] for k in model.SLOTS for i in model.ATTORNEYS if not model.is_junior[i])
model.junior_full_attorney_pair = Constraint(model.COURTROOMS, model.DAYS, rule=junior_full_attorney_pair_rule)

# 6. Calculate the number of assignments for each attorney
def num_assignments_rule(model, i):
    return model.num_assignments[i] == sum(model.x[i, j, k, d] for j in model.COURTROOMS for k in model.SLOTS for d in model.DAYS)
model.num_assignments_constraint = Constraint(model.ATTORNEYS, rule=num_assignments_rule)

# Objective: Level-load the non-junior attorneys evenly
full_attorneys = [i for i in attorneys if is_junior[i] == 0]
model.max_assignments = Var(within=NonNegativeIntegers)

def level_load_objective_rule(model):
    return model.max_assignments
model.objective = Objective(rule=level_load_objective_rule, sense=minimize)

# Additional constraint to level-load the non-junior attorneys
def level_load_constraint_rule(model, i):
    if is_junior[i] == 1:
        return Constraint.Skip
    return model.num_assignments[i] <= model.max_assignments
model.level_load_constraint = Constraint(full_attorneys, rule=level_load_constraint_rule)

# Solving the model
solver = SolverFactory('glpk')
solver.solve(model)

#Debugging
# print("Attorneys:", attorneys)
# print("Junior Attorneys:", junior_attorneys)
# print("Slots:", slots)
# print("Working Days:", working_days)

# Displaying the results
has_results = False
for i in model.ATTORNEYS:
    for d in model.DAYS:
        for j in model.COURTROOMS:
            for k in model.SLOTS:
                if model.x[i, j, k, d].value == 1:
                    has_results = True
                    schedule["Schedule"]["ScheduleAssignments"].append({
                        "ScheduleDate": str(working_days[d]),
                        "AttorneyId": i,
                        "CourtRoomId": j
                    })

print(json.dumps(schedule, indent=2))

# the strain of the constraints were more than the inputs could bear
if not has_results:
    print(json.dumps({"error": "No schedules were generated."}, indent=2))