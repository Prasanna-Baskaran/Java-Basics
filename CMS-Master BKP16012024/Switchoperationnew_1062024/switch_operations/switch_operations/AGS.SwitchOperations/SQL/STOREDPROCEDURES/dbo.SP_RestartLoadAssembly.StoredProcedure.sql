USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SP_RestartLoadAssembly]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[SP_RestartLoadAssembly]
@IP varchar(50)
AS
BEGIN
	Update TblMassModule Set  [Status]=1 Where Ip=@IP and status=2 and EnableState=1
	--select * from TblMassModule where IP='10.10.0.88' and status=2
END

GO
