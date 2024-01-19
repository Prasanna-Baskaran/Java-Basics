USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_GetProcessDate]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Sp_GetProcessDate]
@ProcessDate datetime
as
begin
begin try
	select '0' as Code, 'Success' as [Description]
	select distinct BatchNo from tblCardProduction WITH(NOLOCK) where CONVERT(date,[Uploaded On]) >= CONVERT(date,@ProcessDate)
end try
begin catch
	select '1' as Code, ERROR_MESSAGE() as [Description]
	select TOP 0 BatchNo from tblCardProduction
	
end catch
end

GO
