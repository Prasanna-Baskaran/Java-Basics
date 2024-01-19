USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Usp_getMasConfiguration_20180404]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name- Gufran Khan>
-- Create date: <Create Date,23-10-2017,>
-- Description:	<Description,To get the Customer data modification configuration,>
-- =============================================
Create PROCEDURE [dbo].[Usp_getMasConfiguration_20180404]
	
AS
BEGIN
	SELECT BankId,IssuerNr,ServerIp,ServerPort,FilePathInput,FilePathOutPut,FilePathArchive,Username,dbo.ufn_DecryptPAN(password)[PassWord],keyPath,dbo.ufn_DecryptPAN(Keypassphrase)[Passphrase],Filepath,fileHeader,
	FilePathInput_RePIN,FilePathOutPut_RePIN,FilePathArchive_RePIN,FilePath_RePIN,fileHeader_RePIN,isPGP,Trace,PublicKeyFilePath,PrivateKeyFilePath,
dbo.ufn_decryptpan(Password_PGP)[Password_PGP],InputFilePath_PGP,FiledCount[FieldCount]
	FROM TBLMasConfiguration WHERE ENABLE=1 ORDER BY sequence
END

GO
