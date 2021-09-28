-- ======================================================================
-- Name: [config].[uspReadCRDataDistrics]
-- Desc: Retorna los distitos por canton y provincia
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 05/26/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadCRDataDistrics]
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	[CostaRicaID]
                        ,[ProvinceID]
                        ,[CantonID]
		                ,[DistrictID]
		                ,[District]
                        ,[GAMFlag]
                FROM	[config].[utbCostaRicaData]
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