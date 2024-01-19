USE [SwitchOperations]
GO
/****** Object:  UserDefinedFunction [dbo].[FunGetPassword]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FunGetPassword]
(
	@FirstName VARCHAR(50),
	@MobileNo VARCHAR(20)
)
RETURNS VARCHAR(20)
AS
BEGIN
	DECLARE @Result VARCHAR(20)
	DECLARE @CustomerName VARCHAR(20)
	--DECLARE @MobileNo VARCHAR(20)

	--Password eg Customer Name -ABC MobileNO -9811112233 => PWD : AB@2233

	SET @CustomerName=substring(RTRIM(LTRIM(@FirstName)),1,2)
	SET @MobileNo=substring(RTRIM(LTRIM(@MobileNo)),7,4 )	
	Set @Result=@CustomerName+'@'+@MobileNo	
	RETURN UPPER(@Result)

END

--CustomerID wise password
--ALTER FUNCTION [dbo].[FunGetPassword]
--(
--	@CustomerID BIGINT
--)
--RETURNS VARCHAR(20)
--AS
--BEGIN
--	DECLARE @Result VARCHAR(20)
--	DECLARE @CustomerName VARCHAR(20)
--	DECLARE @MobileNo VARCHAR(20)

--	--Password eg Customer Name -ABC MobileNO -9811112233 => PWD : AB@2233

--	SELECT @CustomerName=substring(RTRIM(LTRIM(FirstName)),1,2),@MobileNo=substring(RTRIM(LTRIM(MobileNo)),7,4 )
--	FROM TblCustomersDetails WITH(NOLOCK)
--	WHERE CustomerID=@CustomerID
--	Set @Result=@CustomerName+'@'+@MobileNo	
--	RETURN @Result

--END


GO
