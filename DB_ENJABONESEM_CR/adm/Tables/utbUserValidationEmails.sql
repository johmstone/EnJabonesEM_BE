CREATE TABLE [adm].[utbUserValidationEmails]
(
	[EVToken]       VARCHAR (5000)  NOT NULL,
    [UserID]	    INT				NOT NULL,
    [IsValid]	    BIT             CONSTRAINT [utbUserValidationEmailsDefaultIsValidTrue] DEFAULT ((1)) NOT NULL,
    [CreationDate]  DATETIME        CONSTRAINT [utbUserValidationEmailsDefaultCreationDateSysDatetime] DEFAULT (sysdatetime()) NOT NULL,
    [CreationUser]  VARCHAR (100)   CONSTRAINT [utbUserValidationEmailsDefaultCreationUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbEVToken] PRIMARY KEY CLUSTERED ([EVToken]),
	CONSTRAINT [FK.adm.utbUsers.adm.utbUserValidationEmails.UserID] FOREIGN KEY ([UserID]) REFERENCES [adm].[utbUsers] ([UserID])
)
