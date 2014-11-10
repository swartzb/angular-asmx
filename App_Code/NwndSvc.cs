using System.Collections.Generic;
using System.Web.Services;
using System.Configuration;
using System.Web.Script.Services;
using BL = BusinessLogic;
using System.Threading;
using System;
using System.Diagnostics;
using DA = DataAccess;
using System.Data.SqlClient;

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
  public List<Employee> GetCanReportTo(int id)
  {
    List<Employee> eList = new List<Employee>();

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
              Employee e = new Employee
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
  public List<Employee> UpdateTerritories(int id, List<string> territoryIDs)
  {
    List<Employee> eList;

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
  public List<Employee> GetEmployees()
  {
    List<Employee> eList;

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
  public BL.Employee.ReturnVal EditEmployee(DA.Employee employee)
  {
    Debug.Print("AddEmployee");
    Thread.Sleep(TimeSpan.FromSeconds(2));
    return BL.Employee.Edit(_connectionString, employee);
  }

  [WebMethod]
  [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
  public BL.Employee.ReturnVal AddEmployee(DA.Employee employee)
  {
    Debug.Print("AddEmployee");
    Thread.Sleep(TimeSpan.FromSeconds(2));
    return BL.Employee.Add(_connectionString, employee);
  }

  [WebMethod]
  [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
  public DA.Employee.Details GetEmployeeDetails(int id, bool newEmployee)
  {
    Debug.Print("GetEmployeeDetails");
    Thread.Sleep(TimeSpan.FromSeconds(2));
    return DA.Employee.GetDetails(_connectionString, newEmployee ? (int?)null : id);
  }

  [WebMethod]
  [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
  public List<Employee> DeleteEmployee(int id)
  {
    List<Employee> eList;

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

  List<Employee> GetEmployeesInner(SqlConnection conn, SqlTransaction txn)
  {
    List<Employee> eList = new List<Employee>();

    string sqlCmd = "SELECT EmployeeID, Name, HireDate, Notes, SupervisorName, CanBeDeleted FROM vwEmployees ORDER BY LastName";
    using (SqlCommand cmd = new SqlCommand(sqlCmd, conn, txn))
    {
      using (SqlDataReader rdr = cmd.ExecuteReader())
      {
        while (rdr.Read())
        {
          Employee e = new Employee
          {
            EmployeeID = rdr.GetInt32(rdr.GetOrdinal("EmployeeID")),
            Name = rdr.GetString(rdr.GetOrdinal("Name")),
            HireDate = rdr.IsDBNull(rdr.GetOrdinal("HireDate"))
                ? (DateTime?)null : rdr.GetDateTime(rdr.GetOrdinal("HireDate")),
            Notes = rdr.IsDBNull(rdr.GetOrdinal("Notes")) ? "" : rdr.GetString(rdr.GetOrdinal("Notes")),
            SupervisorName = rdr.IsDBNull(rdr.GetOrdinal("SupervisorName"))
                ? null : rdr.GetString(rdr.GetOrdinal("SupervisorName")),
            CanBeDeleted = rdr.GetBoolean(rdr.GetOrdinal("CanBeDeleted")),
          };

          eList.Add(e);
        }
      }
    }

    foreach (Employee e in eList)
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
}

public class Employee
{
  public int EmployeeID { get; set; }
  public string LastName { get; set; }
  public string FirstName { get; set; }
  public string Title { get; set; }
  public string TitleOfCourtesy { get; set; }
  public string Name { get; set; }
  public System.Nullable<System.DateTime> BirthDate { get; set; }
  public System.Nullable<System.DateTime> HireDate { get; set; }
  public string Address { get; set; }
  public string City { get; set; }
  public string Region { get; set; }
  public string PostalCode { get; set; }
  public string Country { get; set; }
  public string HomePhone { get; set; }
  public string Extension { get; set; }
  public string Notes { get; set; }
  public System.Nullable<int> ReportsTo { get; set; }
  public string SupervisorName { get; set; }
  public bool CanBeDeleted { get; set; }
  public List<string> TerritoryNames { get; set; }
}

public class Territory
{
  public string TerritoryID { set; get; }
  public string TerritoryDescription { set; get; }
  public bool EmployeeCoversTerritory { get; set; }
}
