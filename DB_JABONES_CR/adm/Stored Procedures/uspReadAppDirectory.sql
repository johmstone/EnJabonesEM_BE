-- ======================================================================
-- Name: [adm].[uspReadAppDirectory]
-- Desc: Retorna las aplicaciones registradas
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 04/20/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspReadAppDirectory]
    @AppID INT = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT  [AppID]
                        ,[AppName]
                        ,[Order]
                        ,[PrivateSite]
                        ,[URL]
                        ,[Description]
                        ,[ActiveFlag]
                FROM    [adm].[utbAppDirectory]
				WHERE	[AppID] = ISNULL(@AppID,[AppID])
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
