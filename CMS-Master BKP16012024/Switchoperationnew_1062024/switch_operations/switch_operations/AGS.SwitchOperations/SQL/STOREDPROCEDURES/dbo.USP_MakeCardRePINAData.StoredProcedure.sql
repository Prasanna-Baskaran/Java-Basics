USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_MakeCardRePINAData]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--select * from [temp_po-old].dbo.cardproductionrenewal
--select * from  TblCardGenRequest
Create procedure [dbo].[USP_MakeCardRePINAData]  --exec [USP_MakeCardRePINAData] 0,'select',''
(
	@Code int,
	@Mode varchar(50),
	@cardNO varchar(100),
	@RespCode varchar(30)
)
as
BEGIN
if(@Mode='select')
BEGIN

SELECT RTRIM(LTRIM(dbo.ufn_DecryptPAN(cardNO)))[CardNo],code,IR.SwitchIssNr[IssuerNo],A.CIFID[CIFID],A.AccountNo[AccountNo]
FROM CardRePINRequest A WITH (NOLOCK)
join Issuer_Nr  IR with (Nolock) on a.BANKid=IR.BANKID
WHERE REJECTED=0 AND PROCESSED=1 AND ISNULL(UPDATED,0)=0
END
if(@Mode='Update')
Begin
	Update CardRePINRequest set updated=1,switchRespCode=@RespCode,updateddate=GETDATE() where code =@Code and dbo.ufn_DecryptPAN(cardNO) = @cardNO
	select 'success'
End
END


GO
