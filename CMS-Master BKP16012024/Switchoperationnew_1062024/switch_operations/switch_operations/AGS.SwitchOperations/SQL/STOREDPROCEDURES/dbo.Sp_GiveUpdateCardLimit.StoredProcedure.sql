USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_GiveUpdateCardLimit]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[Sp_GiveUpdateCardLimit] 
CREATE PROCEDURE [dbo].[Sp_GiveUpdateCardLimit]
	@CustomerID bigint=0,
	@IntPara Smallint=NULL,
	@Limit decimal=NULL	,
	@CreatedByID bigint=0,	
	@AcceptedByID BIGINT =0,
	@CardNo VARCHAR(30)=NULL
AS
BEGIN
Begin Transaction  
	Begin Try    
	--********************************** Search Customer ************************************************
	IF(@IntPara=0)
	BEGIN
	  SELECT Cu.ApplicationFormNo,Cu.CustomerID, Cu.FirstName+' '+cu.MiddleName+' '+Cu.LastName as[Name],Oc.TotalAnnualIncome AS[Annual Income] 
	  ,Convert(numeric(18,0),Cl.Limit) AS [Card Limit]
	   ,ISNULL (Cl.CardLimitStatusID,0) AS [Limit Status ID]
	   ,Fs.FormStatus AS[Card Limit Status]	   
	   ,Cl.Reject_Remark AS[Remark]
	   FROM TblCustomersDetails Cu WITH(NOLOCK) 
	   INNER JOIN TblCustOccupationDtl Oc WITH(NOLOCK) ON Cu.CustomerID=Oc.CustomerID
	   LEFT JOIN TblCardLimit Cl WITH(NOLOCK) ON Cu.CustomerID=Cl.CustomerID
	   INNER JOIN TblFormStatus Fs WITH(NOLOCK) ON ISNULL(Cl.CardLimitStatusID,0)=fs.FormStatusID
	   WHERE Cu.CustomerID=@CustomerID  AND Cu.FormStatusID=1
	END

	------**************************** Card Limit Insert And Update ******************************
	  ELSE	IF(@IntPara=1)
		BEGIN	
				Declare @StrPriOutput varchar(1)='1'		
				Declare @StrPriOutputDesc varchar(200)='Card limit is not save '


				IF(@CustomerID <> 0 AND @Limit IS NOT NULL)
			     BEGIN
		    	--Modify Limit
				  IF EXISTS(Select 1 From TblCardLimit WITH(NOLOCK) WHERE CustomerID=@CustomerID)
				  BEGIN
				  ---Back  up previous limit
				   INSERT INTO TblCardLimit_History (CardLimitID,CustomerID,Limit,CreatedDate,CreatedByID,Modifieddate,ModifiedByID,CardLimitStatusID,CurrentDate,CheckerID,CheckedDate,Reject_Remark)
				    SELECT ID,CustomerID,Limit,CreatedDate,CreatedByID,Modifieddate,ModifiedByID,CardLimitStatusID ,GETDATE(),CheckerID,CheckedDate,Reject_Remark
				         FROM TblCardLimit WITH(NOLOCK) WHERE CustomerID=@CustomerID
                   IF(@@ROWCOUNT>0)
				   BEGIN
				      Update TblCardLimit SET Limit=@Limit,Modifieddate=GETDATE(),ModifiedByID=@CreatedByID,CardLimitStatusID=0,Reject_Remark='' Where CustomerID=@CustomerID 
						SET @StrPriOutput='0'
						SET @StrPriOutputDesc='Card limit is save successfully '
				   END
				   ELSE
				   BEGIN
				     SET @StrPriOutput='0'
					SET @StrPriOutputDesc='Card limit is not save'
				   END	

				  END
			--Add new Limit
				ELSE
				BEGIN
					 INSERT INTO TblCardLimit  (CustomerID,Limit,CreatedDate,CreatedByID,CardLimitStatusID) VALUES (@CustomerID,@Limit,GETDATE(),@CreatedByID,0)
					 IF(@@ROWCOUNT>0)
					 BEGIN					   
					   SET @StrPriOutput='0'
					   SET @StrPriOutputDesc='Card limit is save successfully '
					 END
					 ELSE
					 BEGIN
					  SET @StrPriOutput='1'
					   SET @StrPriOutputDesc='Card limit is not save successfully '
					 END			 			 
				END
			END
				ELSE
				BEGIN
				 Set @StrPriOutput ='1'		
				 Set @StrPriOutputDesc ='Card limit is not save '
				END

				Select @StrPriOutput As Code,@StrPriOutputDesc As [OutputDescription]
	   END

		COMMIT TRANSACTION;    
    End Try  
	 BEGIN CATCH 
	 RollBACK TRANSACTION; 
		SELECT 1  As Code,'Card limit is not save' As [OutputDescription]
	  
			INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
		    
	END CATCH;  	
END

GO
