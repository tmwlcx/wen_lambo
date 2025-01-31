Here is the specification for the Attorney Scheduling Optimization Program in plain text:

---

## Specification for Attorney Scheduling Optimization Program

### Overview
The Attorney Scheduling Optimization Program is designed to fairly and efficiently schedule attorneys into courtrooms, considering various constraints such as attorney unavailability, working days, holidays, and the need for junior attorneys to be paired with full attorneys. The program aims to level-load the assignments among non-junior attorneys, ensuring an even distribution of courtroom assignments.

### Inputs
1. **Attorney List**:
   - A list of attorney names or indices.

```python
attorneys = ["Attorney_1", "Attorney_2", "Attorney_3", ..., "Attorney_10"]
```

2. **Junior Attorneys List**:
   - A list of indices or names of junior attorneys.

```python
junior_attorneys_list = ["Attorney_3", "Attorney_6", "Attorney_8"]
```

3. **Unavailability Data**:
   - A dictionary indicating the unavailability of each attorney on specific days. The keys are (attorney index, day index) tuples, and the values are 1 if the attorney is unavailable on that day, and 0 otherwise.

```python
unavailability_data = {
   (0, 1): 1,
   (1, 5): 1,
   (2, 10): 1,
   # ... more unavailability data
}
```

4. **Month and Year**:
   - The month and year for which the schedule is to be created.

```python
year = 2025
month = 1
```

### Outputs
The program will generate a schedule indicating which attorney is assigned to each courtroom slot on each working day. The schedule will be printed in a readable format.

### Constraints
1. **Slot Filling**: Each slot in each courtroom on each day must be filled by exactly one attorney.
2. **Weekly Limit**: No attorney can be scheduled for more than 3 days in any given week.
3. **Consecutive Days**: No attorney can be scheduled for 3 consecutive days within the same week.
4. **Unavailability**: Attorneys must not be scheduled on days when they are unavailable or on holidays.
5. **Junior Attorney Pairing**: Junior attorneys must be paired with full attorneys in the same courtroom on the same day.
6. **Level-Loading**: The number of assignments for non-junior attorneys should be evenly distributed.

### Approach
1. **Define Sets and Parameters**:
   - Sets for attorneys, days, courtrooms, and slots.
   - Parameters for attorney unavailability and identifying junior attorneys.

2. **Decision Variables**:
   - `x[i, j, k, d]`: A binary variable indicating if attorney `i` is scheduled in courtroom `j` at slot `k` on day `d`.
   - `num_assignments[i]`: An integer variable indicating the number of assignments for attorney `i`.

3. **Constraints**:
   - Ensure each slot is filled.
   - Limit the number of assignments per week.
   - Prevent scheduling for 3 consecutive days.
   - Respect unavailability and holiday constraints.
   - Ensure junior attorneys are paired with full attorneys.

4. **Objective**:
   - Minimize the maximum number of assignments for non-junior attorneys to achieve level-loading.

### Implementation
The implementation involves using the Pyomo library for defining the optimization model, constraints, and objective function. The GLPK solver is used to solve the optimization problem. The results are then displayed in a readable format.

### Example Code

```python
from pyomo.environ import *
from datetime import datetime, timedelta
import holidays

# Function to get the working days and holidays for a given month and year
def get_working_days_and_holidays(year, month):
    us_holidays = holidays.US(years=year)
    days = []
    holiday_indices = []
    
    current_date = datetime(year, month, 1)
    while current_date.month == month:
        if current_date.weekday() < 5:  # Monday to Friday are considered working days
            if current_date in us_holidays:
                holiday_indices.append(len(days))
            days.append(current_date)
        current_date += timedelta(days=1)
    return days, holiday_indices

# Example usage
year = 2025
month = 1
working_days, holiday_indices = get_working_days_and_holidays(year, month)

# Initialize the model
model = ConcreteModel()

# Sets
courtrooms = range(2)
slots = range(2)
attorneys = range(10)  # Assuming 10 attorneys

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
    i: 1 if i in junior_attorneys_list else 0
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

#