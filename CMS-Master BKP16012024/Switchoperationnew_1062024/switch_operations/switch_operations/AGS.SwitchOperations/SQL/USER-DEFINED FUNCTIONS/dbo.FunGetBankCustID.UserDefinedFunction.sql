USE [SwitchOperations]
GO
/****** Object:  UserDefinedFunction [dbo].[FunGetBankCustID]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FunGetBankCustID]
(
	@CustomerID varchar(800)
)
RETURNS VARCHAR(800)
AS
BEGIN
	DECLARE @Result varchar(800),@CustIdentity VARCHAR(20),@BankID VARCHAR(200),@CustIDLen int
	--- get 2 digit random Alphabets 
	DECLARE @RandomString VARCHAR(2)	
	SELECT  @RandomString=Value FROM vw_GetRandomAlphabets

	--SELECT @RandomString= CHAR(FLOOR(65 + (RAND() * 25)))+CHAR(FLOOR(65 + (RAND() * 25)))

	Select @BankID= cu.BankID,@CustIdentity=ISNULL(B.CustIdentity,'')+@RandomString ,@CustIDLen=ISNULL(CustomerIDLen,16) From TblCustomersDetails cu WITH(Nolock)
	INNER JOIN TblBanks B WITH(NOLOCK) ON cu.BankID=B.ID
	where cu.CustomerID=@CustomerID

	Set @Result=@CustIdentity+RIGHT(CONCAT('00000000000000000000', @CustomerID), (@CustIDLen-LEN(@CustIdentity)))	
RETURN @Result
END

GO
