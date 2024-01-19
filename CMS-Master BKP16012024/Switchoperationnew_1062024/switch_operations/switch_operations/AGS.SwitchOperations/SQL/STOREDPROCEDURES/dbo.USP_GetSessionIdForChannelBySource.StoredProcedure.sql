USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_GetSessionIdForChannelBySource]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create proc [dbo].[USP_GetSessionIdForChannelBySource]  --[USP_GetSessionIdForChannel] 1,'1C9D03CC-3351-48E0-B440-334325F74AAB'
(
--@Bankid int,
@SourceId varchar(max)
)
as

Begin

exec [AGSBAT].[CARD_API].dbo.USP_GetSessionIdForChannelBySource @SourceId
	
end
GO
