CREATE TABLE [sal].[utbFacturationInfo]
(
	[FacturationInfoID]	INT				IDENTITY(1,1)	NOT NULL,
	[UserID]			INT				NOT NULL,
	[IdentityType]		VARCHAR(50)		NOT NULL,
	[IdentityID]		VARCHAR(50)		NOT NULL,
	[FullName]			VARCHAR(50)		NOT NULL,
	[PhoneNumber]		INT				NOT NULL,
	[CostaRicaID]		INT				NOT NULL,
	[Street]			VARCHAR(MAX)	NOT NULL,
	[PrimaryFlag]		BIT				CONSTRAINT [utbFacturationInfoDefaultPrimaryFlagTrue] DEFAULT ((0)) NOT NULL,	
	[ActiveFlag]		BIT				CONSTRAINT [utbFacturationInfoDefaultActiveFlagTrue] DEFAULT ((1)) NOT NULL,	
	[InsertDate]		DATETIME		CONSTRAINT [utbFacturationInfoDefaultInsertDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]		VARCHAR (100)	CONSTRAINT [utbFacturationInfoDefaultInsertUserSuser_sSame] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate]	DATETIME		CONSTRAINT [utbFacturationInfoDefaultLastModifyDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser]	VARCHAR (100)	CONSTRAINT [utbFacturationInfoDefaultLastModifyUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
	CONSTRAINT [utbFacturationInfoID] PRIMARY KEY CLUSTERED ([FacturationInfoID] ASC),
    CONSTRAINT [FK.adm.utbUsers.sal.utbFacturationInfo.UserID] FOREIGN KEY ([UserID]) REFERENCES [adm].[utbUsers] ([UserID]),
	CONSTRAINT [FK.config.utbCostaRicaData.sal.utbFacturationInfo.AddressID] FOREIGN KEY ([CostaRicaID]) REFERENCES [config].[utbCostaRicaData] ([CostaRicaID])
);

GO
CREATE TRIGGER [sal].[utrLogFacturationInfo] ON [sal].[utbFacturationInfo]
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
				,'[sal].[utbFacturationInfo]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM	Inserted
	END;
