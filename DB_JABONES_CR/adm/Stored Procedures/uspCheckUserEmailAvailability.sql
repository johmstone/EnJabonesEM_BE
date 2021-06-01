-- ======================================================================
-- Name: [adm].[uspCheckUserEmailAvailability]
-- Desc: Es para validar si un nombre de usuario o email ya existe en la base de datos
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 04/24/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspCheckUserEmailAvailability]
	@Email VARCHAR(50)
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				DECLARE @Validation BIT = 1

				IF EXISTS(SELECT * FROM	[adm].[utbUsers] WHERE [Email] = @Email)
					BEGIN
						SET @Validation = 0
					END				
				SELECT @Validation				
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