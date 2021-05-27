-- ======================================================================
-- Name: [config].[uspReadCRDataCantons]
-- Desc: Retorna los cantones por provincia
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 05/26/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadCRDataCantons]
    @ProvinceID INT
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	DISTINCT
		                [CantonID]
		                ,[Canton]
                FROM	[config].[utbCostaRicaData]
                WHERE	[ProvinceID] = @ProvinceID
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