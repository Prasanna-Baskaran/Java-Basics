USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_GetPREFields_20170914]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[Sp_GetPREFields_20170914]
	@CardProgram VARCHAR(50)='',
	@IssuerNo VARCHAR(200)=''
AS
BEGIN
	SELECT B.PREFormat,Token,OutputPosition,Padding,PadChar,FixLength,replace(B.Switch_CardType,' ','') AS [CardType],StartPos,EndPos,Direction
	 from TblPREStandard P WITH(NOLOCK)
	 INNER JOIN TblBin B WITH(NOLOCk) ON B.CardProgram= P.CardProgram
	 INNER JOIN TblBanks BA WITH(NOLOCk) ON B.BankID=BA.ID
	 Where B.CardProgram=@CardProgram AND BA.BankCode=@IssuerNo
END

GO
