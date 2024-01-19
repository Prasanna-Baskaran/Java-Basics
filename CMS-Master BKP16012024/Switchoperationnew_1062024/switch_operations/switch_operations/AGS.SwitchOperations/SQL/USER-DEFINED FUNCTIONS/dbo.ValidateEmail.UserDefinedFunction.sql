USE [SwitchOperations]
GO
/****** Object:  UserDefinedFunction [dbo].[ValidateEmail]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create FUNCTION [dbo].[ValidateEmail](@EmailID nvarchar(400) )
RETURNS BIT
AS
BEGIN
     DECLARE @result bit
    if(
	patindex ('%[ &'',":;!+=\/()<>]%', @EmailID) > 0 -- Invalid characters
	or patindex ('[@.-_]%', @EmailID) > 0 -- Valid but cannot be starting character
	or patindex ('%[@.-_]', @EmailID) > 0 -- Valid but cannot be ending character
	or @EmailID not like '%@%.%' -- Must contain at least one @ and one .
	or @EmailID like '%..%' -- Cannot have two periods in a row
	or @EmailID like '%@%@%' -- Cannot have two @ anywhere
	or @EmailID like '%.@%' or @EmailID like '%@.%' -- Cannot have @ and . next to each other
	or @EmailID like '%.cm' or @EmailID like '%.co' -- Camaroon or Colombia? Typos. 
	or @EmailID like '%.or' or @EmailID like '%.ne' -- Missing last letter
	)
	Begin
		set @result = 0;
	End
	Else
	BEgin
		set @result = 1
	End

     RETURN @result
END;

GO
