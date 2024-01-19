USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SP_CheckMagstripCard]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_CheckMagstripCard]
	@CardProgram VARCHAR(200)
AS
BEGIN
	Select ISNULL(IsMagstrip,0) From TblBin WITH(NOLOCK)
	where RTRIM(LTRIM(CardProgram))=RTRIM(LTRIM(@CardProgram))
END

GO
