USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_CAGetAllCardGenProcess]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_CAGetAllCardGenProcess] --[Sp_CAGetAllCardGenProcess] 23
	@IssuerNo Varchar(200)

AS
/************************************************************************
Object Name: 
Purpose: Get All Card Generation Process Sequence wise 
Change History
Date         Changed By				Reason
23/09/2017 Diksha Walunj			Newly Developed


*************************************************************************/
BEGIN
	Select convert(varchar,S.ProcessID) AS[ProcessID],P.Filename from  TblCACardGenStatusLog S WITH(NOLOCK)
	INNER JOIN TblCACardGenProcessTypes P WITH(NOLOCK) ON S.ProcessID=P.ProcessID
	where S.IssuerNo=@IssuerNo
	--and s.ProcessID <>3
	order by Processid asc
	--order by status desc,seqno asc
END


GO
