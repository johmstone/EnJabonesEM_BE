CREATE TABLE [sal].[utbOrderDetails]
(
	[DetailID]			INT			IDENTITY(1,1) NOT NULL,
	[OrderID]			INT			NOT NULL,
	[ProductID]			INT			NOT NULL,
	[Qty]				INT			NOT NULL,
	CONSTRAINT [utbDetailID] PRIMARY KEY CLUSTERED ([DetailID] ASC),
	CONSTRAINT [FK.sal.utbOrders.sal.utbOrderDetails.DetailID] FOREIGN KEY ([OrderID]) REFERENCES [sal].[utbOrders] ([OrderID]),
	CONSTRAINT [FK.sal.utbProducts.sal.utbOrderDetails.DetailID] FOREIGN KEY ([ProductID]) REFERENCES [sal].[utbProducts] ([ProductID]),
)
