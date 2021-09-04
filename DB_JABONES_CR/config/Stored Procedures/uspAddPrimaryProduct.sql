-- ======================================================================
-- Name: [config].[uspAddPrimaryProduct]
-- Desc: Se utiliza para agregar Productos Primarios
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 06/02/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspAddPrimaryProduct]
	@InsertUser		VARCHAR(50),
    @Name           VARCHAR(100),
    @Description    VARCHAR(1000),
    @Technique      VARCHAR(100) = NULL,
    @PhotoURL       VARCHAR(500),
    @BrochureURL    VARCHAR(500) = NULL,
    @Properties     VARCHAR(500) = NULL,
    @VisibleFlag    BIT = NULL
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
                DECLARE @PrimaryProductID   INT,
                        @PropertyID         INT
                DECLARE @TableProperties    TABLE ([PrimaryProductID] INT,[PropertyID] INT)

                INSERT INTO	[config].[utbPrimaryProducts] ([Name],[Description],[Technique],[PhotoURL],[BrochureURL],[VisibleFlag],[InsertDate],[InsertUser],[LastModifyDate],[LastModifyUser])
                VALUES (@Name, @Description, @Technique, @PhotoURL, @BrochureURL, ISNULL(@VisibleFlag,0), GETDATE(), @InsertUser, GETDATE(), @InsertUser)                

                IF(@Properties IS NOT NULL)
                    BEGIN
                        SET @PrimaryProductID = SCOPE_IDENTITY()

                        INSERT INTO @TableProperties
                        SELECT	@PrimaryProductID, Value
                        FROM	STRING_SPLIT(@Properties,',')

                        DECLARE ActionProcess CURSOR FOR 
                        SELECT * FROM @TableProperties

                        OPEN ActionProcess
                        FETCH NEXT FROM ActionProcess INTO @PrimaryProductID, @PropertyID

                        WHILE @@FETCH_STATUS = 0
	                        BEGIN 
		                        INSERT INTO [config].[utbProductProperties] ([PrimaryProductID],[PropertyID],[InsertDate],[InsertUser],[LastModifyDate],[LastModifyUser])
		                        VALUES (@PrimaryProductID, @PropertyID, GETDATE(), @InsertUser, GETDATE(), @InsertUser)
		                        FETCH NEXT FROM ActionProcess INTO @PrimaryProductID, @PropertyID
	                        END

                        CLOSE ActionProcess
                        DEALLOCATE ActionProcess
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