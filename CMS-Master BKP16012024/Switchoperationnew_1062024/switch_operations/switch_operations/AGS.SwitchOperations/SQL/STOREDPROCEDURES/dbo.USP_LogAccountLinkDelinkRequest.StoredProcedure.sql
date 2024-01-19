USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_LogAccountLinkDelinkRequest]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[USP_LogAccountLinkDelinkRequest]
(
	@CardNo Varchar(20)='',
	@AccounNo Varchar(20)='',
	@AccountType nvarchar(2)='',
	@AccountQualifier nvarchar(2)='',
	@LinkingFlag varchar(2)='',
	@Response varchar(1000)='',
	@ResponseDesc varchar(max)='',
	@LogId varchar(10)=''
)
as
begin
	if @LogId<>''
		begin
			update AccountLinkingDeLinkingRequest set Response=@Response,ResponseDesc=dbo.ufn_EncryptPAN( @ResponseDesc)  where Cid=@LogId		
			select '' ID
		end
	else
		begin
			insert into AccountLinkingDeLinkingRequest(EncPan,EncAcc,AccountType,AccountQualifier,LinkingFlag,GeneratedOn)
			values(dbo.ufn_EncryptPAN(@CardNo),dbo.ufn_EncryptPAN(@AccounNo),@AccountType,@AccountQualifier,@LinkingFlag,GETDATE())
			select MAX(Cid) ID from AccountLinkingDeLinkingRequest with(nolock)
		end
end
GO
