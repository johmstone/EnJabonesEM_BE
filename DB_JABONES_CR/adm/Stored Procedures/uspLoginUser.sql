-- ======================================================================
-- Name: [adm].[uspLoginUser]
-- Desc: Se utiliza para la validación de los usuarios al logearse
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 04/24/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspLoginUser]
	@Email		VARCHAR(50),
	@Password	VARCHAR(50)
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				DECLARE @UserID	INT,
						@NeedResetPwd BIT,
						@EmailValidated BIT

				IF EXISTS(SELECT * FROM [adm].[utbUsers] WHERE [Email] = @Email)
					BEGIN
						SELECT	@UserID = [UserID]
								,@NeedResetPwd = [NeedResetPwd]
								,@EmailValidated = [EmailValidated]
						FROM	[adm].[utbUsers] 
						WHERE	[Email] = @Email
								AND [PasswordHash] = HASHBYTES('SHA2_512',@Password)
								AND [ActiveFlag] = 1
						
						SELECT	[UserID] = ISNULL(@UserID,-1),
								[NeedResetPwd] = ISNULL(@NeedResetPwd,0),
								[EmailValidated] = ISNULL(@EmailValidated,0)							
					END
				ELSE
					BEGIN
						SELECT	[UserID] = 0 /*Usuario No registrado*/
								,[NeedResetPwd] = 0
								,[EmailValidated] = 0
					END	
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