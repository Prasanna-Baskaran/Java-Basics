USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblCustomerAddress]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblCustomerAddress](
	[CustAddressID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [bigint] NOT NULL,
	[PO_Box_P] [varchar](50) NULL,
	[HouseNo_P] [varchar](50) NULL,
	[StreetName_P] [varchar](50) NULL,
	[Tole_P] [varchar](50) NULL,
	[WardNo_P] [varchar](50) NULL,
	[City_P] [varchar](50) NULL,
	[District_P] [varchar](50) NULL,
	[Phone1_P] [varchar](20) NULL,
	[Phone2_P] [varchar](20) NULL,
	[FAX_P] [varchar](50) NULL,
	[Mobile_P] [varchar](50) NULL,
	[Email_P] [varchar](200) NULL,
	[IsSameAsPermAddr] [bit] NULL,
	[PO_Box_C] [varchar](50) NULL,
	[HouseNo_C] [varchar](50) NULL,
	[StreetName_C] [varchar](50) NULL,
	[Tole_C] [varchar](50) NULL,
	[WardNo_C] [varchar](50) NULL,
	[City_C] [varchar](50) NULL,
	[District_C] [varchar](50) NULL,
	[Phone1_C] [varchar](20) NULL,
	[Phone2_C] [varchar](20) NULL,
	[FAX_C] [varchar](50) NULL,
	[Mobile_C] [varchar](20) NULL,
	[Email_C] [varchar](200) NULL,
	[PO_Box_O] [varchar](50) NULL,
	[StreetName_O] [varchar](50) NULL,
	[City_O] [varchar](50) NULL,
	[Phone1_O] [varchar](50) NULL,
	[Phone2_O] [varchar](50) NULL,
	[FAX_O] [varchar](50) NULL,
	[Mobile_O] [varchar](200) NULL,
	[Email_O] [varchar](50) NULL,
	[District_O] [varchar](100) NULL,
	[ModifiedDate_BS] [datetime] NULL,
	[ModifiedDate_AD] [datetime] NULL,
 CONSTRAINT [PK_TblCustomerAddress_CustAddressID] PRIMARY KEY CLUSTERED 
(
	[CustAddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
