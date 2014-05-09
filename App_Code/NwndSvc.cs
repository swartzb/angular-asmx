using System.Collections.Generic;
using System.Web.Services;
using System.Configuration;
using System.Web.Script.Services;
using BL = BusinessLogic;
using System.Threading;
using System;
using System.Diagnostics;

public class args
{
  public int id { get; set; }
  public string val { get; set; }
  public int? supervisorId { get; set; }
}
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
  public string HelloWorld()
  {
    return "Hello World";
  }

  [WebMethod]
  [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
  public args InputArgsTest(args inArgs)
  {
    Thread.Sleep(TimeSpan.FromSeconds(2));
    args outArgs = new args
    {
      id = -inArgs.id,
      val = inArgs.val + " and output",
      supervisorId = inArgs.supervisorId
    };
    return outArgs;
  }

  [WebMethod]
  [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
  public List<BL.Employee> GetAllEmployees()
  {
    Debug.Print("GetAllEmployees");
    Thread.Sleep(TimeSpan.FromSeconds(2));
    return BL.Employee.GetAll(_connectionString);
  }
}
