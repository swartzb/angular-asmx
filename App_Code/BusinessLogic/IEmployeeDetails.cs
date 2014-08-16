using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DA = DataAccess;

namespace BusinessLogic
{
  /// <summary>
  /// Summary description for IEmployeeDetails
  /// </summary>
  public interface IEmployeeDetails : IEmployeeSummary, DA.IEmployeeDetails
  {

  } 
}