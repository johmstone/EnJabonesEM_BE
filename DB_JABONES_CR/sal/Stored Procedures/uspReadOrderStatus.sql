-- ======================================================================
-- Name: [sal].[uspReadOrderStatus]
-- Desc: Retorna los posibles estados de Ordenes
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 07/08/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [sal].[uspReadOrderStatus]
	@StatusType VARCHAR(10)
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				IF(@StatusType = 'EXTERNAL')
                    BEGIN
                        SELECT	DISTINCT
		                        [StatusID]			= [ExternalStatusID]
		                        ,[InternalStatus]	= NULL
                                ,[ExternalStatusID]
		                        ,[ExternalStatus]
                        FROM	[sal].[utbOrderStatus]
                    END
                ELSE
                    BEGIN
                        SELECT  [StatusID] 
                                ,[InternalStatus]
                                ,[ExternalStatusID]
                                ,[ExternalStatus]
                        FROM    [sal].[utbOrderStatus]
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