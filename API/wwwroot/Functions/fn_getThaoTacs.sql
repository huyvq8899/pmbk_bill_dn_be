USE [DemoRDAccountant]
GO

/****** Object:  UserDefinedFunction [dbo].[fn_getThaoTacs]    Script Date: 7/2/2021 2:29:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER FUNCTION [dbo].[fn_getThaoTacs](
	@functionId NVARCHAR(MAX)
,	@roleId		NVARCHAR(MAX)
,	@selectedFunctionId NVARCHAR(MAX)
)RETURNS @result TABLE(
 ThaoTacId				    NVARCHAR(36),
 PemissionId				NVARCHAR(36),
 FunctionId					NVARCHAR(36),
 RoleId						NVARCHAR(36),
 UserId						NVARCHAR(36),
 FTID						NVARCHAR(36),
 UTID						NVARCHAR(36),
 Ma							NVARCHAR(MAX),
 Ten						NVARCHAR(MAX),
 STT						int,
 Active						bit
)
AS 
BEGIN
	DECLARE @FunctionName NVARCHAR(MAX)
	DECLARE @FunctionFName NVARCHAR(MAX)
	SELECT @FunctionName = Type, @FunctionFName = FunctionName
	FROM Functions
	WHERE FunctionId = @FunctionId

	IF @FunctionName = N'Hóa đơn'
	BEGIN
		INSERT INTO @result 
		  SELECT DISTINCT [tt].[ThaoTacId] AS ThaoTacId
			   , [fnc_tt1].[PermissionId] AS [PemissionId]
			   , [func].[FunctionId] AS FunctionId
			   , [role].[RoleId] AS RoleId
			   , '' AS UserId
			   , [fnc_tt1].[FTID] AS FTID
			   , '' AS UTID
			   , [tt].[Ma] AS Ma
			   , [tt].[Ten] AS Ten
			   , [tt].[STT] AS STT
			   , CASE
					WHEN [func].[FunctionId] IN (SELECT Name FROM dbo.splitstring(@selectedFunctionId)) AND ((
					SELECT COUNT(*)
					FROM [Function_ThaoTacs] AS [x]
					WHERE ([x].[FunctionId] = [func].[FunctionId]) AND ([x].[ThaoTacId] = [tt].[ThaoTacId]) AND ([x].[RoleId] = [role].[RoleId])
			  ) > 0)
			  THEN CASE
				  WHEN [fnc_tt1].[Active] IS NOT NULL AND ([fnc_tt1].[Active] = 1)
				  THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT)
			  END ELSE CAST(0 AS BIT)
		  END AS [Active]
		  FROM [Roles] AS [role]
		  LEFT JOIN [Function_Roles] AS [fnc_role] ON [role].[RoleId] = [fnc_role].[RoleId]
		  LEFT JOIN [Function_ThaoTacs] AS [fnc_tt1] ON [fnc_role].[FunctionId] = [fnc_tt1].[FunctionId]
		  CROSS JOIN [Functions] AS [func]
		  CROSS JOIN [ThaoTacs] AS [tt]
		  WHERE (([func].[FunctionId] = @FunctionId) AND ([role].[RoleId] = @RoleId)) AND ([tt].[Ma] LIKE N'HD' + N'%' AND (LEFT([tt].[Ma], LEN(N'HD')) = N'HD'))
		  ORDER BY [tt].[STT]
	  END
	  ELSE IF @FunctionName = N'Hệ thống'
	  BEGIN
	  	INSERT INTO @result 
		  SELECT [tt].[ThaoTacId] AS ThaoTacId
			   , [fnc_tt1].[PermissionId] AS [PemissionId]
			   , [func].[FunctionId] AS FunctionId
			   , [role].[RoleId] AS RoleId
			   , '' AS UserId
			   , [fnc_tt1].[FTID] AS FTID
			   , '' AS UTID
			   , [tt].[Ma] AS Ma
			   , [tt].[Ten] AS Ten
			   , [tt].[STT] AS STT
			   , CASE
					WHEN [func].[FunctionId] IN (SELECT Name FROM dbo.splitstring(@selectedFunctionId)) AND ((
					SELECT COUNT(*)
					FROM [Function_ThaoTacs] AS [x]
					WHERE ([x].[FunctionId] = [func].[FunctionId]) AND ([x].[ThaoTacId] = [tt].[ThaoTacId]) AND ([x].[RoleId] = [role].[RoleId])
			  ) > 0)
			  THEN CASE
				  WHEN [fnc_tt1].[Active] IS NOT NULL AND ([fnc_tt1].[Active] = 1)
				  THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT)
			  END ELSE CAST(0 AS BIT)
		  END AS [Active]
		  FROM [Roles] AS [role]
		  LEFT JOIN [Function_Roles] AS [fnc_role] ON [role].[RoleId] = [fnc_role].[RoleId]
		  LEFT JOIN [Function_ThaoTacs] AS [fnc_tt1] ON [fnc_role].[FunctionId] = [fnc_tt1].[FunctionId]
		  CROSS JOIN [Functions] AS [func]
		  CROSS JOIN [ThaoTacs] AS [tt]
		  WHERE (([func].[FunctionId] = @FunctionId) AND ([role].[RoleId] = @RoleId)) AND ([tt].[Ma] LIKE N'SYS' + N'%' AND (LEFT([tt].[Ma], LEN(N'SYS')) = N'SYS'))
		  ORDER BY [tt].[STT]
	  END
	  ELSE IF @FunctionName = N'Báo cáo'
	  BEGIN
	  	INSERT INTO @result 
		  SELECT [tt].[ThaoTacId] AS ThaoTacId
			   , [fnc_tt1].[PermissionId] AS [PemissionId]
			   , [func].[FunctionId] AS FunctionId
			   , [role].[RoleId] AS RoleId
			   , '' AS UserId
			   , [fnc_tt1].[FTID] AS FTID
			   , '' AS UTID
			   , [tt].[Ma] AS Ma
			   , [tt].[Ten] AS Ten
			   , [tt].[STT] AS STT
			   , CASE
					WHEN [func].[FunctionId] IN (SELECT Name FROM dbo.splitstring(@selectedFunctionId)) AND ((
					SELECT COUNT(*)
					FROM [Function_ThaoTacs] AS [x]
					WHERE ([x].[FunctionId] = [func].[FunctionId]) AND ([x].[ThaoTacId] = [tt].[ThaoTacId]) AND ([x].[RoleId] = [role].[RoleId])
			  ) > 0)
			  THEN CASE
				  WHEN [fnc_tt1].[Active] IS NOT NULL AND ([fnc_tt1].[Active] = 1)
				  THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT)
			  END ELSE CAST(0 AS BIT)
		  END AS [Active]
		  FROM [Roles] AS [role]
		  LEFT JOIN [Function_Roles] AS [fnc_role] ON [role].[RoleId] = [fnc_role].[RoleId]
		  LEFT JOIN [Function_ThaoTacs] AS [fnc_tt1] ON [fnc_role].[FunctionId] = [fnc_tt1].[FunctionId]
		  CROSS JOIN [Functions] AS [func]
		  CROSS JOIN [ThaoTacs] AS [tt]
		  WHERE (([func].[FunctionId] = @FunctionId) AND ([role].[RoleId] = @RoleId)) AND ([tt].[Ma] LIKE N'BC' + N'%' AND (LEFT([tt].[Ma], LEN(N'BC')) = N'BC'))
		  ORDER BY [tt].[STT]
	  END
	  ELSE IF @FunctionName = N'Tiện ích'
	  BEGIN
	  	INSERT INTO @result 
		  SELECT [tt].[ThaoTacId] AS ThaoTacId
			   , [fnc_tt1].[PermissionId] AS [PemissionId]
			   , [func].[FunctionId] AS FunctionId
			   , [role].[RoleId] AS RoleId
			   , '' AS UserId
			   , [fnc_tt1].[FTID] AS FTID
			   , '' AS UTID
			   , [tt].[Ma] AS Ma
			   , [tt].[Ten] AS Ten
			   , [tt].[STT] AS STT
			   , CASE
					WHEN [func].[FunctionId] IN (SELECT Name FROM dbo.splitstring(@selectedFunctionId)) AND ((
					SELECT COUNT(*)
					FROM [Function_ThaoTacs] AS [x]
					WHERE ([x].[FunctionId] = [func].[FunctionId]) AND ([x].[ThaoTacId] = [tt].[ThaoTacId]) AND ([x].[RoleId] = [role].[RoleId])
			  ) > 0)
			  THEN CASE
				  WHEN [fnc_tt1].[Active] IS NOT NULL AND ([fnc_tt1].[Active] = 1)
				  THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT)
			  END ELSE CAST(0 AS BIT)
		  END AS [Active]
		  FROM [Roles] AS [role]
		  LEFT JOIN [Function_Roles] AS [fnc_role] ON [role].[RoleId] = [fnc_role].[RoleId]
		  LEFT JOIN [Function_ThaoTacs] AS [fnc_tt1] ON [fnc_role].[FunctionId] = [fnc_tt1].[FunctionId]
		  CROSS JOIN [Functions] AS [func]
		  CROSS JOIN [ThaoTacs] AS [tt]
		  WHERE (([func].[FunctionId] = @FunctionId) AND ([role].[RoleId] = @RoleId)) AND ([tt].[Ma] LIKE N'TOOL' + N'%' AND (LEFT([tt].[Ma], LEN(N'TOOL')) = N'TOOL'))
		  ORDER BY [tt].[STT]
	  END
	  ELSE IF @FunctionName = N'Danh mục'
	  BEGIN
	  		INSERT INTO @result 
			  SELECT [tt].[ThaoTacId] AS ThaoTacId
				   , [fnc_tt1].[PermissionId] AS [PemissionId]
				   , [func].[FunctionId] AS FunctionId
				   , [role].[RoleId] AS RoleId
				   , '' AS UserId
				   , [fnc_tt1].[FTID] AS FTID
				   , '' AS UTID
				   , [tt].[Ma] AS Ma
				   , [tt].[Ten] AS Ten
				   , [tt].[STT] AS STT
				   , CASE
						WHEN [func].[FunctionId] IN (SELECT Name FROM dbo.splitstring(@selectedFunctionId)) AND ((
						SELECT COUNT(*)
						FROM [Function_ThaoTacs] AS [x]
						WHERE ([x].[FunctionId] = [func].[FunctionId]) AND ([x].[ThaoTacId] = [tt].[ThaoTacId]) AND ([x].[RoleId] = [role].[RoleId])
				  ) > 0)
				  THEN CASE
					  WHEN [fnc_tt1].[Active] IS NOT NULL AND ([fnc_tt1].[Active] = 1)
					  THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT)
				  END ELSE CAST(0 AS BIT)
			  END AS [Active]
			  FROM [Roles] AS [role]
			  LEFT JOIN [Function_Roles] AS [fnc_role] ON [role].[RoleId] = [fnc_role].[RoleId]
			  LEFT JOIN [Function_ThaoTacs] AS [fnc_tt1] ON [fnc_role].[FunctionId] = [fnc_tt1].[FunctionId]
			  CROSS JOIN [Functions] AS [func]
			  CROSS JOIN [ThaoTacs] AS [tt]
			  WHERE (([func].[FunctionId] = @FunctionId) AND ([role].[RoleId] = @RoleId)) AND ([tt].[Ma] LIKE N'DM' + N'%' AND (LEFT([tt].[Ma], LEN(N'DM')) = N'DM'))
			  ORDER BY [tt].[STT]
	  END
	RETURN;
END

GO

