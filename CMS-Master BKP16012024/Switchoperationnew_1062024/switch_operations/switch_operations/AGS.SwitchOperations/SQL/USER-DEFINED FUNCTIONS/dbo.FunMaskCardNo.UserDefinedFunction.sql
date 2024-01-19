USE [SwitchOperations]
GO
/****** Object:  UserDefinedFunction [dbo].[FunMaskCardNo]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FunMaskCardNo]
(
	@Cardno VARCHAR(20)
)
RETURNS Varchar
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result VARCHAR(20)

	 Select @Result= stuff(@Cardno,7,LEN(@Cardno)-(6+4),'xxxxxxxx')
	-- Return the result of the function
	RETURN @Result

END

GO
