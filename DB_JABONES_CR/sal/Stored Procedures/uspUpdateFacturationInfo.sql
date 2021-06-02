-- ======================================================================
-- Name: [sal].[uspUpdateFacturationInfo]
-- Desc: Se utiliza para actualizar las información de facturación
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 06/01/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [sal].[uspUpdateFacturationInfo]
	@InsertUser		    VARCHAR(50),
    @ActionType         VARCHAR(15),
    @FacturationInfoID  INT
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
				IF(@ActionType = 'DISABLE')
					BEGIN
						UPDATE	[sal].[utbFacturationInfo]
						SET		[ActiveFlag]	    = 0
								,[LastModifyDate]   = GETDATE()
								,[LastModifyUser]   = @InsertUser
						WHERE	[FacturationInfoID]  = @FacturationInfoID						
					END
				ELSE
					BEGIN
						DECLARE @UserID INT
                        SELECT  @UserID = [UserID]
                        FROM    [sal].[utbFacturationInfo]
                        WHERE   [FacturationInfoID] = @FacturationInfoID

                        UPDATE	[sal].[utbFacturationInfo]
						SET		[PrimaryFlag]	    = 1
								,[LastModifyDate]   = GETDATE()
								,[LastModifyUser]   = @InsertUser
						WHERE	[FacturationInfoID]  = @FacturationInfoID

                        UPDATE	[sal].[utbFacturationInfo]
						SET		[PrimaryFlag]	    = 0
								,[LastModifyDate]   = GETDATE()
								,[LastModifyUser]   = @InsertUser
						WHERE	[FacturationInfoID] != @FacturationInfoID
                                AND [UserID] = @UserID                        
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