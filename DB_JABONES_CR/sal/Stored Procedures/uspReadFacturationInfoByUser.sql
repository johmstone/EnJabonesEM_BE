-- ======================================================================
-- Name: [sal].[uspReadFacturationInfoByUser]
-- Desc: Retorna las informaciones de facturación
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 06/01/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [sal].[uspReadFacturationInfoByUser]
    @UserID INT
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	FI.[FacturationInfoID]
						,FI.[UserID]
						,FI.[IdentityType]
						,FI.[IdentityID]
						,FI.[FullName]
						,FI.[PhoneNumber]
						,FI.[CostaRicaID]
						,CR.[ProvinceID]
						,CR.[Province]
						,CR.[CantonID]
						,CR.[Canton]
						,CR.[DistrictID]
						,CR.[District]
						,FI.[Street]
						,FI.[PrimaryFlag]		
				FROM	[sal].[utbFacturationInfo] FI
						LEFT JOIN [config].[utbCostaRicaData] CR ON CR.[CostaRicaID] = FI.[CostaRicaID]
				WHERE	FI.[UserID] = @UserID
						AND [ActiveFlag] = 1
				ORDER BY FI.[PrimaryFlag] DESC
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
