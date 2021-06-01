-- ======================================================================
-- Name: [sal].[uspReadDeliveryAddressByUser]
-- Desc: Retorna las direcciones de envio
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 05/26/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [sal].[uspReadDeliveryAddressByUser]
    @UserID INT
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	DA.[DeliveryAddressID]
						,DA.[ContactName]
						,CR.[CostaRicaID]
						,CR.[ProvinceID]
						,CR.[Province]
						,CR.[CantonID]
						,CR.[Canton]
						,CR.[DistrictID]
						,CR.[District]
						,DA.[Street]
						,DA.[PhoneNumber]
						,DA.[PrimaryFlag]
				FROM	[sal].[utbDeliveryAddresses] DA
						LEFT JOIN [config].[utbCostaRicaData] CR ON CR.[CostaRicaID] = DA.[CostaRicaID]
				WHERE	[DA].[UserID] = @UserID
						AND DA.[ActiveFlag] = 1
				ORDER BY DA.[PrimaryFlag] DESC
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