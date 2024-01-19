USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_GetPREFields]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_GetPREFields]
	@CardProgram VARCHAR(50)='',
	@IssuerNo VARCHAR(200)=''
AS
BEGIN
	SELECT B.PREFormat,Token,OutputPosition,Padding,PadChar,FixLength,replace(B.Switch_CardType,' ','') AS [CardType],StartPos,EndPos,Direction
	 ,B.ATMlimit,B.POSlimit --to get atmlimit and poslimit--9-11-17
	 from TblPREStandard P WITH(NOLOCK)
	 INNER JOIN TblBin B WITH(NOLOCk) ON B.CardProgram= P.CardProgram AND P.BinID=B.ID
	 INNER JOIN TblBanks BA WITH(NOLOCk) ON B.BankID=BA.ID
	 Where B.CardProgram=@CardProgram AND BA.BankCode=@IssuerNo
END

GO
