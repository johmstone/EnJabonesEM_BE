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
						,Products.*
				FROM	[config].[utbPrimaryProducts] PrimaryProducts
						OUTER APPLY (SELECT P.[ProductID]
											,P.[PrimaryProductID]
											,P.[Qty]
											,P.[UnitID]
											,P.[Price]
											,P.[IVA]
											,P.[Discount]
											,P.[ActiveFlag]
											,P.[VisibleFlag]
											,U.[UnitName]
											,U.[Symbol]
									 FROM	[sal].[utbProducts] P
											LEFT JOIN [config].[utbUnits] U ON U.[UnitID] = P.[UnitID]
									 WHERE	P.[PrimaryProductID] = PrimaryProducts.[PrimaryProductID]) Products						
						
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