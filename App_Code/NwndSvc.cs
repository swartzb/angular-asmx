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
  public List<DA.EmployeeSummary> GetCanReportTo(int id)
  {
    Thread.Sleep(TimeSpan.FromSeconds(2));
    return DA.EmployeeSummary.GetCanReportTo(_connectionString, id);
  }

  [WebMethod]
  [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
  public List<DA.Territory> GetTerritoriesForEmployee(int id)
  {
    Debug.Print("Get");
    Thread.Sleep(TimeSpan.FromSeconds(2));
    return DA.Territory.Get(_connectionString, id);
  }

  [WebMethod]
  [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
  public DA.EmployeeSummaryRetVal UpdateTerritoriesForEmployee(int id, List<string> territoryIDs)
  {
    Debug.Print("Update");
    Thread.Sleep(TimeSpan.FromSeconds(2));
    return DA.Territory.Update(_connectionString, id, territoryIDs);
  }

  [WebMethod]
  [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
  public List<Employee> GetEmployees()
  {
    List<Employee> eList;

    Thread.Sleep(TimeSpan.FromSeconds(2));

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
  public DA.EmployeeSummaryRetVal DeleteEmployee(int id)
  {
    Debug.Print("DeleteEmployee");
    Thread.Sleep(TimeSpan.FromSeconds(2));
    return DA.EmployeeSummary.Remove(_connectionString, id);
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
            CanBeDeleted = rdr.IsDBNull(rdr.GetOrdinal("CanBeDeleted"))
                ? false : rdr.GetBoolean(rdr.GetOrdinal("CanBeDeleted")),
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

  public static List<string> GetTerritoryNames(SqlConnection conn, SqlTransaction txn, int id)
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
  public System.Nullable<bool> CanBeDeleted { get; set; }
  public List<string> TerritoryNames { get; set; }
}
