USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_GetProcessedCardRePINAData]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--select * from [temp_po-old].dbo.cardproductionrenewal
--select * from  TblCardGenRequest
CREATE procedure [dbo].[USP_GetProcessedCardRePINAData]  --exec [USP_GetProcessedCardRePINAData] 'CIF_REPIN10042018445.txt','2018-04-12'
(
	@FileName varchar (max),
	@Datetime Datetime
)
as
print @Datetime
BEGIN
SELECT DBO.ufn_DecryptPAN(CARDNO)[CardNo],ISNULL(CIFID,'')[CIFID],ISNULL(AccountNo,'')[AccountNo],updatedDate,'Processed'[ReportType] FROM CardRePINRequest with (Nolock)
where fileName=@FileName and updated=1 and processed=1 and rejected=0 and RequestedDate>@Datetime and  switchRespCode='00'
 
 
 SELECT DBO.ufn_DecryptPAN(CARDNO)[CardNo],ISNULL(CIFID,'')[CIFID],ISNULL(AccountNo,'')[AccountNo],updatedDate,b.Description[Reason],'Rejected'[ReportType]
 FROM CardRePINRequest a with (Nolock) 
 left join TblResponse b  with (nolock) on a.switchrespCode=b.Code
 where fileName=@FileName and processed=1 and rejected=0 
 and RequestedDate>@Datetime and  ISNULL(switchRespCode,'')!='00'
 Union all
 SELECT dbo.ufn_DecryptPAN(cardNo)[CardNo],
 CASE WHEN ISNULL(CIFID,'')='Error In Record' THEN '' ELSE CIFID END CIFID ,
 CASE WHEN ISNULL(AccountNo,'')='Error In Record' THEN '' ELSE AccountNo END AccountNo ,
 updatedDate,Reason,'Rejected'[ReportType] FROM CardRePINRequest WHERE FILENAME=@Filename AND Rejected=1
END


GO
