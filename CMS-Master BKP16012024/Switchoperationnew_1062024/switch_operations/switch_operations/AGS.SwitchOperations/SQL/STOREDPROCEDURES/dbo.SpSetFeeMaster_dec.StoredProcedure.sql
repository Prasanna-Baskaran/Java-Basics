USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SpSetFeeMaster_dec]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SpSetFeeMaster_dec] 

@code int

AS  
BEGIN  
     
		select Userdate,Holidayname from Tbl_UserCalendar where code=@code	
   
END  

GO
