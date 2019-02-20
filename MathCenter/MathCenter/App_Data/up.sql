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
	[StartTime] NVARCHAR(25),
	[Other] NVARCHAR(MAX),

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

/* Seed Placeholder Class */
/* Make it so I can make a random class with ID = -1 */
SET IDENTITY_INSERT [dbo].[Classes] ON; 

INSERT INTO [dbo].[Classes] (ClassID, Other) VALUES
(-1, 'Placeholder');

/* Turn it back off so we can only do it once. */
SET IDENTITY_INSERT [dbo].[Classes] OFF; 

/* Seed Data For Testing */
INSERT INTO Classes(CRN, DeptPrefix, ClassNum, Instructor, Days, StartTime) VALUES
(123, 'MTH', 70, 'Rosales, Kendal', 'MWF', '1200'),
(126, 'MTH', 70, 'Rosales, Kendal', 'MWF', '1300'),
(124, 'PSY', 95, 'Weibe, Ron', 'TR', '1400'),
(125, 'MTH', 111, 'Nerz, Andrew', 'MTWF', '0800')

INSERT INTO Students(VNum, FirstName, LastName, Class) VALUES
('00778899', 'Eryn', 'Murphy', 1),
('00221144', 'Tiffany', 'Jansen', 2),
('00559977', 'Nadine', 'Englund', 3)

INSERT INTO SignIns(Week, Date, Hour, Min, Sec, StudentID) VALUES
(2, '2019-02-05', 14, 30, 15, '00778899'),
(3, '2019-02-12', 15, 25, 15, '00221144'),
(3, '2019-02-13', 09, 35, 12, '00559977')