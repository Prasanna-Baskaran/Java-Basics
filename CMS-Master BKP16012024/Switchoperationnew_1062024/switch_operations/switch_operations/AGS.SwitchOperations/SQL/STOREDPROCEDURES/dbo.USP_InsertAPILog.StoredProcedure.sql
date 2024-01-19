USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_InsertAPILog]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[USP_InsertAPILog]
(
	@RequestData varchar(2000)
)
AS
/*CHANGE MANAGEMENT
CREATED BY: Prerna Patil
CREATED DATE: 10/04/2017
CREATED REASON: LOG ERROR DETAIL
*/
BEGIN
BEGIN TRAN
DECLARE  @StrStatusCode char(2)
,@StrStatusDescription varchar(100)
,@ScopeIdentity bigint
BEGIN TRY
	INSERT INTO [TblAssemblyLog]
	(
		RequestData,	RequestDateTime
	)
	VALUES
	(
		@RequestData,	GETDATE()
	)
	select @ScopeIdentity=SCOPE_IDENTITY()
	SET @StrStatusCode='00'
	SET @StrStatusDescription='SUCCESS'
	COMMIT TRAN
END TRY
BEGIN CATCH
	ROLLBACK TRAN
	SET @StrStatusCode='99'
	SET @StrStatusDescription='Unexpected Error Occurred while Inserting Log.'
	DECLARE @StrProcedure_Name varchar(500),	@ErrorDetail varchar(1000),	@ParameterList varchar(2000)
	SET @StrProcedure_Name=ERROR_PROCEDURE()
	SET @ErrorDetail=ERROR_MESSAGE()
	SET @ParameterList='@RequestData='+ISNULL(@RequestData,'')
	EXEC SPInsertErrorLog @StrProcedureName=@StrProcedure_Name,
								@StrErrorDesc=@ErrorDetail,@StrParameterList=@ParameterList
END CATCH
SELECT @StrStatusCode [StatusCode],@StrStatusDescription [Description],@ScopeIdentity [ScopeIdentity]
END



GO
