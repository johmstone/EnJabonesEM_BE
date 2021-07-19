-- ======================================================================
-- Name: [sal].[uspReadOrderSummary]
-- Desc: Retorna un resumen de la lista de ordenes
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 07/08/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [sal].[uspReadOrderSummary]
	@UserID		INT         = NULL,
	@StartDate  DATETIME	= NULL,
	@EndDate	DATETIME	= NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	DISTINCT 
                        [StatusID]          = OS.[ExternalStatusID]
                        ,[InternalStatus]   = OS.[ExternalStatus]
                        ,OS.[ExternalStatusID]
                        ,OS.[ExternalStatus]
                        ,[QtyOrders]        = TOT.[Qty]

                FROM	[sal].[utbOrderStatus] OS 
                        OUTER APPLY (SELECT [Qty] = COUNT(O.[OrderID]) 
                                     FROM	[sal].[utbOrders] O
                                            LEFT JOIN [sal].[utbOrderStatus] ES ON ES.[StatusID] = O.[StatusID]
                                     WHERE	ES.[ExternalStatusID] = OS.[ExternalStatusID]
                                            AND O.[ActiveFlag] = 1
                                            AND O.[UserID] = ISNULL(@UserID,O.[UserID])
                                            AND O.[InsertDate] >= ISNULL(@StartDate,O.[InsertDate])
                                            AND CONVERT(DATE,O.[InsertDate]) <= ISNULL(@EndDate,O.[InsertDate])) TOT
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