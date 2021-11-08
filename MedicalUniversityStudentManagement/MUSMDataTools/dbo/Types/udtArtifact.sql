CREATE TYPE [dbo].[udtArtifact] AS TABLE
(
	[RequiredArtifactId] INT NOT NULL,
	[StudentId] INT NOT NULL,
	[Document] VARBINARY(MAX) NOT NULL,
	[CheckedOff] BIT NOT NULL
)
