USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[spUpdateUserRights]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[spUpdateUserRights]
@userrights UserRights readonly,
@userid varchar(20),
@SystemID Varchar(200)='2',
@BankID VARCHAR(200)='1'
as
begin
declare @ResponseCode int=0, @ResponseDesc varchar(max) ='Test' 
begin transaction
begin try

--SELECT * INTO TblTempUserRights FROM @userrights 
SELECT * INTO #userrights FROM @userrights 
Update #userrights Set Accesscaption=NULL where AccessCaption=''

DECLARE @SQL VARCHAR(max)=''
DECLARE @UpdateString VARCHAR(MAX)=NUll
 Select  @UpdateString= (coalesce(@UpdateString + ',','') + 't.'+ ltrim(rtrim(OptionNeumonic)) +'=i.' + ltrim(rtrim(OptionNeumonic)))  from #userrights

--Get Option neumonic
DECLARE @OptionNeumonic VARCHAR(MAX)
Select  @OptionNeumonic=dbo.FunGetOptionNeumonic()
--Update query
	SET @SQL='
	declare @ResponseCode int, @ResponseDesc varchar(max)  

	--Pivot table
	Select * Into #UserrightsTemp from (
		select '+@UserID +' AS [UserID],'+@OptionNeumonic+'  from 
		(
		select RoleId AS [UserID],OptionNeumonic,AccessCaption from #userrights 		
		) AS abc
		PIVOT(
			max(accesscaption) for 	[OptionNeumonic] in ( '+@OptionNeumonic+' )
		)piv
	) Result

---Update rights	
		If Exists( Select 1 from TblUserManagement WITH(NOLOCK) Where UserID='+@UserID+' AND SystemID='+@SystemID+')
		BEGIN	

		Update t Set '+@UpdateString+'
				 FROM TblUserManagement t
		 INNER JOIN  #UserrightsTemp i On t.UserID=i.UserID
		 where t.UserID='+@UserID+'	 AND t.SystemID='+@SystemID+'
			--set @ResponseDesc =@@Rowcount ;
			set @ResponseCode = 0
			set @ResponseDesc =''User Rights is provided successfully'' ;
		
			
		END
--- Insert rights
		Else
		BEGIN
		  INSERT INTO TblUserManagement (UserID,'+@OptionNeumonic+',SystemID) 
		  SELECT UserID ,'+@OptionNeumonic+','+@SystemID+'
			from #UserrightsTemp where UserID='+@UserID+'

        	set @ResponseCode = 0
			set @ResponseDesc = ''User Rights is provided successfully'' ;

		END
			SELECT @ResponseCode as ResponseCode, @ResponseDesc as ResponseDesc
drop table #UserrightsTemp
drop table #userrights
'
exec(@SQL)



	commit transaction;
end try
begin catch
	rollback transaction;
	set @ResponseCode = 1
	set @ResponseDesc = 'User Rights is not set ';
	  INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
end catch
	SELECT @ResponseCode as ResponseCode, @ResponseDesc as ResponseDesc
end

GO
