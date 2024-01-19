USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SP_SaveBulkCustomerInfo]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[SP_SaveBulkCustomerInfo]
@CustInfo CustomerInfoType Readonly,
@UploadID VARCHAR(800)

AS
BEGIN
 Begin Transaction  
 Begin Try   
  DECLARE @strCode int=1,@strOutputDesc VARCHAR(800)='Error',@OutputCode Varchar(50)=''
declare @Batchno varchar(20)= 'BATCH' + CONVERT(VARCHAR,GETDATE(),112) + REPLACE(CONVERT(VARCHAR(8),GETDATE(),114),':','')
 if exists(Select  Top 1 1 from @CustInfo)
 BEGIN
 DECLARE @FileID VARCHAR(800),@ReconCount int, @SuccessCount int,@FailedCount int
 SELECT @FileID=FileID From TbldetFileUpload WITH(NOLOCK) WHERE ID=@UploadID

 Select @ReconCount= count(1),@FailedCount= count(1) from @CustInfo
  Update TbldetFileUpload SET [Status]=5,ReconCount=@ReconCount,StartDate=GETDATE() where ID=@UploadID

 ------- Insert Into CustInfo
 INSERT INTO TblBulkCustomerInfo (UploadID,FileID,FirstName,LastName,DOB,MobileNo,Email,Gender,Nationality,Passport_IdentiNo,IssueDate
,StatementDelivery,HouseNo,StreetName,City,District,AccountType,AccountNo,CardPrefix,ProductType,BatchNo,Processed,Rejected,NameOnCard,CreatedDate)
SELECT @UploadID,@FileID,FirstName,LastName,DOB,MobileNo,Email,Gender,Nationality
	,Passport_IdentiNo,IssueDate,StatementDelivery,HouseNo,StreetName,City,District,AccountType,AccountNo,CardPrefix,'',@Batchno,0,0,NameOnCard,Getdate()
	 from @CustInfo

	 ----- Validations
	 Update TblBulkCustomerInfo SET Rejected=1 ,Reason='Invalid FirstName' WHERE processed=0 AND Rejected=0 AND FileID=@FileID AND UploadID=@UploadID AND BatchNo=@Batchno AND((RTRIM(LTRIM(REPLACE(Isnull(FirstName,'0'),' ',''))) LIKE '%[^a-zA-Z ]%')OR( (RTRIM(LTRIM(FirstName)) like '%[^a-zA-Z0-9 ]%')) OR(LEN(LTRIM(RTRIM(FirstName)))=0) OR(LEN(LTRIM(RTRIM(FirstName)))>25))

	 Update TblBulkCustomerInfo SET Rejected=1 ,Reason='Invalid LastName' WHERE processed=0 AND Rejected=0 AND FileID=@FileID AND UploadID=@UploadID AND BatchNo=@Batchno AND((RTRIM(LTRIM(REPLACE(Isnull(LastName,'0'),' ',''))) LIKE '%[^a-zA-Z ]%')OR( (RTRIM(LTRIM(LastName)) like '%[^a-zA-Z0-9 ]%')) OR(LEN(LTRIM(RTRIM(LastName)))=0) OR(LEN(LTRIM(RTRIM(LastName)))>25))

	 Update TblBulkCustomerInfo SET Rejected=1 ,Reason='Invalid Date Of Birth' WHERE processed=0 AND Rejected=0 AND FileID=@FileID AND UploadID=@UploadID AND BatchNo=@Batchno AND ((ISNUMERIC(ISNULL([DOB],''))=0)  OR ((Len(LTrim(RTrim(IsNull([DOB],'')))))<>8) OR (dbo.FunCheckIsDate([DOB])=0))
	 Update TblBulkCustomerInfo SET Rejected=1 ,Reason='Invalid MobileNo' WHERE processed=0 AND Rejected=0 AND FileID=@FileID AND UploadID=@UploadID AND BatchNo=@Batchno AND((RTRIM(LTRIM(ISNULL(MobileNo,''))) Like '%[^0-9]%') Or (Len(LTrim(RTrim(ISNULL(MobileNo,''))))<>10))
	 Update TblBulkCustomerInfo SET Rejected=1 ,Reason='Invalid EmailID' WHERE processed=0 AND Rejected=0 AND FileID=@FileID AND UploadID=@UploadID AND BatchNo=@Batchno AND ((PatIndex('%;%',Email)>0) Or (PatIndex('%,%',Email)>0) Or (PatIndex('%@%.%',Email)=0) OR (LEN(RTRIM(LTRIM(Email)))>50) OR (LEN(RTRIM(LTRIM(Email)))=0))
	 Update TblBulkCustomerInfo SET Rejected=1 ,Reason='Invalid Gender' WHERE processed=0 AND Rejected=0 AND FileID=@FileID AND UploadID=@UploadID AND BatchNo=@Batchno AND((ISNUMERIC(RTRIM(LTRIM(Gender)))=0 ) OR (LEN(RTRIM(LTRIM(Gender)))<>1))
	 Update TblBulkCustomerInfo SET Rejected=1 ,Reason='Invalid Nationality' WHERE processed=0 AND Rejected=0 AND FileID=@FileID AND UploadID=@UploadID AND BatchNo=@Batchno AND ((RTRIM(LTRIM(REPLACE(Isnull(Nationality,'0'),' ',''))) LIKE '%[^a-zA-Z ]%')OR( (RTRIM(LTRIM(Nationality)) like '%[^a-zA-Z0-9 ]%')) OR(LEN(LTRIM(RTRIM(Nationality)))=0) OR(LEN(LTRIM(RTRIM(Nationality)))>15))
	 --Update TblBulkCustomerInfolog SET Rejected=1 ,Reason='Invalid Passport/IdentificationNo' WHERE processed=0 AND Rejected=0 AND BatchNo=@Batchno AND 
	Update b SET b.rejected=1,b.Reason=CASE WHEN ISNULL(cu.CustomerID,0)<>0 THEN 'Passport/IdentificationNo already exists' ELSE 'Invalid Passport/IdentificationNo'END 
	from TblBulkCustomerInfo b
	LEFT JOIN TblCustomersDetails cu WITH(NOLOCK) ON RTRIM(LTRIM(b.[Passport_IdentiNo]))=RTRIM(LTRIM(cu.PassportNo_CitizenShipNo))
	where  b.processed=0 AND b.Rejected=0 AND b.FileID=@FileID AND b.UploadID=@UploadID AND b.BatchNo=@Batchno AND ((ISNULL(cu.CustomerID,0)<>0)OR(LEN(RTRIM(LTRIM(b.[Passport_IdentiNo])))=0) OR ((LEN(RTRIM(LTRIM(b.[Passport_IdentiNo])))>20)))

	 Update TblBulkCustomerInfo SET Rejected=1 ,Reason='Invalid Issue Date' WHERE processed=0 AND Rejected=0 AND FileID=@FileID AND UploadID=@UploadID AND BatchNo=@Batchno AND ((ISNUMERIC(ISNULL(issuedate,''))=0)  OR ((Len(LTrim(RTrim(IsNull(issuedate,'')))))<>8) OR (dbo.FunCheckIsDate(issuedate)=0))
	 Update TblBulkCustomerInfo SET Rejected=1 ,Reason='Invalid Statement Delivery' WHERE processed=0 AND Rejected=0 AND FileID=@FileID AND UploadID=@UploadID AND BatchNo=@Batchno AND((ISNUMERIC(RTRIM(LTRIM(StatementDelivery)))=0 ) OR (LEN(RTRIM(LTRIM(StatementDelivery)))<>1)  OR((RTRIM(LTRIM(StatementDelivery))) not in('1','2','3')))
	  Update TblBulkCustomerInfo SET Rejected=1 ,Reason='Invalid House No' WHERE processed=0 AND Rejected=0 AND FileID=@FileID AND UploadID=@UploadID AND BatchNo=@Batchno AND((LEN(RTRIM(LTRIM(HouseNo)))=0 ) OR (LEN(RTRIM(LTRIM(houseNo)))>15) OR (RTRIM(LTRIM(houseNo)) like '%[^a-zA-Z0-9 ]%'))
	  Update TblBulkCustomerInfo SET Rejected=1 ,Reason='Invalid Street Name' WHERE processed=0 AND Rejected=0 AND FileID=@FileID AND UploadID=@UploadID AND BatchNo=@Batchno AND((LEN(RTRIM(LTRIM(StreetName)))=0 ) OR (LEN(RTRIM(LTRIM(StreetName)))>15) OR (RTRIM(LTRIM(StreetName)) like '%[^a-zA-Z0-9 ]%'))
	  Update TblBulkCustomerInfo SET Rejected=1 ,Reason='Invalid City' WHERE processed=0 AND Rejected=0 AND FileID=@FileID AND UploadID=@UploadID AND BatchNo=@Batchno AND((LEN(RTRIM(LTRIM(City)))=0 ) OR (LEN(RTRIM(LTRIM(City)))>15) OR (RTRIM(LTRIM(City)) like '%[^a-zA-Z0-9 ]%'))
	  Update TblBulkCustomerInfo SET Rejected=1 ,Reason='Invalid District' WHERE processed=0 AND Rejected=0 AND FileID=@FileID AND UploadID=@UploadID AND BatchNo=@Batchno AND((LEN(RTRIM(LTRIM(District)))=0 ) OR (LEN(RTRIM(LTRIM(District)))>15) OR (RTRIM(LTRIM(District)) like '%[^a-zA-Z0-9 ]%'))
	  Update TblBulkCustomerInfo SET Rejected=1 ,Reason='Invalid AccountType' WHERE processed=0 AND Rejected=0  AND FileID=@FileID AND UploadID=@UploadID AND BatchNo=@Batchno AND((LEN(RTRIM(LTRIM(AccountType)))<>1 ) OR (ISNUMERIC(RTRIM(LTRIM(AccountType)))=0) OR((RTRIM(LTRIM(AccountType))) not in('1','2')))
	  Update TblBulkCustomerInfo SET Rejected=1 ,Reason='Invalid AccountNo' WHERE processed=0 AND Rejected=0 AND FileID=@FileID AND UploadID=@UploadID AND BatchNo=@Batchno AND((LEN(RTRIM(LTRIM(AccountNo)))<7 ) OR (LEN(RTRIM(LTRIM(AccountNo)))>16 ) OR (ISNUMERIC(RTRIM(LTRIM(AccountNo)))=0))

	  UPDATE b SET b.Rejected=1,b.reason='Invalid CardPrefix' 
	  FROM  TblBulkCustomerInfo b with(NOLOCK)
	  LEFT JOIN TBlBin bi WITH(NOLOCK) ON RTRIM(LTRIM(b.cardprefix))=RTRIM(LTRIM(bi.cardprefix))
	  where ISNULL(bi.ID,0)=0 AND b.processed=0 AND b.Rejected=0 AND b.FileID=@FileID AND b.UploadID=@UploadID

	  Update TblBulkCustomerInfo SET Rejected=1 ,Reason='Invalid Name on Card' WHERE processed=0 AND Rejected=0 AND FileID=@FileID AND UploadID=@UploadID AND BatchNo=@Batchno AND((RTRIM(LTRIM(REPLACE(Isnull(NameOnCard,'0'),' ',''))) LIKE '%[^a-zA-Z ]%')OR( (RTRIM(LTRIM(NameOnCard)) like '%[^a-zA-Z0-9 ]%')) OR(LEN(LTRIM(RTRIM(NameOnCard)))=0) OR(LEN(LTRIM(RTRIM(NameOnCard)))>25))

	  ----------- validated records
	  Update TblBulkCustomerInfo SET Processed=1,reason='' Where BatchNo=@Batchno AND FileID=@FileID AND UploadID=@UploadID AND ISNULL(Rejected,0)=0 AND processed=0

	  ---- for successful validated records 
	  IF exists(SELECT top 1 1 FROM TblBulkCustomerInfo WITH(NOLOCK) where BatchNo=@Batchno AND FileID=@FileID AND UploadID=@UploadID AND ISNULL(Rejected,0)=0 AND Processed=1)
	  BEGIN
          DECLARE @maxCount int=0,@IntCount int =1
		  DECLARE @TblTem Table(RowID  int IDENTITY(1,1) NOT NULL,Value Int)
		  INSERT INTO @TblTem (Value) SELECT ID From TblBulkCustomerInfo WITH(NOLOCK) where BatchNo=@Batchno AND FileID=@FileID AND ISNULL(Rejected,0)=0 AND UploadID=@UploadID AND Processed=1 AND ISNULL(Authorized,0)=0

		  SELECT @maxCount=count(1) FROM TblBulkCustomerInfo WITH(NOLOCK) where BatchNo=@Batchno AND UploadID=@UploadID AND FileID=@FileID AND ISNULL(Rejected,0)=0 AND Processed=1 AND ISNULL(Authorized,0)=0

		 While (@maxCount>=@IntCount)
		 BEGIN
		 DECLARE @IntID Bigint
		   SELECT @IntID=Value From @TblTem where RowID=@IntCount
		   EXEC Sp_GetCustomerToProcess @IntID
		  SET @IntCount=@IntCount+1
		 END 		   		        
	  END

	  SELECT 0 AS [Code],'Success' AS [OutputDescription],''AS [OutPutCode]

	  SELECT (ISNULL(FirstName,'')+'|'+ISNULL(LastName,'')+'|'+ISNULL(DOB,'')+'|'+ISNULL(MobileNo,'')+'|'+ISNULL(Email,'')+'|'+ISNULL(Gender,'')+'|'+ISNULL(Nationality,'')+'|'+ISNULL(Passport_IdentiNo,'')
            +'|'+ISNULL(IssueDate,'')+'|'+ISNULL(StatementDelivery,'')+'|'+ISNULL(HouseNo,'')+'|'+ISNULL(StreetName,'')+'|'+ISNULL(City,'')+'|'+ISNULL(District,'')+'|'+ISNULL(AccountType,'')+'|'+ISNULL(AccountNo,'')+'|'+ISNULL(CardPrefix,'')+'|'+ISNULL(NameOnCard,'')+'|'
			+ Case WHEN ISNULL(Authorized,0)=1 THEN 'Success'  WHEN Isnull(Reason,'')<>'' THEN Reason ELSE 'Failed' END ) AS [Result]
	   FROM TblBulkCustomerInfo WITH(NOLOCK) where BatchNo=@Batchno AND FileID=@FileID AND UploadID=@UploadID

	   ---Get failed Count
	   Select @FailedCount=COUNT(1) from TblBulkCustomerInfo WITH(NOLOCK)  where BatchNo=@Batchno AND FileID=@FileID AND UploadID=@UploadID AND ISNULL(Authorized,0)<>1

	    ---Get Success Count
	   Select @SuccessCount= COUNT(1) from TblBulkCustomerInfo WITH(NOLOCK)  where BatchNo=@Batchno AND FileID=@FileID AND UploadID=@UploadID AND ISNULL(Authorized,0)=1

	   ----- update fail,recoun,success count and process complete status
	   Update TbldetFileUpload SET FailedCount=@FailedCount,SuccessCount=@SuccessCount ,EndDate=GETDATE(),[Status]=1 where ID=@UploadID

	   INSERT INTO TblBulkCustomerInfoLog (UploadID,FileID,FirstName,LastName,DOB,MobileNo,Email,Gender,Nationality,Passport_IdentiNo
		,IssueDate,StatementDelivery,HouseNo,StreetName,City,District,AccountType,AccountNo,CardPrefix,ProductType,NameOnCard,BatchNo,Rejected
		,Processed,Reason,Authorized,CreatedDate,CustomerID)
		SELECT UploadID,FileID,FirstName,LastName,DOB,MobileNo,Email,Gender,Nationality,Passport_IdentiNo
		,IssueDate,StatementDelivery,HouseNo,StreetName,City,District,AccountType,AccountNo,CardPrefix,ProductType,NameOnCard,BatchNo,Rejected
		,Processed,Reason,Authorized,GETDATE(),CustomerID From TblBulkCustomerInfo WITH(NOLOCK) where BatchNo=@BatchNo AND FileID=@FileID AND UploadID=@UploadID

		DELETE TblBulkCustomerInfo  where BatchNo=@BatchNo AND FileID=@FileID AND UploadID=@UploadID
 END
 	COMMIT TRANSACTION;    
    End Try  
	 BEGIN CATCH 
	 RollBACK TRANSACTION; 		
	  ExceptionErrorLog:
	  ----failed processing
	    Update TbldetFileUpload SET EndDate=GETDATE(),[Status]=4 where ID=@UploadID

			INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date,ParameterList)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE(),'@UploadID='+@UploadID	
		  
		 
	  SELECT 1 AS [Code],'Error' AS [OutputDescription],''AS [OutPutCode]   
	END CATCH;  
END

GO
