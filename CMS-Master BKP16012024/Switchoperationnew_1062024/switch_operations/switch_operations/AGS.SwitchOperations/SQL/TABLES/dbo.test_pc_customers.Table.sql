USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[test_pc_customers]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[test_pc_customers](
	[issuer_nr] [int] NOT NULL,
	[customer_id] [varchar](25) NOT NULL,
	[national_id_nr] [varchar](66) NULL,
	[c1_title] [varchar](10) NULL,
	[c1_first_name] [varchar](100) NULL,
	[c1_initials] [varchar](10) NULL,
	[c1_last_name] [varchar](100) NULL,
	[c1_name_on_card] [varchar](100) NULL,
	[c2_title] [varchar](10) NULL,
	[c2_first_name] [varchar](100) NULL,
	[c2_initials] [varchar](10) NULL,
	[c2_last_name] [varchar](100) NULL,
	[c2_name_on_card] [varchar](100) NULL,
	[c3_title] [varchar](10) NULL,
	[c3_first_name] [varchar](100) NULL,
	[c3_initials] [varchar](10) NULL,
	[c3_last_name] [varchar](100) NULL,
	[c3_name_on_card] [varchar](100) NULL,
	[tel_nr] [varchar](50) NULL,
	[mobile_nr] [varchar](50) NULL,
	[fax_nr] [varchar](50) NULL,
	[email_address] [varchar](320) NULL,
	[postal_address_1] [varchar](100) NULL,
	[postal_address_2] [varchar](100) NULL,
	[postal_city] [varchar](40) NULL,
	[postal_region] [varchar](20) NULL,
	[postal_code] [varchar](20) NULL,
	[postal_country] [char](3) NULL,
	[other_address_1] [varchar](100) NULL,
	[other_address_2] [varchar](100) NULL,
	[other_city] [varchar](40) NULL,
	[other_region] [varchar](20) NULL,
	[other_postal_code] [varchar](20) NULL,
	[other_country] [char](3) NULL,
	[date_of_birth] [char](8) NULL,
	[company_name] [varchar](26) NULL,
	[preferred_lang] [varchar](5) NULL,
	[vip] [int] NOT NULL,
	[vip_lapse_date] [datetime] NULL,
	[last_updated_date] [datetime] NOT NULL,
	[last_updated_user] [varchar](20) NOT NULL,
	[date_deleted] [datetime] NULL,
	[extended_fields] [text] NULL,
	[national_id_nr_encrypted] [varchar](70) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
