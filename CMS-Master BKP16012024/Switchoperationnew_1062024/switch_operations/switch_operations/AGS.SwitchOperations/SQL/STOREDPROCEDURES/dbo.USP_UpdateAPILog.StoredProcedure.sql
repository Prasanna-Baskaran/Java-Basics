USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdateAPILog]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[USP_UpdateAPILog]
(
	@ResponseData varchar(2000),
	@ScopeIdentity bigint
)
AS
/*CHANGE MANAGEMENT
CREATED BY: Prerna Patil
CREATED DATE: 10/04/2017
CREATED REASON: LOG ERROR DETAIL update
*/
BEGIN
BEGIN TRAN
DECLARE  @StrStatusCode char(2)
,@StrStatusDescription varchar(100)
BEGIN TRY
	UPDATE [TblAssemblyLog] SET
			ResponseData=@ResponseData,	ResponseDateTime=getdate()
			WHERE SRNO=@ScopeIdentity

	SET @StrStatusCode='00'
	SET @StrStatusDescription='SUCCESS'
	COMMIT TRAN
END TRY
BEGIN CATCH
	ROLLBACK TRAN
	SET @StrStatusCode='99'
	SET @StrStatusDescription='Unexpected Error Occurred while Updating Log.'
	DECLARE @StrProcedure_Name varchar(500),	@ErrorDetail varchar(1000),	@ParameterList varchar(2000)
	SET @StrProcedure_Name=ERROR_PROCEDURE()
	SET @ErrorDetail=ERROR_MESSAGE()
	SET @ParameterList='@ScopeIdentity='+cast(isnull(@ScopeIdentity,'') as varchar(15))+
						'@ResponseData='+ISNULL(@ResponseData,'')
	EXEC SPInsertErrorLog @StrProcedureName=@StrProcedure_Name,
								@StrErrorDesc=@ErrorDetail,@StrParameterList=@ParameterList
END CATCH
SELECT @StrStatusCode [StatusCode],@StrStatusDescription [Description]
END
GO
