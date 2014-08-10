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

    public EmployeeSummary(DA.EmployeeSummary daEs)
    {
      this.EmployeeID = daEs.EmployeeID;
      this.LastName = daEs.LastName;
      this.FirstName = daEs.FirstName;
      this.Title = daEs.Title;
      this.TitleOfCourtesy = daEs.TitleOfCourtesy;
      this.HireDate = daEs.HireDate;
      this.Notes = daEs.Notes;
      this.ReportsTo = daEs.ReportsTo;

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

    public EmployeeLite Supervisor { get; set; }

    public string DisplayName { get; set; }

    public int EmployeeID { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public string Title { get; set; }

    public string TitleOfCourtesy { get; set; }

    public DateTime? HireDate { get; set; }

    public string Notes { get; set; }

    public System.Nullable<int> ReportsTo { get; set; }

    public bool canDelete { get; set; }

    public static EmployeeSummaryRetVal GetAll(string connectionString)
    {
      DA.EmployeeSummaryRetVal daRetVal = DA.EmployeeSummary.GetAll(connectionString);

      EmployeeSummaryRetVal retVal = new EmployeeSummaryRetVal(daRetVal);

      return retVal;
    }
  }
}
