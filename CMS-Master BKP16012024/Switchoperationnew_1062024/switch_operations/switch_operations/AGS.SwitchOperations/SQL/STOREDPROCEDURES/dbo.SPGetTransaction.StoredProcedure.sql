USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SPGetTransaction]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Rahul>
-- Create date: <06-12-2016,>
-- Description:	<Get Transaction ,>
-- =============================================
CREATE PROCEDURE [dbo].[SPGetTransaction] 
@StrCardNo varchar(20),
@StrRRN Varchar(12),
@Amount numeric(18,2),
@Fromdate Datetime,
@ToDate Datetime,
@Flag int
AS
BEGIN
	
    Select top 1 '4748500100000999'[Card NUmber],'123865381537'[RRN],100 [Amount] ,'Stan'[Stan]

END

GO
