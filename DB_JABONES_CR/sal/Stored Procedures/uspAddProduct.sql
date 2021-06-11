-- ======================================================================
-- Name: [sal].[uspAddProduct]
-- Desc: Se utiliza para agregar Productos
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 06/02/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [sal].[uspAddProduct]
	@InsertUser			VARCHAR(50),
    @PrimaryProductID	INT,
	@Qty				NUMERIC(5,2),
	@UnitID				INT,
	@Price				NUMERIC(8,2),
	@IVA				NUMERIC(5,2),
	@Discount			NUMERIC(5,2)
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
                INSERT INTO [sal].[utbProducts] ([PrimaryProductID],[Qty],[UnitID],[Price],[IVA],[Discount],[InsertUser],[LastModifyUser])
				VALUES (@PrimaryProductID, @Qty, @UnitID, @Price, @IVA, @Discount, @InsertUser, @InsertUser)                
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