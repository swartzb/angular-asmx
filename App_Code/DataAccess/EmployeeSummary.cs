using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace DataAccess
{
  /// <summary>
  /// Summary description for EmployeeSummary
  /// </summary>
  [XmlType("DataAccessEmployeeSummary")]
  public class EmployeeSummary : IEmployeeSummary
  {
    public EmployeeSummary()
    {

    }

#region DA.IEmployeeSummary
    public int EmployeeID { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Title { get; set; }
    public string TitleOfCourtesy { get; set; }
    public DateTime? HireDate { get; set; }
    public string Notes { get; set; }
    public System.Nullable<int> ReportsTo { get; set; }
    public bool canDelete { get; set; }
#endregion

    public static List<EmployeeSummary> SelectAll(SqlConnection conn, SqlTransaction txn)
    {
      List<EmployeeSummary> esList = new List<EmployeeSummary>();

      string sqlCmd = "SELECT EmployeeID, LastName, FirstName, Title, TitleOfCourtesy, HireDate, Notes, ReportsTo FROM Employees";
      using (SqlCommand cmd = new SqlCommand(sqlCmd, conn, txn))
      {
        using (SqlDataReader rdr = cmd.ExecuteReader())
        {
          while (rdr.Read())
          {
            EmployeeSummary es = new EmployeeSummary
            {
              EmployeeID = rdr.GetInt32(rdr.GetOrdinal("EmployeeID")),
              LastName = rdr.GetString(rdr.GetOrdinal("LastName")),
              FirstName = rdr.GetString(rdr.GetOrdinal("FirstName")),
              Title = rdr.IsDBNull(rdr.GetOrdinal("Title")) ? "" : rdr.GetString(rdr.GetOrdinal("Title")),
              TitleOfCourtesy = rdr.IsDBNull(rdr.GetOrdinal("TitleOfCourtesy")) ? "" : rdr.GetString(rdr.GetOrdinal("TitleOfCourtesy")),
              HireDate = rdr.IsDBNull(rdr.GetOrdinal("HireDate"))
                  ? (DateTime?)null : rdr.GetDateTime(rdr.GetOrdinal("HireDate")),
              Notes = rdr.IsDBNull(rdr.GetOrdinal("Notes")) ? "" : rdr.GetString(rdr.GetOrdinal("Notes")),
              ReportsTo = rdr.IsDBNull(rdr.GetOrdinal("ReportsTo"))
                  ? (int?)null : rdr.GetInt32(rdr.GetOrdinal("ReportsTo")),
            };
            esList.Add(es);
          }
        }
      }

      foreach (EmployeeSummary es in esList)
      {
        es.canDelete = !esList.Any(e => e.ReportsTo == es.EmployeeID);
      }

      return esList;
    }

    public static EmployeeSummaryRetVal GetAll(string connectionString)
    {
      EmployeeSummaryRetVal retVal = new EmployeeSummaryRetVal();

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