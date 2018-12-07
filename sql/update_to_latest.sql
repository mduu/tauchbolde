CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE TABLE "AspNetRoles" (
        "Id" text NOT NULL,
        "Name" character varying(256) NULL,
        "NormalizedName" character varying(256) NULL,
        "ConcurrencyStamp" text NULL,
        CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE TABLE "AspNetUsers" (
        "Id" text NOT NULL,
        "UserName" character varying(256) NULL,
        "NormalizedUserName" character varying(256) NULL,
        "Email" character varying(256) NULL,
        "NormalizedEmail" character varying(256) NULL,
        "EmailConfirmed" boolean NOT NULL,
        "PasswordHash" text NULL,
        "SecurityStamp" text NULL,
        "ConcurrencyStamp" text NULL,
        "PhoneNumber" text NULL,
        "PhoneNumberConfirmed" boolean NOT NULL,
        "TwoFactorEnabled" boolean NOT NULL,
        "LockoutEnd" timestamp with time zone NULL,
        "LockoutEnabled" boolean NOT NULL,
        "AccessFailedCount" integer NOT NULL,
        CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE TABLE "AspNetRoleClaims" (
        "Id" serial NOT NULL,
        "RoleId" text NOT NULL,
        "ClaimType" text NULL,
        "ClaimValue" text NULL,
        CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE TABLE "AspNetUserClaims" (
        "Id" serial NOT NULL,
        "UserId" text NOT NULL,
        "ClaimType" text NULL,
        "ClaimValue" text NULL,
        CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE TABLE "AspNetUserLogins" (
        "LoginProvider" text NOT NULL,
        "ProviderKey" text NOT NULL,
        "ProviderDisplayName" text NULL,
        "UserId" text NOT NULL,
        CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
        CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE TABLE "AspNetUserRoles" (
        "UserId" text NOT NULL,
        "RoleId" text NOT NULL,
        CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
        CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE TABLE "AspNetUserTokens" (
        "UserId" text NOT NULL,
        "LoginProvider" text NOT NULL,
        "Name" text NOT NULL,
        "Value" text NULL,
        CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
        CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE TABLE "Diver" (
        "Id" uuid NOT NULL,
        "Fullname" text NOT NULL,
        "Firstname" text NOT NULL,
        "Lastname" text NOT NULL,
        "MemberSince" timestamp without time zone NULL,
        "MemberUntil" timestamp without time zone NULL,
        "AvatarId" text NULL,
        "WebsiteUrl" text NULL,
        "TwitterHandle" text NULL,
        "FacebookId" text NULL,
        "SkypeId" text NULL,
        "Slogan" text NULL,
        "Education" text NULL,
        "Experience" text NULL,
        "MobilePhone" text NULL,
        "NotificationIntervalInHours" integer NOT NULL DEFAULT 1,
        "SendOwnNoticiations" boolean NOT NULL,
        "LastNotificationCheckAt" timestamp without time zone NULL,
        "UserId" text NULL,
        CONSTRAINT "PK_Diver" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Diver_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE TABLE "Events" (
        "Id" uuid NOT NULL,
        "Name" text NOT NULL,
        "OrganisatorId" uuid NOT NULL,
        "Location" text NULL,
        "MeetingPoint" text NULL,
        "Description" text NULL,
        "StartTime" timestamp without time zone NOT NULL,
        "EndTime" timestamp without time zone NULL,
        "Canceled" boolean NOT NULL DEFAULT FALSE,
        "Deleted" boolean NOT NULL DEFAULT FALSE,
        CONSTRAINT "PK_Events" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Events_Diver_OrganisatorId" FOREIGN KEY ("OrganisatorId") REFERENCES "Diver" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE TABLE "Comments" (
        "Id" uuid NOT NULL,
        "EventId" uuid NOT NULL,
        "AuthorId" uuid NOT NULL,
        "CreateDate" timestamp without time zone NOT NULL,
        "ModifiedDate" timestamp without time zone NULL,
        "Text" text NOT NULL,
        CONSTRAINT "PK_Comments" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Comments_Diver_AuthorId" FOREIGN KEY ("AuthorId") REFERENCES "Diver" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_Comments_Events_EventId" FOREIGN KEY ("EventId") REFERENCES "Events" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE TABLE "Notifications" (
        "Id" uuid NOT NULL,
        "RecipientId" uuid NOT NULL,
        "OccuredAt" timestamp without time zone NOT NULL,
        "AlreadySent" boolean NOT NULL DEFAULT FALSE,
        "CountOfTries" integer NOT NULL DEFAULT 0,
        "Message" text NOT NULL,
        "Type" integer NOT NULL,
        "EventId" uuid NOT NULL,
        CONSTRAINT "PK_Notifications" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Notifications_Events_EventId" FOREIGN KEY ("EventId") REFERENCES "Events" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_Notifications_Diver_RecipientId" FOREIGN KEY ("RecipientId") REFERENCES "Diver" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE TABLE "Participants" (
        "Id" uuid NOT NULL,
        "EventId" uuid NOT NULL,
        "ParticipatingDiverId" uuid NOT NULL,
        "CountPeople" integer NOT NULL DEFAULT 1,
        "Note" text NULL,
        "Status" integer NOT NULL,
        "BuddyTeamName" text NULL,
        CONSTRAINT "PK_Participants" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Participants_Events_EventId" FOREIGN KEY ("EventId") REFERENCES "Events" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_Participants_Diver_ParticipatingDiverId" FOREIGN KEY ("ParticipatingDiverId") REFERENCES "Diver" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE UNIQUE INDEX "RoleNameIndex" ON "AspNetRoles" ("NormalizedName");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE INDEX "EmailIndex" ON "AspNetUsers" ("NormalizedEmail");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE INDEX "IX_Comments_AuthorId" ON "Comments" ("AuthorId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE INDEX "IX_Comments_EventId" ON "Comments" ("EventId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE INDEX "IX_Diver_UserId" ON "Diver" ("UserId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE INDEX "IX_Events_OrganisatorId" ON "Events" ("OrganisatorId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE INDEX "IX_Events_StartTime_Deleted" ON "Events" ("StartTime", "Deleted");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE INDEX "IX_Notifications_EventId" ON "Notifications" ("EventId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE INDEX "IX_Notifications_RecipientId" ON "Notifications" ("RecipientId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE INDEX "IX_Participants_EventId" ON "Participants" ("EventId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    CREATE INDEX "IX_Participants_ParticipatingDiverId" ON "Participants" ("ParticipatingDiverId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20181207214004_Initial_Postgres') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20181207214004_Initial_Postgres', '2.2.0-rtm-35687');
    END IF;
END $$;
