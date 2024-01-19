USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SPCheckDirExists]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SPCheckDirExists] 
(@StrDirPath nvarchar(4000),
@IntpriOutputCode Int Output,
@StrpriOutputDesc Varchar(500) OutPut
 )

 /*
 Declare @IntOutPut1 Int
 Declare @StrpriOutputDesc Varchar(500)
 exec SPCheckDirExists 'C:\tetz',@IntOutPut1 output,@StrpriOutputDesc output
 select @IntOutPut1,@StrpriOutputDesc
 */

AS
BEGIN
	Set @IntpriOutputCode=0;
	Set @StrpriOutputDesc='';

	If(Isnull(@StrDirPath,'')<>'')
	Begin
		-- 1 - Variable declaration
		DECLARE @BackupDirName Varchar(50)
		DECLARE @TblDirTree TABLE (subdirectory nvarchar(255), depth INT)

		-- 2 - Initialize variables
		SET @BackupDirName = 'Backup'
		SET @StrDirPath = @StrDirPath +'\' + @BackupDirName

		-- 3 - @@StrDirPath values
		INSERT INTO @TblDirTree(subdirectory, depth)
		EXEC master.sys.xp_dirtree @StrDirPath


		-- 4 - Create the @DataPath directory
		IF NOT EXISTS (SELECT 1 FROM @TblDirTree WHERE subdirectory = @BackupDirName)
		EXEC master.dbo.xp_create_subdir @StrDirPath
	
		IF ((SELECT dbo.FunFileExists(@StrDirPath))=1)
		Begin
			Set @IntpriOutputCode=1;
			Set @StrpriOutputDesc=@StrDirPath;
		End
		Else
			Set @IntpriOutputCode=0;
	End
	Else
	Begin
		Set @IntpriOutputCode=0;
	End
	print @IntpriOutputCode;
END;

GO
