IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE TABLE [Diver] (
        [Id] uniqueidentifier NOT NULL,
        [Fullname] nvarchar(max) NOT NULL,
        [Firstname] nvarchar(max) NOT NULL,
        [Lastname] nvarchar(max) NOT NULL,
        [MemberSince] datetime2 NULL,
        [MemberUntil] datetime2 NULL,
        [WebsiteUrl] nvarchar(max) NULL,
        [TwitterHandle] nvarchar(max) NULL,
        [SkypeId] nvarchar(max) NULL,
        [Slogan] nvarchar(max) NULL,
        [Education] nvarchar(max) NULL,
        [Experience] nvarchar(max) NULL,
        [MobilePhone] nvarchar(max) NULL,
        [NotificationIntervalInHours] int NOT NULL DEFAULT 1,
        [LastNotificationCheckAt] datetime2 NULL,
        [UserId] nvarchar(450) NULL,
        CONSTRAINT [PK_Diver] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Diver_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE TABLE [Events] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [OrganisatorId] uniqueidentifier NOT NULL,
        [Location] nvarchar(max) NULL,
        [MeetingPoint] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [StartTime] datetime2 NOT NULL,
        [EndTime] datetime2 NULL,
        [Canceled] bit NOT NULL DEFAULT 0,
        [Deleted] bit NOT NULL DEFAULT 0,
        CONSTRAINT [PK_Events] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Events_Diver_OrganisatorId] FOREIGN KEY ([OrganisatorId]) REFERENCES [Diver] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE TABLE [Posts] (
        [Id] uniqueidentifier NOT NULL,
        [Category] int NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        [PublishDate] datetime2 NOT NULL,
        [ModificationDate] datetime2 NOT NULL,
        [AuthorId] uniqueidentifier NOT NULL,
        [Title] nvarchar(max) NOT NULL,
        [Text] nvarchar(max) NULL,
        [IntroImageId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Posts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Posts_Diver_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [Diver] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE TABLE [Comments] (
        [Id] uniqueidentifier NOT NULL,
        [EventId] uniqueidentifier NOT NULL,
        [AuthorId] uniqueidentifier NOT NULL,
        [CreateDate] datetime2 NOT NULL,
        [ModifiedDate] datetime2 NULL,
        [Text] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Comments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Comments_Diver_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [Diver] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Comments_Events_EventId] FOREIGN KEY ([EventId]) REFERENCES [Events] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE TABLE [Notifications] (
        [Id] uniqueidentifier NOT NULL,
        [RecipientId] uniqueidentifier NOT NULL,
        [OccuredAt] datetime2 NOT NULL,
        [AlreadySent] bit NOT NULL DEFAULT 0,
        [CountOfTries] int NOT NULL DEFAULT 0,
        [Message] nvarchar(max) NOT NULL,
        [Type] int NOT NULL,
        [EventId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Notifications] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Notifications_Events_EventId] FOREIGN KEY ([EventId]) REFERENCES [Events] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Notifications_Diver_RecipientId] FOREIGN KEY ([RecipientId]) REFERENCES [Diver] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE TABLE [Participants] (
        [Id] uniqueidentifier NOT NULL,
        [EventId] uniqueidentifier NOT NULL,
        [ParticipatingDiverId] uniqueidentifier NOT NULL,
        [CountPeople] int NOT NULL DEFAULT 1,
        [Note] nvarchar(max) NULL,
        [Status] int NOT NULL,
        [BuddyTeamName] nvarchar(max) NULL,
        CONSTRAINT [PK_Participants] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Participants_Events_EventId] FOREIGN KEY ([EventId]) REFERENCES [Events] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Participants_Diver_ParticipatingDiverId] FOREIGN KEY ([ParticipatingDiverId]) REFERENCES [Diver] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE TABLE [PostImages] (
        [Id] uniqueidentifier NOT NULL,
        [PostId] uniqueidentifier NOT NULL,
        [Caption] nvarchar(max) NULL,
        [ImageUrlThumbnail] nvarchar(max) NULL,
        [ImageUrlLarge] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_PostImages] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_PostImages_Posts_PostId] FOREIGN KEY ([PostId]) REFERENCES [Posts] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE INDEX [IX_Comments_AuthorId] ON [Comments] ([AuthorId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE INDEX [IX_Comments_EventId] ON [Comments] ([EventId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE INDEX [IX_Diver_UserId] ON [Diver] ([UserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE INDEX [IX_Events_OrganisatorId] ON [Events] ([OrganisatorId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE INDEX [IX_Events_StartTime_Deleted] ON [Events] ([StartTime], [Deleted]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE INDEX [IX_Notifications_EventId] ON [Notifications] ([EventId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE INDEX [IX_Notifications_RecipientId] ON [Notifications] ([RecipientId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE INDEX [IX_Participants_EventId] ON [Participants] ([EventId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE INDEX [IX_Participants_ParticipatingDiverId] ON [Participants] ([ParticipatingDiverId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE INDEX [IX_PostImages_PostId] ON [PostImages] ([PostId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE INDEX [IX_Posts_AuthorId] ON [Posts] ([AuthorId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    CREATE INDEX [IX_Posts_Category_PublishDate] ON [Posts] ([Category], [PublishDate]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181002051420_initial')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20181002051420_initial', N'2.2.3-servicing-35854');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181015191233_RemovePosts')
BEGIN
    DROP TABLE [PostImages];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181015191233_RemovePosts')
BEGIN
    DROP TABLE [Posts];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181015191233_RemovePosts')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20181015191233_RemovePosts', N'2.2.3-servicing-35854');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181015192404_AddFacebookId')
BEGIN
    ALTER TABLE [Diver] ADD [FacebookId] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181015192404_AddFacebookId')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20181015192404_AddFacebookId', N'2.2.3-servicing-35854');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181016143224_AddAvatarId')
BEGIN
    ALTER TABLE [Diver] ADD [AvatarId] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181016143224_AddAvatarId')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20181016143224_AddAvatarId', N'2.2.3-servicing-35854');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181127061632_Update_AspNetIdentity')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20181127061632_Update_AspNetIdentity', N'2.2.3-servicing-35854');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181127063336_Add_Diver_OwnNotifications')
BEGIN
    ALTER TABLE [Diver] ADD [SendOwnNoticiations] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181127063336_Add_Diver_OwnNotifications')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20181127063336_Add_Diver_OwnNotifications', N'2.2.3-servicing-35854');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190322161054_Add_LogbookEntry')
BEGIN
    CREATE TABLE [LogbookEntries] (
        [Id] uniqueidentifier NOT NULL,
        [Title] nvarchar(max) NULL,
        [Text] nvarchar(max) NULL,
        [TeaserText] nvarchar(max) NULL,
        [IsFavorite] bit NOT NULL,
        [TeaserImage] nvarchar(max) NULL,
        [TeaserImageThumb] nvarchar(max) NULL,
        [ExternalPhotoAlbumUrl] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [ModifiedAt] datetime2 NOT NULL,
        [EditorAuthorId] uniqueidentifier NOT NULL,
        [OriginalAuthorId] uniqueidentifier NOT NULL,
        [EventId] uniqueidentifier NULL,
        CONSTRAINT [PK_LogbookEntries] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_LogbookEntries_Diver_EditorAuthorId] FOREIGN KEY ([EditorAuthorId]) REFERENCES [Diver] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_LogbookEntries_Events_EventId] FOREIGN KEY ([EventId]) REFERENCES [Events] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_LogbookEntries_Diver_OriginalAuthorId] FOREIGN KEY ([OriginalAuthorId]) REFERENCES [Diver] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190322161054_Add_LogbookEntry')
BEGIN
    CREATE INDEX [IX_LogbookEntries_EditorAuthorId] ON [LogbookEntries] ([EditorAuthorId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190322161054_Add_LogbookEntry')
BEGIN
    CREATE INDEX [IX_LogbookEntries_EventId] ON [LogbookEntries] ([EventId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190322161054_Add_LogbookEntry')
BEGIN
    CREATE INDEX [IX_LogbookEntries_OriginalAuthorId] ON [LogbookEntries] ([OriginalAuthorId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190322161054_Add_LogbookEntry')
BEGIN
    CREATE INDEX [IX_LogbookEntries_IsFavorite_CreatedAt] ON [LogbookEntries] ([IsFavorite], [CreatedAt]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190322161054_Add_LogbookEntry')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190322161054_Add_LogbookEntry', N'2.2.3-servicing-35854');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190323082907_Change_Nullables_In_LogbookEntry')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[LogbookEntries]') AND [c].[name] = N'Title');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [LogbookEntries] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [LogbookEntries] ALTER COLUMN [Title] nvarchar(max) NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190323082907_Change_Nullables_In_LogbookEntry')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[LogbookEntries]') AND [c].[name] = N'Text');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [LogbookEntries] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [LogbookEntries] ALTER COLUMN [Text] nvarchar(max) NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190323082907_Change_Nullables_In_LogbookEntry')
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[LogbookEntries]') AND [c].[name] = N'TeaserText');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [LogbookEntries] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [LogbookEntries] ALTER COLUMN [TeaserText] nvarchar(max) NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190323082907_Change_Nullables_In_LogbookEntry')
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[LogbookEntries]') AND [c].[name] = N'ModifiedAt');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [LogbookEntries] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [LogbookEntries] ALTER COLUMN [ModifiedAt] datetime2 NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190323082907_Change_Nullables_In_LogbookEntry')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190323082907_Change_Nullables_In_LogbookEntry', N'2.2.3-servicing-35854');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190327090609_LogbookEntry_EditorAuthorId_Nullable')
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[LogbookEntries]') AND [c].[name] = N'EditorAuthorId');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [LogbookEntries] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [LogbookEntries] ALTER COLUMN [EditorAuthorId] uniqueidentifier NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190327090609_LogbookEntry_EditorAuthorId_Nullable')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190327090609_LogbookEntry_EditorAuthorId_Nullable', N'2.2.3-servicing-35854');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190623072628_AddLogbook_IsPublished')
BEGIN
    ALTER TABLE [LogbookEntries] ADD [IsPublished] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190623072628_AddLogbook_IsPublished')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190623072628_AddLogbook_IsPublished', N'2.2.3-servicing-35854');
END;

GO

