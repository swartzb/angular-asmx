using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using DA = DataAccess;

namespace BusinessLogic
{
  /// <summary>
  /// Summary description for EmployeeDetails
  /// </summary>
  [XmlType("BusinessLogicEmployeeDetails")]
  public class EmployeeDetails : IEmployeeSummary, DA.IEmployeeDetails
  {
    public EmployeeDetails()
    {
      //
      // TODO: Add constructor logic here
      //
    }

    public EmployeeDetails(DA.EmployeeDetails daDetails, int? id)
    {
      //
      // TODO: Add constructor logic here
      //
    }

    public static EmployeeDetails Get(string connectionString, int? id)
    {
      DA.EmployeeDetails daDetails = DA.EmployeeDetails.Get(connectionString, id);

      EmployeeDetails ed = new EmployeeDetails(daDetails, id);

      return ed;
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