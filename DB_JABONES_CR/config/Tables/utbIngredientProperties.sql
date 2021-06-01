CREATE TABLE [config].[utbIngredientProperties]
(
	[IngPropertyID]		INT				IDENTITY(1,1) NOT NULL,
	[IngredientID]		INT				NOT NULL,
	[PropertyID]		INT				NOT NULL,
	[ActiveFlag]		BIT				CONSTRAINT [utbIngredientPropertiesDefaultActiveFlagTrue] DEFAULT ((1)) NOT NULL,	
	[InsertDate]		DATETIME		CONSTRAINT [utbIngredientPropertiesDefaultInsertDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]		VARCHAR (100)	CONSTRAINT [utbIngredientPropertiesDefaultInsertUserSuser_sSame] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate]	DATETIME		CONSTRAINT [utbIngredientPropertiesDefaultLastModifyDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser]	VARCHAR (100)	CONSTRAINT [utbIngredientPropertiesDefaultLastModifyUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
	CONSTRAINT [utbIngPropertyID] PRIMARY KEY CLUSTERED ([IngPropertyID] ASC),
	CONSTRAINT [FK.config.utbIngredients.config.utbIngredientProperties.IngredientID] FOREIGN KEY ([IngredientID]) REFERENCES [config].[utbIngredients] ([IngredientID]),
	CONSTRAINT [FK.config.utbProperties.config.utbIngredientProperties.PropertyID] FOREIGN KEY ([PropertyID]) REFERENCES [config].[utbProperties] ([PropertyID])
);

GO
CREATE TRIGGER [config].[utrLogIngredientProperties] ON [config].[utbIngredientProperties]
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
				,'[config].[utbIngredientProperties]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM	Inserted
	END;
