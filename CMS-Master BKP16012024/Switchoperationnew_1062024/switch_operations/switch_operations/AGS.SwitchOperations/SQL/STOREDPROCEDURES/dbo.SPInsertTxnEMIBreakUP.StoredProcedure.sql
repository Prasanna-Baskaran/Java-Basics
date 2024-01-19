USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SPInsertTxnEMIBreakUP]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Rahul>
-- Create date: <16-12-2016,>
-- Description:	<Insert Transaction EMI BreakUp ,>
-- =============================================
CREATE PROCEDURE [dbo].[SPInsertTxnEMIBreakUP]  -- [SPInsertTxnEMIBreakUP]'123865381537','',100,0,'13 DEC 2073 00:00:00','15 DEC 2073 00:00:00','55.50',1,1,0
@RRN varchar(12),
@NumberOfEMI INT,
@TxnAmount  NUmeric(16,2),
@PerEMIAmount Numeric(16,2),
@EMIStartDate DateTime,
@EMIEndDate DateTime,
@IntrestRate Varchar(10),
@UpdatedBy INT,
@CreatedBy INT,
@TxnAmtWithIntrest Numeric(16,2)
AS
BEGIN

BEGIN TRANSACTION
BEGIN TRY

	
	if NOT EXISTS(Select Code from EMIBreakUp WITH(NOLOCK) where retrieval_reference_nr=@RRN )
		Begin
			Insert into 
			EMIBreakUp(retrieval_reference_nr,NumberOfEMI,TxnAmount,PerEMIAmount,EMIStartDate,EMIEndDate,IntrestRate,UpdatedBy,CreatedBy,TxnAmountWithIntrest)
				values(@RRN,@NumberOfEMI,@TxnAmount,@PerEMIAmount,@EMIStartDate,@EMIEndDate,@IntrestRate,@UpdatedBy,@CreatedBy,@TxnAmtWithIntrest)

			Select '00' As ResponseCode , 'EMI Apply Successfully' As ResponseMsg

		End
	ELSE
		BEGIN
		IF Exists ( Select Code from EMIBreakUp WITH(NOLOCK) where retrieval_reference_nr=@RRN  And ISnull(PayedAmount,0) = 0)
			BEGIN
				Update EMIBreakUp Set NumberOfEMI=@NumberOfEMI,PerEMIAmount=@PerEMIAmount,IntrestRate=@IntrestRate,TxnAmountWithIntrest=@TxnAmtWithIntrest,UpdatedBy=@UpdatedBy,
				UpdatedOn=GETDATE()
				where retrieval_reference_nr=@RRN
			
				Select '00' As ResponseCode , 'EMI Updated Successfully' As ResponseMsg

			END
		Else if Exists  ( Select Code from EMIBreakUp WITH(NOLOCK) where retrieval_reference_nr=@RRN  And ISnull(PayedAmount,0) <> 0)
			Begin
				select '01' As ResponseCode , 'Can Not Update EMI,Partially Paid EMI.' As ResponseMsg
			End
	END
COMMIT TRANSACTION;
END TRY

BEGIN CATCH

	ROLLBACK TRANSACTION;
	select '01' As ResponseCode , 'ERROR Occurs' As ResponseMsg

	INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()


END CATCH;

	
END

GO
