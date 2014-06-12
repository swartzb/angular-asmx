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
  [XmlType("BusinessLogicEmployee")]
  public class Employee
  {
    public class Details
    {
      public List<Employee> employees { get; set; }

      public Details()
      {

      }
    }

    public class ReturnVal
    {
      public int? id { get; set; }
      public int? numRows { get; set; }
      public List<Employee> employees { get; set; }
    }

    public Employee()
    {

    }

    public Employee(Employee e)
    {
      EmployeeID = e.EmployeeID;
      LastName = e.LastName;
      FirstName = e.FirstName;
      Title = e.Title;
      TitleOfCourtesy = e.TitleOfCourtesy;
      BirthDate = e.BirthDate;
      HireDate = e.HireDate;
      Address = e.Address;
      City = e.City;
      Region = e.Region;
      PostalCode = e.PostalCode;
      Country = e.Country;
      HomePhone = e.HomePhone;
      Extension = e.Extension;
      Notes = e.Notes;
      ReportsTo = e.ReportsTo;
      PhotoPath = e.PhotoPath;
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

    static int Delete(SqlConnection conn, SqlTransaction txn, int id)
    {
      int numRows;
      string sqlCmd = "DELETE FROM Employees WHERE (EmployeeID = @id)";
      using (SqlCommand cmd = new SqlCommand(sqlCmd, conn, txn))
      {
        cmd.Parameters.AddWithValue("@id", id);
        numRows = cmd.ExecuteNonQuery();
      }
      return numRows;
    }

    static int Update(SqlConnection conn, SqlTransaction txn, Employee emp)
    {
      int numRows = 0;
      string sqlCmd = "UPDATE Employees SET LastName = @LastName, FirstName = @FirstName, Title = @Title, TitleOfCourtesy = @TitleOfCourtesy, BirthDate = @BirthDate, HireDate = @HireDate,"
        + " Address = @Address, City = @City, Region = @Region, PostalCode = @PostalCode, Country = @Country, HomePhone = @HomePhone, Extension = @Extension, Notes = @Notes,"
        + " ReportsTo = @ReportsTo, PhotoPath = @PhotoPath WHERE (EmployeeID = @EmployeeID)";
      using (SqlCommand cmd = new SqlCommand(sqlCmd, conn, txn))
      {
        cmd.Parameters.AddWithValue("@EmployeeID", emp.EmployeeID);
        cmd.Parameters.AddWithValue("@LastName", emp.LastName);
        cmd.Parameters.AddWithValue("@FirstName", emp.FirstName);
        cmd.Parameters.AddWithValue("@Title",
          string.IsNullOrWhiteSpace(emp.Title) ? DBNull.Value : (object)emp.Title);
        cmd.Parameters.AddWithValue("@TitleOfCourtesy",
          string.IsNullOrWhiteSpace(emp.TitleOfCourtesy) ? DBNull.Value : (object)emp.TitleOfCourtesy);
        cmd.Parameters.AddWithValue("@BirthDate",
          emp.BirthDate.HasValue ? (object)emp.BirthDate.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("@HireDate",
          emp.HireDate.HasValue ? (object)emp.HireDate.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("@Address",
          string.IsNullOrWhiteSpace(emp.Address) ? DBNull.Value : (object)emp.Address);
        cmd.Parameters.AddWithValue("@City",
          string.IsNullOrWhiteSpace(emp.City) ? DBNull.Value : (object)emp.City);
        cmd.Parameters.AddWithValue("@Region",
          string.IsNullOrWhiteSpace(emp.Region) ? DBNull.Value : (object)emp.Region);
        cmd.Parameters.AddWithValue("@PostalCode",
          string.IsNullOrWhiteSpace(emp.PostalCode) ? DBNull.Value : (object)emp.PostalCode);
        cmd.Parameters.AddWithValue("@Country",
          string.IsNullOrWhiteSpace(emp.Country) ? DBNull.Value : (object)emp.Country);
        cmd.Parameters.AddWithValue("@HomePhone",
          string.IsNullOrWhiteSpace(emp.HomePhone) ? DBNull.Value : (object)emp.HomePhone);
        cmd.Parameters.AddWithValue("@Extension",
          string.IsNullOrWhiteSpace(emp.Extension) ? DBNull.Value : (object)emp.Extension);
        cmd.Parameters.AddWithValue("@Notes",
          string.IsNullOrWhiteSpace(emp.Notes) ? DBNull.Value : (object)emp.Notes);
        cmd.Parameters.AddWithValue("@ReportsTo",
          emp.ReportsTo.HasValue ? (object)emp.ReportsTo.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("@PhotoPath",
          string.IsNullOrWhiteSpace(emp.PhotoPath) ? DBNull.Value : (object)emp.PhotoPath);

        numRows = cmd.ExecuteNonQuery();
      }
      return numRows;
    }

    static int Insert(SqlConnection conn, SqlTransaction txn, Employee emp, out int newId)
    {
      int numRows;
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
        cmd.Parameters.AddWithValue("@Title",
          string.IsNullOrWhiteSpace(emp.Title) ? DBNull.Value : (object)emp.Title);
        cmd.Parameters.AddWithValue("@TitleOfCourtesy",
          string.IsNullOrWhiteSpace(emp.TitleOfCourtesy) ? DBNull.Value : (object)emp.TitleOfCourtesy);
        cmd.Parameters.AddWithValue("@BirthDate",
          emp.BirthDate.HasValue ? (object)emp.BirthDate.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("@HireDate",
          emp.HireDate.HasValue ? (object)emp.HireDate.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("@Address",
          string.IsNullOrWhiteSpace(emp.Address) ? DBNull.Value : (object)emp.Address);
        cmd.Parameters.AddWithValue("@City",
          string.IsNullOrWhiteSpace(emp.City) ? DBNull.Value : (object)emp.City);
        cmd.Parameters.AddWithValue("@Region",
          string.IsNullOrWhiteSpace(emp.Region) ? DBNull.Value : (object)emp.Region);
        cmd.Parameters.AddWithValue("@PostalCode",
          string.IsNullOrWhiteSpace(emp.PostalCode) ? DBNull.Value : (object)emp.PostalCode);
        cmd.Parameters.AddWithValue("@Country",
          string.IsNullOrWhiteSpace(emp.Country) ? DBNull.Value : (object)emp.Country);
        cmd.Parameters.AddWithValue("@HomePhone",
          string.IsNullOrWhiteSpace(emp.HomePhone) ? DBNull.Value : (object)emp.HomePhone);
        cmd.Parameters.AddWithValue("@Extension",
          string.IsNullOrWhiteSpace(emp.Extension) ? DBNull.Value : (object)emp.Extension);
        cmd.Parameters.AddWithValue("@Notes",
          string.IsNullOrWhiteSpace(emp.Notes) ? DBNull.Value : (object)emp.Notes);
        cmd.Parameters.AddWithValue("@ReportsTo",
          emp.ReportsTo.HasValue ? (object)emp.ReportsTo.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("@PhotoPath",
          string.IsNullOrWhiteSpace(emp.PhotoPath) ? DBNull.Value : (object)emp.PhotoPath);

        numRows = cmd.ExecuteNonQuery();
        newId = (int)p.Value;
      }
      return numRows;
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

    public static Details GetDetails(string connectionString, int? id)
    {
      Details details = new Details();

      using (SqlConnection conn = new SqlConnection(connectionString))
      {
        conn.Open();
        using (SqlTransaction txn = conn.BeginTransaction())
        {
          details.employees = SelectAll(conn, txn);
          txn.Commit();
        }
      }

      return details;
    }

    public static ReturnVal Remove(string connectionString, int id)
    {
      ReturnVal retVal = new ReturnVal();

      using (SqlConnection conn = new SqlConnection(connectionString))
      {
        conn.Open();
        using (SqlTransaction txn = conn.BeginTransaction())
        {
          retVal.numRows = Delete(conn, txn, id);
          retVal.employees = SelectAll(conn, txn);
          txn.Commit();
        }
      }

      return retVal;
    }

    public static ReturnVal Edit(string connectionString, Employee emp)
    {
      ReturnVal retVal = new ReturnVal { id = emp.EmployeeID };

      using (SqlConnection conn = new SqlConnection(connectionString))
      {
        conn.Open();
        using (SqlTransaction txn = conn.BeginTransaction())
        {
          retVal.numRows = Update(conn, txn, emp);
          retVal.employees = SelectAll(conn, txn);
          txn.Commit();
        }
      }

      return retVal;
    }

    public static ReturnVal Add(string connectionString, Employee emp)
    {
      ReturnVal retVal = new ReturnVal();

      using (SqlConnection conn = new SqlConnection(connectionString))
      {
        conn.Open();
        using (SqlTransaction txn = conn.BeginTransaction())
        {
          int newId;
          retVal.numRows = Insert(conn, txn, emp, out newId);
          retVal.id = newId;
          retVal.employees = SelectAll(conn, txn);
          txn.Commit();
        }
      }

      return retVal;
    }

    public static ReturnVal GetAll(string connectionString)
    {
      ReturnVal retVal = new ReturnVal();

      using (SqlConnection conn = new SqlConnection(connectionString))
      {
        conn.Open();
        using (SqlTransaction txn = conn.BeginTransaction())
        {
          retVal.employees = SelectAll(conn, txn);
          txn.Commit();
        }
      }

      return retVal;
    }
  }
}
