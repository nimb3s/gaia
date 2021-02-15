ALTER DATABASE [$(DatabaseName)]
    ADD FILEGROUP [ExampleLargeBinaryObjects];
GO
ALTER DATABASE [$(DatabaseName)]
	ADD FILE
	(
		NAME = [ExampleLargeBinaryObjects00],
		FILENAME = '$(DataPath)$(DatabaseName)\ExampleLargeBinaryObjects00.ndf', SIZE = 100MB, FILEGROWTH = 100MB
	)
	TO FILEGROUP [ExampleLargeBinaryObjects];
GO

