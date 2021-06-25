-- ======================================================================
-- Name: [sal].[uspReadStagingOrder]
-- Desc: Retorna las información de Ordenes en proceso de pedido
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 06/01/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [sal].[uspReadStagingOrder]
    @StagingOrderID VARCHAR(50)
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT  [StageOrderID]
                        ,[UserID]
                        ,[OrderDate]
                        ,[OrderDetails]
                        ,[DeliveryID]
                FROM    [sal].[utbStagingOrders]
                WHERE   [StageOrderID] = @StagingOrderID
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
