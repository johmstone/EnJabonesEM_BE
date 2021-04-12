CREATE TABLE [config].[utbIngredientUses]
(
	[IngUseID]			INT				IDENTITY(1,1) NOT NULL,
	[IngredientID]		INT				NOT NULL,
	[UseID]				INT				NOT NULL,
	[ActiveFlag]		BIT				CONSTRAINT [utbIngredientUsesDefaultActiveFlagTrue] DEFAULT ((1)) NOT NULL,	
	[InsertDate]		DATETIME		CONSTRAINT [utbIngredientUsesDefaultInsertDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]		VARCHAR (100)	CONSTRAINT [utbIngredientUsesDefaultInsertUserSuser_sSame] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate]	DATETIME		CONSTRAINT [utbIngredientUsesDefaultLastModifyDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser]	VARCHAR (100)	CONSTRAINT [utbIngredientUsesDefaultLastModifyUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
	CONSTRAINT [utbIngUseID] PRIMARY KEY CLUSTERED ([IngUseID] ASC),
	CONSTRAINT [FK.config.utbIngredients.config.utbIngredientUses.IngredientID] FOREIGN KEY ([IngredientID]) REFERENCES [config].[utbIngredients] ([IngredientID]),
	CONSTRAINT [FK.config.utbUses.config.utbIngredientUses.UseID] FOREIGN KEY ([UseID]) REFERENCES [config].[utbUses] ([UseID])
);

GO
CREATE TRIGGER [config].[utrLogIngredientUses] ON [config].[utbIngredientUses]
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
				,'[config].[utbIngredientUses]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM	Inserted
	END;
