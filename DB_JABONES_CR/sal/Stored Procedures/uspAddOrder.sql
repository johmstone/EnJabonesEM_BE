-- ======================================================================
-- Name: [sal].[uspAddOrder]
-- Desc: Se utiliza para agregar ordenes
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 06/02/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [sal].[uspAddOrder]
    @OrderID            VARCHAR(50),
    @OrderDetails	    NVARCHAR(MAX)
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
                DECLARE @StageOrderID       VARCHAR(50)
	                    ,@UserID            INT
                        ,@OrderType         VARCHAR(10)
                        ,@OrderDate         DATETIME
	                    ,@DeliveryID        INT
                        ,@DeliveryAddressID INT
                        ,@FacturationInfoID INT
	                    ,@Discount          NUMERIC(5,2)
                        ,@TotalCart         NUMERIC(8,2) 
                        ,@TotalDelivery     NUMERIC(8,2)
                        ,@ProofPayment      VARCHAR(50)
                        ,@DAContactName     VARCHAR(50)
                        ,@DAPhoneNumber     INT
                        ,@DACostaRicaID     INT
                        ,@DAStreet          VARCHAR(MAX)
                        ,@FIIdentityType    VARCHAR(50)
                        ,@FIIdentityID      VARCHAR(50)
                        ,@FIFullName        VARCHAR(50)
                        ,@FIPhoneNumber     INT
                        ,@FIEmail           VARCHAR(50)
                        ,@FICostaRicaID     INT
                        ,@FIStreet          VARCHAR(MAX)

                SELECT	@StageOrderID   = [StageOrderID]
		                ,@UserID        = [UserID]
		                ,@OrderType     = [OrderType]
		                ,@OrderDate     = [OrderDate]
		                ,@DeliveryID        = CASE WHEN [DAContactName] IS NULL THEN 1 ELSE 2 END
		                ,@DeliveryAddressID = ISNULL([DeliveryAddressID],0)
		                ,@FacturationInfoID = [FacturationInfoID]
		                ,@Discount          = ISNULL([Discount],0)
		                ,@TotalCart         = [TotalCart]
		                ,@TotalDelivery     = [TotalDelivery]
		                ,@ProofPayment      = [ProofPayment]
                        ,@DAContactName     = [DAContactName]
                        ,@DAPhoneNumber     = [DAPhoneNumber]
                        ,@DACostaRicaID     = [DACostaRicaID]
                        ,@DAStreet          = [DAStreet]
                        ,@FIIdentityType    = [FIIdentityType]
                        ,@FIIdentityID      = [FIIdentityID]
                        ,@FIFullName        = [FIFullName]
                        ,@FIPhoneNumber     = [FIPhoneNumber]
                        ,@FIEmail           = [FIEmail]
                        ,@FICostaRicaID     = [FICostaRicaID]
                        ,@FIStreet          = [FIStreet]
                FROM	OPENJSON(@OrderDetails)
                WITH	(
			                StageOrderID		VARCHAR(50)	'$.StageOrder'
			                ,UserID				INT			'$.FacturationInfo.UserID'
			                ,OrderType			VARCHAR(10)	'$.PaymentMethod'
			                ,OrderDate			VARCHAR(30)	'$.OrderDate'
			                ,DeliveryAddressID	INT			'$.DeliveryAddress.DeliveryAddressID'
			                ,FacturationInfoID	INT			'$.FacturationInfo.FacturationInfoID'
			                ,Discount			VARCHAR(50)	'$.Discount'
			                ,TotalCart			NUMERIC(8,2)'$.TotalCart'
			                ,TotalDelivery		NUMERIC(8,2)'$.TotalDelivery'
			                ,ProofPayment		VARCHAR(50)	'$.ProofPayment'
                            ,DAContactName      VARCHAR(50) '$.DeliveryAddress.ContactName'
	                        ,DAPhoneNumber      INT         '$.DeliveryAddress.PhoneNumber'
	                        ,DACostaRicaID      INT         '$.DeliveryAddress.CostaRicaID'
	                        ,DAStreet           VARCHAR(MAX)'$.DeliveryAddress.Street'
                            ,FIIdentityType     VARCHAR(50) '$.FacturationInfo.IdentityType'
	                        ,FIIdentityID       VARCHAR(50) '$.FacturationInfo.IdentityID'
	                        ,FIFullName         VARCHAR(50) '$.FacturationInfo.FullName'
	                        ,FIPhoneNumber      INT         '$.FacturationInfo.PhoneNumber'
	                        ,FIEmail            VARCHAR(50) '$.FacturationInfo.Email'
	                        ,FICostaRicaID      INT         '$.FacturationInfo.CostaRicaID'
	                        ,FIStreet           VARCHAR(MAX)'$.FacturationInfo.Street'
		                )
                
                IF(@UserID = 0)
                    BEGIN
                        INSERT INTO	[sal].[utbFacturationInfo] ([UserID],[IdentityType],[IdentityID],[FullName],[PhoneNumber],[Email],[CostaRicaID],[Street])
                        VALUES (@UserID, @FIIdentityType, @FIIdentityID, @FIFullName, @FIPhoneNumber, @FIEmail, @FICostaRicaID, @FIStreet)

                        SET @FacturationInfoID = SCOPE_IDENTITY()

                        IF(@DeliveryID != 1)
                            BEGIN
                                INSERT INTO	[sal].[utbDeliveryAddresses] ([UserID],[ContactName],[PhoneNumber],[CostaRicaID],[Street])
                                VALUES (@UserID, @DAContactName, @DAPhoneNumber, @DACostaRicaID, @DAStreet)

                                SET @DeliveryAddressID = SCOPE_IDENTITY()
                            END
                    END

                INSERT INTO [sal].[utbOrders] 
                        ([OrderID],[StageOrderID],[UserID],[OrderType],[OrderDate],[DeliveryID],[DeliveryAddressID],[FacturationInfoID]
                        ,[OrderDetails],[Discount],[TotalCart],[TotalDelivery],[StatusID],[ProofPayment])
                VALUES  (@OrderID, @StageOrderID, @UserID, @OrderType, @OrderDate, @DeliveryID, @DeliveryAddressID, @FacturationInfoID, 
                        @OrderDetails, @Discount, @TotalCart, @TotalDelivery,20200, @ProofPayment)
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