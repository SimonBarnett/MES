USE [cpl]
GO
/****** Object:  UserDefinedFunction [dbo].[sp_InventoryPack]    Script Date: 04/27/2019 12:48:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Si
-- Create date: 17/04/19
-- Description:	Get an inventory pack as xml
-- =============================================
ALTER FUNCTION [dbo].[sp_InventoryPack]()
RETURNS xml
AS
BEGIN
	RETURN (
		SELECT (
			select 
				DOCUMENTS.DOCNO as "InventoryPackNo",		
				PART.PARTNAME as "Part",
				cast(TRANSORDER.TQUANT as decimal) / 1000 as "Quantity",
				UNIT.UNITNAME as "Units",
				WAREHOUSES.WARHSNAME as "Location",
				'Available' as "InventoryState",
				
				--TODO: Where do these dates come from?				
				CONVERT ( VARCHAR(20), dbo.MINTODATE(DOCUMENTS.CURDATE), 103 ) as "ReceivedDT",
				CONVERT ( VARCHAR(20), dbo.MINTODATE(DOCUMENTS.CURDATE), 103 ) as "UseByDT",
				
				SUPPLIERS.SUPNAME as "Supplier",
				'ERP' as "Reference1",
				'S0001' as "Reference2",
				null as "Reference3",
				null as "Reference4",
				null as "Reference5",
				null as "Reference6",
				null as "Reference7",
				null as "Reference8",
				null as "Reference9",
				null as "Reference10",
				null as "LocationReference1",
				null as "LocationReference2",
				null as "Locationreference3"
			
			from DOCUMENTS
				join TRANSORDER on DOCUMENTS.DOC = TRANSORDER.DOC
				join PART on TRANSORDER.PART = PART.PART
				join UNIT on PART.UNIT = UNIT.UNIT
				join WAREHOUSES on TRANSORDER.TOWARHS = WAREHOUSES.WARHS
				join PARTPARAM on PARTPARAM.PART = PART.PART
				left outer join SUPPLIERS on PARTPARAM.SUP = SUPPLIERS.SUP
				
			where 0=0
				AND DOCUMENTS.TYPE = 'T'
				AND DOCUMENTS.ZCPL_SEND = 'Y'
			
			for XML PATH('InventoryPack'), type 			
		) for XML PATH('InventoryPacks'), type
	)

END

--select [dbo].[sp_InventoryPack]()

/*
update DOCUMENTS set 
	ZCPL_SEND = ''
where 
	ZCPL_SEND = 'Y'
	
update DOCUMENTS set 
	ZCPL_SEND = 'Y',
	ZCPL_SENT = ''
	
where DOC in (

	select top 2 DOCUMENTS.DOC
	from DOCUMENTS 
		JOIN DOCUMENTSA ON DOCUMENTS.DOC = DOCUMENTSA.DOC
		JOIN DOCSTATS ON DOCUMENTSA.ASSEMBLYSTATUS = DOCSTATS.DOCSTAT
		
	where 0=0
		and DOCUMENTS.TYPE = 'T'
		and DOCSTATS.FINALFLAG = 'Y'

)
*/