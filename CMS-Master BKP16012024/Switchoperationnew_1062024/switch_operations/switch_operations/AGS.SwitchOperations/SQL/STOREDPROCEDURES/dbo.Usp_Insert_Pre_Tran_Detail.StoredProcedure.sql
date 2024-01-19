USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Usp_Insert_Pre_Tran_Detail]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Usp_Insert_Pre_Tran_Detail]                                        
(                                        
 @StrCardNum    varchar(50)                                        
 ,@StrStan   varchar(10)                                                                                                                   
 ,@DecTranAmt    numeric(18,2)   
 ,@IntTranType     varchar(20)
 ,@DTTranDate DateTime                                                                  
 ,@StrREMARK     varchar(500)                                        
 ,@StrTerminalID    varchar(20)                                        
 ,@StrISOString    varchar(Max)  
 ,@IntpriOutput  SmallInt Output
 ,@IntpriOutputCode BigInt Output
 ,@StrpriOutputDesc Varchar(500) Output                                                                                               
)                                        
AS                                        
/*Change Management                                        
Created By: Prerna Patil                                       
Create Date: 20/12/2016                                        
Reason: insert data in Pre TranSaction Dump                                        
*/                                        
BEGIN                                        
 Set @IntpriOutput ='01'                                        
 Set @StrpriOutputDesc='Record not inserted properly in Pre Table.';                                        
                                                                              
BEGIN TRAN                                        
 BEGIN TRY                                        
     DECLARE @SenderMobileNo varchar(25)='',@recieverMobileNO varchar(25)=''                                                                       
            
 INSERT INTO TblEPrabhuPreTranDtl                                        
 (                                        
  [CardNumber] ,[TranAmount]  ,[TranType]  ,[TranDate]  ,[Stan]  ,[TerminalID]  ,[ISOString]  ,[Remark]  ,[CreatedDate]                                                                              
 )                                        
VALUES                                        
(                                        
  @StrCardNum  ,@DecTranAmt  ,@IntTranType  ,@DTTranDate  ,@StrStan  ,@StrTerminalID  ,@StrISOString  ,@StrREMARK  ,Getdate()                                                                              
                                         
 )                                                                 
  Set  @IntpriOutputCode=SCOPE_IDENTITY();                                      
 SET @IntpriOutput ='00';                            
 SET @StrpriOutputDesc ='Success';                                        
COMMIT TRAN                                        
END TRY                                        
BEGIN CATCH                                        
ROLLBACK TRAN                                        
 INSERT INTO TBLERRORDETAIL([Procedure_Name],Error_Desc,Error_Date,ParameterList)                                        
 VALUES(ERROR_PROCEDURE(),ERROR_MESSAGE(),GETDATE()                                        
 ,'@IntTranType= '+ISNULL(@IntTranType,'')+',@StrStan= '+ISNULL(@StrStan,'')+                                        
 ',@DecTranAmt='+CAST(ISNULL(@DecTranAmt,0) AS VARCHAR(20))+                                        
 ',@StrISOString= '+CAST(ISNULL(@StrISOString,0) AS VARCHAR(Max))                                     
 )                                        
                                         
 SET @IntpriOutput ='99'                                        
 SET @StrpriOutputDesc ='Error occured while inserting record in PRE table';                            
END CATCH                                        
                                       
END
GO
