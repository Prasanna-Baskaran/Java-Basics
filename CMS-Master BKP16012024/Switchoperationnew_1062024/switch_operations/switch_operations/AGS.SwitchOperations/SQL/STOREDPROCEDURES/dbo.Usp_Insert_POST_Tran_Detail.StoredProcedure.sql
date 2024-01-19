USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Usp_Insert_POST_Tran_Detail]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Usp_Insert_POST_Tran_Detail]                                        
(                                        
 @StrCardNum    Varchar(50)                                        
 ,@StrStan   varchar(10)                                                                                                                   
 ,@DecTranAmt    numeric(18,2)   
 ,@IntTranType     varchar(20)
 ,@DTTranDate DateTime                                                                  
 ,@StrREMARK     varchar(500)                                        
 ,@StrTerminalID    varchar(20)                                        
 ,@StrISOString    varchar(Max) 
 ,@StrDRCR varchar(2) 
 ,@StrProcessCode [varchar](6)
 ,@StrSwitchAuthID [varchar](50)
 ,@StrSwitchRRN [varchar](25)
 ,@StrSwitchResp [varchar](2)
 ,@StrSwitchAmt [varchar](30)
 ,@IntTempID BigInt
 ,@IntpriOutput  SmallInt OutPut
 ,@IntpriOutputCode BigInt OutPut
 ,@StrpriOutputDesc Varchar(500) OutPut                                                                                                
)                                        
AS                                        
/*Change Management                                        
Created By: Prerna Patil                                       
Create Date: 20/12/2016                                        
Reason: insert data in Pre TranSaction Dump                                        
*/                                        
BEGIN                                        
 Set @IntpriOutput ='01'                                        
 Set @StrpriOutputDesc='Record not inserted properly in Post Table.';                                        
                                                                              
BEGIN TRAN                                        
 BEGIN TRY                                        
            
 INSERT INTO TblEPrabhuPostTranDtl                                        
 (                                        
  [CardNumber] ,[TranAmount]  ,[TranType]  ,[TranDate]  ,[Stan]  ,[TerminalID]  ,[ISOString] ,[DR/CR] ,[Remark] ,[ProcessingCode],[AuthID],[SwitchRRN],[SwitchRespCode],
 [SwitchAmount], [TempRefID], [CreatedDate]                                                                              
 )                                        
VALUES                                        
(                                        
  @StrCardNum  ,@DecTranAmt  ,@IntTranType  ,@DTTranDate  ,@StrStan  ,@StrTerminalID  ,@StrISOString ,@StrDRCR ,@StrREMARK ,@StrProcessCode,@StrSwitchAuthID,
  @StrSwitchRRN,@StrSwitchResp,@StrSwitchAmt,@IntTempID, Getdate()                                                                              
                                         
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
