USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_GetBatchHistory]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_GetBatchHistory]
@BatchDate datetime,
@BatchNo varchar(50)
AS
BEGIN
select [CIF ID],[Customer Name],[Customer Preferred name],[Card Type and Subtype],[AC ID],
	[Address Line 1]+ ' ' +[Address Line 2] +' '+[Address Line 3] as [Address],[City],[Pin Code],
	[Country code],[BatchNo], Case isnull(Rejected,1) when 1 then 'Yes' else 'No' end [Rejected] , Reason from tblCardProduction where BatchNo=@BatchNo --and CONVERT(date,ProcessedOn)=CONVERT(date,@BatchDate) 
END


GO
