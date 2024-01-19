USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertPrepaidRevesalTranLog]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_InsertPrepaidRevesalTranLog]      
(       
 @StrReqISOString    VARCHAR(MAX)      
 ,@StrReqSwitchStan    VARCHAR(50)      
 ,@StrReqTerminalID    VARCHAR(10)      
 ,@StrReqTimeVal    VARCHAR(25)      
 ,@StrReqDateVal    VARCHAR(25)      
 ,@StrReqDateTimeVal   VARCHAR(25)      
 ,@StrReqSwtichRRN    VARCHAR(25)      
 ,@StrReversalRSPString   VARCHAR(MAX)      
 ,@StrReversedResponseCode  VARCHAR(10)      
 ,@StrReversedRRN    VARCHAR(25)      
 ,@ReversedAmount    VARCHAR(25)    
 ,@ReversalRemark  VARCHAR(500)  
 ,@Post_TxnID BIGINT
)      
AS      
BEGIN TRAN      
 BEGIN TRY      
UPDATE TblPrepaidTxn       
 SET REVERSE_RSP=@StrReversedResponseCode      
  ,REVERSE_Amt=@ReversedAmount      
  ,REVERSE_TXN_RRNO=@StrReversedRRN      
  ,REVERSE_Date=GETDATE() ,IsReverse=case when @StrReversedResponseCode='00' then 1 When @StrReversedResponseCode='100' THEN 2  else 0 end     
 WHERE TxnID=@Post_TxnID   
       
INSERT INTO TblPrepaidReversalTranDetail      
(      
 StrReqISOString        
  ,StrReqSwitchStan         
  ,StrReqTerminalID         
  ,StrReqDateVal       
  ,StrReqTimeVal          
  ,StrReqDateTimeVal         
  ,StrReqSwtichRRN         
  ,StrReversalRSPString        
  ,StrReversedResponseCode       
  ,StrReversedRRN         
  ,ReversedAmount  
  ,ReversalRemark  
  ,TxnID        
)      
VALUES      
(      
 @StrReqISOString        
 ,@StrReqSwitchStan          
 ,@StrReqTerminalID       
 ,@StrReqDateVal         
 ,@StrReqTimeVal          
 ,@StrReqDateTimeVal         
 ,@StrReqSwtichRRN          
 ,@StrReversalRSPString         
 ,@StrReversedResponseCode        
 ,@StrReversedRRN          
 ,@ReversedAmount  
  ,@ReversalRemark          
  ,@Post_TxnID        
)      



COMMIT TRAN      
END TRY      
BEGIN CATCH      
ROLLBACK TRAN    
 INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date,ParameterList)      
 VALUES(ERROR_PROCEDURE(),ERROR_MESSAGE(),GETDATE()      
 ,'@StrReqISOString='+ISNULL(@StrReqISOString,'')+'@StrReversalRSPString='+ISNULL(@StrReversalRSPString,''))      
END CATCH 
GO
