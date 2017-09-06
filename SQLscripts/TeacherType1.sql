/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2016 (13.0.4001)
    Source Database Engine Edition : Microsoft SQL Server Express Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2016
    Target Database Engine Edition : Microsoft SQL Server Express Edition
    Target Database Engine Type : Standalone SQL Server
*/

USE [SchoolManagement]
GO

/****** Object:  UserDefinedTableType [dbo].[TeacherType]    Script Date: 9/5/2017 4:55:46 PM ******/
CREATE TYPE [dbo].[TeacherType1] AS TABLE(
	[Id] [int] NULL,
	[First Name] [nvarchar](50) NULL,
	[Last Name] [nvarchar](50) NULL,
	[Number of Students] [int] NULL
)
GO


