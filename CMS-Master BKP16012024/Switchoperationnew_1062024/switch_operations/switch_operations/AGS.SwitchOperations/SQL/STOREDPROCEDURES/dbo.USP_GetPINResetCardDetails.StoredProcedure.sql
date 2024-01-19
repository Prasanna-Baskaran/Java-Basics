USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_GetPINResetCardDetails]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[USP_GetPINResetCardDetails]
	@flag int,--1 to fetch card data,2 to update success flag,3 update reject flag
	@CheckerID BIGINT,
	@RequestTypeID INT=0,
	@Remark VARCHAR(200)='',
	@ReqID VARCHAR(500)='',
	@FormStatusID int,	
	@BankID Varchar(200)=1,
	@SystemID Varchar(200)=1,
	@cardNo varchar(200)=''
as
/*
Flag 1 for to fetch card details
Flag 2 for to update success response
Flag 3 for to update failuer response
Flag 4 for to fetch PIN try count from switch
*/
begin
DECLARE @IssuerNo Int 

	Select @IssuerNo=BankCode From TblBanks WITH(NOLOCK) Where ID=@BankID
	 	 --select * from @tbl
if @flag=1
	begin		  
	 declare @tbl table(value varchar(200),RowID int)
	 insert into @tbl (value,RowID)(SELECT VALUE,RowID FROM dbo.fnSplit(@ReqID,','))	 	  	 
 	 
	 SELECT dbo.ufn_DecryptPAN(DecPAN) CardNo,t.value ID  from @tbl t
		 INNER JOIN TblCardOpsRequestLog ct ON t.value=ct.ID
		 INNER JOIN CardRPAN CP ON ct.CardRPANID=CP.ID
		 where ct.SystemID=@SystemID AND ct.BankID=@BankID	 
	end
else if @flag=2
	begin
		UPDATE TblCardOpsRequestLog SET FormStatusID=@FormStatusID ,CheckerID=@CheckerID ,
		CheckedDate=GETDATE(),IsSuccess=1,SwitchResponse='Success' WHERE ID=@ReqID AND SystemID=@SystemID AND BankID=@BankID
	end
else if @flag=3
	begin
		UPDATE TblCardOpsRequestLog SET FormStatusID=0 ,IsSuccess=0 ,SwitchResponse='Failed' WHERE ID=@ReqID AND SystemID=@SystemID AND BankID=@BankID
	end
else
	begin	
	Declare @Rpan Varchar(200)	
	select @Rpan= encpan,@IssuerNo=IssuerNo from cardrpan with(nolock) where dbo.ufn_decryptpan(decpan)=@Cardno		
	Exec [AGSS1RT].postcard.dbo.USP_GetPINResetTryCount @Rpan,@IssuerNo						
	end
end
GO
