USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetTxnReport_20180329]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_GetTxnReport_20180329]
	@BankID VARCHAR(200)='',
	@SystemID VARCHAR(200)='',
	@FromDate datetime,
	@ToDate datetime,
	@CardNo Varchar(200)='',
	@CustID Varchar(200)=''
AS
/*
 EXEC [SP_GetTxnReport] 1,'2','1-2-2018','10-10-2018','6376762000060108483','UNKPMTUH000000001539'
*/

BEGIN
	DECLARE @IssuerNo VARCHAR(200),@SourceNode VARCHAR(8000), @SinkNode VARCHAR(8000)
	Select @IssuerNo=BankCode,@SourceNode=SourceNodes,@SinkNode=SinkNodes from TblBanks WITH(NOLOCK) WHERE ID=@BankID 	
	  Declare  @TblCardEncPan AS Table
	(EnCPan Varchar(100))
	--select GETDATE()
	--Declare @BankID VARCHAR(200)='1'
	--Declare @SystemID VARCHAR(200)='2'
	--Declare @FromDate datetime='2018-03-08 18:33:57.910'
	--Declare @ToDate datetime='2018-03-08 18:33:57.910'
	--Declare @CardNo Varchar(20)='6376762000060108483'
	--Declare @CustID Varchar(200)=''
	

	Declare @TxtReport table
	(
		TxnDate VarChar(10),
		TxnTime varchar(10),
		TranTypeDesc VarChar(500),
		tran_type varchar(100),
		CardNo Varchar(100),
		FromAccount varchar(100),
		ToAccount varchar(100),
		TerminalID  varchar(100),
		TraceNo  varchar(100),
		RRN  varchar(20),
		STAN  varchar(20),
		AmountReq Numeric(18,2),
		AmountRes  Numeric(18,2),
		Currency varchar(100),
		MerchantName  varchar(100),
		MerchantLocation  varchar(100),
		message_type varchar(100),
		rsp_code_req varchar(100),
		rsp_code_rsp varchar(100),
		pan_encrypted varchar(500)
	)

	insert into @TxtReport (TxnDate,TxnTime, tran_type,CardNo,FromAccount,ToAccount,TerminalID,TraceNo,RRN,STAN,AmountReq,AmountRes,Currency,message_type,rsp_code_req,rsp_code_rsp,MerchantName,MerchantLocation,pan_encrypted)
	exec [AGSOFFICE].postilion_office.dbo.[USP_GetPrabhuTxnDetails] @CardNo, @FromDate ,@ToDate
	--select * from @TxtReport
	Select  a.TxnDate, a.TxnTime,a.tran_type [Tran Type] ,
		b.[description] AS [TranTypeDesc]
		--,a.message_type AS [Msg Type]
		,a.CardNo,a.FromAccount,a.ToAccount,a.TerminalID,a.TraceNo,a.RRN,a.STAN,a.AmountReq,a.AmountRes,a.Currency,a.MerchantName,a.MerchantLocation
		--,ISNULL(f.Description,'') AS  [ResponseDesc]
		--,ISNULL(a.acquiring_inst_id_code,'') [Acquiring BIN]
		--,ISNULL(a.card_product,'') [Card Product]
		--, Case  when (source_node_name in('NIBLSrc','YSESrc','MobileAppSrc','ePrabhuSrc') AND left(a.pan,6) in (Select left(cardprefix,6) from tblbin WITH(NOLOCK))) THEN 'OnUs'
		--when (source_node_name not in('NIBLSrc','YSESrc','MobileAppSrc','ePrabhuSrc') AND left(a.pan,6) in (Select left(cardprefix,6) from tblbin WITH(NOLOCK))) THEN 'RemoteOnUs'
		--else 'OffUs' End [Source]
		--,a.source_node_name,a.sink_node_name
		From @TxtReport a
		INNER JOIN post_tran_types b WITH(NOLOCK) ON a.tran_type=b.code 
		INNER JOIN TblResponse f With (NoLock) On (case when a.message_type in ('0220') then a.rsp_code_req when ISNULL(a.rsp_code_req,'')='' then a.rsp_code_rsp Else a.rsp_code_rsp End)=f.code
		INNER JOIN CardOPAN cp WITH(NOLOCK) ON a.pan_encrypted=cp.EncPAN
		INNER JOIN CardRpan cr WITH(NOLOCK) ON dbo.ufn_DecryptPAN(cp.DecPAN)=  dbo.ufn_DecryptPAN(cr.DecPAN)
		INNER JOIN TblCustomersDetails cu WITH(NOLOCK) On cu.bankcustID=cr.Customer_id
		Where 
		------a.tran_postilion_originated=0 AND ISNULL(tran_type,'')<>'' AND 
		ISNULL(a.rsp_code_rsp,'')<>''
		--AND a.sink_node_name in (select Value from fnSplit(@SinkNode,','))) 
		--AND(((@FromDate='') AND(@ToDate='')) OR (convert(date,datetime_req,103) between convert(date,@FromDate,103) AND convert(date,@ToDate,103)))
		------AND ((@CardNo='') OR (RTRIM(LTRIM(@CardNo))=dbo.ufn_DecryptPAN(cp.DecPAN)))
		------AND (cp.IssuerNo=@IssuerNo)AND (cr.customer_id=@CustID)
		order by TxnDate desc

END

GO
