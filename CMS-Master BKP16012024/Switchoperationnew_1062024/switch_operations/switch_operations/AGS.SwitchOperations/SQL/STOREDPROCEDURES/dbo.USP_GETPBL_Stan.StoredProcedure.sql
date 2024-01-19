USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_GETPBL_Stan]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_GETPBL_Stan]   
AS    
/** NAME : USP_GETPBL_Stan    
** USAGE : TO GET STAN FOR SWITCH
** BY  : SACHIN PARLE
** DATE : 30 JUNE 2016    
EXEC USP_GETPBL_Stan    
**/    
BEGIN    
 SELECT NEXT VALUE FOR [DBO].[PBL_Stan] AS [PBL_Stan]    
END
GO
