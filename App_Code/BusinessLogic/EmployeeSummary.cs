using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using DA = DataAccess;

namespace BusinessLogic
{
  /// <summary>
  /// Summary description for EmployeeSummary
  /// </summary>
  [XmlType("BusinessLogicEmployeeSummary")]
  public class EmployeeSummary : IEmployeeSummary
  {
    public EmployeeSummary()
    {

    }

    public EmployeeSummary(DA.IEmployeeSummary IDaEs)
    {
      this.EmployeeID = IDaEs.EmployeeID;
      this.LastName = IDaEs.LastName;
      this.FirstName = IDaEs.FirstName;
      this.Title = IDaEs.Title;
      this.TitleOfCourtesy = IDaEs.TitleOfCourtesy;
      this.HireDate = IDaEs.HireDate;
      this.Notes = IDaEs.Notes;
      this.ReportsTo = IDaEs.ReportsTo;
      this.canDelete = IDaEs.canDelete;

      DisplayName = LastName + ", " + FirstName;
      if (!string.IsNullOrWhiteSpace(TitleOfCourtesy))
      {
        DisplayName += ", " + TitleOfCourtesy;
      }
      if (!string.IsNullOrWhiteSpace(Title))
      {
        DisplayName += ", " + Title;
      }
    }

#region BL.IEmployeeSummary
    public EmployeeLite Supervisor { get; set; }
    public string DisplayName { get; set; }
#region DA.IEmployeeSummary
    public int EmployeeID { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Title { get; set; }
    public string TitleOfCourtesy { get; set; }
    public DateTime? HireDate { get; set; }
    public string Notes { get; set; }
    public System.Nullable<int> ReportsTo { get; set; }
    public bool canDelete { get; set; }
#endregion
#endregion

    public static EmployeeSummaryRetVal GetAll(string connectionString)
    {
      DA.EmployeeSummaryRetVal daRetVal = DA.EmployeeSummary.GetAll(connectionString);

      EmployeeSummaryRetVal retVal = new EmployeeSummaryRetVal(daRetVal);

      return retVal;
    }
  }
}
