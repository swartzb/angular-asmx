USE [Northwind]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Orders_Employees]') AND parent_object_id = OBJECT_ID(N'[dbo].[Orders]'))
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK_Orders_Employees]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EmployeeTerritories_Employees]') AND parent_object_id = OBJECT_ID(N'[dbo].[EmployeeTerritories]'))
ALTER TABLE [dbo].[EmployeeTerritories] DROP CONSTRAINT [FK_EmployeeTerritories_Employees]
GO

ALTER TABLE [dbo].[EmployeeTerritories]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeTerritories_Employees] FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employees] ([EmployeeID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[EmployeeTerritories] CHECK CONSTRAINT [FK_EmployeeTerritories_Employees]
GO

/****** Object:  View [dbo].[EmployeeSummaries]    Script Date: 09/30/2014 19:50:17 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[EmployeeSummaries]'))
DROP VIEW [dbo].[EmployeeSummaries]
GO

/****** Object:  UserDefinedFunction [dbo].[EmployeeDisplayName]    Script Date: 10/01/2014 20:52:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmployeeDisplayName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[EmployeeDisplayName]
GO

/****** Object:  UserDefinedFunction [dbo].[EmployeeDisplayName]    Script Date: 10/01/2014 20:52:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		me
-- Create date: 10/1/14
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[EmployeeDisplayName] 
(
	-- Add the parameters for the function here
	@id int
)
RETURNS VARCHAR(MAX)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result VARCHAR(MAX)

	-- Add the T-SQL statements to compute the return value here
	IF @id IS NULL
	BEGIN
		SELECT @Result = NULL
	END
	ELSE
	BEGIN
		DECLARE @LastName NVARCHAR(20)
		DECLARE @FirstName NVARCHAR(10)
		DECLARE @Title NVARCHAR(30)
		DECLARE @TitleOfCourtesy NVARCHAR(25)
		SELECT @LastName = LastName, @FirstName = FirstName, @Title = Title, @TitleOfCourtesy = TitleOfCourtesy
			FROM Employees WHERE (EmployeeID = @id)
		SELECT @Result = @LastName + ', ' + @FirstName
		IF @TitleOfCourtesy IS NOT NULL
		BEGIN
			SELECT @Result = @Result + ', ' + @TitleOfCourtesy
		END
		IF @Title IS NOT NULL
		BEGIN
			SELECT @Result = @Result + ', ' + @Title
		END
	END

	-- Return the result of the function
	RETURN @Result

END

GO

/****** Object:  UserDefinedFunction [dbo].[CanBeDeleted]    Script Date: 09/30/2014 19:32:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CanBeDeleted]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CanBeDeleted]
GO

/****** Object:  UserDefinedFunction [dbo].[CanBeDeleted]    Script Date: 09/30/2014 19:32:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		me
-- Create date: 
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[CanBeDeleted] 
(
	-- Add the parameters for the function here
	@id int
)
RETURNS bit
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result bit

	-- Add the T-SQL statements to compute the return value here
	IF EXISTS (SELECT * FROM Employees WHERE ReportsTo = @id)
	BEGIN
	--do what needs to be done if exists
		SELECT @Result = 0
	END
	ELSE
	BEGIN
	--do what needs to be done if not
		SELECT @Result = 1
	END

	-- Return the result of the function
	RETURN @Result

END

GO

/****** Object:  View [dbo].[EmployeeSummaries]    Script Date: 10/01/2014 20:59:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[EmployeeSummaries]
AS
SELECT     EmployeeID, dbo.EmployeeDisplayName(EmployeeID) AS Name, HireDate, Notes, ReportsTo, dbo.EmployeeDisplayName(ReportsTo) AS SupervisorName, 
                      dbo.CanBeDeleted(EmployeeID) AS CanBeDeleted
FROM         dbo.Employees

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[35] 4[26] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Employees"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 370
               Right = 203
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2715
         Alias = 3105
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'EmployeeSummaries'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'EmployeeSummaries'
GO

