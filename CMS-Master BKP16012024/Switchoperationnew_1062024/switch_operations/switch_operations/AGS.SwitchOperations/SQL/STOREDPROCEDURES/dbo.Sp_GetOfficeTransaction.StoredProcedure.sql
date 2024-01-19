USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_GetOfficeTransaction]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_GetOfficeTransaction] 
AS
/*Change Managment
created by : Prerna Patil
Created date: 12/07/2017
Created Reason: Get transaction from office

Execution

--Dev linked server [10.10.0.21]-> [10.10.0.21]

*/ 
BEGIN
	Begin Try  
 Declare @StrStatusCode Varchar(3) ,@StrStatusDesc Varchar(2000) 
 declare @post_tran_id bigint
set @post_tran_id=(SELECT Isnull(MAX(post_tran_id),0) FROM [post_tran_leg_internal]  with(nolock))
 
INSERT INTO [post_tran_leg_internal]
([post_tran_id],[post_tran_cust_id],[settle_entity_id],[batch_nr],[prev_post_tran_id_fast],[next_post_tran_id_fast],[sink_node_name],[tran_postilion_originated]
,[message_type],[tran_type],[tran_nr],[system_trace_audit_nr],[rsp_code_req],[rsp_code_rsp],[abort_rsp_code],[auth_id_rsp],[auth_type],[auth_reason_char]
,[retention_data],[acquiring_inst_id_code],[message_reason_code],[sponsor_bank],[retrieval_reference_nr],[datetime_tran_gmt],[datetime_tran_local]
,[datetime_req],[datetime_rsp],[realtime_business_date],[recon_business_date],[from_account_type],[to_account_type],[from_account_id],[to_account_id]
,[tran_amount_req],[tran_amount_rsp],[settle_amount_impact],[tran_cash_req],[tran_cash_rsp],[tran_currency_code],[tran_tran_fee_req],[tran_tran_fee_rsp]
,[tran_tran_fee_currency_code],[tran_proc_fee_req],[tran_proc_fee_rsp],[tran_proc_fee_currency_code],[settle_amount_req],[settle_amount_rsp],[settle_cash_req]
,[settle_cash_rsp],[settle_tran_fee_req],[settle_tran_fee_rsp],[settle_proc_fee_req],[settle_proc_fee_rsp],[settle_currency_code],[icc_data_req]
,[icc_data_rsp],[pos_entry_mode],[pos_condition_code],[additional_rsp_data],[structured_data_req],[structured_data_rsp],[tran_reversed],[prev_tran_approved]
,[issuer_network_id],[acquirer_network_id],[extended_tran_type],[ucaf_data],[from_account_type_qualifier],[to_account_type_qualifier],[bank_details]
,[payee],[card_verification_result],[online_system_id],[participant_id],[opp_participant_id],[receiving_inst_id_code],[routing_type],[source_node_key]
,[proc_online_system_id],[pos_geographic_data],[payer_account_id],[cvv_available_at_auth],[cvv2_available_at_auth],[network_program_id_actual]
,[network_program_id_min],[network_fee_actual],[network_fee_min],[network_fee_max],[credit_debit_conversion],[tran_nr_prev],[source_node_name]
,[draft_capture],[pan],[card_seq_nr],[expiry_date],[service_restriction_code],[terminal_id],[terminal_owner],[card_acceptor_id_code],[mapped_card_acceptor_id_code]
,[merchant_type],[card_acceptor_name_loc],[address_verification_data],[address_verification_result],[check_data],[totals_group],[card_product],[pos_card_data_input_ability]
,[pos_cardholder_auth_ability],[pos_card_capture_ability],[pos_operating_environment],[pos_cardholder_present],[pos_card_present],[pos_card_data_input_mode]
,[pos_cardholder_auth_method],[pos_cardholder_auth_entity],[pos_card_data_output_ability],[pos_terminal_output_ability],[pos_pin_capture_ability],[pos_terminal_operator]
,[pos_terminal_type],[pan_search],[pan_encrypted],[pan_reference],[mapped_terminal_id],[mapped_extd_ca_term_id],[mapped_extd_ca_id_code],[secure_3d_result]
,[pos_data],[card_acceptor_phone_nr],[amount_available],[ledger_balance])
SELECT  [post_tran_id],[post_tran_cust_id],[settle_entity_id],[batch_nr],[prev_post_tran_id_fast],[next_post_tran_id_fast],[sink_node_name],[tran_postilion_originated]
,[message_type],[tran_type],[tran_nr],[system_trace_audit_nr],[rsp_code_req],[rsp_code_rsp],[abort_rsp_code],[auth_id_rsp],[auth_type],[auth_reason_char]
,[retention_data],[acquiring_inst_id_code],[message_reason_code],[sponsor_bank],[retrieval_reference_nr],[datetime_tran_gmt],[datetime_tran_local]
,[datetime_req],[datetime_rsp],[realtime_business_date],[recon_business_date],[from_account_type],[to_account_type],[from_account_id],[to_account_id]
,[tran_amount_req],[tran_amount_rsp],[settle_amount_impact],[tran_cash_req],[tran_cash_rsp],[tran_currency_code],[tran_tran_fee_req],[tran_tran_fee_rsp]
,[tran_tran_fee_currency_code],[tran_proc_fee_req],[tran_proc_fee_rsp],[tran_proc_fee_currency_code],[settle_amount_req],[settle_amount_rsp],[settle_cash_req]
,[settle_cash_rsp],[settle_tran_fee_req],[settle_tran_fee_rsp],[settle_proc_fee_req],[settle_proc_fee_rsp],[settle_currency_code],[icc_data_req]
,[icc_data_rsp],[pos_entry_mode],[pos_condition_code],[additional_rsp_data],[structured_data_req],[structured_data_rsp],[tran_reversed],[prev_tran_approved]
,[issuer_network_id],[acquirer_network_id],[extended_tran_type],[ucaf_data],[from_account_type_qualifier],[to_account_type_qualifier],[bank_details]
,[payee],[card_verification_result],[online_system_id],[participant_id],[opp_participant_id],[receiving_inst_id_code],[routing_type],[source_node_key]
,[proc_online_system_id],[pos_geographic_data],[payer_account_id],[cvv_available_at_auth],[cvv2_available_at_auth],[network_program_id_actual]
,[network_program_id_min],[network_fee_actual],[network_fee_min],[network_fee_max],[credit_debit_conversion],[tran_nr_prev],[source_node_name]
,[draft_capture],[pan],[card_seq_nr],[expiry_date],[service_restriction_code],[terminal_id],[terminal_owner],[card_acceptor_id_code],[mapped_card_acceptor_id_code]
,[merchant_type],[card_acceptor_name_loc],[address_verification_data],[address_verification_result],[check_data],[totals_group],[card_product],[pos_card_data_input_ability]
,[pos_cardholder_auth_ability],[pos_card_capture_ability],[pos_operating_environment],[pos_cardholder_present],[pos_card_present],[pos_card_data_input_mode]
,[pos_cardholder_auth_method],[pos_cardholder_auth_entity],[pos_card_data_output_ability],[pos_terminal_output_ability],[pos_pin_capture_ability],[pos_terminal_operator]
,[pos_terminal_type],[pan_search],[pan_encrypted],[pan_reference],[mapped_terminal_id],[mapped_extd_ca_term_id],[mapped_extd_ca_id_code],[secure_3d_result]
,[pos_data],[card_acceptor_phone_nr],[amount_available],[ledger_balance]
FROM AGSOFFICE.postilion_office.dbo.[post_tran_leg_internal] A WITH (NOLOCK) 
WHERE post_tran_id>(@post_tran_id)
And (source_node_name in('NIBLSrc','YSESrc','MobileAppSrc','ePrabhuSrc','CLdSrcPRRem') OR  Sink_node_name in ('NIBLSink','YSESink','PBLMIDSnk','PrabhuCBSSnk','PBLCoCardSnk'))
--AND source_node_name In ('RBPOSHypSrc','FBPOSHypSrc','INDPOSHypSrc','ONGOPOSSrc','MVISAIIDSrc','MVISAIID2Src','AGSPGSrc','MCSMONEYSSrc')
--partition
--syncnode

    End Try  
	 BEGIN CATCH 
	 --RollBACK TRANSACTION; 		
	  ExceptionErrorLog:
			--INSERT INTO TblCardAutomationErrorLog(Function_Name,Error_Desc,Error_Date,ParameterList,IssuerNo)                 
		 -- SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE(),'IssuerNumber='+ convert(varchar(5),@IntIssuerNo),@IntIssuerNo
		     SET @strStatusCode='999'        
			 SET @StrStatusDesc='Unexpected error occurred' 
	END CATCH; 
	SELECT @strStatusCode AS [STATUSCODE] ,@strStatusDesc  AS [STATUSDESC]
END

GO
