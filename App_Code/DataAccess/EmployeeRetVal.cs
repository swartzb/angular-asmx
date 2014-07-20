using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAccess
{
  /// <summary>
  /// Summary description for EmployeeRetVal
  /// </summary>
  public class EmployeeRetVal
  {
    public EmployeeRetVal()
    {

    }

    public int? id { get; set; }
    public int? numRows { get; set; }
    public List<EmployeeSummary> employees { get; set; }
  }
  
}