USE [master]
GO
/****** Object:  Database [meetingminutes]    Script Date: 8/25/2019 4:02:42 PM ******/
CREATE DATABASE [meetingminutes]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'meeting', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\meeting.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'meeting_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\meeting_log.ldf' , SIZE = 2560KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [meetingminutes] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [meetingminutes].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [meetingminutes] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [meetingminutes] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [meetingminutes] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [meetingminutes] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [meetingminutes] SET ARITHABORT OFF 
GO
ALTER DATABASE [meetingminutes] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [meetingminutes] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [meetingminutes] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [meetingminutes] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [meetingminutes] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [meetingminutes] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [meetingminutes] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [meetingminutes] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [meetingminutes] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [meetingminutes] SET  DISABLE_BROKER 
GO
ALTER DATABASE [meetingminutes] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [meetingminutes] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [meetingminutes] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [meetingminutes] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [meetingminutes] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [meetingminutes] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [meetingminutes] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [meetingminutes] SET RECOVERY FULL 
GO
ALTER DATABASE [meetingminutes] SET  MULTI_USER 
GO
ALTER DATABASE [meetingminutes] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [meetingminutes] SET DB_CHAINING OFF 
GO
ALTER DATABASE [meetingminutes] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [meetingminutes] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [meetingminutes] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'meetingminutes', N'ON'
GO
ALTER DATABASE [meetingminutes] SET QUERY_STORE = OFF
GO
USE [meetingminutes]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 8/25/2019 4:02:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 8/25/2019 4:02:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 8/25/2019 4:02:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 8/25/2019 4:02:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 8/25/2019 4:02:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 8/25/2019 4:02:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Decision]    Script Date: 8/25/2019 4:02:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Decision](
	[vDecisionID] [varchar](200) NOT NULL,
	[vAgendaID] [varchar](200) NOT NULL,
	[vDecisionDetails] [nvarchar](max) NOT NULL,
	[iIndex] [int] NOT NULL,
 CONSTRAINT [PK_Decision] PRIMARY KEY CLUSTERED 
(
	[vDecisionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Meeting]    Script Date: 8/25/2019 4:02:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Meeting](
	[vMeetingID] [varchar](200) NOT NULL,
	[Id] [nvarchar](128) NOT NULL,
	[vTitle] [nvarchar](max) NOT NULL,
	[dDate] [date] NOT NULL,
	[tStartTime] [time](0) NOT NULL,
	[tEndTime] [time](0) NOT NULL,
	[vLocation] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_Meeting] PRIMARY KEY CLUSTERED 
(
	[vMeetingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MeetingAgenda]    Script Date: 8/25/2019 4:02:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MeetingAgenda](
	[vAgendaID] [varchar](200) NOT NULL,
	[vMeetingID] [varchar](200) NOT NULL,
	[vAgendaName] [nvarchar](max) NOT NULL,
	[vAgendaDetails] [nvarchar](max) NOT NULL,
	[iIndex] [int] NOT NULL,
 CONSTRAINT [PK_MeetingAgenda] PRIMARY KEY CLUSTERED 
(
	[vAgendaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MeetingInvite]    Script Date: 8/25/2019 4:02:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MeetingInvite](
	[vMeetingInviteID] [varchar](200) NOT NULL,
	[vMeetingID] [varchar](200) NOT NULL,
	[Id] [nvarchar](128) NOT NULL,
	[iIndex] [int] NOT NULL,
 CONSTRAINT [PK_MeetingInvite] PRIMARY KEY CLUSTERED 
(
	[vMeetingInviteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_MeetingInvite] UNIQUE NONCLUSTERED 
(
	[vMeetingID] ASC,
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MeetingTask]    Script Date: 8/25/2019 4:02:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MeetingTask](
	[vTaskID] [varchar](200) NOT NULL,
	[vAgendaID] [varchar](200) NOT NULL,
	[vTaskDetails] [nvarchar](max) NOT NULL,
	[iIndex] [int] NOT NULL,
 CONSTRAINT [PK_MeetingTask] PRIMARY KEY CLUSTERED 
(
	[vTaskID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaskAssign]    Script Date: 8/25/2019 4:02:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaskAssign](
	[vTaskAssignID] [varchar](200) NOT NULL,
	[vTaskID] [varchar](200) NOT NULL,
	[Id] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_TaskAssign] PRIMARY KEY CLUSTERED 
(
	[vTaskAssignID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_TaskAssign] UNIQUE NONCLUSTERED 
(
	[vTaskID] ASC,
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Decision]  WITH CHECK ADD  CONSTRAINT [FK_Decision_MeetingAgenda] FOREIGN KEY([vAgendaID])
REFERENCES [dbo].[MeetingAgenda] ([vAgendaID])
GO
ALTER TABLE [dbo].[Decision] CHECK CONSTRAINT [FK_Decision_MeetingAgenda]
GO
ALTER TABLE [dbo].[Meeting]  WITH CHECK ADD  CONSTRAINT [FK_Meeting_AspNetUsers] FOREIGN KEY([Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Meeting] CHECK CONSTRAINT [FK_Meeting_AspNetUsers]
GO
ALTER TABLE [dbo].[MeetingAgenda]  WITH CHECK ADD  CONSTRAINT [FK_MeetingAgenda_Meeting] FOREIGN KEY([vMeetingID])
REFERENCES [dbo].[Meeting] ([vMeetingID])
GO
ALTER TABLE [dbo].[MeetingAgenda] CHECK CONSTRAINT [FK_MeetingAgenda_Meeting]
GO
ALTER TABLE [dbo].[MeetingInvite]  WITH CHECK ADD  CONSTRAINT [FK_MeetingInvite_AspNetUsers] FOREIGN KEY([Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[MeetingInvite] CHECK CONSTRAINT [FK_MeetingInvite_AspNetUsers]
GO
ALTER TABLE [dbo].[MeetingInvite]  WITH CHECK ADD  CONSTRAINT [FK_MeetingInvite_Meeting] FOREIGN KEY([vMeetingID])
REFERENCES [dbo].[Meeting] ([vMeetingID])
GO
ALTER TABLE [dbo].[MeetingInvite] CHECK CONSTRAINT [FK_MeetingInvite_Meeting]
GO
ALTER TABLE [dbo].[MeetingTask]  WITH CHECK ADD  CONSTRAINT [FK_MeetingTask_MeetingAgenda] FOREIGN KEY([vAgendaID])
REFERENCES [dbo].[MeetingAgenda] ([vAgendaID])
GO
ALTER TABLE [dbo].[MeetingTask] CHECK CONSTRAINT [FK_MeetingTask_MeetingAgenda]
GO
ALTER TABLE [dbo].[TaskAssign]  WITH CHECK ADD  CONSTRAINT [FK_TaskAssign_AspNetUsers] FOREIGN KEY([Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[TaskAssign] CHECK CONSTRAINT [FK_TaskAssign_AspNetUsers]
GO
ALTER TABLE [dbo].[TaskAssign]  WITH CHECK ADD  CONSTRAINT [FK_TaskAssign_MeetingTask] FOREIGN KEY([vTaskID])
REFERENCES [dbo].[MeetingTask] ([vTaskID])
GO
ALTER TABLE [dbo].[TaskAssign] CHECK CONSTRAINT [FK_TaskAssign_MeetingTask]
GO
USE [master]
GO
ALTER DATABASE [meetingminutes] SET  READ_WRITE 
GO
