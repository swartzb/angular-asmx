using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using DA = DataAccess;

namespace BusinessLogic
{
  /// <summary>
  /// Summary description for EmployeeSummaryRetVal
  /// </summary>
  [XmlType("BusinessLogicEmployeeSummaryRetVal")]
  public class EmployeeSummaryRetVal
  {
    public EmployeeSummaryRetVal()
    {

    }

    public EmployeeSummaryRetVal(DA.EmployeeSummaryRetVal daRetVal)
    {
      id = daRetVal.id;
      numRows = daRetVal.numRows;
      employees = FromDaList(daRetVal.employees);
    }

    public int? id { get; set; }
    public int? numRows { get; set; }
    public List<EmployeeSummary> employees { get; set; }

    static List<EmployeeSummary> FromDaList(List<DA.EmployeeSummary> daList)
    {
      List<EmployeeSummary> blList = daList
        .Select(daEs => new EmployeeSummary(daEs as DA.IEmployeeSummary))
        .ToList();

      foreach (EmployeeSummary blEs in blList)
      {
        if (blEs.ReportsTo.HasValue)
        {
          EmployeeSummary supervisor = blList.
            Where(e => e.EmployeeID == blEs.ReportsTo.Value).
            Single();
          blEs.Supervisor = new EmployeeLite(supervisor);
        }
      }

      return blList;
    }
  }
}