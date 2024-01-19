Alter PROCEDURE [dbo].[Sp_InsertProcessBulkReissueFile]
  @CustInfo BulkReissueTblType Readonly,
  @UploadID VARCHAR(800)
AS
/************************************************************************
Object Name: 
Purpose: Insert, Validate ,process bulk Reissue File
Change History
Date         Changed By				Reason
06/10/2017  Diksha Walunj			Newly Developed


*************************************************************************/

BEGIN

--drop table bulkReissue
--select * into bulkReissue from @CustInfo

 Begin Transaction  
 Begin Try   
  DECLARE @strCode int=1,@strOutputDesc VARCHAR(800)='Invalid Data',@OutputCode Varchar(50)=''
  declare @Batchno varchar(20)= 'BATCH' + CONVERT(VARCHAR,GETDATE(),112) + REPLACE(CONVERT(VARCHAR(8),GETDATE(),114),':','')
 if exists(Select  Top 1 1 from @CustInfo)
 BEGIN

 DECLARE @FileID VARCHAR(800),@ReconCount int, @SuccessCount int,@FailedCount int,@FileName Varchar(500)
 SELECT @FileID=FileID,@FileName=[FileName] From TbldetFileUpload WITH(NOLOCK) WHERE ID=@UploadID

 Select @ReconCount= count(1),@FailedCount= count(1) from @CustInfo
  Update TbldetFileUpload SET [Status]=5,ReconCount=@ReconCount,StartDate=GETDATE() where ID=@UploadID

  DECLARE  @FileDataTemp AS Table
  (
  ID Numeric(28,0) Identity(1,1) NOT NULL,
   OldCardNumber VARCHAR(200)
  ,HoldRSPCode Varchar(200)
  ,NewBINPrefix VARCHAR(200)
  ,Remark VARCHAR(800)
  ,Reason VARCHAR(800)
  ,[Extra1] varchar(800)
  ,[Extra2] varchar(800)
  ,[Extra3] varchar(800) 
  )

  INSERT INTO @FileDataTemp (OldCardNumber ,  HoldRSPCode,  NewBINPrefix ,  Remark ,  Reason ,  Extra1 ,  Extra2 ,  Extra3 )
  SELECT OldCardNumber ,  HoldRSPCode,  NewBINPrefix ,  Remark ,  Reason ,  Extra1 ,  Extra2 ,  Extra3 FROM @CustInfo
  --SELECT OldCardNumber ,  HoldRSPCode,  NewBINPrefix ,  Remark ,  Reason ,  Extra1 ,  Extra2 ,  Extra3 FROM @CustInfo
  ---- Insert data in temporary table
  Select cu.ID,OldCardNumber ,  HoldRSPCode,  NewBINPrefix ,  Remark ,  Reason ,  Extra1 ,  Extra2 ,  Extra3 
  ,0 AS[IsRejected],convert(varchar(800),'') As [RejectReason],0 AS[IsProcessed],@FileID AS[FileID],@UploadID AS[UploadID] ,M.FileType AS [FileType],ft.ProcessiD As[ProcessiD]
  ,@Batchno AS[BatchNo],m.Participant AS[IssuerNO]
  INTO #BulkFileUpload
  from @FileDataTemp cu
  Left JOIN TblMassFileUpload m WITH(NOLOCK) ON m.fileID=@FileID
  LEFT JOIN TblFileType ft WITH(NOLOCk) ON ft.ID=m.FileType
  
  --- check data exists
  if exists(Select top 1 1 from #BulkFileUpload WITH(NOLOCK))
   BEGIN
  ----- Validate Bulk Data
		Update B SET  B.IsRejected=1,RejectReason='Invalid Old CardNumber'
		  from #BulkFileUpload B  With(NOLOCK)
		  LEFT JOIN CardRPAN CP WITH(NOLOCK) ON B.IssuerNo=CP.IssuerNo AND RTRIM(LTRIM(B.OldCardNumber))=dbo.ufn_decryptPAN(CP.DecPAN)
		  Where ISNULL(CP.ID,0)=0 AND B.IsRejected=0 And B.IsProcessed=0 and B.BatchNo=@Batchno

		 Update cp SET cp.IsRejected=1,cp.RejectReason='Invalid Hold Response Code'
		  From #BulkFileUpload cp With(NOLOCK)
		  INNER JOIN TblBulkFileConfig c WITH(NOLOCK) ON c.FieldName='HoldRSPCode'
		  Where  cp.IsRejected=0 And cp.IsProcessed=0 and cp.BatchNo=@Batchno
		  	And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.HoldRSPCode,''))))=0))
			OR(((( (ISNULL(c.IsNum,0)=1) AND(ISNUMERIC(ISNULL(cp.HoldRSPCode,''))=0)  )
			 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.HoldRSPCode)) like '%[^a-zA-Z ]%'))
			OR((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.HoldRSPCode)) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.HoldRSPCode,'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.HoldRSPCode,'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.HoldRSPCode))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			 OR ((ISnULL(c.IsDateValue,0)<>0) AND(dbo.FunCheckIsDate(cp.HoldRSPCode)=0))
			)AND (Len(RTRIM(LTRIM(cp.HoldRSPCode)))<>0) )
			 )

			 Update cp SET cp.IsRejected=1,cp.RejectReason='Invalid New Bin preffix'
			  From #BulkFileUpload cp With(NOLOCK)
			  LEFT JOIN TblBIN B WITH(NOLOCK) ON cp.NewBINPrefix=b.CardPrefix
			  Inner JOIN TblBanks Ba WITH(NOLOCK) ON cp.IssuerNo=Ba.BankCode AND B.BankID=Ba.ID
			  Where (ISNULL(B.ID,0)=0)   AND   Cp.IsRejected=0 And cp.IsProcessed=0 and cp.BatchNo=@Batchno 

		 --Update cp SET cp.IsRejected=1,cp.RejectReason='Invalid Remark'
		 -- From #BulkFileUpload cp With(NOLOCK)
		 -- INNER JOIN TblBulkFileConfig c WITH(NOLOCK) ON c.FieldName='Remark'
		 -- Where  cp.IsRejected=0 And cp.IsProcessed=0 and cp.BatchNo=@Batchno
		 -- 	And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.Remark,''))))=0))
			--OR(((( (ISNULL(c.IsNum,0)=1) AND(ISNUMERIC(ISNULL(cp.Remark,''))=0)  )
			-- OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.Remark)) like '%[^a-zA-Z ]%'))
			--OR((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.Remark)) like '%[^a-zA-Z0-9 ]%')))
			--OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.Remark,'')))) Between c.MinLen And c.MaxLen)))	  
			--OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.Remark,'')))))>c.MaxLen))
			--OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.Remark))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			-- OR ((ISnULL(c.IsDateValue,0)<>0) AND(dbo.FunCheckIsDate(cp.Remark)=0))
			--)AND (Len(RTRIM(LTRIM(cp.Remark)))<>0) )
			-- )

		Update cp Set cp.IsRejected=1, cp.RejectReason='Duplicate Card Request' 
		FROM #BulkFileUpload cp WITH(NOLOCK)
			 LEFT JOIN (Select Row_number() over (partition by OldCardNumber order by ID)RW ,*
			  from #BulkFileUpload With(NOLOCK) where IsRejected=0  ) A  On  A.RW=1 AND cp.OldCardNumber=A.OldCardNumber AND cp.ProcessiD=A.ProcessiD
			  Where  cp.IsRejected=0 And cp.IsProcessed=0 and cp.BatchNo=@Batchno AND cp.ID>A.ID

         	Update cp Set cp.IsRejected=1, cp.RejectReason='Duplicate Card Request/Card already issued ' 
			 FROM #BulkFileUpload cp WITH(NOLOCK)
			 INNER JOIN CardRPAN P WITH(NOLOCK) ON  P.IssuerNo=CP.IssuerNo AND RTRIM(LTRIM(cp.OldCardNumber))=dbo.ufn_decryptPAN(P.DecPAN)
			 LEFT JOIN TblCardGenRequest CR WITH(NOLOCK) ON P.EncPAN=CR.oldpan AND cp.ProcessiD=CR.ProcessiD
			 LEFT JOIN TblCardGenRequest_History Au WITH(NOLOCK) ON P.EncPAN=Au.oldpan AND cp.ProcessiD=Au.ProcessiD
			  Where  cp.IsRejected=0 And cp.IsProcessed=0 and cp.BatchNo=@Batchno and (ISNULL(CR.ID,0)<>0 OR ISNULL(Au.ID,0)<>0)

   --      Update cp Set cp.IsRejected=1, cp.RejectReason='Duplicate Card Request /Card already issued' 
			--FROM #BulkFileUpload cp WITH(NOLOCK)
			-- LEFT JOIN Tblbu
			--  Where  cp.IsRejected=0 And cp.IsProcessed=0 and cp.BatchNo=@Batchno AND cp.ID>A.ID

			Update cp SET cp.IsRejected=0,cp.IsProcessed=1
			From #BulkFileUpload cp With(NOLOCK)
		    Where  cp.IsRejected=0 And cp.IsProcessed=0 and cp.BatchNo=@Batchno
			
		---- Save fileUpload  record Logs
		   INSERT INTO TblBulkFileUploadHistory ( UploadID,FileID,FileType,OldCardNumber,HoldRSPCode,NewBINPrefix
						,Remark,Extra1,Extra2,Extra3,ProcessID,IsRejected,RejectReason,IsProcessed,BatchNo,[Date])
						SELECT UploadID,FileID,FileType,OldCardNumber,HoldRSPCode,NewBINPrefix
						 ,Remark,Extra1,Extra2,Extra3,ProcessiD,IsRejected,RejectReason,IsProcessed,BatchNo,GETDATE()
						  FROM #BulkFileUpload WITH(NOLOCK)
                         where BatchNo=@Batchno AND FileID=@FileID AND UploadID=@UploadID

			------- Insert validated records for card reissue process			

			INSERT INTO TblErrorDetail values ('TEST','Process Start',getdate(),'')                 
			INSERT INTO TblCardGenRequest (CustomerID,OldCardRPANID,NewBinPrefix,HoldRSPCode,Remark,UploadFileName,BankID,SystemID,ProcessID,UploadID,
			FormStatusID,CreatedDate,OldPan)
			Select p.customer_id,P.ID,cp.NewBINPrefix,cp.HoldRSPCode,cp.Remark,@FileName, B.ID,B.SystemID, cp.ProcessiD,cp.UploadID,0,GETDATE(),
			p.EncPAN
				From #BulkFileUpload cp With(NOLOCK)			
				LEFT JOIN TblBanks B WITH(NOLOCK) ON cp.IssuerNO=B.BankCode
				LEFT JOIN CardRPAN P With(NOLOCK) ON P.IssuerNo=cp.IssuerNO and Dbo.ufn_DecryptPAN(P.DecPAN)=RTRIM(LTRIM(cp.OldCardNumber))  
				Where  cp.IsRejected=0 And cp.IsProcessed=1 and cp.BatchNo=@Batchno

		
		     INSERT INTO TblErrorDetail values ('TEST','Process complete',getdate(),'')                 				

			 SELECT 0 AS [Code],'Success' AS [OutputDescription],''AS [OutPutCode]
				
			 Select (ISNULL(OldCardNumber,'')+'|'+ISNULL(HoldRSPCode,'')+'|'+ISNULL(NewBINPrefix,'')+'|'+ISNULL(Remark,'')+'|'
		     + Case when  ISNULL(Isrejected,0)=0 ANd ISNULL(Isprocessed,0)=1 THEN 'Sucess' WHEN Isnull(RejectReason,'')<>'' THEN 
		     RejectReason ELSE 'Failed' END  ) AS Result
			 from #BulkFileUpload WITH(NOLOCK)
			 where BatchNo=@Batchno AND FileID=@FileID AND UploadID=@UploadID

     ---Get failed Count
	   Select @FailedCount=COUNT(1) from #BulkFileUpload WITH(NOLOCK)  where BatchNo=@Batchno AND FileID=@FileID AND UploadID=@UploadID AND ISNULL(IsProcessed,0)<>1

	    ---Get Success Count
	   Select @SuccessCount= COUNT(1) from #BulkFileUpload WITH(NOLOCK)  where BatchNo=@Batchno AND FileID=@FileID AND UploadID=@UploadID AND ISNULL(IsProcessed,0)=1

	   ----- update fail,recoun,success count and process complete status
	   Update TbldetFileUpload SET FailedCount=@FailedCount,SuccessCount=@SuccessCount ,EndDate=GETDATE(),[Status]=1 where ID=@UploadID	   

	   Drop table #BulkFileUpload
	END
	---- Invalid data found to process
	ELSE
	 BEGIN
  ----failed processing
	    Update TbldetFileUpload SET EndDate=GETDATE(),[Status]=4 ,Error='Error' where ID=@UploadID
		 SELECT 1 AS [Code],@strOutputDesc AS [OutputDescription],''AS [OutPutCode]   
	END
END
 	COMMIT TRANSACTION;    
    End Try  
	 BEGIN CATCH 
	 RollBACK TRANSACTION; 		
	  ExceptionErrorLog:
	  ----failed processing
	    Update TbldetFileUpload SET EndDate=GETDATE(),[Status]=4 ,Error='Error' where ID=@UploadID

			INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date,ParameterList)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE(),'@UploadID='+@UploadID	
		  
		 
	  SELECT 1 AS [Code],'Error' AS [OutputDescription],''AS [OutPutCode]   
	END CATCH;  
END