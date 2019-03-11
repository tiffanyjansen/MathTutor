/* 
	This script creates all of the tables for the database.
	For testing purposes I have added some seed data at the end. 
	I will comment it out later when I have it working right.
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
	[StartTime] NVARCHAR(25),

	CONSTRAINT [PK_dbo.Classes] PRIMARY KEY (ClassID)
);

/* Students Table */
CREATE TABLE [dbo].[Students]
(
	[VNum] NVARCHAR(8) NOT NULL,
	[FirstName] NVARCHAR(MAX) NOT NULL,
	[LastName] NVARCHAR(MAX) NOT NULL,
	[Class] INT NOT NULL,

	CONSTRAINT [PK_dbo.Students] PRIMARY KEY (VNum),
	CONSTRAINT [FK_dbo.Students] FOREIGN KEY (Class) REFERENCES [dbo].[Classes] (ClassID)
);

/* SignIn Table (For Logging Information) */
CREATE TABLE [dbo].[SignIns]
(
	[ID] INT IDENTITY (1,1) NOT NULL,
	[Week] INT NOT NULL,
	[Date] DATE NOT NULL,
	[Hour] INT NOT NULL,
	[Min] INT NOT NUll,
	[Sec] INT NOT NULL,
	[StudentID] NVARCHAR(8) NOT Null,

	CONSTRAINT [PK_dbo.SignIns] PRIMARY KEY CLUSTERED (ID ASC),
	CONSTRAINT [FK_dbo.SignIns] FOREIGN KEY (StudentID) REFERENCES [dbo].[Students] (VNum)
);