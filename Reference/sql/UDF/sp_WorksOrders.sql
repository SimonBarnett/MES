USE [cpl]
GO
/****** Object:  UserDefinedFunction [dbo].[sp_WorksOrders]    Script Date: 04/20/2019 11:45:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Si
-- Create date: 17/04/19
-- Description:	output WorksOrders as xml
-- =============================================
ALTER FUNCTION [dbo].[sp_WorksOrders]()
RETURNS xml
AS
BEGIN
	
	RETURN (
		SELECT (
			select 
				SERIALNAME as "WONumber", 
				'Production' as "WOType", 
				PART.PARTNAME as "Part", 
				SERIAL.QUANT / 1000 as "QuantityOrdered", 
				'12/08/2019' as "PlannedStartDT", 
				'ERP' as "Reference1", 
				'' as "Reference2", 
				'' as "Reference3", 
				'' as "Reference4", 
				'' as "Reference5", 
				'STD' as "BOMVariant", 
				'1234' as "RoutingCode"
			
			from SERIAL 
				join PART on SERIAL.PART = PART.PART
				join SERIALA on SERIAL.SERIAL = SERIALA.SERIAL
				join SERIALSTATUS on SERIALA.SERIALSTATUS = SERIALSTATUS.SERIALSTATUS
				
			where 0=0
				AND SERIAL.CLOSED <> 'C'
				AND SERIAL.SERIAL > 0
				AND SERIALSTATUS.RELEASED = 'Y'
				
			/* TODO:
				and SERIAL.SEND = 1
				and SERIAL.SENT = 0
			*/
			
			for XML PATH('WorksOrder'), type 
		) for XML PATH('WorksOrders'), type
	)

END

--SELECT dbo.sp_WorksOrders()