﻿using System;

namespace DataAccess
{
  /// <summary>
  /// Summary description for IEmployeeSummary
  /// </summary>
  interface IEmployeeSummary
  {
    int EmployeeID { get; set; }
    string LastName { get; set; }
    string FirstName { get; set; }
    string Title { get; set; }
    string TitleOfCourtesy { get; set; }
    Nullable<DateTime> HireDate { get; set; }
    string Notes { get; set; }
  }
}