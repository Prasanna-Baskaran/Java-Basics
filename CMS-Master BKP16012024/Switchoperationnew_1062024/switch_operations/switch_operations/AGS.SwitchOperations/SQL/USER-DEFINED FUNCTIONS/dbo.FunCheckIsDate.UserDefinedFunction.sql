USE [SwitchOperations]
GO
/****** Object:  UserDefinedFunction [dbo].[FunCheckIsDate]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FunCheckIsDate]
(
 @strInput Varchar(8)
)
RETURNS int
AS
BEGIN
DECLARE @Result int	=1
DECLARE @Temp  varchar(10)
--Select LEFT(@Date,2),SUBSTRING(@Date,3,2),RIGHT(@Date,4),LEFT(@Date,2)+'/'+SUBSTRING(@Date,3,2)+'/'+RIGHT(@Date,4)
SET @Temp=SUBSTRING(RTRIM(LTRIM(@strInput)),3,2)+'/'+LEFT(RTRIM(LTRIM(@strInput)),2)+'/'+RIGHT(@strInput,4)

IF ISDATE(@Temp)!=1
SET @Result=0
RETURN @Result
END

GO
