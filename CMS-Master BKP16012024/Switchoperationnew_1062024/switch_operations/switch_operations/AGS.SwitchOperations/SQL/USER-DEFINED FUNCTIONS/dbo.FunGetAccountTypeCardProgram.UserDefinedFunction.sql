USE [SwitchOperations]
GO
/****** Object:  UserDefinedFunction [dbo].[FunGetAccountTypeCardProgram]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FunGetAccountTypeCardProgram]
(
     @IntPara int,
	 @SchemeCode VARCHAR(800),
	 @CardPrefix VARCHAR(800),
	 @Bank VARCHAR(200)	 	
)
RETURNS VARCHAR(800)
AS
BEGIN
	DECLARE @Result VARCHAR(800)

	
	---- For Getting AccountType

	IF(@IntPara=0)
	BEGIN
	  
		SELECT @Result=Case WHEN ISNULL(Ba.SchemeCodeBased,0)=1 THEN ISNULL(B.AccountType,'10') WHEN ISNULL(@SchemeCode,'')='' THEN '10' Else @SchemeCode END 
		from TblBIN B WITH(NOLOCK)
		INNER JOIN TblBanks Ba WITH(NOLOCK) ON B.BankID=Ba.ID
		where ((ISNULL(Ba.SchemeCodeBased,0)=0) AND (B.CardPrefix =@CardPrefix))
		OR((ISNULL(Ba.SchemeCodeBased,0)=1) AND (B.CardPrefix =@CardPrefix) AND (LTRIM(RTRIM(B.Switch_SchemeCode))=LTRIM(RTRIM(@SchemeCode))))
	END
	---- For Getting CardProgram
	ELSE
	BEGIN
		SELECT @Result=ISNULL(B.CardProgram,'') 
		from TblBIN B WITH(NOLOCK)
			INNER JOIN TblBanks Ba WITH(NOLOCK) ON B.BankID=Ba.ID
			where ((ISNULL(Ba.SchemeCodeBased,0)=0) AND (B.CardPrefix =@CardPrefix))
			OR((ISNULL(Ba.SchemeCodeBased,0)=1) AND (B.CardPrefix =@CardPrefix) AND (LTRIM(RTRIM(B.Switch_SchemeCode))=LTRIM(RTRIM(@SchemeCode))))
	END
	RETURN @Result
END


GO
