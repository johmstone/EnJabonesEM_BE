-- ======================================================================
-- Name: [config].[uspReadPrimaryProducts]
-- Desc: Retorna las Productos Primarios
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 06/02/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadPrimaryProducts]
    @PrimaryProductID INT = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	PrimaryProducts.[PrimaryProductID]
						,PrimaryProducts.[Name]
						,PrimaryProducts.[Description]
						,PrimaryProducts.[Technique]
						,PrimaryProducts.[PhotoURL]
						,PrimaryProducts.[BrochureURL]
						,PrimaryProducts.[ActiveFlag]
						,PrimaryProducts.[VisibleFlag]
						,Products.[ProductID]
						,Products.[PrimaryProductID]
						,Products.[Qty]
						,Products.[UnitID]
						,Products.[Price]
						,Products.[IVA]
						,Products.[Discount]
						,Products.[ActiveFlag]
						,Products.[VisibleFlag]
						,Unit.[UnitID]
						,Unit.[UnitName]
						,Unit.[Symbol]
				FROM	[config].[utbPrimaryProducts] PrimaryProducts
						LEFT JOIN [sal].[utbProducts] Products ON Products.[PrimaryProductID] = PrimaryProducts.[PrimaryProductID]
						LEFT JOIN [config].[utbUnits] Unit ON Unit.[UnitID] = Products.[UnitID]
				WHERE	PrimaryProducts.[PrimaryProductID] = ISNULL(@PrimaryProductID,PrimaryProducts.[PrimaryProductID])
				FOR JSON AUTO
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