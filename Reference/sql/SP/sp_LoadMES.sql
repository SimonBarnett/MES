USE [cpl]
GO
/****** Object:  StoredProcedure [dbo].[sp_LoadMES]    Script Date: 04/20/2019 11:44:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Si
-- Create date: 20/04/19
-- Description:	Load outbound transactions from MES.
-- =============================================
ALTER PROCEDURE [dbo].[sp_LoadMES] AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @response xml
	declare @hdoc int
	DECLARE @url VARCHAR(MAX)

	BEGIN TRY
		select @response = 
			dbo.postxml(
				dbo.udf_APIURL() + 'obt.ashx', 
				dbo.getxml(dbo.udf_APIURL() + 'trans.ashx')
			)
		EXEC sp_xml_preparedocument @hdoc OUTPUT, @response

		select	
			[status],
			[message]
		FROM OPENXML(@hdoc, 'response',8)
		with (
			[status] int,
			[message] varchar(max),
			[stacktr] varchar(max)
		)
		EXEC sp_xml_removedocument @hdoc
		
	END TRY

	BEGIN CATCH
		SELECT 500, ERROR_MESSAGE()

	END CATCH


END
