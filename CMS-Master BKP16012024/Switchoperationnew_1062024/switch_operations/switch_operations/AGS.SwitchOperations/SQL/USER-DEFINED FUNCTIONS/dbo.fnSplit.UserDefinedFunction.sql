USE [SwitchOperations]
GO
/****** Object:  UserDefinedFunction [dbo].[fnSplit]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fnSplit] 
   (  @List      varchar(MAX), 
      @Delimiter varchar(5)
   ) 
   RETURNS @TableOfValues table 
      (  RowID   smallint IDENTITY(1,1), 
         [Value] varchar(100) 
      ) 
AS 
   BEGIN
    
      DECLARE @LenString int 
 
      WHILE len( @List ) > 0 
         BEGIN 
         
            SELECT @LenString = 
               (CASE charindex( @Delimiter, @List ) 
                   WHEN 0 THEN len( @List ) 
                   ELSE ( charindex( @Delimiter, @List ) -1 )
                END
               ) 
                                
            INSERT INTO @TableOfValues 
               SELECT substring( @List, 1, @LenString )
                
            SELECT @List = 
               (CASE ( len( @List ) - @LenString ) 
                   WHEN 0 THEN '' 
                   ELSE right( @List, len( @List ) - @LenString - 1 ) 
                END
               ) 
         END
          
      RETURN 
      
   END

GO
