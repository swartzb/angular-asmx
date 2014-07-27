using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace DataAccess
{
  /// <summary>
  /// Summary description for EmployeeDetails
  /// </summary>
  [XmlType("DataAccessEmployeeDetails")]
  public class EmployeeDetails : IEmployeeDetails
  {
    public EmployeeDetails()
    {
      //
      // TODO: Add constructor logic here
      //
    }

    public DateTime? BirthDate { get; set; }

    public string HomePhone { get; set; }

    public List<string> territoryId { get; set; }

    public List<Territory> territories { get; set; }

    public List<EmployeeSummary> employees { get; set; }

    public int EmployeeID { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public string Title { get; set; }

    public string TitleOfCourtesy { get; set; }

    public DateTime? HireDate { get; set; }

    public string Notes { get; set; }

    public int? ReportsTo { get; set; }

    public static EmployeeDetails Get(string connectionString, int? id)
    {
      EmployeeDetails ed;

      using (SqlConnection conn = new SqlConnection(connectionString))
      {
        conn.Open();
        using (SqlTransaction txn = conn.BeginTransaction())
        {
          if (id.HasValue)
          {
            ed = Select(conn, txn, id.Value);
          }
          else
          {
            ed = new EmployeeDetails();
          }
          ed.employees = EmployeeSummary.SelectAll(conn, txn);
          txn.Commit();
        }
      }

      return ed;
    }

    static EmployeeDetails Select(SqlConnection conn, SqlTransaction txn, int id)
    {
      EmployeeDetails ed = new EmployeeDetails();

      List<EmployeeSummary> esList = new List<EmployeeSummary>();

      string sqlCmd = "SELECT EmployeeID, LastName, FirstName, Title, TitleOfCourtesy, BirthDate, HireDate, HomePhone, Notes, ReportsTo" +
        " FROM Employees WHERE (EmployeeID = @EmployeeID)";
      using (SqlCommand cmd = new SqlCommand(sqlCmd, conn, txn))
      {
        SqlParameter p = new SqlParameter("@EmployeeID", id);
        cmd.Parameters.Add(p);
        using (SqlDataReader rdr = cmd.ExecuteReader())
        {
          rdr.Read();
          ed.EmployeeID = rdr.GetInt32(rdr.GetOrdinal("EmployeeID"));
          ed.LastName = rdr.GetString(rdr.GetOrdinal("LastName"));
          ed.FirstName = rdr.GetString(rdr.GetOrdinal("FirstName"));
          ed.Title = rdr.IsDBNull(rdr.GetOrdinal("Title")) ? "" : rdr.GetString(rdr.GetOrdinal("Title"));
          ed.TitleOfCourtesy = rdr.IsDBNull(rdr.GetOrdinal("TitleOfCourtesy")) ? "" : rdr.GetString(rdr.GetOrdinal("TitleOfCourtesy"));
          ed.HireDate = rdr.IsDBNull(rdr.GetOrdinal("HireDate"))
              ? (DateTime?)null : rdr.GetDateTime(rdr.GetOrdinal("HireDate"));
          ed.BirthDate = rdr.IsDBNull(rdr.GetOrdinal("BirthDate"))
              ? (DateTime?)null : rdr.GetDateTime(rdr.GetOrdinal("BirthDate"));
          ed.HomePhone = rdr.IsDBNull(rdr.GetOrdinal("HomePhone")) ? "" : rdr.GetString(rdr.GetOrdinal("HomePhone"));
          ed.Notes = rdr.IsDBNull(rdr.GetOrdinal("Notes")) ? "" : rdr.GetString(rdr.GetOrdinal("Notes"));
          ed.ReportsTo = rdr.IsDBNull(rdr.GetOrdinal("ReportsTo"))
              ? (int?)null : rdr.GetInt32(rdr.GetOrdinal("ReportsTo"));
        }
      }

      return ed;
    }
  } 
}