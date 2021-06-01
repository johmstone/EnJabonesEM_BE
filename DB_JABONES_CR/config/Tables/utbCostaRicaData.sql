CREATE TABLE [config].[utbCostaRicaData]
(
	[CostaRicaID]		INT			NOT NULL	PRIMARY KEY,
	[ProvinceID]		INT			NOT NULL,
	[Province]			VARCHAR(25)	NOT NULL,
	[CantonID]			INT			NOT NULL,
	[Canton]			VARCHAR(25)	NOT NULL,
	[DistrictID]		INT			NOT NULL,
	[District]			VARCHAR(50)	NOT NULL
)
