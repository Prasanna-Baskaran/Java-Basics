USE [SwitchOperations]
GO
/****** Object:  UserDefinedFunction [dbo].[FunFileExists]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create FUNCTION [dbo].[FunFileExists](@StrFullFileName nvarchar(4000) )
RETURNS BIT
AS
BEGIN
     DECLARE @result INT
     EXEC master.dbo.xp_fileexist @StrFullFileName, @result OUTPUT
     RETURN cast(@result as bit)
END;

GO
