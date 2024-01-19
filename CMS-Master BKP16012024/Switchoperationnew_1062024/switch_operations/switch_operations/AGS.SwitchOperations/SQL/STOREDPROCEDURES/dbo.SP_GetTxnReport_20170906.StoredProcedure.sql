USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetTxnReport_20170906]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[SP_GetTxnReport_20170906]
	@BankID VARCHAR(200)='',
	@SystemID VARCHAR(200)='',
	@FromDate VARCHAR(200)='',
	@ToDate VARCHAR(200)='',
	@CardNo Varchar(200)='',
	@CustID Varchar(200)=''
AS
/*
 EXEC [SP_GetTxnReport] 4,'','','','6376762000060108483','UNKPMTUH000000001539'
*/

BEGIN
	DECLARE @IssuerNo VARCHAR(200),@SourceNode VARCHAR(8000), @SinkNode VARCHAR(8000)
	Select @IssuerNo=BankCode,@SourceNode=SourceNodes,@SinkNode=SinkNodes from TblBanks WITH(NOLOCK) WHERE ID=@BankID 	
	--select Value,@IssuerNo from fnSplit(@SourceNode,',')  
	 --select Value from fnSplit(@SourceNode,',')
	  Declare  @TblCardEncPan AS Table
			(EnCPan Varchar(100))

	Insert Into @TblCardEncPan
