﻿-- ======================================================================
-- Name: [adm].[uspWebDirectorybyUser]
-- Desc: Retorna el directorio web por usuario
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 04/21/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspWebDirectorybyUser]
	@UserID INT
	,@AppID INT
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				IF(@AppID = 1) /*Non-Authorize Page*/
					BEGIN
						SELECT	W.[AppID]
								,W.[WebID]
								,W.[DisplayName]
								,W.[Controller]
								,W.[Action]
								,W.[Parameter]
								,W.[Order]
						FROM	[adm].[utbWebDirectory] W				
						WHERE	W.[ActiveFlag] = 1
								AND W.[PublicMenu] = 1
								AND W.[AppID] = @AppID
						ORDER BY W.[Order]
					END
				ELSE 
					BEGIN
						IF(@UserID = 0)
							BEGIN
								SELECT	W.[AppID]
										,W.[WebID]
										,W.[DisplayName]
										,W.[Controller]
										,W.[Action]
										,W.[Parameter]
										,W.[Order]
								FROM	[adm].[utbWebDirectory] W							
								WHERE	W.[ActiveFlag] = 1
										AND W.[AppID] = @AppID
										AND W.[PublicMenu] = 1
								ORDER BY W.[Order]	
							END
						ELSE
							BEGIN
								SELECT	W.[AppID]
										,W.[WebID]
										,W.[DisplayName]
										,W.[Controller]
										,W.[Action]
										,W.[Parameter]
										,W.[Order]
								FROM	[adm].[utbWebDirectory] W
										LEFT JOIN [adm].[utbRightsbyRole] RR ON RR.[WebID] = W.[WebID] 
																				AND RR.[ActiveFlag] = 1
																				AND RR.[Read] = 1
										LEFT JOIN [adm].[utbUsers] U ON U.[RoleID] = RR.[RoleID] 								
								WHERE	W.[ActiveFlag] = 1
										AND U.[UserID] = @UserID				
										AND W.[AppID] = @AppID
										AND (W.[PublicMenu] = 1 OR W.[AdminMenu] = 1)
								ORDER BY W.[Order]	
							END
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