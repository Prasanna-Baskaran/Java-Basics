USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_CA_GetPaths_For_CardGen]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_CA_GetPaths_For_CardGen]
	@issuerNo int=0 ,
	@IntPara int=0

AS
/************************************************************************
Object Name: 
Purpose: Get Configured paths  for CardAutomation and PRE Generation
Change History
Date         Changed By				Reason
26/04/2017 Diksha Walunj			Newly Developed
23/09/2017 Diksha Walunj(ATPCM-223) Modified For RBL CardAutomation added New Paths
04/10/2017 Diksha Walunj(ATPCM224)	Modified For For ReissueFiles gen

*************************************************************************/
BEGIN

IF(@IntPara=0)
BEGIN
	Select BA.ID,Replace(RTRIM(LTRIM(BA.BankName)),' ','') AS [BankName],AGS_SFTPServer,AGS_SFTPPath,AGS_SFTP_User,AGS_SFTP_Pwd,AGS_SFTP_Port,SFTP_CIF_Source_Path,CardCIF_Input_Path
	,CardCIF_Backup,ZipCardFilesPath,CardAutoSourcePath,CardAutoOutputPath_SFTP
	,B_SFTPServer,B_SFTPPath,B_SFTP_User,B_SFTP_Pwd,B_SFTP_Port,B_PRE_DestinationPath_SFTP,Zip_Exe_Path,SFTP_CIF_BackUp_Path,CardAutoBackUpPath,CardAutoFailedPath
	,ISNULL(IsSaveError,0) AS [IsSaveError],IssuerNo,ErrorLogPath,FailedCIFPath,B_SFTP_FailedCIFPath
	,BAT_SourceFilePath,BAT_SourceFilePath_BK,SFTP_BAT_SourceFilePath,SFTP_BAT_SourceFilePath_BK,SFTP_OutputFile_BK_Path    ---ATPCM-223:Diksha Walunj:23/09/17:Get BAT Source Paths
	,P.Filename AS[ReissueFileName]  ------ATPCM-223:Diksha Walunj:23/09/17:For ReissueFiles gen
	from TblCardAutoFilePath CA WITH(NOLOCK)
	INNER JOIN TblBanks BA WITH(NOLOCK) ON CA.IssuerNo=ba.BankCode
	INNER JOIN TblCACardGenProcessTypes P WITH(NOLOCK) ON  P.ProcessID=6   ------ATPCM-223:Diksha Walunj:23/09/17:For ReissueFiles gen
	 where CA.IssuerNo=@issuerNo	
END
ELSE
BEGIN
	SELECT BA.ID,Replace(RTRIM(LTRIM(BA.BankName)),' ','') AS [BankName],CA.IssuerNo, SFTP_OutputFile_Path,SFTP_OutputFile_Failed
	,AGS_SFTPServer,AGS_SFTP_User,AGS_SFTP_Pwd,AGS_SFTP_Port
	,C_SFTP_PRE_Path,C_SFTPServer,C_SFTP_User,C_SFTP_Pwd,C_SFTP_Port,PRE_Input_Path,PRE_Output_Path,Outputfile_failed_Path
	,ISNULL(IsSaveError,0) AS [IsSaveError],ErrorLogPath,PGP_KeyName,PGP_PWD,PGPExePath,AGS_PGP_KeyName,AGS_PGP_PWD
	,PGP_KeyPath,AGS_KeyPath,SFTP_OutputFile_BK_Path,OutputFile_BK_Path
	,AGS_PubKey_Path,AGS_SecKey_Path,B_PubKey_Path,B_SecKey_Path,PubKey_Path,SecKey_Path
  		from TblCardAutoFilePath CA WITH(NOLOCK)
		INNER JOIN TblBanks BA WITH(NOLOCK) ON CA.IssuerNo=ba.BankCode
		 where CA.IssuerNo=@issuerNo	
END
END

GO
