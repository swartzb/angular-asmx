using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessLogic
{
  /// <summary>
  /// Summary description for EmployeeDetails
  /// </summary>
  public class EmployeeDetails : IEmployeeDetails
  {
    public EmployeeDetails()
    {
      //
      // TODO: Add constructor logic here
      //
    }

    public EmployeeLite Supervisor { get; set; }

    public string DisplayName { get; set; }

    public int EmployeeID { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public string Title { get; set; }

    public string TitleOfCourtesy { get; set; }

    public DateTime? HireDate { get; set; }

    public string Notes { get; set; }

    public int? ReportsTo { get; set; }

    public bool canDelete { get; set; }

    public DateTime? BirthDate { get; set; }

    public string HomePhone { get; set; }

    public List<string> territoryIds { get; set; }

    public List<DataAccess.Territory> territories { get; set; }

    public List<DataAccess.EmployeeSummary> employees { get; set; }
  } 
}