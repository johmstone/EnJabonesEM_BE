-- ======================================================================
-- Name: [config].[uspReadIngredients]
-- Desc: Retorna la lista de ingredientes de los productos
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 06/02/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadIngredients]
	@TypeID INT = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	I.[IngredientID]
		                ,I.[IngredientName]
		                ,I.[TypeID]
		                ,T.[TypeName]
		                ,I.[PhotoURL]
                FROM	[config].[utbIngredients] I
		                LEFT JOIN [config].[utbIngredientTypes] T ON T.[TypeID] = I.[TypeID]		
				WHERE	I.[TypeID] = ISNULL(@TypeID, I.[TypeID])
                ORDER BY T.[TypeName], I.[IngredientName]
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