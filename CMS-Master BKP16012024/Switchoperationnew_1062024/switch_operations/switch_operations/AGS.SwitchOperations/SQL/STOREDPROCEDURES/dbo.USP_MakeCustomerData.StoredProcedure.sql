USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_MakeCustomerData]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[USP_MakeCustomerData]  --exec [USP_MakeCustomerData] 0,'select','Sample_25.txt',''
(
	@Code int,
	@Mode varchar(50),
	@CIFId varchar(100),
	@RespCode varchar (30)
)
as
BEGIN
if(@Mode='select')
BEGIN

SELECT substring(CIFID, patindex('%[^0]%',CIFID), 100)+','+LTRIM(RTRIM(((Case when (ISNULL(AADHAR_NO,'')='' and  ISNULL(PAN,'')='' )then 
'000000000000' ELSE Case when ISNULL(AADHAR_NO,'')='' then pan else AADHAR_NO end end ))))+',,'+LTRIM(RTRIM((ISNULL([CUSTOMER NAME],''))))+',,,'+LTRIM(RTRIM((ISNULL([NAME ON CARD],''))))+',,,,,,,,,,,,'+LTRIM(RTRIM((ISNULL([MOBILE_NUMBER],''))))+',,'+LTRIM(RTRIM((ISNULL([EMAIL],''))))+','+LTRIM(RTRIM((ISNULL([Add 1],''))))+','+LTRIM(RTRIM((ISNULL([Add 2],''))))+' '+LTRIM(RTRIM((ISNULL([Add 3],''))))+','+LTRIM(RTRIM((ISNULL([City],''))))+','+LTRIM(RTRIM((ISNULL([State],''))))+','
+LTRIM(RTRIM((ISNULL([Pincode],''))))+','+LTRIM(RTRIM((ISNULL(LEFT([Country],3),''))))+',,,,,,,'+LTRIM(RTRIM((ISNULL([DOB],''))))+',,,0,,'+ '|'+ISNULL(PGKValue,'') 
AS [cifdata]
,[FileName],[BANK],IR.SwitchIssNr[IssuerNo],a.code,a.cifid[cif]
FROM TblCustomerDataModification A WITH (NOLOCK)

join Issuer_Nr  IR with (Nolock) on a.BANK=IR.BANKID
WHERE REJECTED=0 AND PROCESSED=1 AND UPDATED=0
END
if(@Mode='Update')
Begin
	Update TblCustomerDataModification set updated=1,Updated_Date=GETDATE(),switchRespCode=@respCode where code =@Code and CIFid = @CIFId
	select 'success'
End
END


GO
