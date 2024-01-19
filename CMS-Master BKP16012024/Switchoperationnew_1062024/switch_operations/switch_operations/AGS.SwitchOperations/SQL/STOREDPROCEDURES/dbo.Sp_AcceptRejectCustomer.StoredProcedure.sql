USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_AcceptRejectCustomer]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_AcceptRejectCustomer]
   @CustomerID BIGINT =0,
   @CheckerID BIGINT,
   @Checker_Date_NE	datetime=NULL,
   @Chacker_Date_IND	datetime=NULL,
   @FormStatusID int,
   @Remark varchar(50)='' ,
   @ReqCustID VARCHAR(800)=''
AS
BEGIN
Begin Transaction  
	Begin Try    


        declare @tbl table(value varchar(200),RowID int)
	 	 insert into @tbl (value,RowID)(SELECT VALUE,RowID FROM dbo.fnSplit(@ReqCustID,','))

	    
			 IF EXISTS(Select 1 from TblCustomersDetails C WITH(NOLOCK) INNER  JOIN @tbl t ON C.CustomerID= t.value)
			 BEGIN
			 --Formstatus 1 Accept request
				If(@FormStatusID=1)
				BEGIN

			  		Update TblCustomersDetails Set FormStatusID=1,CheckerID=@CheckerID,Checker_Date_IND=GETDATE(),Remark='',IsSuccess=1 WHERE CustomerID in (SELECT value From @tbl)


				 END
				 ELSE IF(@FormStatusID=2)
				 BEGIN
				   Update TblCustomersDetails Set FormStatusID=2,CheckerID=@CheckerID,Checker_Date_IND=GETDATE(),Remark=@Remark,IsSuccess=1 WHERE CustomerID in (SELECT value From @tbl)
				 END
			END
			
			SELECT DISTINCT Cu.bankcustID AS [CustomerID], Cu.FirstName+' '+cu.MiddleName+' '+Cu.LastName as[Name],
			  --Convert(VARCHAR(10),Cu.DOB_AD,120) AS [Date Of Birth],
     			  CASE WHEN (Convert(VARCHAR(10),Cu.DOB_AD,103))='1900-01-01' THEN NULL ELSE (Convert(VARCHAR(10),Cu.DOB_AD,103)) END AS [Date Of Birth],Cu.ApplicationFormNo AS[Application No],Cu.FormStatusID,s.FormStatus,ISNULL(cu.Remark,'') AS [Remark]
					, (Convert(VARCHAR(10),ISNULL(ModifiedDate_IND,Maker_Date_IND),103)) AS [Date],CASE WHEN ISNULL(Cu.IsSuccess,1)=1 THEN 'Success' ELSE 'Failed' END AS [Status]		
					From TblCustomersDetails Cu WITH(NOLOCK)
					INNER JOIN TblFormStatus S WITH(NOLOCK) ON cu.FormStatusID=S.FormStatusID
					INNER JOIN @tbl t ON Cu.CustomerID=t.Value
	

		COMMIT TRANSACTION;    
    End Try  
	 BEGIN CATCH 
	 RollBACK TRANSACTION; 
		SELECT 1  As Code,'Error occurs.' As [OutputDescription]
	  
			INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
		    
	END CATCH;  


END

GO
