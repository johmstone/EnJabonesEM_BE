-- ======================================================================
-- Name: [config].[uspReadPrimaryProducts]
-- Desc: Retorna las Productos Primarios
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 06/02/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadPrimaryProducts]
    @PrimaryProductID INT = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	[PrimaryProductID]
		                ,[Name]
		                ,[Technique]
                        ,[Description]
		                ,[PhotoURL]
		                ,[BrochureURL]
		                ,[ActiveFlag]
		                ,[VisibleFlag]
                FROM	[config].[utbPrimaryProducts]
                WHERE	[PrimaryProductID] = ISNULL(@PrimaryProductID,[PrimaryProductID])
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