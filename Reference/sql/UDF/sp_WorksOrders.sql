USE [cpl]
GO
/****** Object:  UserDefinedFunction [dbo].[sp_WorksOrders]    Script Date: 04/27/2019 12:26:38 ******/
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
				SERIAL.SERIALNAME as "WONumber", 
				'Production' as "WOType", 
				PART.PARTNAME as "Part", 
				SERIAL.QUANT / 1000 as "QuantityOrdered", 
				CONVERT ( VARCHAR(20), dbo.MINTODATE(SERIAL.RELEASEDATE), 103 ) as "PlannedStartDT", 
				'ERP' as "Reference1", 
				NULL as "Reference2", 
				NULL as "Reference3", 
				NULL as "Reference4", 
				NULL as "Reference5", 
				REVISIONS.REVNUM as "BOMVariant", 
				PROCESS.PROCNAME as "RoutingCode"
			
			from SERIAL 
				join PART on SERIAL.PART = PART.PART
				join SERIALA on SERIAL.SERIAL = SERIALA.SERIAL
				join SERIALSTATUS on SERIALA.SERIALSTATUS = SERIALSTATUS.SERIALSTATUS
				join PROCESS on SERIAL.PRODSERIAL = PROCESS.[T$PROC]
				join REVISIONS on SERIAL.REV = REVISIONS.REV
				
			where 0=0
				AND SERIAL.SERIAL > 0
				AND SERIAL.ZCPL_SEND = 'Y'
				AND SERIAL.ZCPL_SENT = ''								
				
				/* Don't send closed or unreleased */
				AND SERIAL.CLOSED <> 'C'				
				AND SERIALSTATUS.RELEASED = 'Y'
										
			for XML PATH('WorksOrder'), type 
		) for XML PATH('WorksOrders'), type
	)

END

--SELECT dbo.sp_WorksOrders()

/*
-- Mark Serial ZCPL_SEND = 'Y' 
UPDATE SERIAL set 
	ZCPL_SEND = 'Y',
	ZCPL_SENT = ''
	
where SERIAL.SERIAL in (
	select TOP 5 SERIAL.SERIAL						
		from SERIAL 
			join PART on SERIAL.PART = PART.PART
			join SERIALA on SERIAL.SERIAL = SERIALA.SERIAL
			join SERIALSTATUS on SERIALA.SERIALSTATUS = SERIALSTATUS.SERIALSTATUS
			
		where 0=0
			AND SERIAL.SERIAL > 0
			--AND SERIAL.ZCPL_SEND = 'Y'								
			/* Don't send cloased or unreleased */
			AND SERIAL.CLOSED <> 'C'				
			AND SERIALSTATUS.RELEASED = 'Y'
)
*/