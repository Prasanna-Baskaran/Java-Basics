USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[usp_bramha]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--select  dbo.ufn_decryptpan(decpan), pan_encrypted,a.customer_id,date_issued,hold_rsp_code, * 
--from [agss1rt].postcard.dbo.pc_cards_55_a a with(nolock) 
--inner join cardrpan c with(nolock ) on c.encpan=a.pan_encrypted

create proc [dbo].[usp_bramha]

As 
begin

select   dbo.ufn_decryptpan(DecPAN),dbo.ufn_decryptpan(DecAcc),  a.customer_id,pan_encrypted,date_issued,a.hold_rsp_code, * 
from [agss1rt].postcard.dbo.pc_cards_55_a a with(nolock) 
left join [agss1rt].postcard.dbo.pc_card_accounts_55_a b with(nolock) on a.pan=b.pan
left join [agss1rt].postcard.dbo.pc_accounts_55_a c with(nolock) on c.account_id=b.account_id
left join CardRAccounts d (nolock) on d.EncAcc=c.account_id_encrypted
left join CardRPAN e (nolock) on e.EncPAN=a.pan_encrypted
where a.customer_id in --('LAT004MKR0196565')--('RAJESH0000325079')
('BIDHYA1000336519',
'BIDHYA1000352481',
'YASODR0000393315',
'RAJESH0000334578',
'RAJESH0000333710',
'RAJESH0000235099',
'YASODR0000430555',
'BIDHYA1000341694',
'YASODR0000443133',
'BIDHYA1000341240',
'RAJESH0000215661',
'YASODR0000421367',
'RAJESH0000325079',
'BIDHYA1000342974',
'RAJESH0000224211',
'BIDHYA1000351539',
'BIDHYA1000342726',
'BIDHYA1000341996',
'YASODR0000481673',
'BIDHYA1000346065',
'RAJESH0000395835',
'BIDHYA1000336610',
'RAJESH0000227578',
'YASODR0000404496',
'BIDHYA1000344920',
'BIDHYA1000357516',
'BIDHYA1000341326',
'BIDHYA1000410441',
'RAJESH0000272448',
'RAJESH0000241622',
'RAJESH0000332463',
'RAJESH0000246228',
'BIDHYA1000344287',
'BIDHYA1000342635',
'BIDHYA1000372796',
'BIDHYA1000341433',
'BIDHYA1000349692',
'BIDHYA1000343422',
'YASODR0000433709',
'YASODR0000417884',
'RAJESH0000287763',
'KISHOR0000241240',
'YASODR0000515206',
'RAJESH0000290869',
'BIDHYA1000378424',
'YASODR0000440137',
'BIDHYA1000345120',
'RAJESH0000330589',
'BIDHYA1000345126',
'RAJESH0000261859',
'BIDHYA1000341861',
'BIDHYA1000344690',
'RAJESH0000234695',
'YASODR0000402324',
'BIDHYA1000344407',
'BIDHYA1000342303',
'BIDHYA1000347752',
'RAJESH0000278355',
'YASODR0000519358',
'BIDHYA1000341769',
'RAJESH0000222917',
'BIDHYA1000339811',
'BIDHYA1000378729',
'RAJESH0000211623',
'BIDHYA1000364682',
'BIDHYA1000344924',
'RAJESH0000222318',
'RAJESH0000276318',
'RAJESH0000335311',
'LAT004MKR0196565',
'YASODR0000517197',
'YASODR0000482967',
'BIDHYA1000341570',
'BIDHYA1000347255')
order by a.customer_id,a.date_issued

end

--select 74+74
GO
