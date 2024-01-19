USE [SwitchOperations]
GO
/****** Object:  UserDefinedFunction [dbo].[LtrimZero]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[LtrimZero]
(
 @strInput Varchar(100)
)
RETURNS varchar(100)
AS
BEGIN
declare @str varchar(100)=@strInput
	set @str = SUBSTRING(@str,PATINDEX('%[^0]%',@str + '.'),Len(@str))
RETURN @str
END

GO
