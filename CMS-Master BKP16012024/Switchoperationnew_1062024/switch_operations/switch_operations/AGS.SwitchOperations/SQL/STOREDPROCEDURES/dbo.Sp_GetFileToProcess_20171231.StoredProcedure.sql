USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_GetFileToProcess_20171231]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[Sp_GetFileToProcess_20171231]
  @IssuerNo int 
AS
BEGIN
		Select f.ID,F.FileID,F.FullFilePath,f.FileName,M.OutputPath,M.FailedPath,M.SFTP_Failed,M.SFTP_StatusFile,M.FileType,RTRIM(LTRIM(M.Seperator)) AS Seperator,
		M.SFTP_Server,M.SFTP_Port,M.SFTP_Path,M.SFTP_User,M.SFTP_PWD
		from TbldetFileUpload F WITH(NOLOCK)
		INNER JOIN TblMassFileUpload M WITH(NOLOCK) ON  F.FileID=M.FileID 
		where ISNULL(F.Status,0)=0 AND M.Participant=@IssuerNo
END

GO
