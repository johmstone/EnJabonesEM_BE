-- ======================================================================
-- Name: [sal].[uspReadOrders]
-- Desc: Retorna las Productos disponibles
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 07/08/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [sal].[uspReadOrders]
	@OrderID	VARCHAR(50)	= NULL,
	@UserID		INT			= NULL,
	@StartDate	DATETIME	= NULL,
	@EndDate	DATETIME	= NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	O.[OrderID]
						,O.[StageOrderID]
						,O.[UserID]
						,U.[FullName]
						,O.[OrderType]
						,O.[OrderDate]
						,O.[DeliveryID]
						,O.[DeliveryAddressID]
						,O.[FacturationInfoID]
						,O.[OrderDetails]
						,O.[Discount]
						,O.[TotalCart]
						,O.[TotalDelivery]
						,O.[StatusID]
						,OS.[InternalStatus]
						,OS.[ExternalStatus]
						,O.[ProofPayment]
						,O.[OrderVerified]
						,O.[ActiveFlag]
						,O.[InsertDate]
				FROM	[sal].[utbOrders] O
						LEFT JOIN [adm].[utbUsers] U ON U.[UserID] = O.[UserID]
						LEFT JOIN [sal].[utbOrderStatus] OS ON OS.[StatusID] = O.[StatusID]
				WHERE   O.[OrderID] = ISNULL(@OrderID,O.[OrderID])
						AND O.[UserID] = ISNULL(@UserID,O.[UserID])
						AND O.[InsertDate] >= ISNULL(@StartDate,O.[InsertDate])
						AND O.[InsertDate] <= ISNULL(@EndDate,O.[InsertDate])
				ORDER	BY O.[InsertDate] DESC, U.[FullName], O.[OrderType]
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