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
	@OrderID			VARCHAR(50)	= NULL,
	@UserID				INT			= NULL,
	@StatusID			INT			= NULL,
	@ExternalStatusID	INT			= NULL,
	@StartDate			DATETIME	= NULL,
	@EndDate			DATETIME	= NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	O.[OrderID]
						,O.[UserID]
						,[FullName]			= ISNULL(U.[FullName],FI.[FullName])
						,O.[OrderType]
						,O.[OrderDate]
						,O.[DeliveryID]
						,[DeliveryMethod]	= DM.[Method]
						,O.[DeliveryAddressID]
						,O.[FacturationInfoID]
						,O.[OrderDetails]
						,O.[Discount]
						,O.[TotalCart]
						,O.[TotalDelivery]
						,O.[StatusID]
						,OS.[InternalStatus]
						,OS.[ExternalStatusID]
						,OS.[ExternalStatus]
						,O.[ProofPayment]
						,O.[OrderVerified]
						,O.[ActiveFlag]
						,O.[InsertDate]
				FROM	[sal].[utbOrders] O
						LEFT JOIN [adm].[utbUsers] U ON U.[UserID] = O.[UserID]
						LEFT JOIN [sal].[utbDeliveryMethods] DM ON DM.[DeliveryID] = O.[DeliveryID]
						LEFT JOIN [sal].[utbFacturationInfo] FI ON FI.[FacturationInfoID] = O.[FacturationInfoID]
						LEFT JOIN [sal].[utbOrderStatus] OS ON OS.[StatusID] = O.[StatusID]
				WHERE	O.[OrderID] = ISNULL(@OrderID,O.[OrderID])
						AND O.[UserID] = ISNULL(@UserID,O.[UserID])
						AND O.[InsertDate] >= ISNULL(@StartDate,O.[InsertDate])
						AND CONVERT(DATE,O.[InsertDate]) <= ISNULL(@EndDate,O.[InsertDate])
						AND O.[StatusID] = ISNULL(@StatusID,O.[StatusID])
						AND OS.[ExternalStatusID] = ISNULL(@ExternalStatusID,OS.[ExternalStatusID])
				ORDER	BY 
						O.[InsertDate] DESC
						,[FullName]
						,[OrderType]
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