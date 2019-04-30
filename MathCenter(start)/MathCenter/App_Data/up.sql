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
	[DeptPrefix] NVARCHAR(10),
	[ClassNum] INT,
	[Instructor] NVARCHAR(MAX),
	[Days] NVARCHAR(10),
	[Time] NVARCHAR(25),
	[Other] NVARCHAR(MAX),

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
	[VNum] NVARCHAR(8) NOT NULL,
	[ClassID] INT NOT NULL,

	CONSTRAINT [FK_dbo.StudentClasses] FOREIGN KEY (VNum) REFERENCES [dbo].[Students] (VNum),
	CONSTRAINT [FK_dbo.StudentClasses1] FOREIGN KEY (ClassID) REFERENCES [dbo].[Classes] (ClassID),
	UNIQUE(VNum, ClassID)
);

/* SignIn Table (For Logging Information) */
CREATE TABLE [dbo].[SignIns]
(
	[ID] INT IDENTITY (1,1) NOT NULL,
	[Week] INT NOT NULL,
	[Date] DATE NOT NULL,
	[Hour] INT NOT NULL,
	[Min] INT NOT NUll,
	[StudentID] NVARCHAR(8) NOT NULL,
	[ClassID] INT NOT NULL,

	CONSTRAINT [PK_dbo.SignIns] PRIMARY KEY CLUSTERED (ID ASC),
	CONSTRAINT [FK_dbo.SignIns1] FOREIGN KEY (StudentID) REFERENCES [dbo].[Students] (VNum),
	CONSTRAINT [FK_dbo.SignIns2] FOREIGN KEY (ClassID) REFERENCES [dbo].[Classes] (ClassID)
);