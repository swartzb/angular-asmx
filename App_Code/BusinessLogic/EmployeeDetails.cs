using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DA = DataAccess;

namespace BusinessLogic
{
  /// <summary>
  /// Summary description for EmployeeDetails
  /// </summary>
  public class EmployeeDetails : IEmployeeSummary, DA.IEmployeeDetails
  {
    public EmployeeDetails()
    {
      //
      // TODO: Add constructor logic here
      //
    }

    #region BL.IEmployeeSummary
    public EmployeeLite Supervisor { get; set; }
    public string DisplayName { get; set; }
    #endregion

    #region DA.IEmployeeDetails
    public DateTime? BirthDate { get; set; }
    public string HomePhone { get; set; }
    public List<DA.Territory> territories { get; set; }
    #endregion

    #region DA.IEmployeeSummary
    public int EmployeeID { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Title { get; set; }
    public string TitleOfCourtesy { get; set; }
    public DateTime? HireDate { get; set; }
    public string Notes { get; set; }
    public int? ReportsTo { get; set; }
    public bool canDelete { get; set; }
    #endregion

    public List<DataAccess.EmployeeSummary> canReportTo { get; set; }
  } 
}