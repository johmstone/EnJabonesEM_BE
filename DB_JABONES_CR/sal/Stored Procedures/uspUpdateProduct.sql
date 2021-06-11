-- ======================================================================
-- Name: [sal].[uspUpdateProduct]
-- Desc: Se utiliza para actualizar Productos
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 06/02/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [sal].[uspUpdateProduct]
	@InsertUser			VARCHAR(50),
    @ProductID			INT,
	@ActionType			VARCHAR(10),
	@Qty				NUMERIC(5,2) = NULL,
	@UnitID				INT = NULL,
	@Price				NUMERIC(8,2) = NULL,
	@IVA				NUMERIC(5,2) = NULL,
	@Discount			NUMERIC(5,2) = NULL,
	@VisibleFlag        BIT = NULL
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
                DECLARE @ActiveFlag BIT
				IF(@ActionType = 'CHGST')
                    BEGIN
                        SELECT  @ActiveFlag = [ActiveFlag]
                        FROM    [sal].[utbProducts]
                        WHERE   [ProductID] = @ProductID

                        UPDATE  [sal].[utbProducts]
                        SET     [ActiveFlag]        = CASE WHEN @ActiveFlag = 1 THEN 0 ELSE 1 END
								,[VisibleFlag]		= CASE WHEN @ActiveFlag = 1 THEN 0 ELSE [VisibleFlag] END
                                ,[LastModifyDate]   = GETDATE()
                                ,[LastModifyUser]   = @InsertUser
                        WHERE   [ProductID]  = @ProductID
                    END
                ELSE
                    BEGIN
                        IF(@ActionType = 'CHGVS')
                            BEGIN
                                SELECT  @VisibleFlag = [VisibleFlag]
										,@ActiveFlag = [ActiveFlag]
                                FROM    [sal].[utbProducts]
                                WHERE   [ProductID] = @ProductID

                                UPDATE  [sal].[utbProducts]
                                SET     [VisibleFlag]       = CASE WHEN @VisibleFlag = 0 THEN 1 ELSE 0 END
										,[ActiveFlag]		= CASE WHEN @VisibleFlag = 0 AND @ActiveFlag = 0 THEN 1
																   ELSE [ActiveFlag] END
                                        ,[LastModifyDate]   = GETDATE()
                                        ,[LastModifyUser]   = @InsertUser
                                WHERE   [ProductID]  = @ProductID
                            END
                        ELSE
                            BEGIN
                                UPDATE  [sal].[utbProducts]
                                SET     [Qty]		= @Qty
                                        ,[UnitID]	= @UnitID
                                        ,[Price]	= @Price
                                        ,[IVA]		= @IVA
                                        ,[Discount]	= @Discount
                                        ,[VisibleFlag]      = ISNULL(@VisibleFlag,0)
                                        ,[LastModifyDate]   = GETDATE()
                                        ,[LastModifyUser]   = @InsertUser
                                WHERE   [ProductID]  = @ProductID
                            END
                    END
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