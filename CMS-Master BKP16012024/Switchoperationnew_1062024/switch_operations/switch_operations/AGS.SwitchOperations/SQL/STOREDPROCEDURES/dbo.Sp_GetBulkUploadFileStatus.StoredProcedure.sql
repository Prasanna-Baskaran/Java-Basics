USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_GetBulkUploadFileStatus]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_GetBulkUploadFileStatus]  
  @BankID Varchar(200),
  @UploadDate datetime ,
  @SystemID Varchar(200)='' ,
  @FileTypeID Varchar(200)=''
AS
BEGIN
		Select f.[FileName],t.[Desc] As[FileType],convert(varchar(12),UploadedDate,103)  AS [UploadDate],ReconCount As [TotalRecords]
		,ISNULL(SuccessCount,'') AS [SuccessCount],ISNULL(FailedCount,'') AS [FailedCount] 
		,case when f.status=1 then'Processed' else fs.formstatus END as  [Status] 
		from TbldetFileUpload f WITH(NOLOCK)
		INNER JOIN TblMassFileUpload M WITH(NOLOCK) ON m.FileID=f.FileID
		INNER Join TblBanks b WITH(NOLOCK) ON M.Participant=b.bankcode
		INNER Join TblFormStatus fs with(Nolock) On ISNULL(f.[Status],0)=fs.FormStatusID
		INNER JOIN TblFileType t WITH(NOLOCK) ON t.ID=M.FileType
		where b.ID=@BankID AND (convert(date,@UploadDate,103)=CONVERT(date,f.UploadedDate,103))
END

GO
