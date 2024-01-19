USE [SwitchOperations]
GO
/****** Object:  UserDefinedFunction [dbo].[FunRemoveLeftZero]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FunRemoveLeftZero]
(
	@StrInput VARCHAR(200)
)
RETURNS VARCHAR(200)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result VARCHAR(200)

	-- Add the T-SQL statements to compute the return value here
	SELECt @Result=(SUBSTRING(@StrInput, PATINDEX('%[^0]%', @StrInput+'.'), LEN(@StrInput)))

	-- Return the result of the function
	RETURN @Result
END


GO
