USE [SwitchOperations]
GO
/****** Object:  View [dbo].[vw_GetRandomAlphabets]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE View [dbo].[vw_GetRandomAlphabets]
AS
-- get random Alphabets 
SELECT CHAR(FLOOR(65 + (RAND() * 25)))+CHAR(FLOOR(65 + (RAND() * 25)))+CHAR(FLOOR(65 + (RAND() * 25)))+CHAR(FLOOR(65 + (RAND() * 25)))+CHAR(FLOOR(65 + (RAND() * 25)))+CHAR(FLOOR(65 + (RAND() * 25)))+CHAR(FLOOR(65 + (RAND() * 25)))+CHAR(FLOOR(65 + (RAND() * 25))) AS Value

GO