Select pan_encrypted From [post_tran_leg_internal] a with(nolock)
		INNER JOIN post_tran_types b WITH(NOLOCK) ON a.tran_type=b.code 
		INNER JOIN TblResponse f With (NoLock) On (case when a.message_type in ('0220') then a.rsp_code_req when ISNULL(a.rsp_code_req,'')='' then a.rsp_code_rsp Else a.rsp_code_rsp End)=f.code
		INNER JOIN CardOPAN cp WITH(NOLOCK) ON a.pan_encrypted=cp.EncPAN
		INNER JOIN CardRpan cr WITH(NOLOCK) ON dbo.ufn_DecryptPAN(cp.DecPAN)=  dbo.ufn_DecryptPAN(cr.DecPAN)
		INNER JOIN TblCustomersDetails cu WITH(NOLOCK) On cu.bankcustID=cr.Customer_id
		Where a.tran_postilion_originated=0 AND ISNULL(tran_type,'')<>'' 	
		AND ISNULL(a.rsp_code_rsp,'')<>''
		AND(a.source_node_name in(select Value from fnSplit(@SourceNode,','))) AND (a.sink_node_name in (select Value from fnSplit(@SinkNode,','))) 
		AND(((@FromDate='') AND(@ToDate='')) OR (convert(date,datetime_req,103) between convert(date,@FromDate,103) AND convert(date,@ToDate,103)))
		AND ((@CardNo='') OR (RTRIM(LTRIM(@CardNo))=dbo.ufn_DecryptPAN(cp.DecPAN)))
		AND (cp.IssuerNo=@IssuerNo)AND (cr.customer_id=@CustID)

	--IF not Exists( Select Top 1 1  From [post_tran_leg_internal] a with(nolock)
	--	INNER JOIN post_tran_types b WITH(NOLOCK) ON a.tran_type=b.code 
	--	INNER JOIN TblResponse f With (NoLock) On (case when a.message_type in ('0220') then a.rsp_code_req when ISNULL(a.rsp_code_req,'')='' then a.rsp_code_rsp Else a.rsp_code_rsp End)=f.code
	--	INNER JOIN CardOPAN cp WITH(NOLOCK) ON a.pan_encrypted=cp.EncPAN
	--	INNER JOIN CardRpan cr WITH(NOLOCK) ON dbo.ufn_DecryptPAN(cp.DecPAN)=  dbo.ufn_DecryptPAN(cr.DecPAN)
	--	INNER JOIN TblCustomersDetails cu WITH(NOLOCK) On cu.bankcustID=cr.Customer_id
	--	Where a.tran_postilion_originated=0 AND ISNULL(tran_type,'')<>'' 	
	--	AND ISNULL(a.rsp_code_rsp,'')<>''
	--	AND(a.source_node_name in(select Value from fnSplit(@SourceNode,','))) AND (a.sink_node_name in (select Value from fnSplit(@SinkNode,','))) 
	--	AND(((@FromDate='') AND(@ToDate='')) OR (convert(date,datetime_req,103) between convert(date,@FromDate,103) AND convert(date,@ToDate,103)))
	--	AND ((@CardNo='') OR (RTRIM(LTRIM(@CardNo))=dbo.ufn_DecryptPAN(cp.DecPAN)))
	--	AND (cp.IssuerNo=@IssuerNo)AND (cr.customer_id=@CustID))
	IF Exists(Select Top 1 1 From @TblCardEncPan)
		BEGIN
		  print @IssuerNo
		   
		   ----- Sink Office encrypted Pan into CardOPan Table
	
				BEGIN TRY
				Declare @StrHostSPID AS Varchar(50)=Host_Name()+ '_' +Convert(Varchar(10),@@SPID,103)
				declare @StrOutPutCode Varchar(3),@StrOutputDesc Varchar(2000)
				Delete From AGSOFFICE.postilion_office.dbo.TblTransactionPans Where HostSPID=@StrHostSPID
				Insert Into AGSOFFICE.postilion_office.dbo.TblTransactionPans 	
				Select Distinct @StrHostSPID,EnCPan,'' FROM @TblCardEncPan
				exec AGSOFFICE.postilion_office.dbo.[SP_CACardUtility_AGS] @IssuerNo, @StrHostSPID ,@StrOutPutCode Output,@StrOutputDesc Output
					--print @StrOutPutCode,@StrOutputDesc
				Delete From AGSOFFICE.postilion_office.dbo.TblTransactionPans Where HostSPID=@StrHostSPID	
				 END TRY
				  BEGIN CATCH
				  	INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date,ParameterList)                 
					 SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
					 ,'@BankID='+@BankID+' ,@FromDate='+@FromDate+' ,@ToDate='+@ToDate+',@CardNo='+dbo.[dbo].[FunMaskCardNo](@CardNo)+',@CustID='+@CustID
				  END CATCH

		END

	Select  REPLACE( CONVERT(VarChar(10),a.datetime_req,103),'/', '-') AS [TxnDate]
		,CONVERT(VARCHAR(8), a.datetime_req, 108) [TxnTime],
		--a.tran_type AS [Tran Type] ,
		b.[description] AS [TranTypeDesc]
		--,a.message_type AS [Msg Type]
		,a.pan AS[CardNo],ISNULL(from_account_id,'') AS[FromAccount]
		,ISNULL(a.to_account_id,'') AS[ToAccount]
		,ISNULL(a.terminal_id,'') AS [TerminalID]
		,ISNULL(a.system_trace_audit_nr,'') AS [TraceNo]
		,ISNULL(a.retrieval_reference_nr,'') [RRN]
		,CASE WHEN ISNUMERIC(RIGHT(CONVERT(VarChar(Max),structured_data_rsp),12))=1 And tran_type In ('00','01') THEN RIGHT(CONVERT(VarChar(Max),structured_data_rsp),12) ELSE '' END [STAN]
		,Cast(CONVERT(Numeric(18,2),ISNULL(tran_amount_req,0)/100)as varchar) [AmountReq]
		,Cast(CONVERT(Numeric(18,2),ISNULL(tran_amount_rsp,0)/100)as varchar) [AmountRes]
		,a.tran_currency_code AS [Currency]
		--, (case when a.message_type in ('0220') then a.rsp_code_req Else a.rsp_code_rsp End)[ResponseCode]		
		,ISNULL(a.card_acceptor_id_code,'') AS[MerchantName]
		,ISNULL(a.card_acceptor_name_loc,'') AS[MerchantLocation]
		,ISNULL(f.Description,'') AS  [ResponseDesc]
		--,ISNULL(a.acquiring_inst_id_code,'') [Acquiring BIN]
		--,ISNULL(a.card_product,'') [Card Product]
		--, Case  when (source_node_name in('NIBLSrc','YSESrc','MobileAppSrc','ePrabhuSrc') AND left(a.pan,6) in (Select left(cardprefix,6) from tblbin WITH(NOLOCK))) THEN 'OnUs'
		--when (source_node_name not in('NIBLSrc','YSESrc','MobileAppSrc','ePrabhuSrc') AND left(a.pan,6) in (Select left(cardprefix,6) from tblbin WITH(NOLOCK))) THEN 'RemoteOnUs'
		--else 'OffUs' End [Source]
		--,a.source_node_name,a.sink_node_name
		From [post_tran_leg_internal] a with(nolock)
		INNER JOIN post_tran_types b WITH(NOLOCK) ON a.tran_type=b.code 
		INNER JOIN TblResponse f With (NoLock) On (case when a.message_type in ('0220') then a.rsp_code_req when ISNULL(a.rsp_code_req,'')='' then a.rsp_code_rsp Else a.rsp_code_rsp End)=f.code
		INNER JOIN CardOPAN cp WITH(NOLOCK) ON a.pan_encrypted=cp.EncPAN
		INNER JOIN CardRpan cr WITH(NOLOCK) ON dbo.ufn_DecryptPAN(cp.DecPAN)=  dbo.ufn_DecryptPAN(cr.DecPAN)
		INNER JOIN TblCustomersDetails cu WITH(NOLOCK) On cu.bankcustID=cr.Customer_id
		Where a.tran_postilion_originated=0 AND ISNULL(tran_type,'')<>'' 	
		AND ISNULL(a.rsp_code_rsp,'')<>''
		AND(a.source_node_name in(select Value from fnSplit(@SourceNode,','))) AND (a.sink_node_name in (select Value from fnSplit(@SinkNode,','))) 
		AND(((@FromDate='') AND(@ToDate='')) OR (convert(date,datetime_req,103) between convert(date,@FromDate,103) AND convert(date,@ToDate,103)))
		AND ((@CardNo='') OR (RTRIM(LTRIM(@CardNo))=dbo.ufn_DecryptPAN(cp.DecPAN)))
		AND (cp.IssuerNo=@IssuerNo)AND (cr.customer_id=@CustID)
		order by convert(datetime,datetime_req,103) desc
END

GO
