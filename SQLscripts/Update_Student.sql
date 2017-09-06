/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2016 (13.0.4001)
    Source Database Engine Edition : Microsoft SQL Server Express Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2017
    Target Database Engine Edition : Microsoft SQL Server Standard Edition
    Target Database Engine Type : Standalone SQL Server
*/

USE [SchoolManagement]
GO
/****** Object:  StoredProcedure [dbo].[Update_Student]    Script Date: 9/5/2017 10:11:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[Update_Student]

      @tblStudent [dbo].[StudentType] READONLY

AS
BEGIN
      SET NOCOUNT ON;
	  
      MERGE INTO Student c1
      USING @tblStudent c2
      ON c1.[Student ID]=c2.[Id]
      WHEN MATCHED THEN
      UPDATE SET c1.[Student Number] = c2.[Student Number]
            ,c1.[First Name] = c2.[First Name]
			,c1.[Last Name] = c2.[Last Name]
			,c1.[Has Scholarship] = c2.[Has Scholarship]
      WHEN NOT MATCHED THEN
      INSERT VALUES(c2.[Id], c2.[Student Number], c2.[First Name], c2.[Last Name], c2.[Has Scholarship]);
	  
END