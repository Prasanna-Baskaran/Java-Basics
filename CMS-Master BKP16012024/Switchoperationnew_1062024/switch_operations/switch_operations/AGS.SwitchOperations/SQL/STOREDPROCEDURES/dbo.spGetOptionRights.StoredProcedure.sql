USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[spGetOptionRights]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[spGetOptionRights]
@Role varchar(20),
@SystemID VARCHAR(200)='2'
as
begin

if(ISNUMERIC(@Role)='0')
begin
	set @Role = '0';
end


declare @Captions varchar(max)='' 
select @Captions+='[' + upper(CaptionName) + '],' from tblAccessCaption
SET @Captions = @Captions + '[ALL]' -- SUBSTRING(@Captions,0,LEN(@Captions))

--Get Option neumonic
DECLARE @OptionNeumonic VARCHAR(MAX)
Select  @OptionNeumonic=dbo.FunGetOptionNeumonic()




DECLARE @Query varchar(max) = '
--User  menu Access
SELECT
    *
	into #userrights
FROM
(
    SELECT
       '+@OptionNeumonic+'
    FROM
        TblUserManagement AS t
		where UserID='+@Role+'
		AND SystemID='+@SystemID+'
) AS SourceTable
UNPIVOT
(
    Value FOR  [OptionNeumonic#] IN
    ('+@OptionNeumonic+')
) AS unpvt



select * from (
--SubMenu
((select a.OptionNeumonic as [OptionNeumonic#], a.OptionName, upper(ac.CaptionName) CaptionName, case when ac.AccessCode not in (select value from fnSplit(isnull(CaptionButtons,''''),'','')) then 2 else Case when isnull(u.Value,'''')='''' then 0 else 1 end end as AccessCaption ,a.displayorder  AS [Order#],a.optionid AS [OptionID#],
Case WHEN ISNULL(a.OptionParentNeumonic,'''')='''' THEN ''0'' Else a.OptionParentNeumonic END   AS [ParentNeumonic#]
 from tbloptions a with(NOLOCK)
left outer join tblAccessCaption ac with(NOLOCK) on 1=1
left outer join #userrights u on u.[OptionNeumonic#]=a.OptionNeumonic and ac.AccessCode in (select Value from fnSplit(u.Value,'',''))
where a.active=1)  
UNION 
-- Main Menu 
(SELECT a.OptionNeumonic as [OptionNeumonic#],a.OptionName, upper(ac.CaptionName) CaptionName -- ,0 as AccessCaption 
 ,case when ac.AccessCode not in (select value from fnSplit(isnull(CaptionButtons,''''),'','')) then 2 else 
 Case when isnull(b.Value,'''')='''' then 0 else 1 end end   as AccessCaption  
,a.displayorder  AS [Order#],a.optionid AS [OptionID#],
Case WHEN ISNULL(a.OptionParentNeumonic,'''')='''' THEN ''0'' Else a.OptionParentNeumonic END   AS [ParentNeumonic#]
 from tbloptions a with(NOLOCK) 
left outer join tblAccessCaption ac with(NOLOCK) on 1=1
left outer JOIN(select  ISNULL(OptionParentNeumonic,'''') AS OptionParentNeumonic ,menu.Value As [Value]
			 from TblOptions opt with(nolock) 
			 INNER JOIN #userrights menu ON opt.OptionNeumonic=menu.OptionNeumonic#			 
			 ) b on a.OptionNeumonic=b.OptionParentNeumonic 
			 and ac.AccessCode in (select Value from fnSplit(b.Value,'',''))
  where a.active=1
 ) 
) )abc
pivot(
	max(accesscaption) for CaptionName in (' + @Captions + ')	
) piv

drop table #userrights
select * from tblAccessCaption 
'
print(@Query)
exec(@Query)



end

GO
