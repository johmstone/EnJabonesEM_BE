CREATE TABLE [config].[utbAddresses]
(
	[AddressID]			INT		IDENTITY(1,1)	NOT NULL,
	[Province]			VARCHAR(50)		NOT NULL,
	[Canton]			VARCHAR(50)		NOT NULL,
	[District]			VARCHAR(50)		NOT NULL,
	[Street]			VARCHAR(MAX)	NOT NULL,
	[PhoneNumber]		INT				NOT NULL,
	[Notes]				VARCHAR(MAX)	NULL,
	[ActiveFlag]		BIT				CONSTRAINT [utbAddressesDefaultActiveFlagTrue] DEFAULT ((1)) NOT NULL,	
	[InsertDate]		DATETIME		CONSTRAINT [utbAddressesDefaultInsertDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]		VARCHAR (100)	CONSTRAINT [utbAddressesDefaultInsertUserSuser_sSame] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate]	DATETIME		CONSTRAINT [utbAddressesDefaultLastModifyDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser]	VARCHAR (100)	CONSTRAINT [utbAddressesDefaultLastModifyUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
	CONSTRAINT [utbAddressID] PRIMARY KEY CLUSTERED ([AddressID] ASC)
);

GO
CREATE TRIGGER [config].[utrLogAddresses] ON [config].[utbAddresses]
FOR INSERT
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
				,'[config].[utbAddresses]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM	Inserted
	END;
