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
                        SELECT  DISTINCT 
		                        [StatusID]          = OS.[ExternalStatusID]
                                ,[InternalStatus]   = OS.[ExternalStatus]
                                ,OS.[ExternalStatusID]
                                ,OS.[ExternalStatus]
		                        ,[QtyOrders]        = TOT.[Qty]
                        FROM    [sal].[utbOrderStatus] OS
		                        OUTER APPLY (SELECT [Qty] = COUNT(O.[OrderID])
					                         FROM	[sal].[utbOrders] O 
													LEFT JOIN [sal].[utbOrderStatus] ST ON ST.[StatusID] = O.[StatusID]
					                         WHERE	ST.[ExternalStatusID] = OS.[ExternalStatusID]
							                        AND O.[ActiveFlag] = 1) TOT
						UNION
						SELECT	[StatusID]			= 10000
                                ,[InternalStatus]	= 'Todos'
                                ,[ExternalStatusID]	= 10000
                                ,[ExternalStatus]	= 'Todos'
		                        ,[QtyOrders]		= 0
                    END
                ELSE
                    BEGIN
                        SELECT  OS.[StatusID] 
                                ,OS.[InternalStatus]
                                ,OS.[ExternalStatusID]
                                ,OS.[ExternalStatus]
		                        ,[QtyOrders] = TOT.[Qty]
                        FROM    [sal].[utbOrderStatus] OS
		                        OUTER APPLY (SELECT [Qty] = COUNT(O.[OrderID])
					                         FROM	[sal].[utbOrders] O 
					                         WHERE	O.[StatusID] = OS.[StatusID]
							                        AND O.[ActiveFlag] = 1) TOT
						UNION
						SELECT	[StatusID]			= 10000
                                ,[InternalStatus]	= 'Todos'
                                ,[ExternalStatusID]	= 10000
                                ,[ExternalStatus]	= 'Todos'
		                        ,[QtyOrders]		= 0
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