USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_GetFileToProcess]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_GetFileToProcess]
  @IssuerNo int 
AS
/************************************************************************
Object Name: 
Purpose: Get uploaded file to process
Change History
Date         Changed By				Reason
21/07/2017  Diksha Walunj			Newly Developed
06/10/2017  Diksha Walunj			ATPCM224: Get Inser validation SP 



*************************************************************************/

BEGIN
		Select f.ID,F.FileID,F.FullFilePath,f.FileName,M.OutputPath,M.FailedPath,M.SFTP_Failed,M.SFTP_StatusFile,M.FileType,RTRIM(LTRIM(M.Seperator)) AS Seperator,
		M.SFTP_Server,M.SFTP_Port,M.SFTP_Path,M.SFTP_User,M.SFTP_PWD
		,FT.InsertValidationSP        --06/10/2017  Diksha Walunj			ATPCM224: Get Inser validation SP 
		from TbldetFileUpload F WITH(NOLOCK)
		INNER JOIN TblMassFileUpload M WITH(NOLOCK) ON  F.FileID=M.FileID 
		INNER JOIN TblFileType FT With(NOLOCK) ON FT.ID=M.FileType    --06/10/2017  Diksha Walunj	ATPCM224: Get Inser validation SP 
		where ISNULL(F.Status,0)=0 AND M.Participant=@IssuerNo
END


GO
