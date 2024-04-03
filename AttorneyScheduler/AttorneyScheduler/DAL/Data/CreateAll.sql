CREATE TABLE Attorney (
    AttorneyId INTEGER PRIMARY KEY,
    AttorneyName TEXT,
    AttorneyTypeId INTEGER,
    IsDeleted INTEGER DEFAULT 0,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (AttorneyTypeId) REFERENCES AttorneyType(AttorneyTypeId)
);

CREATE TABLE AttorneyType (
    AttorneyTypeId INTEGER PRIMARY KEY,
    TypeName TEXT,
    IsDeleted INTEGER DEFAULT 0,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE AttorneyTimeOff (
    AttorneyTimeOffId INTEGER PRIMARY KEY,
    AttorneyId INTEGER,
    TimeOffDateFrom DATETIME,
    TimeOffDateTo DATETIME,
    IsDeleted INTEGER DEFAULT 0,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (AttorneyId) REFERENCES Attorney(AttorneyId)
);

CREATE TABLE CourtRoom (
    CourtRoomId INTEGER PRIMARY KEY,
    CourtRoomName TEXT,
    IsDeleted INTEGER DEFAULT 0,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE Schedule (
    ScheduleId INTEGER PRIMARY KEY,
    ScheduleDateFrom DATETIME,
    ScheduleDateTo DATETIME,
    IsDeleted INTEGER DEFAULT 0,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE ScheduleAssignment (
    ScheduleAssignmentId INTEGER PRIMARY KEY,
    ScheduleId INTEGER,
    ScheduleDate DATETIME,
    AttorneyId INTEGER,
    CourtRoomId INTEGER,
    IsDeleted INTEGER DEFAULT 0,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ScheduleId) REFERENCES Schedule(ScheduleId),
    FOREIGN KEY (AttorneyId) REFERENCES Attorney(AttorneyId),
    FOREIGN KEY (CourtRoomId) REFERENCES CourtRoom(CourtRoomId)
);
