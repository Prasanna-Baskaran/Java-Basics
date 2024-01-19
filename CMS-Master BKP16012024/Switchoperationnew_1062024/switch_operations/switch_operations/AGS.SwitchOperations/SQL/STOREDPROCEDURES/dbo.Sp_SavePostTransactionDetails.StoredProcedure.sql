USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_SavePostTransactionDetails]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sp_SavePostTransactionDetails]

    @TxnTempID BIGINt=0,	
    @SwitchRRN varchar(50)=NULL,
    @SwitchRSPCode varchar(50)=NULL,
    @SwitchProcsCode varchar(50)=NULL,
    @SwitchApiError varchar(50)=NULL,
    @SwitchAmount varchar(50)=NULL,
    @SwitchStan varchar(50)=NULL,
	@SwitchTime varchar(50)=NULL,
	@SwitchDate varchar(50)=NULL,
	@SwitchTeminalID	VARCHAR(50)=NULL,
	@SwitchTransmissionDate VARCHAR(50)=NULL

AS
BEGIN
Begin Transaction  
Begin Try  
		Declare @StrPriOutput varchar(1)='1'		
			Declare @StrPriOutputDesc varchar(200)='Transaction details are not saved.'
			DECLARE @StrOutputTxnID VARCHAR(50)='0'
			DECLARE @TxnID BIGINT
			IF(@TxnTempID>0)
			BEGIN
			 IF Exists(Select 1 from TblPrepaidTxnTemp WITH(NOLOCK) WHERE ID=@TxnTempID )
			 BEGIN
			 --Success/Fail
			     INSERT INTO TblPrepaidTxn (INSTID,FromCardNo,ToCardNo,ToAccountNumber,TxnAmount,TxnDate,TxnTypeID
                  ,TxnSourceid,InputStream,TerminalID
				  ,SwitchRRN,SwitchRSPCode,SwitchProcsCode,SwitchAmount,SwitchStan,SwitchTime,SwitchDate,SwitchTeminalID,SwitchTransmissionDate,TempID)
				  SELECT INSTID,FromCardNo,ToCardNo,ToAccountNumber,TxnAmount,TxnDate,TxnTypeID 
				  ,TxnSourceid,InputStream,TerminalID
				  ,@SwitchRRN,@SwitchRSPCode ,@SwitchProcsCode,@SwitchAmount,@SwitchStan ,@SwitchTime,@SwitchDate,@SwitchTeminalID,@SwitchTransmissionDate,@TxnTempID
				  from TblPrepaidTxnTemp WITH(NOLOCK) WHERE ID=@TxnTempID
				  
				  SELECT @TxnID= SCOPE_IDENTITY()
				  IF(@TxnID >0)
				  BEGIN
				   SET @StrPriOutput=0
				   SET @StrOutputTxnID=@TxnID
				   SET @StrPriOutputDesc='Transaction Details are saved'
				  END
				  ELSE
				  BEGIN
					SET @StrPriOutput=1
				   SET @StrPriOutputDesc='Transaction Details are not saved'
				  END  
			 END
			 END

		Select @StrPriOutput As Code,@StrPriOutputDesc As [Description],@StrOutputTxnID AS[OutPutCode]

COMMIT TRANSACTION;    
 End Try  
 BEGIN CATCH 
 RollBACK TRANSACTION; 
 
 SELECT 97  As Code,'Error occurs.' As [Description] ,@StrOutputTxnID AS[OutPutCode]
	  
			INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
		    
	END CATCH;  


END

GO
