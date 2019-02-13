/* 
	This script creates all of the tables for the database.
	For testing purposes I have added some seed data at the end. 
	I will comment it out later when I have it working right.
*/

/* Classes Table */
CREATE TABLE [dbo].[Classes]
(
	[CRN] INT NOT NULL,
	[DeptPrefix] NVARCHAR(10) NOT NULL,
	[ClassNum] INT NOT NULL,
	[Instructor] NVARCHAR(MAX) NOT NULL,
	[Days] NVARCHAR(10) NOT NULL,
	[StartTime] INT,
	[Other] NVARCHAR(MAX),

	CONSTRAINT [PK_dbo.Classes] PRIMARY KEY (CRN)
);

/* Students Table */
CREATE TABLE [dbo].[Students]
(
	[VNum] INT NOT NULL,
	[FirstName] NVARCHAR(MAX) NOT NULL,
	[LastName] NVARCHAR(MAX) NOT NULL,
	[Class] INT NOT NULL,

	CONSTRAINT [PK_dbo.Students] PRIMARY KEY (VNum),
	CONSTRAINT [FK_dbo.Students] FOREIGN KEY (Class) REFERENCES [dbo].[Classes]
);

/* SignIn Table (For Logging Information) */
CREATE TABLE [dbo].[SignIns]
(
	[ID] INT IDENTITY (1,1) NOT NULL,
	[Week] INT NOT NULL,
	[Date] DATE NOT NULL,
	[Time] TIME NOT NULL,
	[StudentID] INT Not Null,

	CONSTRAINT [PK_dbo.SignIns] PRIMARY KEY CLUSTERED (ID ASC),
	CONSTRAINT [FK_dbo.SignIns] FOREIGN KEY (StudentID) REFERENCES [dbo].[Students]
);

/* Seed Data For Testing */
INSERT INTO Classes(CRN, DeptPrefix, ClassNum, Instructor, Days, StartTime) VALUES
(123, 'MTH', 70, 'Rosales, Kendal', 'MWF', 1200),
(124, 'MTH', 95, 'Weibe, Ron', 'TR', 1400),
(125, 'MTH', 111, 'Nerz, Andrew', 'MTWF', 0800)

INSERT INTO Students(VNum, FirstName, LastName, Class) VALUES
(00778899, 'Eryn', 'Murphy', 123),
(00221144, 'Tiffany', 'Jansen', 124),
(00559977, 'Nadine', 'Englund', 125)

INSERT INTO SignIns(Week, Date, Time, StudentID) VALUES
(2, '2019-02-05', '14:30:15', 00778899),
(3, '2019-02-12', '15:25:15', 00221144),
(3, '2019-02-13', '09:35:12', 00559977)