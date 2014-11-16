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
using System.Data;

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
  public List<Employee> UpdateSupervisor(int empId, int supId, bool hasValue)
  {
    List<Employee> eList = new List<Employee>();

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
  public List<Employee> EditEmployee(Employee employee)
  {
    List<Employee> eList;

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
  public BL.Employee.ReturnVal AddEmployee(DA.Employee employee)
  {
    Debug.Print("AddEmployee");
    Thread.Sleep(TimeSpan.FromSeconds(2));
    return BL.Employee.Add(_connectionString, employee);
  }

  [WebMethod]
  [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
  public Employee GetEmployee(int id, bool newEmployee)
  {
    Thread.Sleep(TimeSpan.FromSeconds(1));

    if (newEmployee)
    {
      return new Employee();
    }

    Employee e = new Employee();

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

    string sqlCmd = "SELECT EmployeeID, Name, HireDate, Notes, SupervisorName, ReportsTo, CanBeDeleted FROM vwEmployees ORDER BY LastName";
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
            ReportsTo = rdr.IsDBNull(rdr.GetOrdinal("ReportsTo"))
                ? (int?)null : rdr.GetInt32(rdr.GetOrdinal("ReportsTo")),
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

  int Update(SqlConnection conn, SqlTransaction txn, Employee emp)
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
