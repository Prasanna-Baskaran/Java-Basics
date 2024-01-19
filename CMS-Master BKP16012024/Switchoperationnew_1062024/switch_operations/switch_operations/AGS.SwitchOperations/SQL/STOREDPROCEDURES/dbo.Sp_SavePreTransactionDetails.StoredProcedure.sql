USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_SavePreTransactionDetails]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sp_SavePreTransactionDetails]
    @IntPara INT=0,
	@TxnAmount Decimal,
	@TxnTypeID int,
	@TxnSourceid smallint=1,
	@TerminalID VARCHAR(50)='',
	@INSTID VARCHAR(25)='',
	@FromCardNo VARCHAR(50)='',
	@ToCardNo VARCHAR(50)='',
	@ToAccountNumber VARCHAR(50)='',
	@InputStream VARCHAR(5000)			
AS
BEGIN
Begin Transaction  
Begin Try  
		Declare @StrPriOutput varchar(1)='1'		
			Declare @StrPriOutputDesc varchar(200)='Transaction details are not saved.'
			DECLARE @StrOutputTxnID VARCHAR(50)='0'
			DECLARE @TxnID BIGINT

			INSERT INTO TblPrepaidTxnTemp (INSTID,FromCardNo,ToCardNo,ToAccountNumber,TxnAmount
                                    ,TxnDate,TxnTypeID,TxnSourceid,TerminalID,InputStream)
                VALUES(@INSTID,@FromCardNo,@ToCardNo,@ToAccountNumber,@TxnAmount,GETDATE(),@TxnTypeID,@TxnSourceid,@TerminalID,@InputStream)

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



		Select @StrPriOutput As Code,@StrPriOutputDesc As [Description] ,@StrOutputTxnID AS[OutPutCode]

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
