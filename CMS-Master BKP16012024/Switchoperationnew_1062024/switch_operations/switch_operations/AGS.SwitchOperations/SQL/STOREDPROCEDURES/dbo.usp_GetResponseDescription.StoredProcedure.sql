USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[usp_GetResponseDescription]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_GetResponseDescription]
	-- Add the parameters for the stored procedure here
	@ResponseCode varchar(4)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [Description] FROM [dbo].[TblResponse] WITH(NOLOCK) WHERE Code = @ResponseCode
END

GO
