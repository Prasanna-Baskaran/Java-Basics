USE [SwitchOperations]
GO
/****** Object:  UserDefinedFunction [dbo].[FunGetApplicationNo]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[FunGetApplicationNo]
(
	@CustomerID VARCHAR(200)
)
RETURNS VARCHAR(20)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result VARCHAR(20)	
	
	Set @Result= 'AG'+RIGHT(CONCAT('00000000000000000000', @CustomerID), 15)
	
	RETURN  @Result

END

GO
