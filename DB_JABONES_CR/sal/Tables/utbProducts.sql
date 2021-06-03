CREATE TABLE [sal].[utbProducts]
(
	[ProductID]			INT				IDENTITY(1,1) NOT NULL,
	[PrimaryProductID]	INT				NOT NULL,
	[Qty]				NUMERIC(5,2)	NOT NULL,
	[UnitID]			INT				NOT NULL,
	[Price]				NUMERIC(8,2)	NOT NULL,
	[IVA]				NUMERIC(5,2)	CONSTRAINT [utbProductsDefaultIVATrue] DEFAULT ((0)) NOT NULL,	
	[Discount]			NUMERIC(5,2)	CONSTRAINT [utbProductsDefaultDiscountTrue] DEFAULT ((0)) NOT NULL,
	[ActiveFlag]		BIT				CONSTRAINT [utbProductsDefaultActiveFlagTrue] DEFAULT ((1)) NOT NULL,	
	[VisibleFlag]		BIT				CONSTRAINT [utbProductsDefaultVisibleFlagTrue] DEFAULT ((1)) NOT NULL,	
	[InsertDate]		DATETIME		CONSTRAINT [utbProductsDefaultInsertDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]		VARCHAR (100)	CONSTRAINT [utbProductsDefaultInsertUserSuser_sSame] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate]	DATETIME		CONSTRAINT [utbProductsDefaultLastModifyDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser]	VARCHAR (100)	CONSTRAINT [utbProductsDefaultLastModifyUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
	CONSTRAINT [utbProductID] PRIMARY KEY CLUSTERED ([ProductID] ASC),
	CONSTRAINT [FK.config.utbPrimaryProducts.sal.utbProducts.PrimaryProductID] FOREIGN KEY ([PrimaryProductID]) REFERENCES [config].[utbPrimaryProducts] ([PrimaryProductID]),
	CONSTRAINT [FK.config.utbUnits.sal.utbProducts.UnitID] FOREIGN KEY ([UnitID]) REFERENCES [config].[utbUnits] ([UnitID]),
);

GO
CREATE TRIGGER [sal].[utrLogProducts] ON [sal].[utbProducts]
FOR INSERT, UPDATE
AS
	BEGIN
		DECLARE @INSERTUPDATE VARCHAR(30)
		DECLARE @StartValues	XML = (SELECT * FROM Deleted [Values] for xml AUTO, ELEMENTS XSINIL)
		DECLARE @EndValues		XML = (SELECT * FROM Inserted [Values] for xml AUTO, ELEMENTS XSINIL)

		CREATE TABLE #DBCC (EventType varchar(50), Parameters varchar(50), EventInfo nvarchar(max))

		INSERT INTO #DBCC
		EXEC ('DBCC INPUTBUFFER(@@SPID)')

		--Assume it is an insert
		SET @INSERTUPDATE ='INSERT'
		--If there's data in deleted, it's an update
		IF EXISTS(SELECT * FROM Deleted)
		  SET @INSERTUPDATE='UPDATE'

		INSERT INTO [adm].[utbLogActivities] ([ActivityType],[TargetTable],[SQLStatement],[StartValues],[EndValues],[User],[LogActivityDate])
		SELECT	@INSERTUPDATE
				,'[sal].[utbProducts]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM	Inserted
	END;
