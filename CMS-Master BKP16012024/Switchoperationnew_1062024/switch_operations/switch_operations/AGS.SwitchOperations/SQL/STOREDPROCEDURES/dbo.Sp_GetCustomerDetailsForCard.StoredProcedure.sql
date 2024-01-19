USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_GetCustomerDetailsForCard]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Sp_GetCustomerDetailsForCard '22.12.2016'
CREATE PROCEDURE [dbo].[Sp_GetCustomerDetailsForCard]
	@StrDate varchar(200) =NULL
AS
BEGIN

Declare @Yesterday varchar(100)  
  if(ISNULL(@StrDate,0)<> 0)  
  Begin  

 set @Yesterday= (SELECT CONVERT(varchar, DATEADD(d,0,Convert(Date,@StrDate,103)), 104))
 
  End  
  Else  
  Begin  
    set @Yesterday=(SELECT CONVERT(varchar, DATEADD(d,-1,GETDATE()), 104)) 
  End  

--set @Yesterday = (SELECT CONVERT(varchar, DATEADD(d,0,Convert(Date,'22.12.2016',103)), 104))


--Select c.CustomerID,
--((RIGHT(CONCAT('00000000000000000000', c.CustomerID), 16))    --CIF ID
--+'|'+(c.FirstName+' '+c.MiddleName+' '+c.LastName)  --Customer Name
--+'|'+(c.FirstName+' '+c.MiddleName+' '+c.LastName)  --Name on Card
--+'|'+Convert(Varchar(8),'')                        --BIN Prefix
--+'|'+(RIGHT(CONCAT('00000000000000000000', c.CustomerID), 16))    --AccountNo
--+'|'+replace(convert(VARCHAR(16),c.Maker_Date_IND,103),'/','')   --Account Opening Date
--+'|'+replace(convert(VARCHAR(16),c.Maker_Date_IND,103),'/','')    --CIF creation date
--+'|'+'P.O. BOX -'+ISNULL(cd.PO_Box_P,'')+' '+' House No -'+ISNULL(cd.HouseNo_P,'')    --ADDRESS 1
--+'|'+ISNULL(cd.StreetName_P,'')+' '+ISNULL(cd.Tole_P,'')+' '+' Ward No -'+ISNULL(cd.WardNo_P,'')  --ADDRESS 2
--+'|'+cd.City_P                                                             --CITY
--+'|'+''                                                               --State 
--+'|'+cd.PO_Box_P                                                   --Pin Code
--+'|'+'NEPAL'                                                       --COUNTRY 
--+'|'+c.MotherName                                                --MOTHER NAME
--+'|'+replace(convert(VARCHAR(16),c.DOB_AD,103),'/','')            --DOB
--+'|'+'977'                                              --Country Code
--+'|'+'977'                                            --STD Code
--+'|'+c.MobileNo                                       --MOBILE NO 
--+'|'+cd.Email_P                                        --EMAIL 
--+'|'+''                                                 --SCHEME CODE
--+'|'+''                                                   --BRANCH CODE 
--+'|'+replace(convert(VARCHAR(16),c.Maker_Date_IND,103),'/','')  --ENTERED DATE
--+'|'+replace(convert(VARCHAR(16),c.Checker_Date_IND,103),'/','')  --VERIFIED DATE
--+'|'+''                                                     --PAN NO
--+'|'+'02'                            --MODE OF OPERATION                             
--+'|'+''                              --FOURTH LINE EMBOSSING
--+'|'+''                              --AADHAR NO
--+'|'+'Y'                             --ISSUE DEBIT CARD
--+'|'+'03') AS [Card Detail]          --PIN MAILER  (Physical)
--from TblCustomersDetails c WITH(NOLOCK)
--INNER JOIN TblCustomerAddress cd WITH(NOLOCK) ON c.CustomerID=cd.CustomerID
--WHERE c.FormStatusID=1 
--AND  convert(varchar(50),C.Checker_Date_IND,104)=@Yesterday  
--AND ISNULL(IsCardSuccess,0)<>1

Select c.CustomerID [Customer ID],
RIGHT(CONCAT('00000000000000000000', c.CustomerID), 16) [CIF ID],
c.FirstName+' '+c.MiddleName+' '+c.LastName  [Customer Name],
Convert(Varchar(8),'') [BIN Prefix],
RIGHT(CONCAT('00000000000000000000', c.CustomerID), 16)[Account No],
convert(VARCHAR(16),c.Maker_Date_IND,103) [Account Opening Date],
'P.O. BOX -'+ISNULL(cd.PO_Box_P,'')+' '+' House No -'+ISNULL(cd.HouseNo_P,'')    [ADDRESS 1],
ISNULL(cd.StreetName_P,'')+' '+ISNULL(cd.Tole_P,'')+' '+' Ward No -'+ISNULL(cd.WardNo_P,'') [ADDRESS 2],
convert(VARCHAR(16),c.DOB_AD,103) [DOB],c.MobileNo [MOBILE NO],cd.Email_P [EMAIL],convert(VARCHAR(16),c.Checker_Date_IND,103) [VERIFIED DATE]
from TblCustomersDetails c WITH(NOLOCK)
INNER JOIN TblCustomerAddress cd WITH(NOLOCK) ON c.CustomerID=cd.CustomerID
WHERE c.FormStatusID=1 
AND  C.Checker_Date_IND is not null
AND ISNULL(IsCardSuccess,0)<>1
and not exists(select 1 from tblCardProduction cp where cp.[CIF ID]=c.CustomerID and Rejected=0)

END

GO
