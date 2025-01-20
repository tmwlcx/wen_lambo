CREATE TABLE Attorney (
    AttorneyId INTEGER PRIMARY KEY,
    AttorneyName TEXT,
    AttorneyTypeId INTEGER,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (AttorneyTypeId) REFERENCES AttorneyType(AttorneyTypeId),
    UNIQUE(AttorneyName,AttorneyTypeId)
);

CREATE TABLE AttorneyType (
    AttorneyTypeId INTEGER PRIMARY KEY,
    TypeName TEXT UNIQUE,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO AttorneyType(TypeName)
VALUES('Manager'), ('Attorney'), ('Junior Attorney');

CREATE TABLE AttorneyTimeOff (
    AttorneyTimeOffId INTEGER PRIMARY KEY,
    AttorneyId INTEGER,
    TimeOffDateFrom DATETIME,
    TimeOffDateTo DATETIME,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (AttorneyId) REFERENCES Attorney(AttorneyId)
);

CREATE TABLE CourtRoom (
    CourtRoomId INTEGER PRIMARY KEY,
    CourtRoomName TEXT UNIQUE,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO CourtRoom(CourtRoomName)
VALUES('CourtRoom 1'), ('CourtRoom 2 Judicial Boogaloo');

CREATE TABLE Schedule (
    ScheduleId INTEGER PRIMARY KEY,
    ScheduleYear INTEGER,
    ScheduleMonth INTEGER,
    NumSlots INTEGER,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE ScheduleAssignment (
    ScheduleAssignmentId INTEGER PRIMARY KEY,
    ScheduleId INTEGER,
    ScheduleDate DATETIME,
    AttorneyId INTEGER,
    CourtRoomId INTEGER,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ScheduleId) REFERENCES Schedule(ScheduleId),
    FOREIGN KEY (AttorneyId) REFERENCES Attorney(AttorneyId),
    FOREIGN KEY (CourtRoomId) REFERENCES CourtRoom(CourtRoomId)
);
