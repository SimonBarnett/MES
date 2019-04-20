USE [cpl]
GO
/****** Object:  UserDefinedFunction [dbo].[sp_InventoryPack]    Script Date: 04/20/2019 11:45:37 ******/
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
				'1' as "InventoryPackNo",		
				'' as "Part",
				'1' as "Quantity",
				'ea' as "Units",
				'RHC1' as "Location",
				'Available' as "InventoryState",
				'09/11/2019' as "ReceivedDT",
				'12/12/2019' as "UseByDT",
				'S12345' as "Supplier",
				'ERP' as "Reference1",
				'S0001' as "Reference2",
				'' as "Reference3",
				'' as "Reference4",
				'' as "Reference5",
				'' as "Reference6",
				'' as "Reference7",
				'' as "Reference8",
				'' as "Reference9",
				'' as "Reference10",
				'' as "LocationReference1",
				'' as "LocationReference2",
				'' as "Locationreference3"
			
			for XML PATH('InventoryPack'), type 
		) for XML PATH('InventoryPacks'), type
	)

END
