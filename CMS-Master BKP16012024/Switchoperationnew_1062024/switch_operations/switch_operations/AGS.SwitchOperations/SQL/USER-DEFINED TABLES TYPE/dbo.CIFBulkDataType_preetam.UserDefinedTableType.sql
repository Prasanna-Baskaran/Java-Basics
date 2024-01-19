USE [SwitchOperations]
GO
/****** Object:  UserDefinedTableType [dbo].[CIFBulkDataType_preetam]    Script Date: 08-06-2018 16:58:16 ******/
CREATE TYPE [dbo].[CIFBulkDataType_preetam] AS TABLE(
	[CustomerID] [varchar](200) NULL,
	[AccountNo] [varchar](20) NULL,
	[FirstName] [varchar](50) NULL,
	[MiddleName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[DOB] [varchar](15) NULL,
	[MobileNo] [varchar](20) NULL,
	[Nationality] [varchar](50) NULL,
	[Gender] [varchar](50) NULL,
	[Marital_Status] [varchar](50) NULL,
	[Passport_IdentificationNO] [varchar](50) NULL,
	[IssueDate] [varchar](15) NULL,
	[PO_Box_P] [varchar](100) NULL,
	[House_No_P] [varchar](100) NULL,
	[StreetName_P] [varchar](100) NULL,
	[Tole_P] [varchar](100) NULL,
	[WardNo_P] [varchar](100) NULL,
	[City_P] [varchar](100) NULL,
	[District_P] [varchar](100) NULL,
	[Phone1_P] [varchar](20) NULL,
	[Phone2_P] [varchar](20) NULL,
	[FAX_P] [varchar](20) NULL,
	[Mobile_P] [varchar](20) NULL,
	[Email_P] [varchar](50) NULL,
	[IsSame_As_CAddr] [varchar](10) NULL,
	[PO_Box_C] [varchar](100) NULL,
	[House_No_C] [varchar](100) NULL,
	[StreetName_C] [varchar](100) NULL,
	[Tole_C] [varchar](100) NULL,
	[WardNo_C] [varchar](100) NULL,
	[City_C] [varchar](100) NULL,
	[District_C] [varchar](100) NULL,
	[Phone1_C] [varchar](20) NULL,
	[Phone2_C] [varchar](20) NULL,
	[FAX_C] [varchar](20) NULL,
	[Mobile_C] [varchar](20) NULL,
	[Email_C] [varchar](50) NULL,
	[Mother_Name] [varchar](200) NULL,
	[Father_Name] [varchar](200) NULL,
	[Grandfather_Name] [varchar](200) NULL,
	[Spouse_Name] [varchar](200) NULL,
	[Account_Type] [varchar](100) NULL,
	[Product_type] [varchar](200) NULL,
	[Name_On_Card] [varchar](100) NULL,
	[Statement_Delivery] [varchar](200) NULL,
	[Email] [varchar](50) NULL,
	[System] [varchar](50) NULL
)
GO
