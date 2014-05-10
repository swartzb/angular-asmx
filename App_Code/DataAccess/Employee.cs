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

    static void Delete(SqlConnection conn, SqlTransaction txn, int id)
    {
      string sqlCmd = "DELETE FROM Employees WHERE (EmployeeID = @id)";
      using (SqlCommand cmd = new SqlCommand(sqlCmd, conn, txn))
      {
        cmd.Parameters.AddWithValue("@id", id);
        int numRows = cmd.ExecuteNonQuery();
      }
    }

    static void Insert(SqlConnection conn, SqlTransaction txn, Employee emp)
    {
      string sqlCmd = "INSERT INTO Employees (LastName, FirstName, Title, TitleOfCourtesy, BirthDate, HireDate, Address, City, Region, PostalCode, Country, HomePhone, Extension, Notes, ReportsTo, PhotoPath)"
        + " VALUES (@LastName,@FirstName,@Title,@TitleOfCourtesy,@BirthDate,@HireDate,@Address,@City,@Region,@PostalCode,@Country,@HomePhone,@Extension,@Notes,@ReportsTo,@PhotoPath)"
        + " SELECT @id = SCOPE_IDENTITY()";
      using (SqlCommand cmd = new SqlCommand(sqlCmd, conn, txn))
      {
        SqlParameter p = new SqlParameter
        {
          ParameterName = "@id",
          SqlDbType = SqlDbType.Int,
          Direction = ParameterDirection.Output
        };
        cmd.Parameters.Add(p);
        cmd.Parameters.AddWithValue("@LastName", emp.LastName);
        cmd.Parameters.AddWithValue("@FirstName", emp.FirstName);
        cmd.Parameters.AddWithValue("@Title", emp.Title);
        cmd.Parameters.AddWithValue("@TitleOfCourtesy", emp.TitleOfCourtesy);
        cmd.Parameters.AddWithValue("@BirthDate",
          emp.BirthDate.HasValue ? (object)emp.BirthDate.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("@HireDate",
          emp.HireDate.HasValue ? (object)emp.HireDate.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("@Address", emp.Address);
        cmd.Parameters.AddWithValue("@City", emp.City);
        cmd.Parameters.AddWithValue("@Region", emp.Region);
        cmd.Parameters.AddWithValue("@PostalCode", emp.PostalCode);
        cmd.Parameters.AddWithValue("@Country", emp.Country);
        cmd.Parameters.AddWithValue("@HomePhone", emp.HomePhone);
        cmd.Parameters.AddWithValue("@Extension", emp.Extension);
        cmd.Parameters.AddWithValue("@Notes", emp.Notes);
        cmd.Parameters.AddWithValue("@ReportsTo",
          emp.ReportsTo.HasValue ? (object)emp.ReportsTo.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("@PhotoPath", emp.PhotoPath);

        int numRows = cmd.ExecuteNonQuery();
        emp.EmployeeID = (int)p.Value;
      }
    }

    static List<Employee> SelectAll(SqlConnection conn, SqlTransaction txn)
    {
      List<Employee> employeeList = new List<Employee>();

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

      return employeeList;
    }

    public static List<Employee> Remove(string connectionString, int id)
    {
      List<Employee> employeeList = null;

      using (SqlConnection conn = new SqlConnection(connectionString))
      {
        conn.Open();
        using (SqlTransaction txn = conn.BeginTransaction())
        {
          Delete(conn, txn, id);
          employeeList = SelectAll(conn, txn);
          txn.Commit();
        }
      }

      return employeeList;
    }

    public static List<Employee> Add(string connectionString, Employee emp)
    {
      List<Employee> employeeList = null;

      using (SqlConnection conn = new SqlConnection(connectionString))
      {
        conn.Open();
        using (SqlTransaction txn = conn.BeginTransaction())
        {
          Insert(conn, txn, emp);
          employeeList = SelectAll(conn, txn);
          txn.Commit();
        }
      }

      return employeeList;
    }

    public static List<Employee> GetAll(string connectionString)
    {
      List<Employee> employeeList = null;

      using (SqlConnection conn = new SqlConnection(connectionString))
      {
        conn.Open();
        using (SqlTransaction txn = conn.BeginTransaction())
        {
          employeeList = SelectAll(conn, txn);
          txn.Commit();
        }
      }

      return employeeList;
    }
  }
}
