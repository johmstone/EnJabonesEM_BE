-- ======================================================================
-- Name: [adm].[uspReadAdmins]
-- Desc: Retorna los usuarios con roles administrativos
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 04/20/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspReadAdmins]
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	U.[Email]      
                FROM	[adm].[utbRoles] R
		                INNER JOIN [adm].[utbUsers] U ON U.[RoleID] = R.[RoleID] AND U.[ActiveFlag] = 1
                WHERE	R.[RoleName] LIKE '%Administrador%'
		                AND R.[ActiveFlag] = 1
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