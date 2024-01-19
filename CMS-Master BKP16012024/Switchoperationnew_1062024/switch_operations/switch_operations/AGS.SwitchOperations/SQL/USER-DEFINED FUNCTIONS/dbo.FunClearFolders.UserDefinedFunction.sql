USE [SwitchOperations]
GO
/****** Object:  UserDefinedFunction [dbo].[FunClearFolders]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create FUNCTION [dbo].[FunClearFolders](
@StrOriginalPath nvarchar(4000),
@StrDestPath nvarchar(4000) 

)
RETURNS BIT
AS
BEGIN
	Declare @NewAccFileName As Varchar(100),@NewCustFileName As Varchar(100),@NewCustAccFileName As Varchar(100)
	DECLARE	@TodayDate as varchar(40),@TodayHour as varchar(40),@TodayMin as varchar(40),@TodaySec as varchar(40),
	@Cmdstr as varchar(4000),@Result As BIT

	SET @TodayDate = CONVERT(varchar(10), GETDATE(), 112)
	SET @TodayHour = DATEPART(hh,GETDATE())
	SET @TodayMin = DATEPART(mi,GETDATE())
	SET @TodaySec = DATEPART(ss,GETDATE())

	SELECT @NewAccFileName = 'accounts' + '_' + @TodayDate + '_' + @TodayHour + '_' + @TodayMin + '_' + @TodaySec + '.txt'
	SELECT @NewCustFileName = 'Customers' + '_' + @TodayDate + '_' + @TodayHour + '_' + @TodayMin + '_' + @TodaySec + '.txt'
	SELECT @NewCustAccFileName = 'CustomerAccounts' + '_' + @TodayDate + '_' + @TodayHour + '_' + @TodayMin + '_' + @TodaySec + '.txt'

	SET @Cmdstr='MOVE '+ @StrOriginalPath + '\accounts.txt '+ @StrDestPath +'\' + @NewAccFileName 
	EXEC master..xp_cmdshell @cmdstr
	SET @Cmdstr='MOVE '+ @StrOriginalPath + '\Customers.txt '+ @StrDestPath +'\' + @NewCustFileName 
	EXEC master..xp_cmdshell @cmdstr
	SET @Cmdstr='MOVE '+ @StrOriginalPath + '\CustomerAccounts.txt '+ @StrDestPath +'\' + @NewCustAccFileName 
	EXEC master..xp_cmdshell @cmdstr

	Set @Result=@@RowCount

    RETURN cast(@Result as bit)
END;

GO
