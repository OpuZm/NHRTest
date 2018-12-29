USE [AutoDeploy]
GO

/****** Object:  Table [dbo].[Deploy]    Script Date: 2016/4/17 15:30:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Deploy](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AppId] [int] NOT NULL,
	[DeployCode] [nvarchar](50) NULL,
	[DeployPackage] [nvarchar](100) NULL,
	[DeployContent] [nvarchar](1000) NULL,
	[UploadUserId] [int] NULL,
	[UploadTime] [datetime] NULL,
	[DeployType] [int] NULL,
	[DeployUserId] [int] NULL,
	[DeployTime] [datetime] NULL,
 CONSTRAINT [PK_Deploy] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用程序标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Deploy', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用程序编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Deploy', @level2type=N'COLUMN',@level2name=N'AppId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部署编号(三位项目编码+两位应用编码)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Deploy', @level2type=N'COLUMN',@level2name=N'DeployCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部署包' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Deploy', @level2type=N'COLUMN',@level2name=N'DeployPackage'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部署内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Deploy', @level2type=N'COLUMN',@level2name=N'DeployContent'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上传者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Deploy', @level2type=N'COLUMN',@level2name=N'UploadUserId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上传时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Deploy', @level2type=N'COLUMN',@level2name=N'UploadTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作类型,1:上传 2:部署预上线 3:部署线上 4:回滚预上线 5:回滚线上' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Deploy', @level2type=N'COLUMN',@level2name=N'DeployType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部署用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Deploy', @level2type=N'COLUMN',@level2name=N'DeployUserId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部署时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Deploy', @level2type=N'COLUMN',@level2name=N'DeployTime'
GO

