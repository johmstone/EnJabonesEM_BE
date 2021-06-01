-- ======================================================================
-- Name: [adm].[uspReadUsers]
-- Desc: Retorna los usuarios registrados
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 04/20/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspReadUsers]
	@UserID		INT = NULL,
	@Email		VARCHAR(100) = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	U.[UserID]
						,U.[RoleID]
						,U.[FullName]
						,U.[Email]
						,U.[PhotoPath]
						,U.[EmailValidated]
						,U.[Subscriber]
						,U.[NeedResetPwd]
						,U.[ActiveFlag]
						,U.[LastActivityDate]
						,U.[CreationDate]
						,[RoleName] = CASE WHEN R.[RoleID] = 1 THEN 'Usuario' ELSE R.[RoleName] END
				FROM	[adm].[utbUsers] U
						LEFT JOIN [adm].[utbRoles] R ON R.RoleID = U.[RoleID]
				WHERE	U.[UserID] = ISNULL(@UserID,[UserID])
						AND U.[Email] = ISNULL(@Email,U.[Email])
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