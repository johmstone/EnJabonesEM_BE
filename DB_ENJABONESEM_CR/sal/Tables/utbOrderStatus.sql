CREATE TABLE [sal].[utbOrderStatus]
(
	[StatusID]			INT				IDENTITY(1,1) NOT NULL,
	[Status]			VARCHAR(100)	NOT NULL,
	[InsertDate]		DATETIME		CONSTRAINT [utbOrderstatusDefaultInsertDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]		VARCHAR (100)	CONSTRAINT [utbOrderstatusDefaultInsertUserSuser_sSame] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate]	DATETIME		CONSTRAINT [utbOrderstatusDefaultLastModifyDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser]	VARCHAR (100)	CONSTRAINT [utbOrderstatusDefaultLastModifyUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
	CONSTRAINT [utbStatusID] PRIMARY KEY CLUSTERED ([StatusID] ASC),
);

GO
CREATE TRIGGER [sal].[utrLogOrderStatus] ON [sal].[utbOrderStatus]
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
				,'[sal].[utbOrderstatus]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM	Inserted
	END;
