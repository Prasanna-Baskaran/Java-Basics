USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[test_pc_cards]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[test_pc_cards](
	[issuer_nr] [int] NOT NULL,
	[pan] [varchar](66) NOT NULL,
	[seq_nr] [char](3) NOT NULL,
	[card_program] [varchar](20) NOT NULL,
	[default_account_type] [char](2) NOT NULL,
	[card_status] [int] NOT NULL,
	[card_custom_state] [int] NULL,
	[expiry_date] [varchar](4) NOT NULL,
	[hold_rsp_code] [char](2) NULL,
	[track2_value] [varchar](20) NULL,
	[track2_value_offset] [int] NULL,
	[pvki_or_pin_length] [int] NULL,
	[pvv_or_pin_offset] [varchar](12) NULL,
	[pvv2_or_pin2_offset] [varchar](12) NULL,
	[validation_data_question] [varchar](50) NULL,
	[validation_data] [varchar](50) NULL,
	[cardholder_rsp_info] [varchar](50) NULL,
	[mailer_destination] [int] NOT NULL,
	[discretionary_data] [varchar](13) NULL,
	[date_issued] [datetime] NULL,
	[date_activated] [datetime] NULL,
	[issuer_reference] [varchar](20) NULL,
	[branch_code] [varchar](10) NULL,
	[last_updated_date] [datetime] NOT NULL,
	[last_updated_user] [varchar](20) NOT NULL,
	[customer_id] [varchar](25) NULL,
	[batch_nr] [int] NULL,
	[company_card] [int] NULL,
	[date_deleted] [datetime] NULL,
	[pvki2_or_pin2_length] [int] NULL,
	[extended_fields] [text] NULL,
	[expiry_day] [char](2) NULL,
	[from_date] [char](4) NULL,
	[from_day] [char](2) NULL,
	[contactless_disc_data] [varchar](13) NULL,
	[dcvv_key_index] [int] NULL,
	[pan_encrypted] [varchar](70) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
