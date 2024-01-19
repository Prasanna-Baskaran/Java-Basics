USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetConfigPathForFileUpload]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetConfigPathForFileUpload]
	@IssuerNo  Varchar(200)
	AS
BEGIN
	SELECT FileID ,Path,FileName,SFTP_Server,SFTP_Port,SFTP_Path,SFTP_User,SFTP_PWD ,FileType,SFTP_BackUp,InputPath,OutputPath
	FROM TblMassFileUpload  WITH(NOLOCK)	
	where Participant=@IssuerNo
END

GO
