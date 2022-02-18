CREATE DATABASE QUIZ_DB

USE QUIZ_DB

CREATE TABLE TBL_ADMIN
(
AD_ID INT PRIMARY KEY IDENTITY,
AD_NAME NVARCHAR(20) NOT NULL,
AD_PASSWORD NVARCHAR(20) NOT NULL
)

INSERT INTO TBL_ADMIN VALUES ('Admin','admin')
INSERT INTO TBL_ADMIN VALUES ('Root','admin')

CREATE TABLE TBL_CATEGORY
(
CAT_ID INT PRIMARY KEY IDENTITY,
CAT_NAME NVARCHAR(50) NOT NULL,
CAT_FK_ADID INT FOREIGN KEY REFERENCES TBL_ADMIN(AD_ID),
CAT_ENCRYPTEDSTRING NVARCHAR(MAX) NOT NULL
)
drop table TBL_CATEGORY
drop table TBL_QUESTION

CREATE TABLE TBL_QUESTION
(
QUESTION_ID INT PRIMARY KEY IDENTITY,
QUESTION_TXT NVARCHAR(MAX) NOT NULL,
OPA NVARCHAR(MAX) NOT NULL,
OPB NVARCHAR(MAX) NOT NULL,
OPC NVARCHAR(MAX) NOT NULL,
OPD NVARCHAR(MAX) NOT NULL,
COP NVARCHAR(10) NOT NULL,
QUESTION_FK_CAT INT FOREIGN KEY REFERENCES TBL_CATEGORY(CAT_ID)
)

CREATE TABLE TBL_STUDENT(
STD_ID INT PRIMARY KEY IDENTITY,
STD_NAME NVARCHAR(50) NOT NULL,
STD_PASSWORD NVARCHAR(50) NOT NULL,
STD_IMAGE NVARCHAR(MAX) NOT NULL,
)

CREATE TABLE TBL_SETEXAM
(
EXAM_ID INT PRIMARY KEY IDENTITY,
EXAM_DATE DATETIME NOT NULL,
EXAM_FK_STD INT FOREIGN KEY REFERENCES TBL_STUDENT(STD_ID),
EXAM_NAME NVARCHAR(50) NOT NULL,
EXAM_STD_SCORE INT NOT NULL
)

SELECT * FROM TBL_ADMIN
SELECT * FROM TBL_QUESTION
SELECT * FROM TBL_CATEGORY
SELECT * FROM TBL_STUDENT
SELECT * FROM TBL_SETEXAM