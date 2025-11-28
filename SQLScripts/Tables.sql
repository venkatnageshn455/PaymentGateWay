-- Create DB (if needed)
-- CREATE DATABASE Nagesh;
-- GO
-- USE Nagesh;
-- GO

CREATE TABLE DailySequences (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId NVARCHAR(200) NOT NULL,
    Date DATE NOT NULL,
    LastSeq INT NOT NULL,
    CONSTRAINT UQ_DailySequence_User_Date UNIQUE (UserId, Date)
);

CREATE TABLE Payments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId NVARCHAR(200) NOT NULL,
    Reference NVARCHAR(100) NULL,
    ClientRequestId NVARCHAR(200) NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Currency NVARCHAR(10) NOT NULL,
    CreatedAt DATETIME2 NOT NULL,
    CONSTRAINT UQ_Payments_User_ClientRequest UNIQUE (UserId, ClientRequestId)
);

CREATE INDEX IX_Payments_Reference ON Payments(Reference);

CREATE TABLE PaymentLogs (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId NVARCHAR(200) NOT NULL,
    Action NVARCHAR(100) NOT NULL,
    RequestJson NVARCHAR(MAX) NULL,
    ResponseJson NVARCHAR(MAX) NULL,
    IsSuccess BIT NOT NULL,
    ErrorMessage NVARCHAR(MAX) NULL,
    CreatedAt DATETIME2 NOT NULL
);

