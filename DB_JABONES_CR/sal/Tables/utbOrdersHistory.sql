CREATE TABLE [sal].[utbOrdersHistory]
(
	[HistoryID]			INT				IDENTITY(1,1) NOT NULL,
	[OrderID]			VARCHAR(50)		NOT NULL,
	[StatusID]			INT				NOT NULL,
	[OrderVerified]		BIT				NOT NULL,	
	[ActivityDate]		DATETIME		CONSTRAINT [utbOrdersHistoryDefaultActivityDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]		VARCHAR (100)	CONSTRAINT [utbOrdersHistoryDefaultInsertUserSuser_sSame] DEFAULT (suser_sname()) NOT NULL,    
	CONSTRAINT [HistoryID] PRIMARY KEY CLUSTERED ([HistoryID] ASC),
	CONSTRAINT [FK.sal.utbOrders.sal.utbOrdersHistory.OrderID] FOREIGN KEY ([OrderID]) REFERENCES [sal].[utbOrders] ([OrderID]),
	CONSTRAINT [FK.sal.utbOrderStatus.sal.utbOrdersHistory.StatusID] FOREIGN KEY ([StatusID]) REFERENCES [sal].[utbOrderStatus] ([StatusID]),
)
