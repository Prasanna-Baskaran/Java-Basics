USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_GetUnauthorisedDetailsForCard]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Sp_GetUnauthorisedDetailsForCard '22.12.2016'
CREATE PROCEDURE [dbo].[Sp_GetUnauthorisedDetailsForCard]
AS
BEGIN
select Code [Sr No], [CIF ID],[Customer Name],[Customer Preferred name],[Card Type and Subtype],[AC ID],
	[Address Line 1]+ ' ' +[Address Line 2] +' '+[Address Line 3] as [Address],[City],[Pin Code],
	[Country code],[BatchNo] from tblCardProduction where ISNULL(IsAuthorised,0)=0 AND ISNULL(Processed,0)=1 and ISNULL(Rejected,0)=0
END

GO
