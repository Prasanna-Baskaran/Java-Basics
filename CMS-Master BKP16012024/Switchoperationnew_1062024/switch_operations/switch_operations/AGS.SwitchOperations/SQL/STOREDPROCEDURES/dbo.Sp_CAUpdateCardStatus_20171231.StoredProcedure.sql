USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_CAUpdateCardStatus_20171231]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


Create PROCEDURE [dbo].[Sp_CAUpdateCardStatus_20171231] 
@UniqueID as Varchar(50),
@IntIssuerNo Int=0,
@StrOutPutCode Varchar(3) Output,
@StrOutputDesc Varchar(2000) Output

AS
/*Change Managment
created by : Prerna Patil
Created date: 26/07/2017
Created Reason: TO Check ProperPath and Move/Rname Old Files

*/ 
BEGIN
	Begin Try  
	Set @StrOutPutCode='900'
	Set @StrOutputDesc='Failed' 	 
	 
	
	Update Auth  Set CardGenStatus=Case When [CardStatus]='00' Then 1 Else 4 END,CardGenStatusRemark=[CardStatus] + '|' + [CardStatusRemark]	
	From TblAuthorizedCardLog Auth With(Nolock)
	Inner Join [TblCAProcessedCard] CAPCR With(nolock) On Auth.[CIF ID]=CAPCR.CardCustomerID
	Where UniqueID=@UniqueID And [IssuerNum]=@IntIssuerNo And  Auth.Date>=DATEADD(hh, -15, CreatedDate) And Isnull(CardGenStatus,0)=0

	--Insert the log into history
		Insert Into [TblCAProcessedCard_History] ([CAPRCode],[UniqueID],[CardCustomerID],[CardSequenceNum],[CardStatus],[CardStatusRemark],[CAPRCreatedDate],[IssuerNum])
		(SELECT [Code] ,[UniqueID],[CardCustomerID],[CardSequenceNum],[CardStatus],[CardStatusRemark],[CreatedDate],[IssuerNum]
		FROM [dbo].[TblCAProcessedCard] With(ReadPast)
		Where [IssuerNum]=@IntIssuerNo AND [UniqueID]<>@UniqueID)

		 If(@@rowcount>0)
		 Begin
			Delete FROM [dbo].[TblCAProcessedCard] Where [IssuerNum]=@IntIssuerNo AND [UniqueID]<>@UniqueID
		 END

LblResult: 
   
    End Try  
	 BEGIN CATCH 
	 --RollBACK TRANSACTION; 		
	  ExceptionErrorLog:
			INSERT INTO TblCardAutomationErrorLog(Function_Name,Error_Desc,Error_Date,ParameterList,IssuerNo)          
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE(),'IssuerNumber='+ convert(varchar(5),@IntIssuerNo),@IntIssuerNo
		      SET @StrOutPutCode='999'        
			 SET @StrOutputDesc='Unexpected error occurred [Sp_PreStandard]' 
	END CATCH; 
END

GO
