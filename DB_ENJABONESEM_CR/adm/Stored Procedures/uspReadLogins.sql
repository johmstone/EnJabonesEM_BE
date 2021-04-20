-- ======================================================================
-- Name: [adm].[uspReadLogins]
-- Desc: Retorna los logins registrados
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 04/20/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspReadLogins]
    @StartDate  DATETIME,
    @EndDate    DATETIME,
    @UserID     INT = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT  L.[LoginID]
                        ,L.[UserID]
                        ,U.[FullName]
                        ,L.[IP]
                        ,L.[Country]
                        ,L.[Region]
                        ,L.[City]
                        ,L.[LoginDate]
                FROM    [adm].[utbLogins] L
                        LEFT JOIN [adm].[utbUsers] U ON U.[UserID] = L.[UserID]
                WHERE   L.[UserID] = ISNULL(@UserID,L.[UserID])
                        AND [LoginDate] >= @StartDate
                        AND [LoginDate] <= @EndDate
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
