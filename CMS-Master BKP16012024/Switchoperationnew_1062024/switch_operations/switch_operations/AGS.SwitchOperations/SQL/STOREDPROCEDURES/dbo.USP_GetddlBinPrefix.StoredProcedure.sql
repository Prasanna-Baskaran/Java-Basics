USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_GetddlBinPrefix]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_GetddlBinPrefix] --[USP_GetddlBinPrefix] 2
@bankID varchar (10)
AS 
BEGIN
SELECT distinct CardPrefix FROM TblBIN with (nolock) where BankID=@BankID
END

GO
