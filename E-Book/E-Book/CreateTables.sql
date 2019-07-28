﻿CREATE TABLE Workers
(
	[ID] INT PRIMARY KEY IDENTITY, 
	[Username] VARCHAR(20) UNIQUE NOT NULL,
	[Hash] VARCHAR(MAX) NOT NULL,
	[Hashkey] VARCHAR(MAX) NOT NULL,
);

CREATE TABLE Books
(
	[ID] INT PRIMARY KEY IDENTITY,
	[BookName] VARCHAR(100) NOT NULL,
	[Authors] VARCHAR(255) NOT NULL,
	[Year]	SMALLINT,
	[Category] VARCHAR(50) NOT NULL,
	[BookCopies] SMALLINT NOT NULL
);

CREATE TABLE BookCopies
(
	[ID] INT PRIMARY KEY IDENTITY,
	[BookID] INT NOT NULL,
	[BookCode] VARCHAR(10) UNIQUE NOT NULL,
	[Notes] VARCHAR(100)
);

CREATE TABLE BorrowedBooks
(
	[ID] INT PRIMARY KEY IDENTITY,
	[LibID] INT NOT NULL,
	[CopyID] INT NOT NULL,
	[ExpDate] DATE
);

CREATE TABLE Members
(
	[ID] INT PRIMARY KEY IDENTITY,
	[FullName] VARCHAR(50),
	[LibID] VARCHAR(10) UNIQUE,
	[ExpData] DATE,
	[PlaceOfBirth] VARCHAR(20),
	[DateOfBirth] DATE,
	[Address] VARCHAR(100),
	[Email] VARCHAR(100),
	[MobilePhone] VARCHAR(20)
);

ALTER TABLE BookCopies
ADD FOREIGN KEY (BookID) REFERENCES Books(ID);

ALTER TABLE BorrowedBooks
ADD FOREIGN KEY (LibID) REFERENCES Members(Id),
FOREIGN KEY (CopyID) REFERENCES BookCopies(Id);