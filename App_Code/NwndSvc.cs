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
  public List<DA.Territory> GetTerritories(int id)
  {
    List<DA.Territory> tList = new List<DA.Territory>();

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
              DA.Territory t = new DA.Territory
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

        eList = DA.Employee.GetAll(conn, txn);
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

        eList = DA.Employee.GetAll(conn, txn);
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
        eList = DA.Employee.GetAll(conn, txn);
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
        int numRows = employee.Update(conn, txn);
        eList = DA.Employee.GetAll(conn, txn);
        txn.Commit();
      }
    }

    return eList;
  }

  /// <summary>
  /// Add a new employee.
  /// </summary>
  /// <param name="employee">new employee to add</param>
  /// <returns>updated list of employee information</returns>
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
        int numRows = employee.Insert(conn, txn, out newId);
        eList = DA.Employee.GetAll(conn, txn);
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

  /// <summary>
  /// Delete the employee with the given id.
  /// </summary>
  /// <param name="id">employee id</param>
  /// <returns>updated list of employee information</returns>
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
        string sqlCmd;

        sqlCmd = "DELETE FROM EmployeeTerritories WHERE (EmployeeID = @id)";
        using (SqlCommand cmd = new SqlCommand(sqlCmd, conn, txn))
        {
          cmd.Parameters.AddWithValue("@id", id);
          numRows = cmd.ExecuteNonQuery();
        }

        sqlCmd = "DELETE FROM Employees WHERE (EmployeeID = @id)";
        using (SqlCommand cmd = new SqlCommand(sqlCmd, conn, txn))
        {
          cmd.Parameters.AddWithValue("@id", id);
          numRows = cmd.ExecuteNonQuery();
        }

        eList = DA.Employee.GetAll(conn, txn);
        txn.Commit();
      }
    }

    return eList;
  }
}
