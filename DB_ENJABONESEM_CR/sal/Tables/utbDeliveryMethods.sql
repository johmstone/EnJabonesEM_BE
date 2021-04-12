CREATE TABLE [sal].[utbDeliveryMethods]
(
	[DeliveryID]		INT				IDENTITY(1,1) NOT NULL,
	[DeliveryMethod]	VARCHAR(50)		NOT NULL,
	[ActiveFlag]		BIT				CONSTRAINT [utbDeliveryMethodsDefaultActiveFlagTrue] DEFAULT ((1)) NOT NULL,	
	[InsertDate]		DATETIME		CONSTRAINT [utbDeliveryMethodsDefaultInsertDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]		VARCHAR (100)	CONSTRAINT [utbDeliveryMethodsDefaultInsertUserSuser_sSame] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate]	DATETIME		CONSTRAINT [utbDeliveryMethodsDefaultLastModifyDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser]	VARCHAR (100)	CONSTRAINT [utbDeliveryMethodsDefaultLastModifyUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
	CONSTRAINT [utbDeliveryID] PRIMARY KEY CLUSTERED ([DeliveryID] ASC),
);

GO
CREATE TRIGGER [sal].[utrLogDeliveryMethods] ON [sal].[utbDeliveryMethods]
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
				,'[sal].[utbDeliveryMethods]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM	Inserted
	END;

