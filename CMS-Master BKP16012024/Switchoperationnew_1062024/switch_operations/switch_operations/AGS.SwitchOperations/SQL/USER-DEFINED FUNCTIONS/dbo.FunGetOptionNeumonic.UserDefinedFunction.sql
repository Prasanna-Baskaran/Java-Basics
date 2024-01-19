USE [SwitchOperations]
GO
/****** Object:  UserDefinedFunction [dbo].[FunGetOptionNeumonic]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[FunGetOptionNeumonic]
(
)
RETURNS VARCHAR(MAX)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result VARCHAR(MAX)

	Select @Result= COALESCE(@Result+',' ,'')+ replace(OptionNeumonic,' ','')   from TblOptions WITH(NOLOCK) 
    where OptionParentNeumonic<>'' AND ISNULL(Active,0)=1
	--Select @Result

	-- Return the result of the function
	RETURN @Result

END

GO
