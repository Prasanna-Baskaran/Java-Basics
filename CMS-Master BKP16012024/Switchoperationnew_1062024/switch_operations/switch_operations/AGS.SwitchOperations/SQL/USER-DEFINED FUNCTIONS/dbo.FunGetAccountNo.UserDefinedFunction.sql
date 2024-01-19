USE [SwitchOperations]
GO
/****** Object:  UserDefinedFunction [dbo].[FunGetAccountNo]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FunGetAccountNo]
(
	@CustomerID varchar(800)
)
RETURNS VARBINARY(max)
AS
BEGIN
	DECLARE @Result varbinary(max)
	DECLARE @AccountNo varchar(800),@SystemID VARCHAR(200),@BankID VARCHAR(200),@MaxCount varchar(800)

	Select @AccountNo =ISNULL(convert(varchar(800),AccNo),'') ,@SystemID=SystemID,@BankID=BankID
	From TblCustomersDetails WITH(NOLOCK)
	where CustomerID=@CustomerID	
	IF(@AccountNo='')
	BEGIN
	
	   Select @MaxCount=(Max(convert(bigint,ISNULL(dbo.ufn_decryptPAN(AccNo),0)))+1) from  TblCustomersDetails WITH(Nolock)
		where SystemID= @SystemID AND BankID=@BankID
		
		Select @Result= dbo.ufn_EncryptPAN(@MaxCount)From TblCustomersDetails WITH(NOLOCK)
		where CustomerID=@CustomerID	
				
	END
	ELSE
	BEGIN		
	  Select  @Result=AccNo
	  From TblCustomersDetails WITH(NOLOCK)
		where CustomerID=@CustomerID	
	END
RETURN @Result
END

GO
