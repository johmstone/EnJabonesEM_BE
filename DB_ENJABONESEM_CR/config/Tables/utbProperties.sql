CREATE TABLE [config].[utbProperties]
(
	[PropertyID]		INT				IDENTITY(1,1) NOT NULL,
	[Property]			VARCHAR(100)	NOT NULL,
	[InsertDate]		DATETIME		CONSTRAINT [utbPropertiesDefaultInsertDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]		VARCHAR (100)	CONSTRAINT [utbPropertiesDefaultInsertUserSuser_sSame] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate]	DATETIME		CONSTRAINT [utbPropertiesDefaultLastModifyDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser]	VARCHAR (100)	CONSTRAINT [utbPropertiesDefaultLastModifyUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
	CONSTRAINT [utbPropertyID] PRIMARY KEY CLUSTERED ([PropertyID] ASC),
);

GO
CREATE TRIGGER [config].[utrLogProperties] ON [config].[utbProperties]
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
				,'[config].[utbProperties]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM	Inserted
	END;
