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

   public int Update(SqlConnection conn, SqlTransaction txn)
    {
      int numRows = 0;
      string sqlCmd = "UPDATE Employees SET LastName = @LastName, FirstName = @FirstName, Title = @Title, TitleOfCourtesy = @TitleOfCourtesy, HireDate = @HireDate,"
        + " Notes = @Notes WHERE (EmployeeID = @EmployeeID)";
      using (SqlCommand cmd = new SqlCommand(sqlCmd, conn, txn))
      {
        cmd.Parameters.AddWithValue("@EmployeeID", this.EmployeeID);
        cmd.Parameters.AddWithValue("@LastName", this.LastName);
        cmd.Parameters.AddWithValue("@FirstName", this.FirstName);
        cmd.Parameters.AddWithValue("@Title",
          string.IsNullOrWhiteSpace(this.Title) ? DBNull.Value : (object)this.Title);
        cmd.Parameters.AddWithValue("@TitleOfCourtesy",
          string.IsNullOrWhiteSpace(this.TitleOfCourtesy) ? DBNull.Value : (object)this.TitleOfCourtesy);
        cmd.Parameters.AddWithValue("@HireDate",
          this.HireDate.HasValue ? (object)this.HireDate.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("@Notes",
          string.IsNullOrWhiteSpace(this.Notes) ? DBNull.Value : (object)this.Notes);

        numRows = cmd.ExecuteNonQuery();
      }
      return numRows;
    }

    public int Insert(SqlConnection conn, SqlTransaction txn, out int newId)
    {
      int numRows;
      string sqlCmd = "INSERT INTO Employees (LastName, FirstName, Title, TitleOfCourtesy, HireDate, Notes)"
        + " VALUES (@LastName,@FirstName,@Title,@TitleOfCourtesy,@HireDate,@Notes)"
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
        cmd.Parameters.AddWithValue("@LastName", this.LastName);
        cmd.Parameters.AddWithValue("@FirstName", this.FirstName);
        cmd.Parameters.AddWithValue("@Title",
          string.IsNullOrWhiteSpace(this.Title) ? DBNull.Value : (object)this.Title);
        cmd.Parameters.AddWithValue("@TitleOfCourtesy",
          string.IsNullOrWhiteSpace(this.TitleOfCourtesy) ? DBNull.Value : (object)this.TitleOfCourtesy);
        cmd.Parameters.AddWithValue("@HireDate",
           this.HireDate.HasValue ? (object)this.HireDate.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("@Notes",
          string.IsNullOrWhiteSpace(this.Notes) ? DBNull.Value : (object)this.Notes);

        numRows = cmd.ExecuteNonQuery();
        newId = (int)p.Value;
      }
      return numRows;
    }

   public static List<Employee> GetAll(SqlConnection conn, SqlTransaction txn)
    {
      List<Employee> eList = new List<Employee>();

      string sqlCmd = "SELECT EmployeeID, Name, HireDate, Notes, SupervisorName, ReportsTo, CanBeDeleted FROM vwEmployees ORDER BY LastName";
      using (SqlCommand cmd = new SqlCommand(sqlCmd, conn, txn))
      {
        using (SqlDataReader rdr = cmd.ExecuteReader())
        {
          while (rdr.Read())
          {
            Employee e = new Employee
            {
              EmployeeID = rdr.GetInt32(rdr.GetOrdinal("EmployeeID")),
              Name = rdr.GetString(rdr.GetOrdinal("Name")),
              HireDate = rdr.IsDBNull(rdr.GetOrdinal("HireDate"))
                  ? (DateTime?)null : rdr.GetDateTime(rdr.GetOrdinal("HireDate")),
              Notes = rdr.IsDBNull(rdr.GetOrdinal("Notes")) ? "" : rdr.GetString(rdr.GetOrdinal("Notes")),
              SupervisorName = rdr.IsDBNull(rdr.GetOrdinal("SupervisorName"))
                  ? null : rdr.GetString(rdr.GetOrdinal("SupervisorName")),
              ReportsTo = rdr.IsDBNull(rdr.GetOrdinal("ReportsTo"))
                  ? (int?)null : rdr.GetInt32(rdr.GetOrdinal("ReportsTo")),
              CanBeDeleted = rdr.GetBoolean(rdr.GetOrdinal("CanBeDeleted")),
            };

            eList.Add(e);
          }
        }
      }

      foreach (Employee e in eList)
      {
        e.TerritoryNames = Territory.GetNames(conn, txn, e.EmployeeID);
      }

      return eList;
    }
  }
}
