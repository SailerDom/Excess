/****** Object:  ForeignKey [FK_MappedVariables_Instances]    Script Date: 10/12/2011 18:23:54 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MappedVariables_Instances]') AND parent_object_id = OBJECT_ID(N'[dbo].[MappedVariables]'))
ALTER TABLE [dbo].[MappedVariables] DROP CONSTRAINT [FK_MappedVariables_Instances]
GO
/****** Object:  ForeignKey [FK_StatusHistory_Instances]    Script Date: 10/12/2011 18:23:54 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StatusHistory_Instances]') AND parent_object_id = OBJECT_ID(N'[dbo].[StatusHistory]'))
ALTER TABLE [dbo].[StatusHistory] DROP CONSTRAINT [FK_StatusHistory_Instances]
GO
/****** Object:  ForeignKey [FK_DebugTraces_Instances]    Script Date: 10/12/2011 18:23:54 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DebugTraces_Instances]') AND parent_object_id = OBJECT_ID(N'[dbo].[DebugTraces]'))
ALTER TABLE [dbo].[DebugTraces] DROP CONSTRAINT [FK_DebugTraces_Instances]
GO
/****** Object:  Table [dbo].[MappedVariables]    Script Date: 10/12/2011 18:23:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MappedVariables]') AND type in (N'U'))
DROP TABLE [dbo].[MappedVariables]
GO
/****** Object:  ForeignKey [FK_InstanceMetadata_Instances]    Script Date: 05/24/2012 09:30:00 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InstanceMetadata_Instances]') AND parent_object_id = OBJECT_ID(N'[dbo].[InstanceMetadata]'))
ALTER TABLE [dbo].[InstanceMetadata] DROP CONSTRAINT [FK_InstanceMetadata_Instances]
GO
/****** Object:  Table [dbo].[StatusHistory]    Script Date: 10/12/2011 18:23:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StatusHistory]') AND type in (N'U'))
DROP TABLE [dbo].[StatusHistory]
GO
/****** Object:  Table [dbo].[DebugTraces]    Script Date: 10/12/2011 18:23:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DebugTraces]') AND type in (N'U'))
DROP TABLE [dbo].[DebugTraces]
GO
/****** Object:  Table [dbo].[InstanceMetadata]    Script Date: 05/24/2012 09:30:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InstanceMetadata]') AND type in (N'U'))
DROP TABLE [dbo].[InstanceMetadata]
GO
/****** Object:  Table [dbo].[Instances]    Script Date: 10/12/2011 18:23:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Instances]') AND type in (N'U'))
DROP TABLE [dbo].[Instances]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StoreVersionTable]') AND type in (N'U'))
DROP TABLE [dbo].[StoreVersionTable]
GO
/****** Object:  Table [dbo].[StoreVersionTable]	Script Date: 01/24/2012 00:00:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StoreVersionTable]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[StoreVersionTable](
    [Major] [int] NOT NULL,
    [Minor] [int] NOT NULL,
    [Build] [int] NOT NULL,
    [Revision] [int] NOT NULL,
    [LastUpdated] [datetime] NULL
CONSTRAINT [PK_StoreVersion] PRIMARY KEY CLUSTERED 
(
    [Major] ASC,
	[Minor] ASC,
	[Build] ASC,
	[Revision] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
END
GO
/****** Object:  Table [dbo].[Instances]    Script Date: 10/12/2011 18:23:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Instances]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Instances](
    [InstanceCounter] [bigint] IDENTITY(1,1) NOT NULL,
    [InstanceId] [uniqueidentifier] NOT NULL,
    [SessionId] [nvarchar](255) NULL,
    [ScopeId] [uniqueidentifier] NULL,
    [WorkflowName] [nvarchar](255) NOT NULL,
    [WorkflowId] [uniqueidentifier] NOT NULL,
    [MonitoringParameter] [nvarchar](255) NULL,
    [ActivationInfo] [xml] NULL,
    [WorkflowStatus] [nvarchar](40) NULL,
    [WorkflowStatusDetails] [nvarchar](max) NULL,
    [InstanceCreated] [datetime] NULL,
    [LastModified] [datetime] NOT NULL,
    [SequenceNumber] [bigint] NOT NULL,
    [Expiration] [datetime] NULL,
    [ExecutionTime] [datetime] NULL,	
    [DatabaseWriteTime] [datetime] NULL
     CONSTRAINT [PK_Instances] PRIMARY KEY CLUSTERED 
    (
        [InstanceCounter] ASC
    )WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Instances]') AND name = N'IX_Instances_InstanceId')
CREATE UNIQUE NONCLUSTERED INDEX [IX_Instances_InstanceId] ON [dbo].[Instances] 
(
    [InstanceId] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = ON, DROP_EXISTING = OFF, ONLINE = OFF)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Instances]') AND name = N'IX_Instances_MonitoringParameter')
CREATE NONCLUSTERED INDEX [IX_Instances_MonitoringParameter] ON [dbo].[Instances] 
(
    [MonitoringParameter] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Instances]') AND name = N'IX_Instances_ScopeId')
CREATE NONCLUSTERED INDEX [IX_Instances_ScopeId] ON [dbo].[Instances] 
(
    [ScopeId] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Instances]') AND name = N'IX_Instances_WorkflowId')
CREATE NONCLUSTERED INDEX [IX_Instances_WorkflowId] ON [dbo].[Instances] 
(
    [WorkflowId] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Instances]') AND name = N'IX_Instances_Expiration')
CREATE NONCLUSTERED INDEX [IX_Instances_Expiration] ON [dbo].[Instances] 
(
    [Expiration] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Instances]') AND name = N'IX_Instances_WorkflowName')
CREATE NONCLUSTERED INDEX [IX_Instances_WorkflowName] ON [dbo].[Instances] 
(
    [WorkflowName] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
/****** Object:  Table [dbo].[StatusHistory]    Script Date: 10/12/2011 18:23:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StatusHistory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[StatusHistory](
    [InstanceId] [uniqueidentifier] NOT NULL,
    [RecordNumber] [bigint] NOT NULL,
    [Message] [nvarchar](MAX) NULL,
    [CreationTime] [datetime] NOT NULL
 CONSTRAINT [PK_StatusHistory] PRIMARY KEY CLUSTERED 
(
    [InstanceId] ASC,
    [RecordNumber] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = ON)
)
END
GO
/****** Object:  Table [dbo].[DebugTraces]    Script Date: 10/12/2011 18:23:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DebugTraces]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DebugTraces](
    [InstanceId] [uniqueidentifier] NOT NULL,
    [RecordNumber] [bigint] NOT NULL,
    [Message] [nvarchar](MAX) NOT NULL,
    [CreationTime] [datetime] NOT NULL,
    [GroupId] [uniqueidentifier] NOT NULL,
    [Level] [int] NULL,
    [Name] [nvarchar](50) NULL,
    [Category] [nvarchar](50) NULL,
    [Data] [varbinary](MAX) NULL
 CONSTRAINT [PK_DebugTraces] PRIMARY KEY CLUSTERED 
(
    [InstanceId] ASC,
    [RecordNumber] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = ON)
)
END
GO
/****** Object:  Table [dbo].[MappedVariables]    Script Date: 10/12/2011 18:23:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MappedVariables]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MappedVariables](
    [InstanceId] [uniqueidentifier] NOT NULL,
    [Namespace] [nvarchar](255) NOT NULL,
    [Name] [nvarchar](180) NOT NULL,
    [Value] [nvarchar](255) NULL,
 CONSTRAINT [PK_MappedVariables] PRIMARY KEY CLUSTERED 
(
    [InstanceId] ASC,
    [Namespace] ASC,
    [Name] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = ON)
)
END
GO
/****** Object:  Table [dbo].[InstanceMetadata]    Script Date: 05/24/2012 09:30:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InstanceMetadata]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InstanceMetadata](
    [InstanceId] uniqueidentifier NOT NULL,
    [Property] [nvarchar](50) NOT NULL,
    [Value] [nvarchar](255) NULL,
 CONSTRAINT [PK_ActivationMetadata] PRIMARY KEY CLUSTERED 
(
    [InstanceId] ASC,
    [Property] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
END
GO
/****** Object:  ForeignKey [FK_MappedVariables_Instances]    Script Date: 10/12/2011 18:23:54 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MappedVariables_Instances]') AND parent_object_id = OBJECT_ID(N'[dbo].[MappedVariables]'))
ALTER TABLE [dbo].[MappedVariables]  WITH CHECK ADD  CONSTRAINT [FK_MappedVariables_Instances] FOREIGN KEY([InstanceId])
REFERENCES [dbo].[Instances] ([InstanceId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MappedVariables_Instances]') AND parent_object_id = OBJECT_ID(N'[dbo].[MappedVariables]'))
ALTER TABLE [dbo].[MappedVariables] CHECK CONSTRAINT [FK_MappedVariables_Instances]
GO
/****** Object:  ForeignKey [FK_StatusHistory_Instances]    Script Date: 10/12/2011 18:23:54 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StatusHistory_Instances]') AND parent_object_id = OBJECT_ID(N'[dbo].[StatusHistory]'))
ALTER TABLE [dbo].[StatusHistory]  WITH CHECK ADD  CONSTRAINT [FK_StatusHistory_Instances] FOREIGN KEY([InstanceId])
REFERENCES [dbo].[Instances] ([InstanceId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StatusHistory_Instances]') AND parent_object_id = OBJECT_ID(N'[dbo].[StatusHistory]'))
ALTER TABLE [dbo].[StatusHistory] CHECK CONSTRAINT [FK_StatusHistory_Instances]
GO
/****** Object:  ForeignKey [FK_DebugTraces_Instances]    Script Date: 10/12/2011 18:23:54 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DebugTraces_Instances]') AND parent_object_id = OBJECT_ID(N'[dbo].[DebugTraces]'))
ALTER TABLE [dbo].[DebugTraces]  WITH CHECK ADD  CONSTRAINT [FK_DebugTraces_Instances] FOREIGN KEY([InstanceId])
REFERENCES [dbo].[Instances] ([InstanceId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DebugTraces_Instances]') AND parent_object_id = OBJECT_ID(N'[dbo].[DebugTraces]'))
ALTER TABLE [dbo].[DebugTraces] CHECK CONSTRAINT [FK_DebugTraces_Instances]
GO
/****** Object:  ForeignKey [FK_InstanceMetadata_Instances]    Script Date: 05/24/2012 09:30:00 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InstanceMetadata_Instances]') AND parent_object_id = OBJECT_ID(N'[dbo].[InstanceMetadata]'))
ALTER TABLE [dbo].[InstanceMetadata]  WITH CHECK ADD  CONSTRAINT [FK_InstanceMetadata_Instances] FOREIGN KEY([InstanceId])
REFERENCES [dbo].[Instances] ([InstanceId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InstanceMetadata_Instances]') AND parent_object_id = OBJECT_ID(N'[dbo].[InstanceMetadata]'))
ALTER TABLE [dbo].[InstanceMetadata] CHECK CONSTRAINT [FK_InstanceMetadata_Instances]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[InstanceMetadata]') AND name = N'IX_InstanceMetadata_Property_Value')
CREATE NONCLUSTERED INDEX [IX_InstanceMetadata_Property_Value] ON [dbo].[InstanceMetadata] 
(
	[Property] ASC,
	[Value] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
