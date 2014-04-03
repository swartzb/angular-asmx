using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace DataAccess
{
  /// <summary>
  /// Summary description for Employee
  /// </summary>
  [XmlType("NewTypeName")]
  public class Employee
  {
    public Employee()
    {

    }

    public int EmployeeID { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public string Title { get; set; }

    public string TitleOfCourtesy { get; set; }

    public System.Nullable<System.DateTime> BirthDate { get; set; }

    public System.Nullable<System.DateTime> HireDate { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string Region { get; set; }

    public string PostalCode { get; set; }

    public string Country { get; set; }

    public string HomePhone { get; set; }

    public string Extension { get; set; }

    //public System.Data.Linq.Binary Photo { get; set; }

    public string Notes { get; set; }

    public System.Nullable<int> ReportsTo { get; set; }

    public string PhotoPath { get; set; }

    public static List<Employee> GetAll(string connectionString)
    {
      List<Employee> employeeList = new List<Employee>();

      using (SqlConnection conn = new SqlConnection(connectionString))
      {
        conn.Open();
        using (SqlTransaction txn = conn.BeginTransaction())
        {
          string sqlCmd = "SELECT EmployeeID, LastName, FirstName, Title, TitleOfCourtesy, BirthDate, HireDate, Address, City, Region, PostalCode," +
            " Country, HomePhone, Extension, Photo, Notes, ReportsTo, PhotoPath FROM Employees";
          using (SqlCommand cmd = new SqlCommand(sqlCmd, conn, txn))
          {
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
              while (rdr.Read())
              {
                Employee e = new Employee
                {
                  EmployeeID = rdr.GetInt32(rdr.GetOrdinal("EmployeeID")),
                  LastName = rdr.GetString(rdr.GetOrdinal("LastName")),
                  FirstName = rdr.GetString(rdr.GetOrdinal("FirstName")),
                  Title = rdr.IsDBNull(rdr.GetOrdinal("Title")) ? "" : rdr.GetString(rdr.GetOrdinal("Title")),
                  TitleOfCourtesy = rdr.IsDBNull(rdr.GetOrdinal("TitleOfCourtesy")) ? "" : rdr.GetString(rdr.GetOrdinal("TitleOfCourtesy")),
                  BirthDate = rdr.IsDBNull(rdr.GetOrdinal("BirthDate"))
                      ? (DateTime?)null : rdr.GetDateTime(rdr.GetOrdinal("BirthDate")),
                  HireDate = rdr.IsDBNull(rdr.GetOrdinal("HireDate"))
                      ? (DateTime?)null : rdr.GetDateTime(rdr.GetOrdinal("HireDate")),
                  Address = rdr.IsDBNull(rdr.GetOrdinal("Address")) ? "" : rdr.GetString(rdr.GetOrdinal("Address")),
                  City = rdr.IsDBNull(rdr.GetOrdinal("City")) ? "" : rdr.GetString(rdr.GetOrdinal("City")),
                  Region = rdr.IsDBNull(rdr.GetOrdinal("Region")) ? "" : rdr.GetString(rdr.GetOrdinal("Region")),
                  PostalCode = rdr.IsDBNull(rdr.GetOrdinal("PostalCode")) ? "" : rdr.GetString(rdr.GetOrdinal("PostalCode")),
                  Country = rdr.IsDBNull(rdr.GetOrdinal("Country")) ? "" : rdr.GetString(rdr.GetOrdinal("Country")),
                  HomePhone = rdr.IsDBNull(rdr.GetOrdinal("HomePhone")) ? "" : rdr.GetString(rdr.GetOrdinal("HomePhone")),
                  Extension = rdr.IsDBNull(rdr.GetOrdinal("Extension")) ? "" : rdr.GetString(rdr.GetOrdinal("Extension")),
                  Notes = rdr.IsDBNull(rdr.GetOrdinal("Notes")) ? "" : rdr.GetString(rdr.GetOrdinal("Notes")),
                  ReportsTo = rdr.IsDBNull(rdr.GetOrdinal("ReportsTo"))
                      ? (int?)null : rdr.GetInt32(rdr.GetOrdinal("ReportsTo")),
                  PhotoPath = rdr.IsDBNull(rdr.GetOrdinal("PhotoPath")) ? "" : rdr.GetString(rdr.GetOrdinal("PhotoPath")),
                };
                employeeList.Add(e);
              }
            }
          }
          txn.Commit();
        }
      }

      return employeeList;
    }
  }
}
