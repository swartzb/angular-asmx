using System;
using DA = DataAccess;

namespace BusinessLogic
{
  /// <summary>
  /// Summary description for IEmployeeSummary
  /// </summary>
  public interface IEmployeeSummary : DA.IEmployeeSummary
  {
    EmployeeLite Supervisor { get; set; }
    string DisplayName { get; set; }
  }
}