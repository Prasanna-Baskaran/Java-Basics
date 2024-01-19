USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_GetCardGenerateRequestData]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,Sheetal,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,To authorise the card generation request and set authorise as 1,>
-- =============================================


--select * from [temp_po-old].dbo.cardproductionrenewal
--select * from  TblCardGenRequest
CREATE procedure [dbo].[USP_GetCardGenerateRequestData]  --exec USP_GetCardGenerateRequestData '0000004774','6376761000020048044','OldCardRPANID Exist'
(
	--@CustomerID varchar(500),--=null
	--@OldCardRPANID varchar(500),--=null CardNo
	--@NewBinPrefix varchar(500)=null,
	--@HoldRSPCode varchar(500)=null,
	--@RSPCode varchar(500)=null,
	--@SwitchResponse varchar(500)=null,
	--@STAN varchar(500)=null,
	--@RRN varchar(500)=null,
	--@AuthID varchar(500)=null,
	--@Remark varchar(500)=null,
	--@FormStatusID int=null,
	--@IsRejected smallint=null,
	--@RejectReason varchar(500)=null,
	--@MakerID varchar(500)=null,
     @CheckerID varchar(500),
	----@IsAuthorized smallint=null,
	--@UploadFileName varchar(500)=null,
	--@BankID varchar(500)=null,
	--@SystemID varchar(500)=null,
	--@ProcessID int=null,
	--@schemecode varchar(500)=null,
	--@Account1 varchar(500)=null,
	--@Account2 varchar(500)=null,
	--@Account3 varchar(500)=null,
	--@Account4 varchar(500)=null,
	--@Account5 varchar(500)=null,
	--@Reserved1 varchar(500)=null,
	--@Reserved2 varchar(500)=null,
	--@Reserved3 varchar(500)=null,
	--@Reserved4 varchar(500)=null,
	--@Reserved5 varchar(500)=null,
	--@Branch_Code varchar(500)=null,
	--@ExpiryDate varchar(500)=null,
	--@New_Card varchar(500)=null,
	--@Customer_Name varchar(500)=null,
	--@New_Card_Activation_Date varchar(500)=null,
	@CheckedIDList varchar(500)=null,
	@Mode varchar(50)
)
as
begin

 
if(@Mode='select')

Begin
	--select ID,CustomerID,OldCardRPANID,NewBinPrefix,HoldRSPCode,RSPCode,SwitchResponse,STAN,
	--RRN,AuthID,Remark,FormStatusID,IsRejected,RejectReason,MakerID,CreatedDate,CheckerID,CheckedDate,
	--IsAuthorized,UploadFileName,BankID,SystemID,ProcessID,schemecode,Account1,Account2,Account3,
	--Account4,Account5,Reserved1,Reserved2,Reserved3,Reserved4,Reserved5,Branch_Code,ExpiryDate,
	--New_Card,Customer_Name,New_Card_Activation_Date from TblCardGenRequest
	--where IsAuthorized = 0
	select ID,CustomerID,NewBinPrefix,CheckerID,CheckedDate,HoldRSPCode,RSPCode,IsAuthorized,Remark from TblCardGenRequest
	where isnull(IsAuthorized,0)=0
End

if(@Mode='update')
Begin
	update TblCardGenRequest set CheckedDate=getdate(),CheckerID=@CheckerID
	, IsAuthorized=1
	where id in ( 
--select value from dbo.fnSplit('1,2,3,4,5,6',',')
	select value from dbo.fnSplit(@CheckedIDList,',')
	)
	select 'Success' 'Success'
End
END

GO
