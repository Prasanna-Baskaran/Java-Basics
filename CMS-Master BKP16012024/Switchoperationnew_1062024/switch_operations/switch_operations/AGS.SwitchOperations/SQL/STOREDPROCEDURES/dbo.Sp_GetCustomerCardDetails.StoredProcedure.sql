USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_GetCustomerCardDetails]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create proc [dbo].[Sp_GetCustomerCardDetails]
(
 --   @ApplicationNo VARCHAR(20)='',
	--@MobileNo Varchar(12)='',
	--@CustomerID Bigint =0,
	--@DOB_AD Datetime=NULL,
	--@DOB_BS Datetime=NULL,
	--@IdentificationNo VARCHAR(50)='',
	@CustomerName VARCHAR(200),
	--@IntPara tinyint =0,
	--@CreatedDate Datetime=NULL,
	@SystemID VARCHAR(200),
	@BankID VARCHAR(200),
	@CardNo VARCHAR(20)	
	--@BankCustID VARCHAR(200)=''
)
as
declare @AccountNo varchar(100)=''
begin
exec [Sp_GetCustomerCardDetailsFromSwitch] @SystemID=@SystemID,@BankID=@BankID,@CardNO=@CardNo,@IntPara=0,@Name=@CustomerName,@AccountNo=@AccountNo
end
GO
