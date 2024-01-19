USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_ApproveRejectProductType]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Sp_ApproveRejectProductType]
   @ID INT =0,
   @CheckerID BIGINT,   
   @FormStatusID int,
   @Remark varchar(50)=''
AS
BEGIN
	Begin Transaction  
	Begin Try    
			Declare @StrPriOutput varchar(1)='1'					
			Declare @StrPriOutputDesc varchar(200)='Product type is not Approved/Rejected'

			IF Exists(SELECT 1 FROM TblProductType WITH(NOLOCK) WHERE ID=@ID)
			BEGIN
			--Formstatus 1 Accept 
				If(@FormStatusID=1)
				 BEGIN		       

				   UPDATE TblProductType SET FormStatusID=@FormStatusID,CheckerID=@CheckerID,CheckedDate=GETDATE(),Remark='' WHERE ID=@ID
				    SET @StrPriOutput='0'
				    SET @StrPriOutputDesc='Product type is approved'
				 END
			--Formstatus 2 reject 
			  ELSE If(@FormStatusID=2)
				 BEGIN
			        UPDATE TblProductType SET FormStatusID=@FormStatusID,CheckerID=@CheckerID,CheckedDate=GETDATE(),Remark=@Remark WHERE ID=@ID
				    SET @StrPriOutput='0'
				    SET @StrPriOutputDesc='Product type is rejected'
				 END
			 END
			 ELSE
			 BEGIN
			   SET @StrPriOutput='1'
			   IF(@FormStatusID=1)
			    BEGIN
			    SET @StrPriOutputDesc='Product type is not approved'
			   END
			   ELSE
			    BEGIN
				 SET @StrPriOutputDesc='Product type is not rejected'
				END
			 END			
						Select @StrPriOutput As Code,@StrPriOutputDesc As [OutputDescription]

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
