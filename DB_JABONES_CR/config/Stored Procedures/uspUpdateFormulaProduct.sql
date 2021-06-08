-- ======================================================================
-- Name: [config].[uspUpdateFormulaProduct]
-- Desc: Se utiliza para actualizar Formulas
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 06/02/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspUpdateFormulaProduct]
	@InsertUser		VARCHAR(50),
    @JSON           NVARCHAR(MAX)
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
                DECLARE	@Data TABLE ([PrimaryProductID] INT, [IngredientID] INT, [Qty] NUMERIC(5,2), [UnitID] INT)
                DECLARE	@PrimaryProductID	INT
		                ,@IngredientID		INT
		                ,@Qty				NUMERIC(5,2)
		                ,@UnitID			INT

                INSERT INTO @Data
                SELECT	* FROM OPENJSON (@JSON)  
                WITH (   
		                PrimaryProductID	INT				'$.PrimaryProductID',  
                        IngredientID		INT				'$.IngredientID',  
                        Qty					NUMERIC (5,2)	'$.Qty',
		                UnitID				INT				'$.UnitID'
	                ) 


				DELETE FROM [config].[utbFormulas]
				WHERE [PrimaryProductID] = (SELECT TOP 1 [PrimaryProductID] FROM @Data)

                DECLARE ActionProcess CURSOR FOR 
                SELECT * FROM @Data

                OPEN ActionProcess
                FETCH NEXT FROM ActionProcess INTO @PrimaryProductID, @IngredientID, @Qty , @UnitID

                WHILE @@FETCH_STATUS = 0
	                BEGIN 
						INSERT INTO [config].[utbFormulas] ([PrimaryProductID],[IngredientID],[Qty],[UnitID],[InsertDate],[InsertUser],[LastModifyDate],[LastModifyUser])
		                VALUES (@PrimaryProductID, @IngredientID, @Qty, @UnitID, GETDATE(), @InsertUser, GETDATE(), @InsertUser)
		                FETCH NEXT FROM ActionProcess INTO @PrimaryProductID, @IngredientID, @Qty , @UnitID
	                END

                CLOSE ActionProcess
                DEALLOCATE ActionProcess
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