USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[usp_get_SFTP_Deatils_20180411]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create PROCEDURE [dbo].[usp_get_SFTP_Deatils_20180411] --  usp_get_SFTP_Deatils '55'
	@IssuerNo varchar (5)
AS
BEGIN

	select 
	   [SERVER_IP]
	   ,SERVER_PORT
	   ,Username
	   ,dbo.ufn_DecryptPAN(password)[Password]
	   ,KeyPath
	   ,dbo.ufn_DecryptPAN(Keypassphrase)[Keypassphrase]
	   ,InputfilePath
	   ,ArchiveFilePath
	   ,outputFilePath
	   ,filePath
	  ,[smtpServer]
      ,[SMTPport]
      ,[SFTPUserName]
      ,dbo.ufn_DecryptPAN([SFTPPassword]) as [SFTPPassword]
      ,[SFTPDirectory]
      ,[SFTPDirectoryMove]
      ,[SFTPDirectoryReports]
      ,[SFTPDirectoryReissue]
      ,[SFTPDirectoryMoveReisse]
      ,[SFTPDirectoryReissueReports]
      ,[downloadFilePath]
      ,[ProcessdFilePath]
      ,[SwitchFilePath]
      ,[downloadReissueFilePath]
      ,[ProcessdReissueFilePath]
      ,[SwitchReissueFilePath]
      ,[SWITCHSFTP]
      ,[SWITCHPORT]
      ,[SWITCHSFTPDirectory]
      ,[SWITCHSFTPDirectoryReissue]
      ,[SWITCHUserName]
      ,Convert(varchar(max),[SWITCHPassword]) as [SWITCHPassword]
      ,[SP]
      ,[SPReissue]
      ,[rarWorkingDirectory]
      ,[LogPath]
      ,[ISMail]
      ,[ALTEMAILFRM]
      ,[ALTSMTPCLIENT]
      ,[ALTUSER]
      ,[ALTPASSWORD]
      ,[mailTo]
      ,[ErrorMailTo]
      ,[ErrorBCC]
      ,[BCC]
      ,[Mailport]
      ,[RejectedFilePath]
	  ,CardRenewalHeader
	  ,CardReissueHeader
	  ,[Issuerno] 
	  ,CardUpgrdeHeader
	  ,SFTPDirectoryUpgrade
	  ,SFTPDirectoryMoveUpgrade
	  ,SFTPDirectoryUpgradeReports
	  ,downloadUpgradeFilePath
	  ,ProcessdUpgradeFilePath
	  ,SwitchUpgradeFilePath
	  ,SWITCHSFTPDirectoryUpgrade
	  ,SPUpgrade
	  from sftp_details  nolock where [Issuerno]=@IssuerNo
END





GO
