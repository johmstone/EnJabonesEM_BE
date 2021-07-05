CREATE TABLE [sal].[utbStagingOrders]
(
	[StageOrderID]		VARCHAR(50)		NOT NULL,
	[UserID]			INT				NOT NULL,			
	[OrderDate]			DATETIME		CONSTRAINT [utbStagingOrdersDefaultOrderDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
	[OrderDetails]		VARCHAR(MAX)	NOT NULL,
	[DeliveryID]		INT				NOT NULL,
	[CompletedFlag]		BIT				CONSTRAINT [utbStagingOrdersDefaultCompletedFlag] DEFAULT ((0)) NOT NULL,	
	CONSTRAINT [utbStageOrderID] PRIMARY KEY CLUSTERED ([StageOrderID]),
	CONSTRAINT [FK.sal.utbDeliveryMethods.sal.utbStagingOrders.DeliveryID] FOREIGN KEY ([DeliveryID]) REFERENCES [sal].[utbDeliveryMethods] ([DeliveryID])
);