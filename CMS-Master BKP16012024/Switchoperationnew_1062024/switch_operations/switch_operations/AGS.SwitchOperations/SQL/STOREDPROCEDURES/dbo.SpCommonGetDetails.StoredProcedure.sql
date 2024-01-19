USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SpCommonGetDetails]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec SpCommonGetDetails @IntPriContextKey=19,@StrPriPara1=1
CREATE Procedure [dbo].[SpCommonGetDetails] 
@IntPriContextKey int=0,  
@StrPriPara1 varchar(100)='',  
@StrPriPara2 varchar(100)='',  
@StrPriPara3 varchar(500)='' ,
@StrPriPara4 varchar(100)=''  

As    
Begin   
--Begin Transaction  
	Begin Try   
		
		DECLARE @SystemID VARCHAR(500)
		DECLARE @BankID VARCHAR(500)
		DECLARE @LoginID VARCHAR(500)

		--For Fee Grid to Display
		If @IntPriContextKey=1
		Begin    
			Select  [FeeName] as [Fee Names],amount as[Amount Charge],interestrate as[InterestRate] ,Fee.[FeeCode] as [Seq No]
						From TblFeeDetails FeeDtl With(ReadPast)     
			Inner Join TblFeeMaster Fee With(ReadPast) On FeeDtl.[FeeCode]=Fee.[FeeCode]
			Order by Fee.[FeeCode] 
		End

		--For UserRole dropdown
		
		If @IntPriContextKey=2
		Begin    
			Select  RoleName[RoleName],UserRoleID[UserRoleID] From TblUserRole  With(ReadPast)  
		End
		
		--For Installment Grid to display
		If @IntPriContextKey=3
		Begin   
			Select [InMonths] as [Months] ,[InIntrestRate] as [Interest Rate],InstPrgCode as[Seq.No]
			From TblInstallmentPrg Inst With(ReadPast)  order by  InstPrgCode 
		End

		--CardType dropdown fill 
		If @IntPriContextKey=4
		Begin    
			Select CardTypeID,CardTypeName,CardTypeDesc 
			From TblCardType  With(NOLOCK) 
		End

		--Reward Min, Max amount
		If @IntPriContextKey=5
		Begin    
			select ct.CardTypeID, ct.CardTypeName, isnull(MinTransferPoints,0) MinTransferPoints, isnull(MaxTransferPoints,0) MaxTransferPoints from TblCardType ct  with(nolock)
			left outer join tblRewardConfig rc  with(nolock) on rc.CardType=ct.CardTypeID
		End

		--MoneyToPoint
		If @IntPriContextKey=6
		Begin    
			select ct.CardTypeID, ct.CardTypeName, isnull(MoneyToPoint,0) MoneyToPoint from TblCardType ct with(nolock)
			left outer join tblRewardConfig rc with(nolock) on rc.CardType=ct.CardTypeID
		End

		--PointToMoney
		If @IntPriContextKey=7
		Begin    
			select ct.CardTypeID, ct.CardTypeName, isnull(PointToMoney,0) PointToMoney from TblCardType ct with(nolock)
			left outer join tblRewardConfig rc with(nolock) on rc.CardType=ct.CardTypeID
		End
		--ApplicationNo DropDown
		IF @IntPriContextKey=8
		BEGIN
		--Accepted Customer
			if(@StrPriPara1=1)
			BEGIN
			  SELECT CustomerID,ApplicationFormNo  FROM TblCustomersDetails with(nolock)
			  WHERE isnull(ApplicationFormNo,'') <>'' AND FormStatusID=1
			END
          --All  Customer
			ELSE
			BEGIN
			  SELECT CustomerID,ApplicationFormNo  FROM TblCustomersDetails with(nolock)
			  WHERE isnull(ApplicationFormNo,'') <>''
			END
		END
		IF @IntPriContextKey=9
		BEGIN
		  SELECT  CAST(convert(varchar(10), UserDate, 120) AS VARCHAR) as[Date],HolidayName as[Holiday]  FROM Tbl_UserCalendar with(nolock)
		 
		END
		
		IF @IntPriContextKey=10
		BEGIN
					  --select OptionId, OptionNeumonic, OptionName, isnull(OptionParentNeumonic,'') OptionParentNeumonic, isnull(URL,'') URL, Active,GlyphiconClass AS [ClassName] from TblOptions with(nolock) where Active=1 order by ISNULL(DisplayOrder,999)
		IF(@StrPriPara1<>'')
		  BEGIN
		   DECLARE @UserID VARCHAR(500)=@StrPriPara1
			SET @SystemID=@StrPriPara2
			--Get Option neumonic
			DECLARE @OptionNeumonic VARCHAR(MAX)
			--for list of access pages
		    if (@StrPriPara3='1')
			BEGIN			   	
				 --Get Access rights
					Select  @OptionNeumonic=dbo.FunGetOptionNeumonic()


					DECLARE @Query varchar(max) = '

					SELECT
    
						OptionNeumonic,Value AS [AccessCaptions]
					FROM
					(
						SELECT
						   '+@OptionNeumonic+'
						FROM
							TblUserManagement AS t
							where UserID='+@UserID+'
							  AND SystemID='+@SystemID+'
					) AS SourceTable
					UNPIVOT
					(
						Value FOR  [OptionNeumonic] IN	    
						('+@OptionNeumonic+')
					) AS unpvt
					'
					print(@Query)
					exec(@Query)
			END
			ELSE
			BEGIN
			--get accessed menu
			
			Select  @OptionNeumonic=dbo.FunGetOptionNeumonic()

			DECLARE @SQL VARCHAR(max)='

			SELECT
				*
			into #menu
			FROM
			(
				SELECT
					'+@OptionNeumonic+'
				FROM
					TblUserManagement AS t
					where UserID='+@UserID+'
					AND SystemID='+@SystemID+'
			) AS SourceTable
			UNPIVOT
			(
				Value FOR  [OptionNeumonic#] IN
				('+@OptionNeumonic+')
			) AS unpvt		

			Select * from(

			select OptionId, OptionNeumonic, OptionName, isnull(OptionParentNeumonic,'''') OptionParentNeumonic, isnull(URL,'''') URL, Active,GlyphiconClass AS [ClassName],DisplayOrder
			 from TblOptions op with(nolock) 
			 INNER JOIN #menu menu ON op.OptionNeumonic=menu.OptionNeumonic# 
			 Union 
			 Select op.OptionId, op.OptionNeumonic, op.OptionName, isnull(op.OptionParentNeumonic,'''') OptionParentNeumonic,
			  isnull(op.URL,'''') URL, op.Active,op.GlyphiconClass AS [ClassName] ,op.DisplayOrder
			 from
			 tbloptions op with(nolock) INNER JOIN
			 (select  ISNULL(OptionParentNeumonic,'''') AS OptionParentNeumonic
			 from TblOptions opt with(nolock) 
			 INNER JOIN #menu menu ON opt.OptionNeumonic=menu.OptionNeumonic#
			 
			 ) b on op.OptionNeumonic=b.OptionParentNeumonic
 
			 )temp where Active=1 
			 order by ISNULL(DisplayOrder,999),optionid
 
			drop table #menu '
			PRINT(@SQL)
			exec(@SQL)
			END
			END
			

		END
		
		IF @IntPriContextKey=11
		BEGIN
		
		if(@StrPriPara1 <>'' AND @StrPriPara2 <>''  )
		BEGIN
			SET @SystemID=@StrPriPara1 
			SET @BankID=@StrPriPara2
			SET @LoginID=@StrPriPara3

			Select u.UserID,u.FirstName from tbluser u WITH(NOLOCK) 
			   where exists(select Value from fnSplit(@SystemID,'|') intersect select value from fnSplit(u.SystemID,',')) 
			   AND u.BankID=@BankID
			   -- for system User show all else only created user			
			     AND(((Select ISNULL(CreatedBy,0) from TblUser WITH(NOLOCK) where UserID=@LoginID) =0) OR(u.CreatedBy=@LoginID))
				 	 AND ISNULL(u.IsActive,0)=1	
			 
		  END
		  else
		  BEGIN
		   select UserID,FirstName  from tbluser WITH(NOLOCK)
		  END
		END


		--for Institution master
		IF @IntPriContextKey=12
		BEGIN
		   SET @BankID=@StrPriPara1
		   SET @SystemID=@StrPriPara2

		   IF(@StrPriPara1 = '' AND @StrPriPara2 = '')
		   BEGIN
			 SET @BankID=1
			 SET @SystemID=1
			END

		  Select ID,InstitutionID,INSTDesc AS[Description] from TblInstitutionDtl WITH(NOLOCK) where BankID=@BankID AND SystemID=@SystemID
		END
		
		
		IF @IntPriContextKey=13
		BEGIN
		   SET @BankID=@StrPriPara1
		   SET @SystemID=@StrPriPara2

		 IF(@StrPriPara1 = '' AND @StrPriPara2 = '')
		   BEGIN
			 SET @BankID=1
			 SET @SystemID=1
		   END
		   Select  UR.RoleName as [User Role],UR.UserRoleID As [UserId],convert(varchar(12),UR.CreatedDate,103) AS [Created Date],Case when UR.BankID=0 Then '1' else '0' END AS [IsDefault] 
			 ,Case WHEN ISNULL(UR.CreatedBy,0)=0 THEN 'System' ELSE U.FirstName+' '+U.LastName END AS [Created By]
			 from TblUserRole UR WITH(NOLOCK)
			 LEFT JOIN TblUser U WITH(NOLOCK) ON UR.CreatedBy=u.UserID
			  where (UR.BankID=0) OR ( UR.BankID=@BankID)
			  order by UR.SystemID
		END

		-- CardType Master
		IF @IntPriContextKey=14
		BEGIN
		  SET @BankID=@StrPriPara1
		SET @SystemID=@StrPriPara2

		 IF(@StrPriPara1 = '' AND @StrPriPara2 = '')
		   BEGIN
			 SET @BankID=1
			 SET @SystemID=1
			END

		  Select CardTypeID As[ID],CardTypeName AS [CardType] ,CardTypeDesc AS[Description] from  TblCardType WITH(NOLOCK)
		  where SystemID=@SystemID AND BankID=@BankID
		END

		--Bin Master
		IF @IntPriContextKey=15
		BEGIN
		SET @BankID=@StrPriPara2
		SET @SystemID=@StrPriPara3

		 IF(@StrPriPara2 = '' AND @StrPriPara3 = '')
		   BEGIN
			 SET @BankID=1
			 SET @SystemID=1
			END
		--Accepted BIN
			if(@StrPriPara1=1)
			BEGIN
			  SELECT ID,CardPrefix  FROM TblBIN WITH(NOLOCK) WHERE -- formstatusid=1   AND 
			  BankID=@BankID AND SystemID=@SystemID
			END
			--All BIN
			ELSE
			BEGIN			  
				 
				Select bin.ID,CardPrefix--,Switch_SchemeCode AS  [Scheme Code],Switch_CardType AS [Card Type]
				,bin.BinDesc AS [Description]
				 from TblBIN Bin WITH(NOLOCK)
				 --INNER JOIN Switch_AccountType AT WITH(NOLOCK) ON ISNULL(Bin.Switch_AccountType,10)=AT.Code
				 where bin.SystemID=@SystemID AND bin.BankID=@BankID
			END
		END
		---------------------User Details------------------------------
		
		IF @IntPriContextKey=16
		BEGIN
		if(@StrPriPara1 <> '' AND @StrPriPara2 <> '' )
		BEGIN

		SET @BankID=@StrPriPara1
		SET @SystemID=@StrPriPara2
		SET @LoginID=@StrPriPara3
		 

		select UD.FirstName , UD.LastName ,UD.UserName ,UR.RoleName as[UserRole],UD.MobileNo ,
		  UD.EmailId ,(case when UD.IsActive=1 then 'Active' else 'In Active' end) as [UserStatus] ,UD.UserRoleid[RoleId],UD.isactive[Activeid],UD.UserID as[UserID]		 
		  ,UD.SystemID AS [SystemID]
		  ,case WHEN ((ISNULL(ud.CreatedBy,0)<>0)OR ((ISNULL(UD.CreatedBy,0)=0)) AND (Ud.UserID=@LoginID) ) THEN '1' ELSE '0' END AS [IsEdit]  --newly add to show edit option
		  from dbo.TblUser  UD with(nolock)
		  left join  tbluserrole UR  with(nolock) on UR.UserRoleID=UD.UserRoleID 
		  where (RTRIM(LTRIM(UD.BankID))=RTRIM(LTRIM(@BankID))) --AND (LTRIM(RTRIM(UD.SystemID))=LTRIM(RTRIM(@SystemID)))
		   AND exists(select Value from fnSplit(@SystemID,'|') intersect select value from fnSplit(UD.SystemID,','))
		      AND(((Select ISNULL(CreatedBy,0) from TblUser WITH(NOLOCK) where UserID=@LoginID) =0) OR((UD.CreatedBy=@LoginID)OR(UD.UserID=@LoginID))) --newly add 
		END
		ELSE
		BEGIN
		  select UD.FirstName , UD.LastName ,UD.UserName ,UR.RoleName as[UserRole],UD.MobileNo ,
		  UD.EmailId ,(case when UD.IsActive=1 then 'Active' else 'In Active' end) as [UserStatus] ,UD.UserRoleid[RoleId],UD.isactive[Activeid],UD.UserID as[UserID]		 
		  ,UD.SystemID AS [SystemID],Sy.SystemName AS [SystemName]
		  from dbo.TblUser  UD with(nolock) 
		  left join  tbluserrole UR  with(nolock) on UR.UserRoleID=UD.UserRoleID 
		  INNER JOIN TblSystem Sy WITH(NOLOCK) ON UD.SystemID=Sy.SystemID
        END
		END
		--Product Type Master
		IF @IntPriContextKey=17
		BEGIN

		SET @BankID=@StrPriPara2
		SET @SystemID=@StrPriPara3

		 IF(@StrPriPara2 = '' AND @StrPriPara3 = '')
		   BEGIN
			 SET @BankID=1
			 SET @SystemID=1
			END

		--Accepted ProductType
			if(@StrPriPara1=1)
			BEGIN
			  SELECT ID,ProductType,ProductTypeDesc FROM TblProductType WITH(NOLOCK) WHERE --formstatusid=1 and
			   SystemID=@SystemID AND BankID=@BankID
			END
			--All Product Type
			ELSE
			BEGIN
				--Select PR.ID,PR.ProductType,PR.ProductTypeDesc AS [Description],bin.CardPrefix,Inst.InstitutionID AS [INSTID],
				--	  CT.CardTypeName AS [Card Type],FS.FormStatus AS [Status],Maker.FirstName+' '+Maker.LastName AS [Configured By],Checker.FirstName+' '+Checker.LastName AS [Authorized By] ,PR.Remark
				--	  ,ISNULL(PR.FormStatusID,0) AS [StatusID] ,PR.INST_ID,PR.BIN_ID,PR.CardType_ID  
				--	from TblProductType PR WITH(NOLOCK)
				--		LEFT JOIN TblUser Maker WITH(NOLOCK) ON PR.MakerID=Maker.UserID
				--		LEFT JOIN TblUser Checker WITH(NOLOCK) ON PR.CheckerID=Checker.UserID
				--		INNER JOIN TblFormStatus FS WITH(NOLOCK) ON ISNULL(PR.FormStatusID,0)=FS.FormStatusID
				--		INNER JOIN TblBIN bin WITH(NOLOCK) ON PR.BIN_ID=bin.ID
				--		INNER JOIN  TblInstitutionDtl Inst WITH(NOLOCK) ON PR.INST_ID=Inst.ID
				--		INNER JOIN TblCardType CT WITH(NOLOCK) ON PR.CardType_ID=CT.CardTypeID

				---------------------------
				Select PR.ID,PR.ProductType,PR.ProductTypeDesc AS [Description],bin.CardPrefix,Inst.InstitutionID AS [INSTID],
					  CT.CardTypeName AS [Card Type]--,FS.FormStatus AS [Status]
					  ,Maker.FirstName+' '+Maker.LastName AS [Configured By]
					  ,ISNULL(PR.FormStatusID,0) AS [StatusID] ,PR.INST_ID,PR.BIN_ID,PR.CardType_ID  
					   from TblProductType PR WITH(NOLOCK)
						LEFT JOIN TblUser Maker WITH(NOLOCK) ON PR.MakerID=Maker.UserID
						--LEFT JOIN TblUser Checker WITH(NOLOCK) ON PR.CheckerID=Checker.UserID
						--INNER JOIN TblFormStatus FS WITH(NOLOCK) ON ISNULL(PR.FormStatusID,0)=FS.FormStatusID
						INNER JOIN TblBIN bin WITH(NOLOCK) ON PR.BIN_ID=bin.ID
						INNER JOIN  TblInstitutionDtl Inst WITH(NOLOCK) ON PR.INST_ID=Inst.ID
						INNER JOIN TblCardType CT WITH(NOLOCK) ON PR.CardType_ID=CT.CardTypeID
						where PR.SystemID=@SystemID AND PR.BankID=@BankID
				
			END
			END
			
			---------------Courier Details----------------
			IF @IntPriContextKey=18
		BEGIN
		 SET @BankID=@StrPriPara1
		SET @SystemID=@StrPriPara2

		 IF(@StrPriPara1 = '' AND @StrPriPara2 = '')
		   BEGIN
			 SET @BankID=1
			 SET @SystemID=1
			END
		  
		  select [CourierName] as[CourierName],[OfficeAddress] as [Office],contactno as[ContactNo],
          case when Status=1 then 'Active' else case when status=2 then 'In-Active' else ''end end as[Status], Status as [StatusId],id as[CourierId]
		  from dbo.TblCourier with(nolock) 
		  where SystemID=@SystemID AND BankID=@BankID
		  
		END
		--------------- CARD Operation -------------------
		IF @IntPriContextKey=19
		BEGIN
		--Card operation requests
         If(@StrPriPara1=1)
		 BEGIN
		    SELECT ID,RequestType FROM TblCardRequests WITH(NOLOCK) WHERE --ISNULL(SwitchCode,0)<>0
			Flag='C' and RequestType <> 'RePin'
			union all
			SELECT ID,RequestType FROM TblCardRequests WITH(NOLOCK) WHERE --ISNULL(SwitchCode,0)<>0
			RequestType ='Pin Reset'
		 END
		 ELSE
		 BEGIN
		    SELECT ID,RequestType FROM TblCardRequests WITH(NOLOCK) 
		 END		   
		END

		------------------------------ System -------------------------------
		IF(@IntPriContextKey=20)
		BEGIN
		--Login User wise System List
		IF (@StrPriPara1<>'')
		BEGIN
		--Login User wise System List
		  IF(@StrPriPara2='')
		 BEGIN
		  declare @tbl table(value varchar(200),RowID int) DECLARE @SystemLst VARCHAR(500)
			 Select @SystemLst= systemId from tbluser WITH(NOLOCK) where UserID=@StrPriPara1
	 			 insert into @tbl (value,RowID)(SELECT VALUE,RowID FROM dbo.fnSplit(@SystemLst,','))

			Select SystemID,SystemName 
			from TblSystem Sy WITH(NOLOCK)
			INNER JOIN @tbl temp ON Sy.SystemID=temp.value
		END
		--System Rights
		ELSE IF (@StrPriPara2='1')
		BEGIN
		  Select SystemID from TblUser WITH(NOLOCK) Where UserID=@StrPriPara1
		END

		END
		--Bank Wise System List
		ELSE 
		BEGIN
		-- SELECT SystemID,SystemName FROM TblSystem WITH(NOLOCK)
			Select SystemID,SystemName from TblSystem WITH(NOLOCK)
			where SystemID in (SELECT VALUE FROM dbo.fnSplit((Select SystemID From TblBanks WITH(NOLOCK) where ID=@StrPriPara2),','))
		 END
		END

		------------------------------ check user's rights -----------------
		IF(@IntPriContextKey=21)
		BEGIN
		  
			SET @LoginID=@StrPriPara1
			Declare @Option VARCHAR(100)=@StrPriPara2

			DECLARE @SQLQuery VARCHAR(max)='

			Select '+@Option+' from TblUserManagement
			where UserID='+@LoginID+' AND ISNULL('+@Option+','''')<>''''
			'
			Print @SQLQuery
			EXEC(@SQLQuery)
		   
		END

		-------------------------- Get all Accepted Cust details for card genration  which not processed --
		IF(@IntPriContextKey=22)
		BEGIN
		
		--DECLARE @Date VARCHAR(500)=@StrPriPara3
		SET @SystemID=@StrPriPara1 
		SET @BankID=@StrPriPara2

		 --IF(@StrPriPara1 = '' AND @StrPriPara2 = '')
		 --  BEGIN
			-- SET @BankID=1
			-- SET @SystemID=1
			--END

			Select c.CustomerID [CustomerID],
			--RIGHT(CONCAT('00000000000000000000', ISNULL(c.BankCustID,c.CustomerID)), 20)
			c.BankCustID AS [CIF ID],
			c.FirstName+' '+c.MiddleName+' '+c.LastName  [CustomerName],
			Convert(Varchar(12),b.CardPrefix) [CardPrefix],
			--RIGHT(CONCAT('00000000000000000000', dbo.ufn_decryptPAN(c.AccNo)), 16)[Account No],
			dbo.ufn_decryptPAN(c.AccNo) AS [AccountNo],
			case when ISNULL(cd.PO_Box_P,'')<>'' THEN 'PO_BOX-'+ISNULL(cd.PO_Box_P,'') ELSE '' END
			+case when ISNULL(cd.HouseNo_P,'')<>'' THEN' HouseNo -'+ISNULL(cd.HouseNo_P,'')ELSE '' END    [Address1],
			ISNULL(cd.StreetName_P,'')+' '+ISNULL(cd.Tole_P,'') [Address2],
			ISNULL(cd.WardNo_P,'') AS [Address3],
			convert(VARCHAR(16),c.DOB_AD,103) [Date Of Birth],c.MobileNo [Mobile No],cd.Email_P [Email],convert(VARCHAR(16),c.Checker_Date_IND,103) [VerifiedDate]
			,ISNULL(Cp.Reason,'') AS [RejectReason]
			from TblCustomersDetails c WITH(NOLOCK)
			INNER JOIN TblCustomerAddress cd WITH(NOLOCK) ON c.CustomerID=cd.CustomerID
			LEFT JOIN tblCardProduction CP WITH(NOLOCK) on Cp.[CIF ID]=convert(varchar,c.bankcustID)
			INNER JOIN TblProducttype PT with(NOLOCK) ON c.ProductType_ID=PT.ID
			INNER JOIN TblBIN B with(NOLOCK) on pt.BIN_ID=b.ID						
			WHERE (ISNULL(c.FormStatusID,0)=1 )
			AND (ISNULL(IsCardSuccess,0)<>1)
			AND (((ISNULL(cp.[CIF ID],'')='') AND (c.bankcustID not in (select  ac.[CIF ID] from TblAuthorizedCardLog AC WITH(NOLOCK) )))OR(ISNULL(CP.Rejected,0)=1))
			AND ((c.BankID=@BankID)) 
			AND ((c.SystemID=@SystemID)) 		


		END
		--------- Get Process card req for autherize
		IF(@IntPriContextKey=23)
		BEGIN
		SET @SystemID=@StrPriPara1 
		SET @BankID=@StrPriPara2
		 IF(@StrPriPara1 = '' AND @StrPriPara2 = '')
		   BEGIN
			 SET @BankID=1
			 SET @SystemID=1
			END
			select Code, [CIF ID],[Customer Name],[Customer Preferred name] AS [Name On Card],[Card Type and Subtype] as [Card Prefix],[AC ID] AS [AccountNo],
			[Address Line 1]+ ' ' +[Address Line 2] +' '+[Address Line 3] as [Address],[City],
			convert(varchar(12),ProcessedOn,103) AS [Processed Date]
			 from tblCardProduction where ISNULL(IsAuthorised,0)=0 AND ISNULL(Processed,0)=1 and ISNULL(Rejected,0)=0			
			AND ( (SystemID=@SystemID))
			AND ((Bank=@BankID))
		END

		------------------------------- for Status dropdown for Reports -------------------------
		IF(@IntPriContextKey=24)
		BEGIN
			--SET @SystemID=@StrPriPara1 
			--SET @BankID=@StrPriPara2

			Select FormStatusID,FormStatus from TblFormStatus WITH(NOLOCK)
			where FormStatusID in (0,1,4)
		END
		-------------------------------------- for Product dropdown for Reports ---------------
		IF(@IntPriContextKey=25)
		BEGIN
			SET @SystemID=@StrPriPara1 
			SET @BankID=@StrPriPara2

			SELECT ID,ProductType from TblProductType WITH(NOLOCK)
			where SystemID=@SystemID AND BankID=@BankID		
		END
		----------------------------------------- Get User Wise SessionKey ------------------------------
		IF(@IntPriContextKey=26)
		BEGIN
			SET @SystemID=@StrPriPara1 
			SET @BankID=@StrPriPara2
			SET @UserID=@StrPriPara3

			SELECT ISNULL(UsrSessionKey,'') AS[AuthSessionKey] from TblUser WITH(NOLOCK)
			where SystemID=@SystemID AND BankID=@BankID	 and UserID=@UserID	
		END
		-------------------------------------- Get AccountType DropDownlist for Enrolment Section --------
		IF(@IntPriContextKey=27)
		BEGIN
			SET @SystemID=@StrPriPara1 
			SET @BankID=@StrPriPara2

		  Select AccountTypeID,AccountTypeName,AccountTypeCode 
		     From TblAccounttype With(NOLOCK)
		END
	--COMMIT TRANSACTION;  
	 End Try  
	 BEGIN CATCH  
	 --RollBACK TRANSACTION; 
		--SELECT   
		--	ERROR_NUMBER() AS ErrorNumber  
		--	,ERROR_SEVERITY() AS ErrorSeverity  
		--	,ERROR_STATE() AS ErrorState  
		--	,ERROR_PROCEDURE() AS ErrorProcedure  
		--	,ERROR_LINE() AS ErrorLine  
		--	,ERROR_MESSAGE() AS ErrorMessage;  
  
			INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
		  
	
	END CATCH;  
End   

GO
