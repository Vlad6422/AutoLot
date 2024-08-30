IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Customers] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [TimeStamp] rowversion NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Makes] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    [TimeStamp] rowversion NULL,
    CONSTRAINT [PK_Makes] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [CreditRisks] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [CustomerId] int NOT NULL,
    [TimeStamp] rowversion NULL,
    CONSTRAINT [PK_CreditRisks] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CreditRisks_Customers] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Inventory] (
    [Id] int NOT NULL IDENTITY,
    [MakeId] int NOT NULL,
    [Color] nvarchar(50) NOT NULL,
    [PetName] nvarchar(50) NOT NULL,
    [TimeStamp] rowversion NULL,
    CONSTRAINT [PK_Inventory] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Make_Inventory] FOREIGN KEY ([MakeId]) REFERENCES [Makes] ([Id])
);
GO

CREATE TABLE [Orders] (
    [Id] int NOT NULL IDENTITY,
    [CustomerId] int NOT NULL,
    [CarId] int NOT NULL,
    [TimeStamp] rowversion NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Orders_Customers] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Orders_Inventory] FOREIGN KEY ([CarId]) REFERENCES [Inventory] ([Id])
);
GO

CREATE INDEX [IX_CreditRisks_CustomerId] ON [CreditRisks] ([CustomerId]);
GO

CREATE INDEX [IX_Inventory_MakeId] ON [Inventory] ([MakeId]);
GO

CREATE INDEX [IX_Orders_CarId] ON [Orders] ([CarId]);
GO

CREATE UNIQUE INDEX [IX_Orders_CustomerId_CarId] ON [Orders] ([CustomerId], [CarId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240815140043_Initial', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Inventory] DROP CONSTRAINT [FK_Make_Inventory];
GO

IF SCHEMA_ID(N'Logging') IS NULL EXEC(N'CREATE SCHEMA [Logging];');
GO

ALTER SCHEMA [dbo] TRANSFER [Orders];
GO

ALTER SCHEMA [dbo] TRANSFER [Inventory];
GO

ALTER SCHEMA [dbo] TRANSFER [Customers];
GO

ALTER SCHEMA [dbo] TRANSFER [CreditRisks];
GO

ALTER TABLE [dbo].[Inventory] ADD [IsDrivable] bit NOT NULL DEFAULT CAST(1 AS bit);
GO

ALTER TABLE [dbo].[Customers] ADD [FullName] AS [LastName] + ', ' + [FirstName];
GO

ALTER TABLE [dbo].[CreditRisks] ADD [FullName] AS [LastName] + ', ' + [FirstName];
GO

CREATE TABLE [Logging].[SeriLogs] (
    [Id] int NOT NULL IDENTITY,
    [Message] nvarchar(max) NULL,
    [MessageTemplate] nvarchar(max) NULL,
    [Level] nvarchar(128) NULL,
    [TimeStamp] datetime2 NULL DEFAULT (GetDate()),
    [Exception] nvarchar(max) NULL,
    [Properties] Xml NULL,
    [LogEvent] nvarchar(max) NULL,
    [SourceContext] nvarchar(max) NULL,
    [RequestPath] nvarchar(max) NULL,
    [ActionName] nvarchar(max) NULL,
    [ApplicationName] nvarchar(max) NULL,
    [MachineName] nvarchar(max) NULL,
    [FilePath] nvarchar(max) NULL,
    [MemberName] nvarchar(max) NULL,
    [LineNumber] int NULL,
    CONSTRAINT [PK_SeriLogs] PRIMARY KEY ([Id])
);
GO

ALTER TABLE [dbo].[Inventory] ADD CONSTRAINT [FK_Make_Inventory] FOREIGN KEY ([MakeId]) REFERENCES [Makes] ([Id]) ON DELETE NO ACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240815144340_UpdatedEntities', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO


                exec (N' 
                CREATE PROCEDURE [dbo].[GetPetName]
                    @carID int,
                    @petName nvarchar(50) output
                AS
                    SELECT @petName = PetName from dbo.Inventory where Id = @carID
                ')
GO


                exec (N' 
                CREATE VIEW [dbo].[CustomerOrderView]
                AS
                    SELECT dbo.Customers.FirstName, dbo.Customers.LastName, dbo.Inventory.Color, dbo.Inventory.PetName, dbo.Inventory.IsDrivable, dbo.Makes.Name AS Make
                    FROM   dbo.Orders 
                    INNER JOIN dbo.Customers ON dbo.Orders.CustomerId = dbo.Customers.Id 
                    INNER JOIN dbo.Inventory ON dbo.Orders.CarId = dbo.Inventory.Id
                    INNER JOIN dbo.Makes ON dbo.Makes.Id = dbo.Inventory.MakeId
                ')
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240815150632_SQL', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240819113404_UpdateFluentApiOrder', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DROP INDEX [IX_Orders_CustomerId_CarId] ON [dbo].[Orders];
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dbo].[Orders]') AND [c].[name] = N'CarId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [dbo].[Orders] ALTER COLUMN [CarId] int NULL;
GO

CREATE UNIQUE INDEX [IX_Orders_CustomerId_CarId] ON [dbo].[Orders] ([CustomerId], [CarId]) WHERE [CarId] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240828120611_Orders_CarId_nullableOn', N'8.0.8');
GO

COMMIT;
GO

