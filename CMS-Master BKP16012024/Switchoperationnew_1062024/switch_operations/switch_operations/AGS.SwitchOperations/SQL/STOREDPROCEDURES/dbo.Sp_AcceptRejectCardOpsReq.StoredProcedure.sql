USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_AcceptRejectCardOpsReq]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_AcceptRejectCardOpsReq]	
	@CheckerID BIGINT,
	@RequestTypeID INT=0,
	@Remark VARCHAR(200)='',
	@ReqID VARCHAR(500)='',
	@FormStatusID int,	
	@BankID Varchar(200)=1,
	@SystemID Varchar(200)=1

AS
BEGIN
	Begin Transaction  
	Begin Try    

	DECLARE @IssuerNo Int=23

	Select @IssuerNo=BankCode From TblBanks WITH(NOLOCK) Where ID=@BankID
	  
	 declare @tbl table(value varchar(200),RowID int)
	 	 insert into @tbl (value,RowID)(SELECT VALUE,RowID FROM dbo.fnSplit(@ReqID,','))
	 
	IF EXISTS( Select COUNT(1) fROM @tbl)
	 BEGIN
	 --ACCEPT REQ
	    IF(@FormStatusID=1)
		BEGIN

		  ----SWITCH card hotlist/ Card status Update Call
		   
			DECLARE @IntCurID int
			DECLARE @IntMaxCount int
			DECLARE @IntRowID int=1

			Declare @IntpriOutput int=1 ,@StrpriOutputDesc Varchar(500)='' 

			  DECLARE	@RPAN			VarChar(Max)			  
			  , @HRC VARCHAR(5)
			  ,@Remarks	VarChar(500)=''
			  ,	@Login			Varchar(100) = 'AGS_App'
			  

			 SELECT @IntMaxCount= Count(1) from @tbl t
			 INNER JOIN TblCardOpsRequestLog ct ON t.value=ct.ID
			 INNER JOIN CardRPAN CP ON ct.CardRPANID=CP.ID
			 where ct.SystemID=@SystemID AND ct.BankID=@BankID


		 IF(@IntMaxCount>0)
		 BEGIN
		  
		 While (@IntRowID<=@IntMaxCount)
			BEGIN
			   print @IntRowID
			   print @IntMaxCount
			  SET @IntpriOutput=1
			  --Get current ID  to  update
			 SELECT @IntCurID=value from @tbl WHERE RowID=@IntRowID							 

				 SELECT 	@RPAN=CP.EncPAN,@HRC=CL.SwitchHRC
				  from TblCardOpsRequestLog CL WITH(NOLOCK)
				  INNER JOIN CARDRPAN CP WITH(NOLOCK) ON CL.CardRPANID=CP.ID 
				  Where CL.ID= @IntCurID And CL.SystemID=@SystemID AND CL.BankID=@BankID

	
				  --*****************  Switch Sp Card Hotlist Call *******************
				  BEGIN Try				 
					exec  AGSS1RT.postcard.dbo.[usp_hotlistCard_AGS] @PAN= @RPAN,@IssuerNo=@IssuerNo,@HRC=@HRC ,@Login=@Login,@IntpriOutput=@IntpriOutput output,@StrpriOutputDesc =@StrpriOutputDesc OUTPUT				 				 
			      END TRY
					BEGIN CATCH				
						SET @IntpriOutput=1;
						SET @StrpriOutputDesc='Error occurs';								
					END CATCH				 
					---- Successful Card hotlist
				 IF(@IntpriOutput=0)
				  BEGIN
		              UPDATE TblCardOpsRequestLog SET FormStatusID=@FormStatusID ,CheckerID=@CheckerID ,CheckedDate=GETDATE(),IsSuccess=1,SwitchResponse=@StrpriOutputDesc WHERE ID=@IntCurID AND SystemID=@SystemID AND BankID=@BankID
				  END
				  ELSE
				  BEGIN
		            UPDATE TblCardOpsRequestLog SET FormStatusID=0 ,IsSuccess=0 ,SwitchResponse=@StrpriOutputDesc WHERE ID=@IntCurID AND SystemID=@SystemID AND BankID=@BankID
				  END
				SET @IntRowID +=1				
			END
		 END		  		

		SELECT  Co.ID, C.bankCustID AS [CustomerID],dbo.ufn_DecryptPAN(Pan.DecPAN) AS [CardNo],(C.FirstName +' '+ISNULL(LastName,''))  AS [CustomerName]
			,fs.FormStatus AS [Status],fs.FormStatusID ,cr.RequestType,Case WHEN ISNULL(Co.ISSuccess,0)=1 THEN 'Success' ELSE 'Failed' END AS[Response] 
			,co.RequestTypeID
			From TblCardOpsRequestLog Co WITH(NOLOCK)
			INNER JOIN TblCustomersDetails C WITH(NOLOCK)  ON C.CustomerID=Co.CustomerID
			INNER JOIN CardRPAN Pan WITH(NOLOCK) ON co.CardRPANID=pan.ID
			INNER JOIN TblFormStatus fs WITH(NOLOCK) ON ISNULL(co.FormStatusID,0)=fs.FormStatusID
			INNER JOIN TblCardRequests CR WITH(NOLOCK) ON Co.RequestTypeID=Cr.ID
			INNER JOIN @tbl temp ON temp.Value=co.ID	
			where co.SystemID=@SystemID and co.BankID=@BankID	 
		END
		--REJECT REQUEST	    
		ELSE IF(@FormStatusID=2)
		BEGIN
		  UPDATE TblCardOpsRequestLog SET FormStatusID=@FormStatusID ,CheckerID=@CheckerID ,CheckedDate=GETDATE(),Remark=@Remark,IsSuccess=1,SwitchResponse='' WHERE ID IN(SELECT value FROM @tbl)  AND SystemID=@SystemID AND BankID=@BankID

		  SELECT  Co.ID, C.bankcustID AS [CustomerID],dbo.ufn_DecryptPAN(Pan.DecPAN) AS [CardNo],(C.FirstName +' '+ISNULL(LastName,'')) AS [CustomerName]
			,fs.FormStatus AS [Status],fs.FormStatusID ,cr.RequestType,Case WHEN ISNULL(Co.ISSuccess,0)=1 THEN 'Success' ELSE 'Failed' END AS[Response] 
			,co.RequestTypeID
			From TblCardOpsRequestLog Co WITH(NOLOCK)
			INNER JOIN TblCustomersDetails C WITH(NOLOCK)  ON C.CustomerID=Co.CustomerID
			INNER JOIN CardRPAN Pan WITH(NOLOCK) ON co.CardRPANID=pan.ID
			INNER JOIN TblFormStatus fs WITH(NOLOCK) ON ISNULL(co.FormStatusID,0)=fs.FormStatusID
			INNER JOIN TblCardRequests CR WITH(NOLOCK) ON Co.RequestTypeID=Cr.ID
			INNER JOIN @tbl temp ON temp.Value=co.ID	
			where co.SystemID=@SystemID and co.BankID=@BankID		 
		END	  	
     END
		

	COMMIT TRANSACTION;    
    End Try  
	 BEGIN CATCH 
	 RollBACK TRANSACTION; 
		
	  
			INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
		    
	END CATCH;  	
END

GO
