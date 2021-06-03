-- ======================================================================
-- Name: [config].[uspUpdatePrimaryProduct]
-- Desc: Se utiliza para actualizar Productos Primarios
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 06/02/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspUpdatePrimaryProduct]
	@InsertUser		    VARCHAR(50),
    @PrimaryProductID   INT,
    @ActionType         VARCHAR(10),
    @Name               VARCHAR(100) = NULL,
    @Description        VARCHAR(1000) = NULL,
    @Technique          VARCHAR(100) = NULL,
    @PhotoURL           VARCHAR(500) = NULL,
    @BrochureURL        VARCHAR(500) = NULL,
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
                        FROM    [config].[utbPrimaryProducts]
                        WHERE   [PrimaryProductID] = @PrimaryProductID

                        UPDATE  [config].[utbPrimaryProducts]
                        SET     [ActiveFlag]        = CASE WHEN @ActiveFlag = 1 THEN 0 ELSE 1 END
								,[VisibleFlag]		= CASE WHEN @ActiveFlag = 1 THEN 0 ELSE [VisibleFlag] END
                                ,[LastModifyDate]   = GETDATE()
                                ,[LastModifyUser]   = @InsertUser
                        WHERE   [PrimaryProductID]  = @PrimaryProductID
                    END
                ELSE
                    BEGIN
                        IF(@ActionType = 'CHGVS')
                            BEGIN
                                SELECT  @VisibleFlag = [VisibleFlag]
										,@ActiveFlag = [ActiveFlag]
                                FROM    [config].[utbPrimaryProducts]
                                WHERE   [PrimaryProductID] = @PrimaryProductID

                                UPDATE  [config].[utbPrimaryProducts]
                                SET     [VisibleFlag]       = CASE WHEN @VisibleFlag = 0 THEN 1 ELSE 0 END
										,[ActiveFlag]		= CASE WHEN @VisibleFlag = 0 AND @ActiveFlag = 0 THEN 1
																   ELSE [ActiveFlag] END
                                        ,[LastModifyDate]   = GETDATE()
                                        ,[LastModifyUser]   = @InsertUser
                                WHERE   [PrimaryProductID]  = @PrimaryProductID
                            END
                        ELSE
                            BEGIN
                                UPDATE  [config].[utbPrimaryProducts]
                                SET     [Name]              = @Name
                                        ,[Description]      = @Description
                                        ,[Technique]        = @Technique
                                        ,[PhotoURL]         = @PhotoURL
                                        ,[BrochureURL]      = @BrochureURL
                                        ,[VisibleFlag]      = ISNULL(@VisibleFlag,0)
                                        ,[LastModifyDate]   = GETDATE()
                                        ,[LastModifyUser]   = @InsertUser
                                WHERE   [PrimaryProductID]  = @PrimaryProductID
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