CREATE TABLE [config].[utbFormulas]
(
	[FormulaID]			INT				IDENTITY(1,1) NOT NULL,
	[PrimaryProductID]	INT				NOT NULL,
	[IngredientID]		INT				NOT NULL,
	[Qty]				NUMERIC(5,2)	NOT NULL,
	[UnitID]			INT				NOT NULL,	
	[InsertDate]		DATETIME		CONSTRAINT [utbFormulasDefaultInsertDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]		VARCHAR (100)	CONSTRAINT [utbFormulasDefaultInsertUserSuser_sSame] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate]	DATETIME		CONSTRAINT [utbFormulasDefaultLastModifyDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser]	VARCHAR (100)	CONSTRAINT [utbFormulasDefaultLastModifyUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
	CONSTRAINT [utbFormulaID] PRIMARY KEY CLUSTERED ([FormulaID] ASC),
	CONSTRAINT [FK.config.utbPrimaryProducts.config.utbFormulas.PrimaryProductID] FOREIGN KEY ([PrimaryProductID]) REFERENCES [config].[utbPrimaryProducts] ([PrimaryProductID]),
	CONSTRAINT [FK.config.utbIngredients.config.utbFomurlas.IngredientID] FOREIGN KEY ([IngredientID]) REFERENCES [config].[utbIngredients] ([IngredientID]),
	CONSTRAINT [FK.config.utbUnits.config.utbFormulas.UnitID] FOREIGN KEY ([UnitID]) REFERENCES [config].[utbUnits] ([UnitID]),
);

GO
CREATE TRIGGER [config].[utrLogFormulas] ON [config].[utbFormulas]
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
				,'[config].[utbFormulas]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM	Inserted
	END;