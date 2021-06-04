-- ======================================================================
-- Name: [config].[uspReadFormulas]
-- Desc: Retorna la formula de los productos
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 06/02/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadFormulas]
    @PrimaryProductID INT
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	F.[FormulaID]
						,F.[PrimaryProductID]
						,F.[IngredientID]
						,I.[IngredientName]
						,IT.[TypeName]
						,F.[Qty]
						,F.[UnitID]
						,U.[UnitName]
						,U.[Symbol]
				FROM	[config].[utbFormulas] F 
						LEFT JOIN [config].[utbIngredients] I ON I.[IngredientID] = F.[IngredientID]
						LEFT JOIN [config].[utbIngredientTypes] IT ON IT.[TypeID] = I.[TypeID]
						LEFT JOIN [config].[utbUnits] U ON U.[UnitID] = F.[UnitID]
				WHERE	F.[PrimaryProductID] = @PrimaryProductID			
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