﻿/* 
	This script creates all of the tables for the database.
*/

/* Classes Table */
CREATE TABLE [dbo].[Classes]
(
	[ClassID] INT IDENTITY(1,1) NOT NULL,
	[CRN] INT,
	[DeptPrefix] NVARCHAR(10) NOT NULL,
	[ClassNum] NVARCHAR(4) NOT NULL,
	[Instructor] NVARCHAR(MAX) NOT NULL,
	[Days] NVARCHAR(10),
	[Time] NVARCHAR(25),

	CONSTRAINT [PK_dbo.Classes] PRIMARY KEY (ClassID)
);

/* Students Table */
CREATE TABLE [dbo].[Students]
(
	[VNum] NVARCHAR(8) NOT NULL,
	[FirstName] NVARCHAR(MAX) NOT NULL,
	[LastName] NVARCHAR(MAX) NOT NULL,

	CONSTRAINT [PK_dbo.Students] PRIMARY KEY (VNum),
);

/* StudentClass Table */
/* Note: this is the table that will allow students to sign in for more than one class. */
CREATE TABLE [dbo].[StudentClasses]
(
	[ID] INT IDENTITY(1,1) NOT NULL,
	[VNum] NVARCHAR(8) NOT NULL,
	[ClassId] INT NOT NULL,

	CONSTRAINT [PK_dbo.StudentClasses] PRIMARY KEY (ID),
	CONSTRAINT [FK_dbo.StudentClasses] FOREIGN KEY (VNum) REFERENCES [dbo].[Students] (VNum),
	CONSTRAINT [FK_dbo.StudentClasses1] FOREIGN KEY (ClassID) REFERENCES [dbo].[Classes] (ClassID)
);

/* SignIn Table (For Logging Information) */
CREATE TABLE [dbo].[SignIns]
(
	[ID] INT IDENTITY (1,1) NOT NULL,
	[Week] INT NOT NULL,
	[Date] DATE NOT NULL,
	[Hour] INT NOT NULL,
	[Min] INT NOT NULL,
	[StudentID] NVARCHAR(8) NOT NULL,
	[ClassId] INT NOT NULL

	CONSTRAINT [PK_dbo.SignIns] PRIMARY KEY CLUSTERED (ID ASC),
	CONSTRAINT [FK_dbo.SignIns1] FOREIGN KEY (StudentID) REFERENCES [dbo].[Students] (VNum),
	CONSTRAINT [FK_dbo.SignIns2] FOREIGN KEY (ClassID) REFERENCES [dbo].[Classes] (ClassID)
);