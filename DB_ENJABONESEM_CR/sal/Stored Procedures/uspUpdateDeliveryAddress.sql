-- ======================================================================
-- Name: [sal].[uspUpdateDeliveryAddress]
-- Desc: Se utiliza para actualizar las direcciones de envío
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 05/27/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [sal].[uspUpdateDeliveryAddress]
	@InsertUser		    VARCHAR(50),
    @ActionType         VARCHAR(15),
    @DeliveryAddressID  INT,
    @ContactName        VARCHAR(50) = NULL,
    @PhoneNumber        INT = NULL,
    @CostaRicaID        INT = NULL,
    @Street             VARCHAR(MAX) = NULL
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
				IF(@ActionType = 'DISABLE')
					BEGIN
						UPDATE	[sal].[utbDeliveryAddresses]
						SET		[ActiveFlag]	    = 0
								,[LastModifyDate]   = GETDATE()
								,[LastModifyUser]   = @InsertUser
						WHERE	[DeliveryAddressID] = @DeliveryAddressID						
					END
				ELSE
					BEGIN
						IF(@ActionType = 'SETPRIMARY')
                            BEGIN
                                DECLARE @UserID INT
                                SELECT  @UserID = [UserID]
                                FROM    [sal].[utbDeliveryAddresses]
                                WHERE   [DeliveryAddressID] = @DeliveryAddressID

                                UPDATE	[sal].[utbDeliveryAddresses]
						        SET		[PrimaryFlag]	    = 1
								        ,[LastModifyDate]   = GETDATE()
								        ,[LastModifyUser]   = @InsertUser
						        WHERE	[DeliveryAddressID] = @DeliveryAddressID

                                UPDATE	[sal].[utbDeliveryAddresses]
						        SET		[PrimaryFlag]	    = 0
								        ,[LastModifyDate]   = GETDATE()
								        ,[LastModifyUser]   = @InsertUser
						        WHERE	[DeliveryAddressID] != @DeliveryAddressID
                                        AND [UserID] = @UserID
                            END
                        ELSE
                            BEGIN
                                UPDATE  [sal].[utbDeliveryAddresses]
                                SET     [ContactName] = @ContactName
                                        ,[PhoneNumber] = @PhoneNumber
                                        ,[CostaRicaID] = @CostaRicaID
                                        ,[Street] = @Street
                                        ,[LastModifyDate] = GETDATE()
                                        ,[LastModifyUser] = @InsertUser
                                WHERE   [DeliveryAddressID] = @DeliveryAddressID                                
                            END
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