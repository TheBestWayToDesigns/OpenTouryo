USE [Workflow]
GO

SELECT [Id]
      ,[Section]
      ,[Name]
      ,[PositionTitlesId]
  FROM [dbo].[M_User]
GO

SELECT [Id]
      ,[SubSystemId]
      ,[WorkflowName]
      ,[WfPositionId]
      ,[WorkflowNo]
      ,[FromUserId]
      ,[ActionType]
      ,[ToUserId]
      ,[ToUserPositionTitlesId]
      ,[SortIndex]
      ,[NextWfPositionId]
      ,[NextWorkflowNo]
      ,[MailTemplateId]
      ,[ReserveArea]
  FROM [dbo].[M_Workflow]
GO

SELECT [WorkflowControlNo]
      ,[SubSystemId]
      ,[WorkflowName]
      ,[UserId]
      ,[UserInfo]
      ,[ReserveArea]
      ,[CreatedDate]
      ,[EndDate]
  FROM [dbo].[T_Workflow]
GO

SELECT [WorkflowControlNo]
      ,[HistoryNo]
      ,[WfPositionId]
      ,[WorkflowNo]
      ,[FromUserId]
      ,[FromUserInfo]
      ,[ActionType]
      ,[ToUserId]
      ,[ToUserInfo]
      ,[ToUserPositionTitlesId]
      ,[NextWfPositionId]
      ,[NextWorkflowNo]
      ,[ReserveArea]
      ,[ExclusiveKey]
      ,[ReplyDeadline]
      ,[CreatedDate]
      ,[AcceptanceDate]
      ,[AcceptanceUserId]
      ,[AcceptanceUserInfo]
  FROM [dbo].[T_CurrentWorkflow]
GO

SELECT [WorkflowControlNo]
      ,[HistoryNo]
      ,[WfPositionId]
      ,[WorkflowNo]
      ,[FromUserId]
      ,[FromUserInfo]
      ,[ActionType]
      ,[ToUserId]
      ,[ToUserInfo]
      ,[ToUserPositionTitlesId]
      ,[NextWfPositionId]
      ,[NextWorkflowNo]
      ,[ReserveArea]
      ,[ReplyDeadline]
      ,[StartDate]
      ,[AcceptanceDate]
      ,[AcceptanceUserId]
      ,[AcceptanceUserInfo]
      ,[EndDate]
  FROM [dbo].[T_WorkflowHistory]
GO
