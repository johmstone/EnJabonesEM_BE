-- ======================================================================
-- Name: [sal].[uspReadOrderHistory]
-- Desc: Retorna las actividades de una orden
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 07/08/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [sal].[uspReadOrderHistory]
	@OrderID			VARCHAR(50),
    @ActivityType       VARCHAR(10)
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
                IF(@ActivityType = 'INTERNAL')
                    BEGIN
				        SELECT	SH.[StatusID]
		                        ,OS.[InternalStatus]
		                        ,OS.[ExternalStatusID]
		                        ,OS.[ExternalStatus]
		                        ,SH.[OrderVerified]
		                        ,SH.[ActivityDate]
		                        ,[InsertUser] = CASE WHEN U.[FullName] IS NOT NULL THEN U.[FullName] ELSE 'System User' END
                        FROM	[sal].[utbOrdersHistory] SH
		                        LEFT JOIN [sal].[utbOrderStatus] OS ON OS.[StatusID] = SH.[StatusID]
								LEFT JOIN [adm].[utbUsers] U ON U.[Email] = SH.[InsertUser]
                        WHERE	SH.[OrderID] = @OrderID
                        ORDER	BY [ActivityDate]
                    END
                ELSE
                    BEGIN
                        SELECT	[Row]				= ROW_NUMBER() OVER (PARTITION BY OS.[ExternalStatusID] ORDER BY SH.[ActivityDate])
		                        ,[StatusID]			= OS.[ExternalStatusID]
		                        ,[InternalStatus]	= OS.[ExternalStatus]
		                        ,OS.[ExternalStatusID]
		                        ,OS.[ExternalStatus]
		                        ,SH.[OrderVerified]
		                        ,SH.[ActivityDate]
		                        ,[InsertUser] = CASE WHEN U.[FullName] IS NOT NULL THEN U.[FullName] ELSE 'System User' END
                        INTO	#MainData
                        FROM	[sal].[utbOrdersHistory] SH
		                        LEFT JOIN [sal].[utbOrderStatus] OS ON OS.[StatusID] = SH.[StatusID]
								LEFT JOIN [adm].[utbUsers] U ON U.[Email] = SH.[InsertUser]
                        WHERE	SH.[OrderID] = @OrderID
                        ORDER	BY [ActivityDate]

                        SELECT	MD.[StatusID]
		                        ,MD.[InternalStatus]
		                        ,MD.[ExternalStatusID]
		                        ,MD.[ExternalStatus]
		                        ,MD.[OrderVerified]
		                        ,MD.[ActivityDate]
		                        ,MD.[InsertUser]
                        FROM	#MainData MD
                        WHERE	MD.[Row] = 1

                        DROP TABLE #MainData
                    END
			-- =======================================================

        END TRY
        BEGIN CATCH

            SELECT  @lErrorMessage = ERROR_MESSAGE() ,
                    @lErrorSeverity = ERROR_SEVERITY() ,
                    @lErrorState = ERROR_STATE()       

            RAISERROR (@lErrorMessage, @lErrorSeverity, @lErrorState);
        END CATCH
    END
    SET NOCOUNT OFF