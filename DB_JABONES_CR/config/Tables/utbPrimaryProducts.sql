CREATE TABLE [config].[utbPrimaryProducts]
(
	[PrimaryProductID]	INT				IDENTITY(1,1) NOT NULL,
	[Name]				VARCHAR(100)	NOT NULL,
	[Description]		VARCHAR(1000)	NOT NULL,
	[Technique]			VARCHAR(100)	NULL,	
	[PhotoURL]			VARCHAR(500)	NOT NULL,
	[BrochureURL]		VARCHAR(500)	NULL,
	[ActiveFlag]		BIT				CONSTRAINT [utbPrimaryProductsDefaultActiveFlagTrue] DEFAULT ((1)) NOT NULL,	
	[VisibleFlag]		BIT				CONSTRAINT [utbPrimaryProductsDefaultVisibleFlagTrue] DEFAULT ((1)) NOT NULL,	
	[InsertDate]		DATETIME		CONSTRAINT [utbPrimaryProductsDefaultInsertDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]		VARCHAR (100)	CONSTRAINT [utbPrimaryProductsDefaultInsertUserSuser_sSame] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate]	DATETIME		CONSTRAINT [utbPrimaryProductsDefaultLastModifyDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser]	VARCHAR (100)	CONSTRAINT [utbPrimaryProductsDefaultLastModifyUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
	CONSTRAINT [utbPrimaryProductID] PRIMARY KEY CLUSTERED ([PrimaryProductID] ASC),
);

GO
CREATE TRIGGER [config].[utrLogPrimaryProducts] ON [config].[utbPrimaryProducts]
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
				,'[config].[utbPrimaryProducts]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM	Inserted
	END;