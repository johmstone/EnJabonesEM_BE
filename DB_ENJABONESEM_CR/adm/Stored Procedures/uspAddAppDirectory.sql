﻿-- ======================================================================
-- Name: [adm].[uspAddAppDirectory]
-- Desc: Se utiliza para registrar un nuevo login
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 04/20/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspAddAppDirectory]
    @InsertUser     VARCHAR(50),
	@AppName	    VARCHAR(50),
	@Order          INT,
	@PrivateSite    BIT,
	@URL            VARCHAR(200),
	@Description	VARCHAR(MAX)
AS 
    BEGIN
        SET NOCOUNT ON
        SET XACT_ABORT ON
                           
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT
            DECLARE @lLocalTran BIT = 0
                               
            IF @@TRANCOUNT = 0 
                BEGIN
                    BEGIN TRANSACTION
                    SET @lLocalTran = 1
                END

            -- =======================================================
				INSERT INTO [adm].[utbAppDirectory] ([AppName],[Order],[PrivateSite],[URL],[Description],[CreationUser],[LastModifyUser])
				VALUES (@AppName, @Order, @PrivateSite, @URL, @Description, @InsertUser, @InsertUser)
			-- =======================================================

        IF ( @@trancount > 0
                 AND @lLocalTran = 1
               ) 
                BEGIN
                    COMMIT TRANSACTION
                END
        END TRY
        BEGIN CATCH
            IF ( @@trancount > 0
                 AND XACT_STATE() <> 0
               ) 
                BEGIN
                    ROLLBACK TRANSACTION
                END

            SELECT  @lErrorMessage = ERROR_MESSAGE() ,
                    @lErrorSeverity = ERROR_SEVERITY() ,
                    @lErrorState = ERROR_STATE()       

            RAISERROR (@lErrorMessage, @lErrorSeverity, @lErrorState);
        END CATCH
    END

    SET NOCOUNT OFF
    SET XACT_ABORT OFF