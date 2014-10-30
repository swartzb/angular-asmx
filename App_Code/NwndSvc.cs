using System.Collections.Generic;
using System.Web.Services;
using System.Configuration;
using System.Web.Script.Services;
using BL = BusinessLogic;
using System.Threading;
using System;
using System.Diagnostics;
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
  public List<DA.Territory> GetTerritoriesForEmployee(int id)
  {
    Debug.Print("GetTerritoriesForEmployee");
    Thread.Sleep(TimeSpan.FromSeconds(2));
    return DA.Territory.GetTerritoriesForEmployee(_connectionString, id);
  }

  [WebMethod]
  [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
  public DA.EmployeeSummaryRetVal UpdateTerritoriesForEmployee(int id, List<string> territoryIDs)
  {
    Debug.Print("UpdateTerritoriesForEmployee");
    Thread.Sleep(TimeSpan.FromSeconds(2));
    return DA.Territory.UpdateTerritoriesForEmployee(_connectionString, id, territoryIDs);
  }

  [WebMethod]
  [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
  public DA.EmployeeSummaryRetVal GetAllEmployees()
  {
    Debug.Print("GetAllEmployees");
    Thread.Sleep(TimeSpan.FromSeconds(2));
    return DA.EmployeeSummary.GetAll(_connectionString);
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
}
