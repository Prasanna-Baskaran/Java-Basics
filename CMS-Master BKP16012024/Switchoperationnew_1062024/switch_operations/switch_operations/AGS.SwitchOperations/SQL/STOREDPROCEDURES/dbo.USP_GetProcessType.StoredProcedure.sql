USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_GetProcessType]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--select * from [dbo].[TblCACardGenProcessTypes]
CREATE procedure [dbo].[USP_GetProcessType]
as
begin 
select * from TblCACardGenProcessTypes where ProcessType='CardReissue'

end
--select * from TblCACardGenProcessTypes
--select * from TblCardGenRequest

GO
