USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_AuthoriseCustomerDetailsForCard]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Sp_AuthoriseCustomerDetailsForCard]
@CardNos varchar(max),
@UserId varchar(200),
@Bank varchar(200)=1,
@SystemID varchar(200)=1
as
begin
declare @code int = 0, @description as varchar(500) = 'SUCCESS'

begin tran
begin try
	SELECT @code AS [Code], @description [Description]
	
	update dbo.tblCardProduction set IsAuthorised=1, AuthorisedBy=@UserId where Code in (select Value from fnSplit(@CardNos,','))
	select Code [#Code],[CIF ID],[Customer Name],[Card Type and Subtype] as [CardPrefix],[AC ID] AS [AccountNo],
	[AC open date] AS [Date],[City],[Mobile phone number] AS [MobileNo],[Email id] AS [Email],
	--[Address Line 1]+ ' ' +[Address Line 2] +' '+[Address Line 3] as [Address],[City],
	convert(varchar(12),ProcessedOn,103) AS [Date], Case isnull(IsAuthorised,0) when 1 then 'Card request accepted' else 'Card request is not accepted' end [Status] 
	from dbo.tblCardProduction where Code in (select Value from fnSplit(@CardNos,','))
	  AND SystemID=@SystemID  AND Bank=@Bank
	commit tran
end try
begin catch
	rollback tran
	SELECT 1 AS [Code], ERROR_MESSAGE() [Description]

	INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
	SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+' Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
end catch
end

GO
