using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace DataAccess
{
  /// <summary>
  /// Summary description for Employee
  /// </summary>
  [XmlType("DataAccessEmployee")]
  public class Employee
  {
    public int EmployeeID { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Title { get; set; }
    public string TitleOfCourtesy { get; set; }
    public string Name { get; set; }
    public System.Nullable<System.DateTime> BirthDate { get; set; }
    public System.Nullable<System.DateTime> HireDate { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Region { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
    public string HomePhone { get; set; }
    public string Extension { get; set; }
    public string Notes { get; set; }
    public System.Nullable<int> ReportsTo { get; set; }
    public string SupervisorName { get; set; }
    public bool CanBeDeleted { get; set; }
    public List<string> TerritoryNames { get; set; }
  }
}
