﻿-- ======================================================================
-- Name: [sal].[uspUpdateOrder]
-- Desc: Se utiliza para actualizar Ordenes
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 06/02/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [sal].[uspUpdateOrder]
	@InsertUser			VARCHAR(50),
    @OrderID            VARCHAR(50),
	@ActionType			VARCHAR(10),
	@StatusID           INT = NULL,
    @OrderVerified      BIT = NULL
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
				IF(@ActionType = 'CHGST')
                    BEGIN
                        IF(@StatusID = 30100) /*Pago Confirmado*/
                            BEGIN
                                SET @OrderVerified = 1
                            END
                        ELSE 
                            BEGIN
                                SELECT  @OrderVerified = [OrderVerified]
                                FROM    [sal].[utbOrders]
                                WHERE   [OrderID]  = @OrderID
                            END

                        UPDATE  [sal].[utbOrders]
                        SET     [StatusID] = @StatusID
                                ,[OrderVerified]    = @OrderVerified
                                ,[LastModifyDate]   = GETDATE()
                                ,[LastModifyUser]   = @InsertUser
                        WHERE   [OrderID]  = @OrderID

                        INSERT INTO [sal].[utbOrdersHistory] ([OrderID],[StatusID],[OrderVerified],[ActivityDate],[InsertUser])
                        VALUES  (@OrderID,@StatusID,@OrderVerified,GETDATE(),@InsertUser)
                    END
                ELSE
                    BEGIN
                        UPDATE  [sal].[utbOrders]
                        SET     [StatusID]          = 30100 /*Pago Confirmado*/
                                ,[OrderVerified]    = 1 
                                ,[LastModifyDate]   = GETDATE()
                                ,[LastModifyUser]   = @InsertUser
                        WHERE   [OrderID]  = @OrderID    
                        
                        INSERT INTO [sal].[utbOrdersHistory] ([OrderID],[StatusID],[OrderVerified],[ActivityDate],[InsertUser])
                        VALUES  (@OrderID,30100,1,GETDATE(),@InsertUser)
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