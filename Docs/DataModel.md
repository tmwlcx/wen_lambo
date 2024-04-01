# Data Model
*Feel free to make edits and suggestions, I'm sure this will evolve as we go along.*
</br>

| **Attorney**        |                      |
|---------------------|----------------------|
| AttorneyId          | [PK]                 |
| Name                |                      |
| AttorneyTypeId      | [FK] References AttorneyType.AttorneyTypeId |
| UserId              | [FK] References User.UserId --nullable |
| IsDeleted           |                      |
| CreatedDate         |                      |
| CreatedByUserId     | [FK] References User.UserId |
| UpdatedDate         |                      |
| UpdatedByUserId     | [FK] References User.UserId |
</br>

| **AttorneyType**    |                      |
|---------------------|----------------------|
| AttorneyTypeId      | [PK]                 |
| TypeName            |                      |
| IsDeleted           |                      |
</br>

| **AttorneyTimeOff**  |                      |
|----------------------|----------------------|
| AttorneyTimeOffId    | [PK]                 |
| AttorneyId           | [FK] References Attorney.AttorneyId |
| TimeOffDateFrom      |                      |
| TimeOffDateTo        |                      |
| IsDeleted            |                      |
| CreatedDate          |                      |
| CreatedByUserId      | [FK] References User.UserId |
| UpdatedDate          |                      |
| UpdatedByUserId      | [FK] References User.UserId |
</br>

| **CourtRoom**       |                      |
|---------------------|----------------------|
| CourtRoomId         | [PK]                 |
| CourtRoomName       |                      |
| IsDeleted           |                      |
</br>

| **Schedule**        |                      |
|---------------------|----------------------|
| ScheduleId          | [PK]                 |
| ScheduleDateFrom    |                      |
| ScheduleDateTo      |                      |
| IsFinalized         |                      |
| IsDeleted           |                      |
| CreatedDate         |                      |
| CreatedByUserId     | [FK] References User.UserId |
| UpdatedDate         |                      |
| UpdatedByUserId     | [FK] References User.UserId |
</br>

| **ScheduleAssignment** |                      |
|------------------------|----------------------|
| ScheduleAssignmentId   | [PK]                 |
| ScheduleId             | [FK] References Schedule.ScheduleId |
| ScheduleDate           |                      |
| AttorneyId             | [FK] References Attorney.AttorneyId |
| CourtRoomId            | [FK] References CourtRoom.CourtRoomId |
| IsDeleted              |                      |
| CreatedDate            |                      |
| CreatedByUserId        | [FK] References User.UserId |
| UpdatedDate            |                      |
| UpdatedByUserId        | [FK] References User.UserId |
</br>

| **User**            |                      |
|---------------------|----------------------|
| UserId              | [PK]                 |
| Username            |                      |
| Password            |                      |
| IsDeleted           |                      |
| CreatedDate         |                      |
| UpdatedDate         |                      |

</br></br>
<img src="https://i.imgflip.com/8l5211.jpg"/>