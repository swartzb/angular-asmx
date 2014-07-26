using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace DataAccess
{
  /// <summary>
  /// Summary description for EmployeeSummaryRetVal
  /// </summary>
  [XmlType("DataAccessEmployeeSummaryRetVal")]
  public class EmployeeSummaryRetVal
  {
    public EmployeeSummaryRetVal()
    {

    }

    public int? id { get; set; }
    public int? numRows { get; set; }
    public List<EmployeeSummary> employees { get; set; }
  }
  
}