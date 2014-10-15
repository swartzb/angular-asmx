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

/****** Object:  StoredProcedure [dbo].[EmployeeTerritoriesTest]    Script Date: 10/14/2014 20:07:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmployeeTerritoriesTest]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[EmployeeTerritoriesTest]
GO

/****** Object:  StoredProcedure [dbo].[TerritoriesForEmployeeTest]    Script Date: 10/12/2014 20:36:06 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerritoriesForEmployeeTest]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TerritoriesForEmployeeTest]
GO

/****** Object:  StoredProcedure [dbo].[EmployeeSummariesTest]    Script Date: 10/12/2014 20:07:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmployeeSummariesTest]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[EmployeeSummariesTest]
GO

/****** Object:  StoredProcedure [dbo].[CanReportToTest]    Script Date: 10/12/2014 19:49:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CanReportToTest]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CanReportToTest]
GO
/****** Object:  UserDefinedFunction [dbo].[CanReportTo]    Script Date: 10/08/2014 20:22:26 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CanReportTo]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CanReportTo]
GO

/****** Object:  UserDefinedFunction [dbo].[CanReportTo]    Script Date: 10/08/2014 20:22:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		me
-- Create date: 
-- Description:	Test if employee 1 can report to employee 2 without creating
-- loops in the org chart
-- =============================================
CREATE FUNCTION [dbo].[CanReportTo] 
(
	-- Add the parameters for the function here
	@empId1 int,
	@empId2 int
)
RETURNS bit
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result bit
	DECLARE @thisEmpId int

	-- Add the T-SQL statements to compute the return value here
	SELECT @thisEmpId = @empId2
	WHILE @thisEmpId IS NOT NULL
	BEGIN
		IF @thisEmpId = @empId1
		BEGIN
			RETURN 0
		END
		ELSE
		BEGIN
			SELECT @thisEmpId = ReportsTo FROM Employees WHERE (@thisEmpId = EmployeeID)
		END
	END
	RETURN 1

END

GO

/****** Object:  UserDefinedFunction [dbo].[EmployeeCoversTerritory]    Script Date: 10/05/2014 16:07:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmployeeCoversTerritory]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[EmployeeCoversTerritory]
GO

/****** Object:  UserDefinedFunction [dbo].[EmployeeCoversTerritory]    Script Date: 10/05/2014 16:07:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		me
-- Create date: 
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[EmployeeCoversTerritory] 
(
	-- Add the parameters for the function here
	@employeeId int,
	@territoryId int
)
RETURNS bit
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result bit

	-- Add the T-SQL statements to compute the return value here
	IF @employeeId IS NULL
	BEGIN
		SELECT @Result = 0
	END
	ELSE
	BEGIN
		IF EXISTS (SELECT EmployeeID, TerritoryID FROM EmployeeTerritories WHERE ((EmployeeID = @employeeId) AND (TerritoryID = @territoryId)))
		BEGIN
			SELECT @Result = 1
		END
		ELSE
		BEGIN
			SELECT @Result = 0
		END
	END

	-- Return the result of the function
	RETURN @Result

END

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

/****** Object:  StoredProcedure [dbo].[CanReportToTest]    Script Date: 10/12/2014 19:49:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		me
-- Create date: 
-- Description:	test the CanReportTo function
-- =============================================
CREATE PROCEDURE [dbo].[CanReportToTest] 
	-- Add the parameters for the stored procedure here
	@Id int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT     EmployeeID, dbo.EmployeeDisplayName(EmployeeID) AS Name
	FROM         Employees
	WHERE     (dbo.CanReportTo(@Id, EmployeeID) = 1)
	ORDER BY EmployeeID
END

GO

/****** Object:  StoredProcedure [dbo].[EmployeeSummariesTest]    Script Date: 10/12/2014 20:07:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		me
-- Create date: 
-- Description:	test employee summaries
-- =============================================
CREATE PROCEDURE [dbo].[EmployeeSummariesTest] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT     EmployeeID, dbo.EmployeeDisplayName(EmployeeID) AS Name, HireDate, Notes, dbo.EmployeeDisplayName(ReportsTo) AS SupervisorName, 
						  dbo.CanBeDeleted(EmployeeID) AS CanBeDeleted
	FROM         Employees
	ORDER BY LastName

END

GO

/****** Object:  StoredProcedure [dbo].[TerritoriesForEmployeeTest]    Script Date: 10/12/2014 20:36:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		me
-- Create date: 
-- Description:	test territories for employee
-- =============================================
CREATE PROCEDURE [dbo].[TerritoriesForEmployeeTest] 
	-- Add the parameters for the stored procedure here
	@id int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT     TerritoryID, TerritoryDescription, dbo.EmployeeCoversTerritory(@id, TerritoryID) AS EmployeeCoversTerritory
	FROM         Territories
	ORDER BY TerritoryDescription
END

GO

/****** Object:  StoredProcedure [dbo].[EmployeeTerritoriesTest]    Script Date: 10/14/2014 20:07:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		me
-- Create date: 
-- Description:	test employee territories
-- =============================================
CREATE PROCEDURE [dbo].[EmployeeTerritoriesTest] 
	-- Add the parameters for the stored procedure here
	@id int = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT     T.TerritoryDescription
	FROM         Territories AS T INNER JOIN
						  EmployeeTerritories AS ET ON T.TerritoryID = ET.TerritoryID
	WHERE     (ET.EmployeeID = @id)
	ORDER BY T.TerritoryDescription
END

GO

