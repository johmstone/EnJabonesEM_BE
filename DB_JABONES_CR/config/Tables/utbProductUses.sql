CREATE TABLE [config].[utbProductUses]
(
	[IngUseID]			INT				IDENTITY(1,1) NOT NULL,
	[PrimaryProductID]	INT				NOT NULL,
	[UseID]				INT				NOT NULL,
	[ActiveFlag]		BIT				CONSTRAINT [utbProductUsesDefaultActiveFlagTrue] DEFAULT ((1)) NOT NULL,	
	[InsertDate]		DATETIME		CONSTRAINT [utbProductUsesDefaultInsertDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]		VARCHAR (100)	CONSTRAINT [utbProductUsesDefaultInsertUserSuser_sSame] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate]	DATETIME		CONSTRAINT [utbProductUsesDefaultLastModifyDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser]	VARCHAR (100)	CONSTRAINT [utbProductUsesDefaultLastModifyUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
	CONSTRAINT [utbIngUseID] PRIMARY KEY CLUSTERED ([IngUseID] ASC),
	CONSTRAINT [FK.config.utbPrimaryProducts.config.utbProductUses.IngredientID] FOREIGN KEY ([PrimaryProductID]) REFERENCES [config].[utbPrimaryProducts] ([PrimaryProductID]),
	CONSTRAINT [FK.config.utbUses.config.utbProductUses.UseID] FOREIGN KEY ([UseID]) REFERENCES [config].[utbUses] ([UseID])
);

GO
CREATE TRIGGER [config].[utrLogProductUses] ON [config].[utbProductUses]
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
				,'[config].[utbProductUses]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM	Inserted
	END;
