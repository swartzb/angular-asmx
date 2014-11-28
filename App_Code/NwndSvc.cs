using System.Collections.Generic;
using System.Web.Services;
using System.Configuration;
using System.Web.Script.Services;
using System.Threading;
using System;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;
using DA = DataAccess;

/// <summary>
/// Summary description for NwndSvc
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class NwndSvc : System.Web.Services.WebService
{
  string _connectionString;

  public NwndSvc()
  {
    _connectionString = ConfigurationManager.ConnectionStrings["NorthwindConnectionString"].ConnectionString;

    //Uncomment the following line if using designed components 
    //InitializeComponent(); 
  }

  [WebMethod]
  [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
  public List<DA.Employee> GetCanReportTo(int id)
  {
    List<DA.Employee> eList = new List<DA.Employee>();

    Thread.Sleep(TimeSpan.FromSeconds(1));

    using (SqlConnection conn = new SqlConnection(_connectionString))
    {
      conn.Open();
      using (SqlTransaction txn = conn.BeginTransaction())
      {
        string sqlCmd = "SELECT EmployeeID, Name, ReportsTo FROM vwEmployees" +
          " WHERE (dbo.CanReportTo(@id, EmployeeID) = 1) ORDER BY Name";
        using (SqlCommand cmd = new SqlCommand(sqlCmd, conn, txn))
        {
          cmd.Parameters.AddWithValue("@id", id);
          using (SqlDataReader rdr = cmd.ExecuteReader())
          {
            while (rdr.Read())
            {
              DA.Employee e = new DA.Employee
              {
                EmployeeID = rdr.GetInt32(rdr.GetOrdinal("EmployeeID")),
                Name = rdr.GetString(rdr.GetOrdinal("Name")),
                ReportsTo = rdr.IsDBNull(rdr.GetOrdinal("ReportsTo"))
                    ? (int?)null : rdr.GetInt32(rdr.GetOrdinal("ReportsTo")),
              };

              eList.Add(e);
            }
          }
        }

        txn.Commit();
      }
    }

    return eList;
  }

  [WebMethod]
  [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
  public List<Territory> GetTerritories(int id)
  {
    List<Territory> tList = new List<Territory>();

    Thread.Sleep(TimeSpan.FromSeconds(1));

    using (SqlConnection conn = new SqlConnection(_connectionString))
    {
      conn.Open();
      using (SqlTransaction txn = conn.BeginTransaction())
      {
        string sqlCmd = "SELECT TerritoryID, TerritoryDescription, dbo.EmployeeCoversTerritory(@id, TerritoryID) AS EmployeeCoversTerritory FROM Territories ORDER BY TerritoryDescription";
        using (SqlCommand cmd = new SqlCommand(sqlCmd, conn, txn))
        {
          cmd.Parameters.AddWithValue("@id", id);
          using (SqlDataReader rdr = cmd.ExecuteReader())
          {
            while (rdr.Read())
            {
              Territory t = new Territory
              {
                TerritoryID = rdr.GetString(rdr.GetOrdinal("TerritoryID")),
                TerritoryDescription = rdr.GetString(rdr.GetOrdinal("TerritoryDescription")),
                EmployeeCoversTerritory = rdr.GetBoolean(rdr.GetOrdinal("EmployeeCoversTerritory")),
              };
              tList.Add(t);
            }
          }
        }

        txn.Commit();
      }
    }

    return tList;
  }

  [WebMethod]
  [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
  public List<DA.Employee> UpdateSupervisor(int empId, int supId, bool hasValue)
  {
    List<DA.Employee> eList = new List<DA.Employee>();

    Thread.Sleep(TimeSpan.FromSeconds(1));

    using (SqlConnection conn = new SqlConnection(_connectionString))
    {
      conn.Open();
      using (SqlTransaction txn = conn.BeginTransaction())
      {
        int numRows;
        string cmdStr = "UPDATE Employees SET ReportsTo = @supId WHERE (EmployeeID = @empId)";
        using (SqlCommand cmd = new SqlCommand(cmdStr, conn, txn))
        {
          cmd.Parameters.AddWithValue("@empId", empId);
          SqlParameter p = new SqlParameter
          {
            ParameterName = "@supId",
            IsNullable = true,
            SqlDbType = SqlDbType.Int,
            Value = hasValue ? (object)supId : DBNull.Value
          };
          cmd.Parameters.Add(p);
          numRows = cmd.ExecuteNonQuery();
        }

        eList = GetEmployeesInner(conn, txn);
        txn.Commit();
      }
    }

    return eList;
  }

  [WebMethod]
  [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
  public List<DA.Employee> UpdateTerritories(int id, List<string> territoryIDs)
  {
    List<DA.Employee> eList;

    Thread.Sleep(TimeSpan.FromSeconds(1));

    using (SqlConnection conn = new SqlConnection(_connectionString))
    {
      conn.Open();
      using (SqlTransaction txn = conn.BeginTransaction())
      {
        int numRows;
        string delCmdStr = "DELETE FROM EmployeeTerritories WHERE (EmployeeID = @id)";
        using (SqlCommand cmd = new SqlCommand(delCmdStr, conn, txn))
        {
          cmd.Parameters.AddWithValue("@id", id);
          numRows = cmd.ExecuteNonQuery();
        }

        foreach (string tId in territoryIDs)
        {
          string insCmdStr = "INSERT INTO EmployeeTerritories (EmployeeID, TerritoryID) VALUES (@eId, @tId)";
          using (SqlCommand cmd = new SqlCommand(insCmdStr, conn, txn))
          {
            cmd.Parameters.AddWithValue("@eId", id);
            cmd.Parameters.AddWithValue("@tId", tId);
            numRows = cmd.ExecuteNonQuery();
          }
        }

        eList = GetEmployeesInner(conn, txn);
        txn.Commit();
      }
    }

    return eList;
  }

  [WebMethod]
  [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
  public List<DA.Employee> GetEmployees()
  {
    List<DA.Employee> eList;

    Thread.Sleep(TimeSpan.FromSeconds(1));

    using (SqlConnection conn = new SqlConnection(_connectionString))
    {
      conn.Open();
      using (SqlTransaction txn = conn.BeginTransaction())
      {
        eList = GetEmployeesInner(conn, txn);
        txn.Commit();
      }
    }

    return eList;
  }

  [WebMethod]
  [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
  public List<DA.Employee> EditEmployee(DA.Employee employee)
  {
    List<DA.Employee> eList;

    Thread.Sleep(TimeSpan.FromSeconds(1));

    using (SqlConnection conn = new SqlConnection(_connectionString))
    {
      conn.Open();
      using (SqlTransaction txn = conn.BeginTransaction())
      {
        int numRows = Update(conn, txn, employee); 
        eList = GetEmployeesInner(conn, txn);
        txn.Commit();
      }
    }

    return eList;
  }

  [WebMethod]
  [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
  public List<DA.Employee> AddEmployee(DA.Employee employee)
  {
    List<DA.Employee> eList;

    Thread.Sleep(TimeSpan.FromSeconds(1));

    using (SqlConnection conn = new SqlConnection(_connectionString))
    {
      conn.Open();
      using (SqlTransaction txn = conn.BeginTransaction())
      {
        int newId;
        int numRows = Insert(conn, txn, employee, out newId);
        eList = GetEmployeesInner(conn, txn);
        txn.Commit();
      }
    }

    return eList;
  }

  [WebMethod]
  [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
  public DA.Employee GetEmployee(int id, bool newEmployee)
  {
    Thread.Sleep(TimeSpan.FromSeconds(1));

    if (newEmployee)
    {
      return new DA.Employee();
    }

    DA.Employee e = new DA.Employee();

    using (SqlConnection conn = new SqlConnection(_connectionString))
    {
      conn.Open();
      using (SqlTransaction txn = conn.BeginTransaction())
      {
        string sqlCmd = "SELECT EmployeeID, LastName, FirstName, Title, TitleOfCourtesy, Name, HireDate, Notes FROM vwEmployees WHERE (EmployeeID = @id)";
        using (SqlCommand cmd = new SqlCommand(sqlCmd, conn, txn))
        {
          cmd.Parameters.AddWithValue("@id", id);
          using (SqlDataReader rdr = cmd.ExecuteReader())
          {
            rdr.Read();
            e.EmployeeID = rdr.GetInt32(rdr.GetOrdinal("EmployeeID"));
            e.LastName = rdr.GetString(rdr.GetOrdinal("LastName"));
            e.FirstName = rdr.GetString(rdr.GetOrdinal("FirstName"));
            e.Title = rdr.IsDBNull(rdr.GetOrdinal("Title")) ? "" : rdr.GetString(rdr.GetOrdinal("Title"));
            e.TitleOfCourtesy = rdr.IsDBNull(rdr.GetOrdinal("TitleOfCourtesy")) ? "" : rdr.GetString(rdr.GetOrdinal("TitleOfCourtesy"));
            e.Name = rdr.GetString(rdr.GetOrdinal("Name"));
            e.HireDate = rdr.IsDBNull(rdr.GetOrdinal("HireDate"))
                ? (DateTime?)null : rdr.GetDateTime(rdr.GetOrdinal("HireDate"));
            e.Notes = rdr.IsDBNull(rdr.GetOrdinal("Notes")) ? "" : rdr.GetString(rdr.GetOrdinal("Notes"));
          }
        }
      }
    }

    return e;
  }

  [WebMethod]
  [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
  public List<DA.Employee> DeleteEmployee(int id)
  {
    List<DA.Employee> eList;

    Thread.Sleep(TimeSpan.FromSeconds(1));

    using (SqlConnection conn = new SqlConnection(_connectionString))
    {
      conn.Open();
      using (SqlTransaction txn = conn.BeginTransaction())
      {
        int numRows;
        string sqlCmd = "DELETE FROM Employees WHERE (EmployeeID = @id)";
        using (SqlCommand cmd = new SqlCommand(sqlCmd, conn, txn))
        {
          cmd.Parameters.AddWithValue("@id", id);
          numRows = cmd.ExecuteNonQuery();
        }

        eList = GetEmployeesInner(conn, txn);
        txn.Commit();
      }
    }

    return eList;
  }

  List<DA.Employee> GetEmployeesInner(SqlConnection conn, SqlTransaction txn)
  {
    List<DA.Employee> eList = new List<DA.Employee>();

    string sqlCmd = "SELECT EmployeeID, Name, HireDate, Notes, SupervisorName, ReportsTo, CanBeDeleted FROM vwEmployees ORDER BY LastName";
    using (SqlCommand cmd = new SqlCommand(sqlCmd, conn, txn))
    {
      using (SqlDataReader rdr = cmd.ExecuteReader())
      {
        while (rdr.Read())
        {
          DA.Employee e = new DA.Employee
          {
            EmployeeID = rdr.GetInt32(rdr.GetOrdinal("EmployeeID")),
            Name = rdr.GetString(rdr.GetOrdinal("Name")),
            HireDate = rdr.IsDBNull(rdr.GetOrdinal("HireDate"))
                ? (DateTime?)null : rdr.GetDateTime(rdr.GetOrdinal("HireDate")),
            Notes = rdr.IsDBNull(rdr.GetOrdinal("Notes")) ? "" : rdr.GetString(rdr.GetOrdinal("Notes")),
            SupervisorName = rdr.IsDBNull(rdr.GetOrdinal("SupervisorName"))
                ? null : rdr.GetString(rdr.GetOrdinal("SupervisorName")),
            ReportsTo = rdr.IsDBNull(rdr.GetOrdinal("ReportsTo"))
                ? (int?)null : rdr.GetInt32(rdr.GetOrdinal("ReportsTo")),
            CanBeDeleted = rdr.GetBoolean(rdr.GetOrdinal("CanBeDeleted")),
          };

          eList.Add(e);
        }
      }
    }

    foreach (DA.Employee e in eList)
    {
      e.TerritoryNames = GetTerritoryNames(conn, txn, e.EmployeeID);
    }

    return eList;
  }

  List<string> GetTerritoryNames(SqlConnection conn, SqlTransaction txn, int id)
  {
    List<string> names = new List<string>();

    string sqlCmd = "SELECT TerritoryDescription FROM Territories WHERE (dbo.EmployeeCoversTerritory(@id, TerritoryID) = 1) ORDER BY TerritoryDescription";
    using (SqlCommand cmd = new SqlCommand(sqlCmd, conn, txn))
    {
      cmd.Parameters.AddWithValue("@id", id);
      using (SqlDataReader rdr = cmd.ExecuteReader())
      {
        while (rdr.Read())
        {
          string name = rdr.GetString(rdr.GetOrdinal("TerritoryDescription"));
          names.Add(name.Trim());
        }
      }
    }

    return names;
  }

  int Insert(SqlConnection conn, SqlTransaction txn, DA.Employee emp, out int newId)
  {
    int numRows;
    string sqlCmd = "INSERT INTO Employees (LastName, FirstName, Title, TitleOfCourtesy, HireDate, Notes)"
      + " VALUES (@LastName,@FirstName,@Title,@TitleOfCourtesy,@HireDate,@Notes)"
      + " SELECT @id = SCOPE_IDENTITY()";
    using (SqlCommand cmd = new SqlCommand(sqlCmd, conn, txn))
    {
      SqlParameter p = new SqlParameter
      {
        ParameterName = "@id",
        SqlDbType = SqlDbType.Int,
        Direction = ParameterDirection.Output
      };
      cmd.Parameters.Add(p);
      cmd.Parameters.AddWithValue("@LastName", emp.LastName);
      cmd.Parameters.AddWithValue("@FirstName", emp.FirstName);
      cmd.Parameters.AddWithValue("@Title",
        string.IsNullOrWhiteSpace(emp.Title) ? DBNull.Value : (object)emp.Title);
      cmd.Parameters.AddWithValue("@TitleOfCourtesy",
        string.IsNullOrWhiteSpace(emp.TitleOfCourtesy) ? DBNull.Value : (object)emp.TitleOfCourtesy);
     cmd.Parameters.AddWithValue("@HireDate",
        emp.HireDate.HasValue ? (object)emp.HireDate.Value : DBNull.Value);
      cmd.Parameters.AddWithValue("@Notes",
        string.IsNullOrWhiteSpace(emp.Notes) ? DBNull.Value : (object)emp.Notes);

      numRows = cmd.ExecuteNonQuery();
      newId = (int)p.Value;
    }
    return numRows;
  }

  int Update(SqlConnection conn, SqlTransaction txn, DA.Employee emp)
  {
    int numRows = 0;
    string sqlCmd = "UPDATE Employees SET LastName = @LastName, FirstName = @FirstName, Title = @Title, TitleOfCourtesy = @TitleOfCourtesy, HireDate = @HireDate,"
      + " Notes = @Notes WHERE (EmployeeID = @EmployeeID)";
    using (SqlCommand cmd = new SqlCommand(sqlCmd, conn, txn))
    {
      cmd.Parameters.AddWithValue("@EmployeeID", emp.EmployeeID);
      cmd.Parameters.AddWithValue("@LastName", emp.LastName);
      cmd.Parameters.AddWithValue("@FirstName", emp.FirstName);
      cmd.Parameters.AddWithValue("@Title",
        string.IsNullOrWhiteSpace(emp.Title) ? DBNull.Value : (object)emp.Title);
      cmd.Parameters.AddWithValue("@TitleOfCourtesy",
        string.IsNullOrWhiteSpace(emp.TitleOfCourtesy) ? DBNull.Value : (object)emp.TitleOfCourtesy);
      cmd.Parameters.AddWithValue("@HireDate",
        emp.HireDate.HasValue ? (object)emp.HireDate.Value : DBNull.Value);
      cmd.Parameters.AddWithValue("@Notes",
        string.IsNullOrWhiteSpace(emp.Notes) ? DBNull.Value : (object)emp.Notes);

      numRows = cmd.ExecuteNonQuery();
    }
    return numRows;
  }
}

public class Territory
{
  public string TerritoryID { set; get; }
  public string TerritoryDescription { set; get; }
  public bool EmployeeCoversTerritory { get; set; }
}
