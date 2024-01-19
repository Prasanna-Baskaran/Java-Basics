USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SP_CAGetSwitchCardAccountLinkage_AGS]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------
CREATE PROCEDURE [dbo].[SP_CAGetSwitchCardAccountLinkage_AGS]
@IssuerNo Int,
@PANIssuer PANIssuerType Readonly,
@IntpriOutput int OUTPUT,
@StrpriOutputDesc Varchar(500) OUTPUT
AS
/*CHANGE MANAGEMENT    
CREATED BY:   Diksha Walunj 
CREATED DATE: 03/10/2017    
CREATED REASON: Get Card ,Account details by issuerNo,PAN

Declare @IntpriOutput int ,@StrpriOutputDesc Varchar(500) 
DECLARE @PANIssuer AS PANIssuerType
INSERT INTO @PANIssuer Values ('01H2105A89B7D0F1A4F3C5473B4DC82DC0B6FE','18')
exec [SP_CAGetSwitchCardAccountLinkage_AGS] 1,@PANIssuer,@IntpriOutput output ,@StrpriOutputDesc output
select @IntpriOutput,@StrpriOutputDesc
*/   
BEGIN
BEGIN TRY
		SET @IntpriOutput=1;
		SET @StrpriOutputDesc='Error Occured';

		DECLARE @RTCardsTable VARCHAR(100),@RTCardsAccountTable VARCHAR(100),@RTAccountTable VARCHAR(100),@RTCustomerTable VARCHAR(100)
		
		
		--Get pc_cards table from issuer number			
		BEGIN TRY						
		  EXEC [AGSS1RT].postcard.dbo.bancs_get_dyn_tbl_name @IssuerNo, 1,'pc_cards', @RTCardsTable OUTPUT
		END TRY
		BEGIN CATCH
			SET @IntpriOutput=98;
			SET @StrpriOutputDesc='Issuer not present';
			GOTO LblResult
		END CATCH

		--Get pc_cards_accounts table from issuer number			
		BEGIN TRY						
		   EXEC [AGSS1RT].postcard.dbo.bancs_get_dyn_tbl_name @IssuerNo, 3,'pc_card_accounts', @RTCardsAccountTable OUTPUT
		END TRY
		BEGIN CATCH
			SET @IntpriOutput=98;
			SET @StrpriOutputDesc='Issuer not present';
			GOTO LblResult
		END CATCH

		--Get pc_accounts table from issuer number			
		BEGIN TRY						
		   EXEC [AGSS1RT].postcard.dbo.bancs_get_dyn_tbl_name @IssuerNo, 2,'pc_accounts', @RTAccountTable OUTPUT
		END TRY
		BEGIN CATCH
			SET @IntpriOutput=98;
			SET @StrpriOutputDesc='Issuer not present';
			GOTO LblResult
		END CATCH

		-- Get pc_customers table from issuer number
		 Begin Try
		 EXEC [AGSS1RT].postcard.dbo.bancs_get_dyn_tbl_name @IssuerNo, 8,'pc_customers', @RTCustomerTable OUTPUT
		 End Try
 
		 Begin Catch
		 SET @IntpriOutput=98;
		 SET @StrpriOutputDesc='Issuer not present';
		 End Catch
							
		DECLARE @StrSQL NVARCHAR(MAX)
		DECLARE @ParmDef NVARCHAR(500);

	Select * Into #PanTemp from @PANIssuer
	--	Select @RTCardsTable,@RTCardsAccountTable,@RTAccountTable
		-- Get Card Account details	

		SET @StrSQL='	

		if Exists(Select top 1 1  from [AGSS1RT].postcard.dbo.'+@RTCardsTable+' c WITH(NOLOCK)
		              INNER JOIN #PanTemp Pan  WITH(NOLOCK)  ON  c.issuer_nr=Pan.IssuerNo AND c.pan_encrypted=pan.EncPAN
						LEFT JOIN [AGSS1RT].postcard.dbo.'+@RTCardsAccountTable+' Ac WITH(NOLOCK) ON c.pan=Ac.pan AND ISNULL(Ac.date_deleted,0)=0
						LEFT JOIN [AGSS1RT].postcard.dbo.'+@RTAccountTable+' a WITH(NOLOCK) ON a.account_id=ac.account_id	AND ISNULL(a.date_deleted,'''')=''''					
						where Isnull(c.date_deleted,'''')='''' AND ISNULL(ac.account_type_qualifier,0)<>0
			         
						 )
				Begin		

		--	print 1
					 SELECT c.customer_id,c.pan_encrypted,a.account_id_encrypted,a.account_type,ac.account_type_qualifier ,c.issuer_nr,ISNULL(c.default_account_type,'''') AS[DefaultAccType]
						from [AGSS1RT].postcard.dbo.'+@RTCardsTable+' c WITH(NOLOCK)						
						 INNER JOIN #PanTemp Pan  WITH(NOLOCK)  ON  c.issuer_nr=Pan.IssuerNo AND c.pan_encrypted=pan.EncPAN
						LEFT JOIN [AGSS1RT].postcard.dbo.'+@RTCardsAccountTable+' Ac WITH(NOLOCK) ON c.pan=Ac.pan AND ISNULL(Ac.date_deleted,'''')=''''
						LEFT JOIN [AGSS1RT].postcard.dbo.'+@RTAccountTable+' a WITH(NOLOCK) ON a.account_id=ac.account_id AND	ISNULL(a.date_deleted,'''')=''''					
						where Isnull(c.date_deleted,'''')='''' 
						 AND ISNULL(ac.account_type_qualifier,0)<>0
						  
								

					Set @IntpriOutput=0;
					Set @StrpriOutputDesc=''Success'';
				END			
			 Else
			 Begin				
				--Card Not present
				Set @IntpriOutput=99;
				Set @StrpriOutputDesc=''Card not found'';
			  End
				'
		SET @ParmDef = N'@IntpriOutput INT OUTPUT,@StrpriOutputDesc Varchar(500) OUTPUT';
	
		--print (@StrSQL)
		EXEC SP_EXECUTESQL @StrSQL,@ParmDef,@IntpriOutput=@IntpriOutput OUTPUT,@StrpriOutputDesc=@StrpriOutputDesc OUTPUT;

		drop table #PanTemp
END TRY
BEGIN CATCH 
 INSERT INTO TblCardAutomationErrorLog(Function_Name,Error_Desc,Error_Date,ParameterList,IssuerNo)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(100)),GETDATE(),'SwitchRSP:@IntpriOutput='+Convert(varchar(20),@IntpriOutput)+'|@StrpriOutputDesc='+@StrpriOutputDesc,@IssuerNo
END CATCH
LblResult:
if(@IntpriOutput<>0)
BEGIN
	SELECT '','','','','','','','','','','','','','','',''
	INSERT INTO TblCardAutomationErrorLog(Function_Name,Error_Desc,Error_Date,ParameterList,IssuerNo)                 
		  SELECT 'SP_CAGetSwitchCardAccountLinkage_AGS','SwitchRSP:@IntpriOutput='+Convert(varchar(20),@IntpriOutput)+'|@StrpriOutputDesc='+@StrpriOutputDesc,GETDATE(),'@PANIssuer tableType',@IssuerNo
	END
END

GO
