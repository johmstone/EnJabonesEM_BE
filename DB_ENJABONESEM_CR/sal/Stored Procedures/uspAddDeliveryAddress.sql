-- ======================================================================
-- Name: [sal].[uspAddDeliveryAddress]
-- Desc: Se utiliza para agregar las direcciones de envío
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 05/27/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [sal].[uspAddDeliveryAddress]
	@InsertUser		VARCHAR(50),
    @UserID         INT,
    @ContactName    VARCHAR(50),
    @PhoneNumber    INT,
    @CostaRicaID    INT,
    @Street         VARCHAR(MAX)
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
                IF EXISTS (SELECT * FROM [sal].[utbDeliveryAddresses] WHERE [UserID] = @UserID AND [PrimaryFlag] = 1 AND [ActiveFlag] = 1)
                    BEGIN
                        INSERT INTO	[sal].[utbDeliveryAddresses] ([UserID],[ContactName],[PhoneNumber],[CostaRicaID],[Street],[InsertDate],[InsertUser],[LastModifyDate],[LastModifyUser])
                        VALUES (@UserID, @ContactName, @PhoneNumber, @CostaRicaID, @Street, GETDATE(), @InsertUser, GETDATE(), @InsertUser)
                    END
                ELSE
                    BEGIN
                        INSERT INTO	[sal].[utbDeliveryAddresses] ([UserID],[ContactName],[PhoneNumber],[CostaRicaID],[Street],[PrimaryFlag],[InsertDate],[InsertUser],[LastModifyDate],[LastModifyUser])
                        VALUES (@UserID, @ContactName, @PhoneNumber, @CostaRicaID, @Street, 1, GETDATE(), @InsertUser, GETDATE(), @InsertUser)
                    END				
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