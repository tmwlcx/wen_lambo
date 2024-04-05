# Attorney Scheduling App Requirements Document

## Background
A user had the following request for an application to schedule attorneys in courtrooms:

>You have a list of attorneys and a calendar with 2 courtrooms, each with 2 attorneys slots per courtroom. 5 days a week. You have attorney unavailability for the entire month. No attorney can be in court more than 3 days a week. No attorney can have 3 days back to back. Can you write a program that schedules attorneys fairly into the courtroom assignments while considering their unavailability and restrictions on days in court.

>![](./img/atty_calendar.png)

***

## Requirements:
1. In version v1, this will be a single user app. 
1. It needs to be self contained and must be able to be run in a straightforward way by the user (i.e., minimize creation of support files, ballooning files, etc.).
1. When a user opens the application for the first time, they should be able to enter information for the judges and attorneys.     
    >**Julpa**: Do judges have a schedule/unavailability similar to attorneys? Is there blackout dates that we need to consider either for the judge or courtroom (holidays, etc.)?
1. The user should be able to add unavailability (i.e., blackout dates) for the following:
    * each attorney
    * each judge (??)
    * each courtroom (??)
1. Some attorneys are in-training and need to be identified as such. They MUST not be assigned to be in court alone. They must also not be assigned in court with other attorneys of the same class.  
1. No attorney should be in court more than 3 days a week AND no attorney should be scheduled more than 2 days back-to-back, e.g.:
    * Attorney A **can** have the following weekly schedules: [M, T, Th], [Th, F], [M, W, F]
    * Attorney A **cannot** have the following weekly schedules: [M, T, W], [M, T, Th, F]
1. Attorneys (and judges(?) and coutrooms(?)) should not be scheduled during their periods of unavailability. 
1. Once all of the information for the attorneys and judges is input to the system, the user selects the month they want to populate information for and clicks a button to calculate the solution for the month. 
1. The user should be able to re-run the solution. The user should also be able to manually edit the calendar, if desired.  
1. The data input should persist between sessions and should be visible the next time the user runs the application. 
1. The end result of the program is a calendar like the one shown above that shows 1-month. Listed in the days of the calendar should be the following information: 
    * the judge (courtroom) 
    >**Julpa**: do the courtroom/courtroom numbers change or is a judge assigned to one courtroom and it never changes?
    * The attorneys appearing in front of that judge. 
1. The UI should be clean and orderly, links to different views must be descriptive so that a user can intuitively interact with the application. 
1. The user should be able to easily export the calendar for viewing as an image or printing.
1. There should be a help file somewhere that instructs the user on how to use the application. 


## Future Requirements/Roadmap Items
It is somewhat possible that future requirements or feature requests may be added. These should not impact the delivery of v1 but developers should be aware of them in order to plan to scale the architecture in the following ways include:
  * The ability for more than one user to interact with the application.
  * The ability for more than one level of user (e.g., managing attorney, regular attorney, attorney in training).
  * The ability to host the application on a corporate server. 
  * The ability to host the application elsewhere?
  * Other ways of interacting with the application like an app on a phone?

