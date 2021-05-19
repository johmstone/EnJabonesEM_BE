-- ======================================================================
-- Name: [adm].[uspValidateUserEmail]
-- Desc: Valida si el Token esta activo
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 04/20/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspValidateUserEmail]
	@EVToken VARCHAR(MAX)
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
                DECLARE @UserID     INT
                        ,@isValid   BIT
                        ,@Result    INT = 0

				SELECT	@UserID = [UserID]
                        ,@isValid = [IsValid]
				FROM	[adm].[utbUserValidationEmails]
				WHERE	[EVToken] = @EVToken

                IF (@UserID IS NULL)
                    BEGIN
                        SET @Result = -1 -- EVToken not exists
                    END
                ELSE 
                    BEGIN
                        IF(@isValid = 0)
                            BEGIN
                                SET @Result = 0 -- Invalid EVToken
                            END
                        ELSE
                            BEGIN
                                UPDATE  [adm].[utbUsers] 
                                SET     [EmailValidated] = 1
                                        ,[LastModifyDate] = GETDATE()
                                WHERE   [UserID] = @UserID

                                UPDATE  [adm].[utbUserValidationEmails]
                                SET     [IsValid] = 0
                                WHERE   [EVToken] = @EVToken

                                SET @Result = @UserID  -- Email Validated
                            END
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