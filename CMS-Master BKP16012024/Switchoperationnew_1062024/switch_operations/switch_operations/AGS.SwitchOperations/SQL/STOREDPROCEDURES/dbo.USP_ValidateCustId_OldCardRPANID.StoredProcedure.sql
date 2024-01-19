
--exec USP_ValidateCustId_OldCardRPANID '','6080140000152391','46604600','41','OldCardRPANID Exist','24',7,2
ALTER PROCEDURE [dbo].[USP_ValidateCustId_OldCardRPANID]
(
@CustomerID varchar(100),
@CardNo varchar(100),
@NewBinPrefix VARCHAR(100),
@HoldRSPCode VARCHAR(100),
@Mode varchar(50),
@CheckerID varchar (20),
--2-12-17
--new params added
@BankID int,
@SystemID int
)
as
begin
--SELECT * FROM CardRPAN
DECLARE @ID  VARCHAR (100)
DECLARE @ISVALID VARCHAR (100)
DECLARE @ISSUER_NR VARCHAR (10)

SELECT @ISSUER_NR=B.BankCode--get issuerno of respective bank
FROM TBLUSER A WITH (NOLOCK)
	INNER JOIN TblBanks B WITH (NOLOCK) ON A.BANKID=B.ID
WHERE USERID=@CheckerID--loginid from session

Declare @FetchCustId varchar(100)
Declare @oldpan varchar(200)

SELECT @ID=ID ,@FetchCustId=customer_id,@oldpan=EncPAN 
FROM CardRPAN WITH(NOLOCK) 
WHERE IssuerNo=@ISSUER_NR and dbo.ufn_DecryptPAN(DecPAN)=@CardNo

--select @ISSUER_NR

 if(@Mode='OldCardRPANID Exist')
BEGIN
	IF EXISTS(SELECT top 1 1  FROM CardRPAN WITH  (NOLOCK)  WHERE EncPAN=@oldpan and((isnull(@CustomerID,'')='')or( customer_id=@CustomerID)))
		BEGin
			SET @ISVALID='TRUE'
		END
	ELSE
		BEGIN 
			SET @ISVALID ='FALSE'
		END

	print @ISVALID
	IF @ISVALID='TRUE'
		BEGIN
		 IF NOT EXISTS ( SELECT  1 FROM TblCardGenRequest A WITH (NOLOCK)  WHERE  OldPan=@oldpan) 
			 BEGIN
			 IF NOT  EXISTS (SELECT  1 FROM TblCardGenRequest_History A WITH (NOLOCK)  WHERE  OldPan=@oldpan)
				 BEGIn
					select 'Card NOT in table|SUCCESS' AS [StatusMessage]
				 END
			 ELSE
				 BEGIN
					select 'Card Already Resissued|Invalid' AS [StatusMessage]
				 END
			 END
		ELSE
		BEGIN
			select 'Card Already Resissued|Invalid' AS [StatusMessage]
		END
	  ENd
	ELSE
		BEGIN
			select 'Invalid Card and Customerid Combination|Invalid' AS [StatusMessage]
		END

END

if(@Mode='insert')
	Begin
		INSERT INTO TblCardGenRequest(CustomerID,OldCardRPANID,NewBinPrefix,HoldRSPCode,CheckerID,IsAuthorized,CreatedDate,Remark,FormStatusID,ProcessID,BankID,SystemID,OldPan)
		VALUES(@FetchCustId,@ID,@NewBinPrefix,@HoldRSPCode,@CheckerID,0,getdate(),'Reissue',0,6,@BankID,@SystemID,@oldpan)	
		select 'successfully inserted' AS [StatusMessage], 'Success' AS [StatusCode]
	end

end
