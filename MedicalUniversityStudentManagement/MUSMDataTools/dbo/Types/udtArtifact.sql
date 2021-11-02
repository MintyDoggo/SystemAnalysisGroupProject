CREATE TYPE [dbo].[udtArtifact] AS TABLE
(
	[RequiredArtifactId] INT NOT NULL,
	[StudentId] INT NOT NULL,
	[DocumentReference] VARCHAR(64) NOT NULL,
	[CheckedOff] BIT NOT NULL
)
