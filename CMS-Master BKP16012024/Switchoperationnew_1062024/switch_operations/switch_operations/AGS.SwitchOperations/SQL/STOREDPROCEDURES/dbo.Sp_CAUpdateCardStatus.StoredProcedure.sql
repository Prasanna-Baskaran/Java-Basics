USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_CAUpdateCardStatus]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_CAUpdateCardStatus] 
@UniqueID as Varchar(50),
@IntIssuerNo Int=0,
@StrProcessID Varchar(10)='0' ,  
@StrOutPutCode Varchar(3) Output,  
@StrOutputDesc Varchar(2000) Output

AS 
/************************************************************************
Object Name: 
Purpose: Update Card Gen status
Change History
Date         Changed By				Reason
26/07/2017  Prerna Patil			Newly Developed
04/10/2017  Diksha Walunj          ATPCM224: Modification: Remove left 0s then compare CIF ID 
											,StrProcessID i/p para  added for reissue card gen status update


*************************************************************************/


BEGIN
	Begin Try  
	Set @StrOutPutCode='900'
	Set @StrOutputDesc='Failed' 	 
	 
	 -- ATPCM224 : Diksha : 09/10/2017 :Start
	 -- for Reissue requests cardgenstatus update
	 if(@StrProcessID='6')
	 BEGIN
	    Update Auth  Set Auth.CardGenStatus=Case When [CardStatus]='00' Then 1 Else 4 END,Auth.CardGenStatusRemark=[CardStatus] + '|' + [CardStatusRemark]	
		From TblCardGenRequest_History Auth With(Nolock)	
		Inner Join [TblCAProcessedCard] CAPCR With(nolock) On Auth.CustomerID=CAPCR.CardCustomerID
		Where UniqueID=@UniqueID And [IssuerNum]=@IntIssuerNo And  Auth.Date>=DATEADD(hh, -15, CAPCR.CreatedDate) And Isnull(CardGenStatus,0)=0
	 END
	 ELSE
	 BEGIN
	 -- ATPCM224 : Diksha : 09/10/2017 :End
	 	
		Update Auth  Set Auth.CardGenStatus=Case When [CardStatus]='00' Then 1 Else 4 END,Auth.CardGenStatusRemark=[CardStatus] + '|' + [CardStatusRemark]	
		From TblAuthorizedCardLog Auth With(Nolock)	
		Inner Join [TblCAProcessedCard] CAPCR With(nolock) On dbo.FunRemoveLeftZero(Auth.[CIF ID])=dbo.FunRemoveLeftZero(CAPCR.CardCustomerID)  --//ATPCM224 Modification : Diksha Walunj: 04/10/2017: Modification Remove left 0s then compare CIF ID
		Where UniqueID=@UniqueID And [IssuerNum]=@IntIssuerNo And  Auth.Date>=DATEADD(hh, -15, CreatedDate) And Isnull(CardGenStatus,0)=0
	END
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
			 SET @StrOutputDesc='Unexpected error occurred [Sp_CAUpdateCardStatus]' 
	END CATCH; 
END

GO
