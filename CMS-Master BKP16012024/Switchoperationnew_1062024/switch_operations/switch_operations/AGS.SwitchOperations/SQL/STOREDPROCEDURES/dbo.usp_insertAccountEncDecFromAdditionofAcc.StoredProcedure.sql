USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[usp_insertAccountEncDecFromAdditionofAcc]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[usp_insertAccountEncDecFromAdditionofAcc]
(
	@BankId varchar(10),
	@EncAcc varchar(100),
	@Accno varchar(100),
	@AccounType varchar(4),
	@Currency varchar(10)
)
as
begin
Declare @issuerno numeric
select @issuerno=BankCode from tblbanks nolock where ID=@BankId

insert into test(c1,c2,c3,c4)
values(@BankId,@issuerno,@EncAcc,@Accno)

	if not exists( select top 1 1 from CardRAccounts nolock where IssuerNo=@issuerno and EncAcc=@EncAcc)
	begin
		insert into CardRAccounts(EncAcc,DecAcc,IssuerNo,AccountType,CreatedDate)
		values(@EncAcc,dbo.ufn_EncryptPAN(@Accno),@issuerno,@AccounType,getdate())
	end
end
GO
