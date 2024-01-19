USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_UpdateFileUploadStatus]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------
CREATE PROCEDURE [dbo].[Sp_UpdateFileUploadStatus] 
  @UploadID Varchar(800)
AS
/************************************************************************
Object Name: 
Purpose: To update uploaded file status when file is not proper
Change History
Date         Changed By				Reason
07/10/2017	Diksha Walunj			Newly Developed

*************************************************************************/
BEGIN
	   Update TbldetFileUpload SET EndDate=GETDATE(),[Status]=4 ,Error='Invalid file data' where ID=@UploadID
END

GO
