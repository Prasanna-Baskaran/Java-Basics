USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[usp_insertLog]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Gufran Khan>
-- Create date: <Create Date,04-11-2017,>
-- Description:	<Description,,Insert the error log>
-- =============================================
Create PROCEDURE [dbo].[usp_insertLog] 
	-- Add the parameters for the stored procedure here
	@Exception varchar (max),
	@methodName varchar (max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO MassLog	(METHOD,EXCEPTION,DATE_INSERT) VALUES (@methodName,@Exception,GETDATE())
END

GO
