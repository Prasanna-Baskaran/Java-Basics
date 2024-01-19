USE [SwitchOperations]
GO
/****** Object:  UserDefinedFunction [dbo].[udf_split]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create Function [dbo].[udf_split]
(
	@sInputList Varchar(8000), -- List of delimited items  
	@sDelimiter VarChar(8000) = ',' -- delimiter that separates items  
) 
Returns @List Table (item VarChar(8000))  
Begin  
 Declare @sItem VarChar(8000)  
   
 While CharIndex(@sDelimiter,@sInputList,0) <> 0  
  Begin  
   Select   
   @sItem=RTrim(LTrim(SubString(@sInputList,1,CharIndex(@sDelimiter,@sInputList,0)-1))),  
   @sInputList=RTrim(LTrim(SubString(@sInputList,CharIndex(@sDelimiter,@sInputList,0)+Len(@sDelimiter),Len(@sInputList))))  
   
   If Len(@sItem) > 0  
    Insert Into @List Select @sItem  
 End  
   
 If Len(@sInputList) > 0  
  Insert Into @List Select @sInputList -- Put the last item in  
  
 Return  
End



GO
