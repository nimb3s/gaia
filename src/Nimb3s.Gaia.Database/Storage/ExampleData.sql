ALTER DATABASE [$(DatabaseName)]
    ADD FILEGROUP [ExampleData];
GO


ALTER DATABASE [$(DatabaseName)]
	ADD FILE
	(
		NAME = [ServicesData00],
		FILENAME = '$(DataPath)$(DatabaseName)\ExampleData00.ndf', SIZE = 100MB, FILEGROWTH = 100MB
	)
	TO FILEGROUP [ExampleData];
GO
	