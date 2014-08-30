using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAccess
{
  /// <summary>
  /// Summary description for IEmployeeDetails
  /// </summary>
  public interface IEmployeeDetails : IEmployeeSummary
  {
    System.Nullable<System.DateTime> BirthDate { get; set; }
    string HomePhone { get; set; }
    List<Territory> territories { get; set; }
  } 
}