USE [master]
GO
/****** Object:  Database [Corrida]    Script Date: 02/09/2019 23:40:56 ******/
CREATE DATABASE [Corrida]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Corrida', FILENAME = N'E:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\Corrida.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Corrida_log', FILENAME = N'E:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\Corrida_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [Corrida] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Corrida].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Corrida] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Corrida] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Corrida] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Corrida] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Corrida] SET ARITHABORT OFF 
GO
ALTER DATABASE [Corrida] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Corrida] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Corrida] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Corrida] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Corrida] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Corrida] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Corrida] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Corrida] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Corrida] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Corrida] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Corrida] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Corrida] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Corrida] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Corrida] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Corrida] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Corrida] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Corrida] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Corrida] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Corrida] SET  MULTI_USER 
GO
ALTER DATABASE [Corrida] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Corrida] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Corrida] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Corrida] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Corrida] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Corrida', N'ON'
GO
ALTER DATABASE [Corrida] SET QUERY_STORE = OFF
GO
USE [Corrida]
GO
/****** Object:  Table [dbo].[Corrida]    Script Date: 02/09/2019 23:40:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Corrida](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Inicio] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Track]    Script Date: 02/09/2019 23:40:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Track](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdCorrida] [int] NOT NULL,
	[Hora] [datetime] NOT NULL,
	[Elevacao] [float] NULL,
	[Batimento] [int] NULL,
	[CadenciaPasso] [int] NULL,
	[lat] [float] NULL,
	[lon] [float] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Track]  WITH CHECK ADD  CONSTRAINT [FK__track__IdCorrida__38996AB5] FOREIGN KEY([IdCorrida])
REFERENCES [dbo].[Corrida] ([Id])
GO
ALTER TABLE [dbo].[Track] CHECK CONSTRAINT [FK__track__IdCorrida__38996AB5]
GO
USE [master]
GO
ALTER DATABASE [Corrida] SET  READ_WRITE 
GO
