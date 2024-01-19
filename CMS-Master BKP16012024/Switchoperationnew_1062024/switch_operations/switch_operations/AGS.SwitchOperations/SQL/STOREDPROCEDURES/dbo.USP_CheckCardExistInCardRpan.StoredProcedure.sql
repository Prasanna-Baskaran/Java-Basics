USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_CheckCardExistInCardRpan]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--select * from TestCarddetails
--update TestCarddetails set issuerno=23,bankid=1,systemid=2 where cardno=20002
Create proc [dbo].[USP_CheckCardExistInCardRpan]
(
@CardNo varchar(50),
@BankId int,
@SystemId varchar(10)
)
as
begin
if exists(Select Top 1 1 from Tblcustomersdetails c WITH(NOLOCK)INNER JOIN cardrpan P WITH(NOLOCK) ON RTRIM(LTRIM(C.BankCustID))=RTRIM(LTRIM(P.customer_id))
				--Where dbo.ufn_decryptpan(P.decpan)=@CardNo AND ((@BankCustID='') OR(P.customer_id=@BankCustID) OR(c.CustomerID=@BankCustID))
				where c.BankID=@BankId AND c.SystemID=@SystemId and dbo.ufn_decryptpan(P.decpan)=@CardNo)
				select 'Card details exist|Exist' as [StatusMessage] 
end 
GO
