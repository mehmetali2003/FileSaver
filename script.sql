USE [master]
GO
/****** Object:  Database [FileDB]    Script Date: 6/5/2022 10:21:09 PM ******/
CREATE DATABASE [FileDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'FileDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\FileDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'FileDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\FileDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [FileDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FileDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FileDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FileDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FileDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FileDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FileDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [FileDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [FileDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FileDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FileDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FileDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FileDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FileDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FileDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FileDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FileDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [FileDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FileDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FileDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FileDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FileDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FileDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [FileDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FileDB] SET RECOVERY FULL 
GO
ALTER DATABASE [FileDB] SET  MULTI_USER 
GO
ALTER DATABASE [FileDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FileDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [FileDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [FileDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [FileDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [FileDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'FileDB', N'ON'
GO
ALTER DATABASE [FileDB] SET QUERY_STORE = OFF
GO
USE [FileDB]
GO
/****** Object:  User [system]    Script Date: 6/5/2022 10:21:09 PM ******/
CREATE USER [system] FOR LOGIN [NT AUTHORITY\SYSTEM] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_accessadmin] ADD MEMBER [system]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [system]
GO
ALTER ROLE [db_datareader] ADD MEMBER [system]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [system]
GO
/****** Object:  Table [dbo].[FileDBResource]    Script Date: 6/5/2022 10:21:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileDBResource](
	[ResourceId] [bigint] IDENTITY(1,1) NOT NULL,
	[ResourceKey] [varchar](35) NOT NULL,
	[ResourceValue] [varchar](150) NOT NULL,
	[Active] [bit] NOT NULL,
	[DtTimeStamp] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MappingColumn]    Script Date: 6/5/2022 10:21:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MappingColumn](
	[IdColumn] [bigint] IDENTITY(1,1) NOT NULL,
	[IdTable] [bigint] NOT NULL,
	[DsColumnName] [varchar](50) NOT NULL,
	[DsDbType] [varchar](50) NOT NULL,
	[DsClassMember] [varchar](50) NOT NULL,
	[DsRequiredMsg] [varchar](70) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdColumn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MappingTable]    Script Date: 6/5/2022 10:21:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MappingTable](
	[IdTable] [bigint] IDENTITY(1,1) NOT NULL,
	[DsTable] [varchar](50) NOT NULL,
	[DsColumnPk] [varchar](50) NOT NULL,
	[DsClassName] [varchar](50) NOT NULL,
	[FlCached] [bit] NOT NULL,
	[DsMemberPk] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdTable] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Person]    Script Date: 6/5/2022 10:21:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Person](
	[PersonId] [bigint] NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[Surname] [varchar](50) NOT NULL,
	[Age] [int] NOT NULL,
	[Sex] [char](1) NOT NULL,
	[Mobile] [varchar](10) NOT NULL,
	[Active] [bit] NOT NULL,
	[DtTimeStamp] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[FileDBResource] ON 

INSERT [dbo].[FileDBResource] ([ResourceId], [ResourceKey], [ResourceValue], [Active], [DtTimeStamp]) VALUES (57, N'DeleteFail', N'Delete operation failed!', 1, CAST(N'2022-06-05T21:45:54.140' AS DateTime))
INSERT [dbo].[FileDBResource] ([ResourceId], [ResourceKey], [ResourceValue], [Active], [DtTimeStamp]) VALUES (58, N'DeleteFailConstraint', N'Constraint prevented delete operation!', 1, CAST(N'2022-06-05T21:45:54.140' AS DateTime))
INSERT [dbo].[FileDBResource] ([ResourceId], [ResourceKey], [ResourceValue], [Active], [DtTimeStamp]) VALUES (59, N'UpdateFail', N'Update operation failed!', 1, CAST(N'2022-06-05T21:45:54.140' AS DateTime))
INSERT [dbo].[FileDBResource] ([ResourceId], [ResourceKey], [ResourceValue], [Active], [DtTimeStamp]) VALUES (60, N'SavePersonFileFailed', N'Error occurred while person file saving!', 1, CAST(N'2022-06-05T21:45:54.140' AS DateTime))
INSERT [dbo].[FileDBResource] ([ResourceId], [ResourceKey], [ResourceValue], [Active], [DtTimeStamp]) VALUES (61, N'SavePersonFileSucceeded', N'Person file saved!', 1, CAST(N'2022-06-05T21:45:54.140' AS DateTime))
INSERT [dbo].[FileDBResource] ([ResourceId], [ResourceKey], [ResourceValue], [Active], [DtTimeStamp]) VALUES (62, N'SavePersonFilePartialySucceeded', N'People with following Id(s) could not be saved!', 1, CAST(N'2022-06-05T21:45:54.140' AS DateTime))
INSERT [dbo].[FileDBResource] ([ResourceId], [ResourceKey], [ResourceValue], [Active], [DtTimeStamp]) VALUES (63, N'ReqMsgIdentity', N'Identity is a required field!', 1, CAST(N'2022-06-05T21:45:54.140' AS DateTime))
INSERT [dbo].[FileDBResource] ([ResourceId], [ResourceKey], [ResourceValue], [Active], [DtTimeStamp]) VALUES (64, N'ReqMsgFirstName', N'FirstName is a required field', 1, CAST(N'2022-06-05T21:45:54.140' AS DateTime))
INSERT [dbo].[FileDBResource] ([ResourceId], [ResourceKey], [ResourceValue], [Active], [DtTimeStamp]) VALUES (65, N'ReqMsgSurname', N'Surname is a required field', 1, CAST(N'2022-06-05T21:45:54.140' AS DateTime))
INSERT [dbo].[FileDBResource] ([ResourceId], [ResourceKey], [ResourceValue], [Active], [DtTimeStamp]) VALUES (66, N'ReqMsgAge', N'Age is a required field', 1, CAST(N'2022-06-05T21:45:54.140' AS DateTime))
INSERT [dbo].[FileDBResource] ([ResourceId], [ResourceKey], [ResourceValue], [Active], [DtTimeStamp]) VALUES (67, N'ReqMsgSex', N'Sex is a required field', 1, CAST(N'2022-06-05T21:45:54.140' AS DateTime))
INSERT [dbo].[FileDBResource] ([ResourceId], [ResourceKey], [ResourceValue], [Active], [DtTimeStamp]) VALUES (68, N'ReqMsgMobile', N'Mobile is a required field', 1, CAST(N'2022-06-05T21:45:54.140' AS DateTime))
INSERT [dbo].[FileDBResource] ([ResourceId], [ResourceKey], [ResourceValue], [Active], [DtTimeStamp]) VALUES (69, N'ReqMsgActive', N'Active is a required field', 1, CAST(N'2022-06-05T21:45:54.140' AS DateTime))
INSERT [dbo].[FileDBResource] ([ResourceId], [ResourceKey], [ResourceValue], [Active], [DtTimeStamp]) VALUES (70, N'ReqMsgTimeStamp', N'Insert time is a required field', 1, CAST(N'2022-06-05T21:45:54.140' AS DateTime))
INSERT [dbo].[FileDBResource] ([ResourceId], [ResourceKey], [ResourceValue], [Active], [DtTimeStamp]) VALUES (71, N'ReqMsgResourceId', N'ResourceId is a required field', 1, CAST(N'2022-06-05T21:45:54.140' AS DateTime))
INSERT [dbo].[FileDBResource] ([ResourceId], [ResourceKey], [ResourceValue], [Active], [DtTimeStamp]) VALUES (72, N'ReqMsgResourceKey', N'ResourceKey is a required field', 1, CAST(N'2022-06-05T21:45:54.140' AS DateTime))
INSERT [dbo].[FileDBResource] ([ResourceId], [ResourceKey], [ResourceValue], [Active], [DtTimeStamp]) VALUES (73, N'ReqMsgResourceValue', N'ResourceValue is a required field', 1, CAST(N'2022-06-05T21:45:54.140' AS DateTime))
INSERT [dbo].[FileDBResource] ([ResourceId], [ResourceKey], [ResourceValue], [Active], [DtTimeStamp]) VALUES (74, N'SuccessfulDataImport', N'Data Imported to the Grid successfully.', 1, CAST(N'2022-06-05T21:45:54.140' AS DateTime))
INSERT [dbo].[FileDBResource] ([ResourceId], [ResourceKey], [ResourceValue], [Active], [DtTimeStamp]) VALUES (75, N'InvalidFileTypeToImport', N'Please check the selected file type', 1, CAST(N'2022-06-05T21:45:54.140' AS DateTime))
SET IDENTITY_INSERT [dbo].[FileDBResource] OFF
GO
SET IDENTITY_INSERT [dbo].[MappingColumn] ON 

INSERT [dbo].[MappingColumn] ([IdColumn], [IdTable], [DsColumnName], [DsDbType], [DsClassMember], [DsRequiredMsg]) VALUES (15, 1, N'PersonId', N'DbType.Int64', N'PersonId', N'ReqMsgIdentity')
INSERT [dbo].[MappingColumn] ([IdColumn], [IdTable], [DsColumnName], [DsDbType], [DsClassMember], [DsRequiredMsg]) VALUES (16, 1, N'FirstName', N'DbType.String', N'FirstName', N'ReqMsgFirstName')
INSERT [dbo].[MappingColumn] ([IdColumn], [IdTable], [DsColumnName], [DsDbType], [DsClassMember], [DsRequiredMsg]) VALUES (17, 1, N'Surname', N'DbType.String', N'Surname', N'ReqMsgSurname')
INSERT [dbo].[MappingColumn] ([IdColumn], [IdTable], [DsColumnName], [DsDbType], [DsClassMember], [DsRequiredMsg]) VALUES (18, 1, N'Age', N'DbType.Int32', N'Age', N'ReqMsgAge')
INSERT [dbo].[MappingColumn] ([IdColumn], [IdTable], [DsColumnName], [DsDbType], [DsClassMember], [DsRequiredMsg]) VALUES (19, 1, N'Sex', N'DbType.String', N'Sex', N'ReqMsgSex')
INSERT [dbo].[MappingColumn] ([IdColumn], [IdTable], [DsColumnName], [DsDbType], [DsClassMember], [DsRequiredMsg]) VALUES (20, 1, N'Mobile', N'DbType.String', N'Mobile', N'ReqMsgMobile')
INSERT [dbo].[MappingColumn] ([IdColumn], [IdTable], [DsColumnName], [DsDbType], [DsClassMember], [DsRequiredMsg]) VALUES (21, 1, N'Active', N'DbType.Byte', N'Active', N'ReqMsgActive')
INSERT [dbo].[MappingColumn] ([IdColumn], [IdTable], [DsColumnName], [DsDbType], [DsClassMember], [DsRequiredMsg]) VALUES (22, 1, N'DtTimeStamp', N'DbType.DateTime', N'DtTimeStamp', N'ReqMsgTimeStamp')
INSERT [dbo].[MappingColumn] ([IdColumn], [IdTable], [DsColumnName], [DsDbType], [DsClassMember], [DsRequiredMsg]) VALUES (23, 2, N'ResourceId', N'DbType.Int64', N'ResourceId', N'ReqMsgResourceId')
INSERT [dbo].[MappingColumn] ([IdColumn], [IdTable], [DsColumnName], [DsDbType], [DsClassMember], [DsRequiredMsg]) VALUES (24, 2, N'ResourceKey', N'DbType.String', N'ResourceKey', N'ReqMsgResourceKey')
INSERT [dbo].[MappingColumn] ([IdColumn], [IdTable], [DsColumnName], [DsDbType], [DsClassMember], [DsRequiredMsg]) VALUES (25, 2, N'ResourceValue', N'DbType.String', N'ResourceValue', N'ReqMsgResourceValue')
INSERT [dbo].[MappingColumn] ([IdColumn], [IdTable], [DsColumnName], [DsDbType], [DsClassMember], [DsRequiredMsg]) VALUES (26, 2, N'Active', N'DbType.Byte', N'Active', N'ReqMsgActive')
INSERT [dbo].[MappingColumn] ([IdColumn], [IdTable], [DsColumnName], [DsDbType], [DsClassMember], [DsRequiredMsg]) VALUES (27, 2, N'DtTimeStamp', N'DbType.DateTime', N'DtTimeStamp', N'ReqMsgTimeStamp')
SET IDENTITY_INSERT [dbo].[MappingColumn] OFF
GO
SET IDENTITY_INSERT [dbo].[MappingTable] ON 

INSERT [dbo].[MappingTable] ([IdTable], [DsTable], [DsColumnPk], [DsClassName], [FlCached], [DsMemberPk]) VALUES (1, N'Person', N'PersonId', N'Person', 0, N'PersonId')
INSERT [dbo].[MappingTable] ([IdTable], [DsTable], [DsColumnPk], [DsClassName], [FlCached], [DsMemberPk]) VALUES (2, N'FileDBResource', N'ResourceId', N'FileDBResource', 1, N'ResourceId')
SET IDENTITY_INSERT [dbo].[MappingTable] OFF
GO
INSERT [dbo].[Person] ([PersonId], [FirstName], [Surname], [Age], [Sex], [Mobile], [Active], [DtTimeStamp]) VALUES (3, N'Gerhard', N'Pretorious', 28, N'M', N'82123456', 1, CAST(N'2022-06-05T21:42:54.780' AS DateTime))
INSERT [dbo].[Person] ([PersonId], [FirstName], [Surname], [Age], [Sex], [Mobile], [Active], [DtTimeStamp]) VALUES (4, N'Draven', N'Wolf', 33, N'M', N'7232623', 1, CAST(N'2022-06-05T21:42:54.877' AS DateTime))
INSERT [dbo].[Person] ([PersonId], [FirstName], [Surname], [Age], [Sex], [Mobile], [Active], [DtTimeStamp]) VALUES (6, N'Cornelius', N'Van der Merwe', 19, N'M', N'829876543', 1, CAST(N'2022-06-05T21:42:55.013' AS DateTime))
INSERT [dbo].[Person] ([PersonId], [FirstName], [Surname], [Age], [Sex], [Mobile], [Active], [DtTimeStamp]) VALUES (8, N'Andrew', N'Caruthers', 45, N'M', N'7212345679', 1, CAST(N'2022-06-05T21:42:55.217' AS DateTime))
INSERT [dbo].[Person] ([PersonId], [FirstName], [Surname], [Age], [Sex], [Mobile], [Active], [DtTimeStamp]) VALUES (10, N'Patience', N'Ndlovu', 60, N'F', N'745551234', 1, CAST(N'2022-06-05T21:42:55.377' AS DateTime))
INSERT [dbo].[Person] ([PersonId], [FirstName], [Surname], [Age], [Sex], [Mobile], [Active], [DtTimeStamp]) VALUES (1, N'Joe', N'Van Tonder', 34, N'M', N'725329854', 1, CAST(N'2022-06-05T21:42:54.620' AS DateTime))
INSERT [dbo].[Person] ([PersonId], [FirstName], [Surname], [Age], [Sex], [Mobile], [Active], [DtTimeStamp]) VALUES (2, N'Pieter', N'Nel', 25, N'M', N'824981596', 0, CAST(N'2022-06-05T21:42:54.717' AS DateTime))
INSERT [dbo].[Person] ([PersonId], [FirstName], [Surname], [Age], [Sex], [Mobile], [Active], [DtTimeStamp]) VALUES (7, N'Tebogo', N'Gumede', 21, N'M', N'73439233', 1, CAST(N'2022-06-05T21:42:55.130' AS DateTime))
INSERT [dbo].[Person] ([PersonId], [FirstName], [Surname], [Age], [Sex], [Mobile], [Active], [DtTimeStamp]) VALUES (9, N'Janine', N'Erasmus', 49, N'F', N'8823263', 1, CAST(N'2022-06-05T21:42:55.297' AS DateTime))
INSERT [dbo].[Person] ([PersonId], [FirstName], [Surname], [Age], [Sex], [Mobile], [Active], [DtTimeStamp]) VALUES (5, N'Suzanne', N'Smith', 33, N'F', N'823687234', 1, CAST(N'2022-06-05T21:42:54.957' AS DateTime))
GO
ALTER TABLE [dbo].[MappingColumn] ADD  DEFAULT (NULL) FOR [DsRequiredMsg]
GO
USE [master]
GO
ALTER DATABASE [FileDB] SET  READ_WRITE 
GO
