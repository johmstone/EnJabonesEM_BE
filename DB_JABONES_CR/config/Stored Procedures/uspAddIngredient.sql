-- ======================================================================
-- Name: [config].[uspAddIngredient]
-- Desc: Se utiliza para agregar Ingredientes
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 06/02/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspAddIngredient]
	@InsertUser		VARCHAR(50),
    @IngredientName VARCHAR(100),
    @TypeID         INT,
    @PhotoURL       VARCHAR(500) = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        SET XACT_ABORT ON
                           
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT
            DECLARE @lLocalTran BIT = 0
                               
            IF @@TRANCOUNT = 0 
                BEGIN
                    BEGIN TRANSACTION
                    SET @lLocalTran = 1
                END

            -- =======================================================
                DECLARE @Result BIT
                IF EXISTS (SELECT * FROM [config].[utbIngredients] WHERE [IngredientName] = @IngredientName AND [TypeID] = @TypeID)
                    BEGIN
                        SET @Result = 0
                    END
                ELSE 
                    BEGIN
                        INSERT INTO	[config].[utbIngredients] ([IngredientName],[TypeID],[PhotoURL],[InsertDate],[InsertUser],[LastModifyDate],[LastModifyUser])
                        VALUES (@IngredientName, @TypeID, @PhotoURL, GETDATE(), @InsertUser, GETDATE(), @InsertUser)            
                        
                        SET @Result = 1
                    END
                SELECT [Result] = @Result
			-- =======================================================

        IF ( @@trancount > 0
                 AND @lLocalTran = 1
               ) 
                BEGIN
                    COMMIT TRANSACTION
                END
        END TRY
        BEGIN CATCH
            IF ( @@trancount > 0
                 AND XACT_STATE() <> 0
               ) 
                BEGIN
                    ROLLBACK TRANSACTION
                END

            SELECT  @lErrorMessage = ERROR_MESSAGE() ,
                    @lErrorSeverity = ERROR_SEVERITY() ,
                    @lErrorState = ERROR_STATE()       

            RAISERROR (@lErrorMessage, @lErrorSeverity, @lErrorState);
        END CATCH
    END

    SET NOCOUNT OFF
    SET XACT_ABORT OFF