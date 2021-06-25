CREATE TABLE [sal].[utbStagingOrders]
(
	[StageOrderID]		VARCHAR(25)		NOT NULL,
	[UserID]			INT				NOT NULL,			
	[OrderDate]			DATETIME		CONSTRAINT [utbStagingOrdersDefaultOrderDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
	[OrderDetails]		VARCHAR(MAX)	NOT NULL,
	[DeliveryID]		INT				NOT NULL
	CONSTRAINT [utbStageOrderID] PRIMARY KEY CLUSTERED ([StageOrderID]),
	CONSTRAINT [FK.sal.utbDeliveryMethods.sal.utbStagingOrders.DeliveryID] FOREIGN KEY ([DeliveryID]) REFERENCES [sal].[utbDeliveryMethods] ([DeliveryID])
);