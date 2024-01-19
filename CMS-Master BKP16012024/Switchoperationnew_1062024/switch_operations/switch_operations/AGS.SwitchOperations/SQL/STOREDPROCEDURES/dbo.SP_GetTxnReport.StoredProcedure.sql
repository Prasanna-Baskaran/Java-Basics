USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetTxnReport]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--select getdate()
 --exec SP_GetTxnReport @BankID=1,@FromDate='2018-02-26 00:00:00.000',@ToDate='2018-02-27 00:00:00.000',@CardNo='6376761000020018385'
CREATE PROCEDURE [dbo].[SP_GetTxnReport]
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
	--select top 1 @FromDate [Fromdate], @ToDate [Todate],getdate() [currentdate] into Delete1 from TblResponse
	--select * from delete1

	Declare @PanEncOffice varchar(1000)
	Declare @PanEncRealitime varchar(1000)
	Declare @CustomerId varchar(1000)
	DECLARE @IssuerNo VARCHAR(200),@SourceNode VARCHAR(8000), @SinkNode VARCHAR(8000)
	Select @IssuerNo=BankCode,@SourceNode=SourceNodes,@SinkNode=SinkNodes from TblBanks WITH(NOLOCK) WHERE ID=@BankID 	
	
	if(cast(@ToDate As date)=cast(GETDATE() As date))
	begin 
		set @ToDate=Getdate()
	end
	--select @IssuerNo
	select top 1 @PanEncOffice=EncPAN from CardOPAN with(nolock) where IssuerNo=@IssuerNo and dbo.ufn_DecryptPAN(DecPAN)=@CardNo
	select top 1 @PanEncRealitime=EncPAN,@CustomerId=customer_id from CardRPAN with(nolock) where IssuerNo=@IssuerNo and dbo.ufn_DecryptPAN(DecPAN)=@CardNo
	
	
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
		TxnDate VarChar(50),
		TxnTime varchar(50),
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
		pan_encrypted varchar(500),
		auth_id_rsp varchar(50),
		structured_data_rsp varchar(Max)
	)

	--select  @PanEncOffice, @FromDate ,@ToDate
	insert into @TxtReport (TxnDate,TxnTime, tran_type,CardNo,FromAccount,ToAccount,TerminalID,TraceNo,RRN,STAN,AmountReq,AmountRes,
	Currency,message_type,rsp_code_req,rsp_code_rsp,MerchantName,auth_id_rsp,structured_data_rsp )
	select  Convert(varchar(20),datetime_req,103), Convert(varchar(20),datetime_req,8), tran_type, pan_encrypted, from_account_id, to_account_id, terminal_id, 
	system_trace_audit_nr, retrieval_reference_nr, system_trace_audit_nr, (tran_amount_req/100)AS tran_amount_req , (tran_amount_rsp/100)As tran_amount_rsp, 
	tran_currency_code, message_type, rsp_code_req, rsp_code_rsp, card_acceptor_id_code,auth_id_rsp ,structured_data_rsp	
	--#post_tran_details
	from post_tran_leg_internal with(nolock) 
	--where datetime_req between Convert(varchar(11),@FromDate,105) and Convert(varchar(11),@ToDate,105) 
	where datetime_req between  @FromDate and  @ToDate
	and pan_encrypted = @PanEncOffice
	and tran_postilion_originated=0
	--exec [AGSOFFICE].postilion_office.dbo.[USP_GetPrabhuTxnDetails] @PanEnc, @FromDate ,@ToDate
	


	--select '@TxtReport',* from @TxtReport

		Select  a.TxnDate, a.TxnTime,a.tran_type [Tran Type] ,
		b.[description] AS [TranTypeDesc],
		--,a.message_type AS [Msg Type]		
		@cardno CardNo,
		--isnull(cu.customer_id,'') Customer_id 
		@CustomerId Customer_id 
		--,a.FromAccount,a.ToAccount
		,a.TerminalID,
		isnull(a.TraceNo,'') As TraceNo ,
		case when CHARINDEX('RRN212',structured_data_rsp)>0
		then  isnull( substring(structured_data_rsp,CHARINDEX('RRN212',structured_data_rsp)+6,12)  ,'')
		else ''
		end AS RRN,
		a.AmountReq,a.AmountRes,
		isnull(a.Currency,'') Currency
		--,a.MerchantName,a.MerchantLocation
		,f.Description [Response Desc]
		From @TxtReport a
		inner JOIN post_tran_types b WITH(NOLOCK) ON a.tran_type=b.code 
		inner JOIN TblResponse f With (NoLock) On 
		(case when a.message_type in ('0220') then a.rsp_code_req when ISNULL(a.rsp_code_req,'')='' then a.rsp_code_rsp Else a.rsp_code_rsp End)=f.code
		--inner JOIN CardOPAN cr WITH(NOLOCK) ON cr.EncPAN=@PanEnc --and EncPAN=a.pan_encrypted 
		inner JOIN CardOPAN cu WITH(NOLOCK) On cu.EncPAN=@PanEncOffice
		Where 
		--@CustID='MUNU001000173458' and
		------a.tran_postilion_originated=0 AND ISNULL(tran_type,'')<>'' AND 
		ISNULL(a.rsp_code_rsp,'')<>''
		order by TxnDate desc

END

GO
