USE [cpl]
GO
/****** Object:  StoredProcedure [dbo].[sp_MESWorksOrders]    Script Date: 04/24/2019 15:13:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Si
-- Create date: 20/04/19
-- Description:	Post Works Orders
--				from dbo.sp_WorksOrders()
-- =============================================
ALTER PROCEDURE [dbo].[sp_MESWorksOrders]
AS
BEGIN	
	SET NOCOUNT ON;

	declare @response xml
	declare @hdoc int

	BEGIN TRY

		select @response = dbo.postxml(
			dbo.udf_APIURL() + 'mes.ashx', 
			dbo.sp_WorksOrders()
		)
		EXEC sp_xml_preparedocument @hdoc OUTPUT, @response

		declare @status int
		select	
			@status = [status]
		FROM OPENXML(@hdoc, 'response',8)
			with (
				[status] int,
				[message] varchar(max),
				[stacktr] varchar(max)
			)
		
		if (@status = 200)			
			begin
				update SERIAL set
					ZCPL_SENT = 'Y',
					ZCPL_SEND = ''
					
				WHERE SERIALNAME IN (				
					select	
						[id]
					FROM OPENXML(@hdoc, 'response/row',8)
					with (
						[id] varchar(max),
						[sucsess] varchar(max),
						[msg] varchar(max)
					)
					where [sucsess] = 'True'
				)
			end	
			
		EXEC sp_xml_removedocument @hdoc
		
	END TRY

	BEGIN CATCH
		SELECT 500, ERROR_MESSAGE()

	END CATCH


END

--EXEC dbo.[sp_MESWorksOrders]