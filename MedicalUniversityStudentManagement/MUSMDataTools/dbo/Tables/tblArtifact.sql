CREATE TABLE [dbo].[tblArtifact]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[RequiredArtifactId] INT NOT NULL FOREIGN KEY REFERENCES tblRequiredArtifact(Id) ON DELETE CASCADE, -- the type of artifact
	[StudentId] INT NOT NULL FOREIGN KEY REFERENCES tblStudent(Id) ON DELETE CASCADE, -- the owning student

	[Document] VARBINARY(MAX) NOT NULL,
	[CheckedOff] BIT NOT NULL
)
