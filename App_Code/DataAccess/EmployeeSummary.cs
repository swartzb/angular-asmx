﻿using System;
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
  public class EmployeeSummary
  {
    public EmployeeSummary()
    {

    }

    public int EmployeeID { get; set; }
    public string Name { get; set; }
    public DateTime? HireDate { get; set; }
    public string Notes { get; set; }
    public System.Nullable<int> ReportsTo { get; set; }
    public string SupervisorName { get; set; }
    public System.Nullable<bool> CanBeDeleted { get; set; }

    public static EmployeeSummaryRetVal Remove(string connectionString, int id)
    {
      EmployeeSummaryRetVal retVal = new EmployeeSummaryRetVal();

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

    public static List<EmployeeSummary> SelectAll(SqlConnection conn, SqlTransaction txn)
    {
      List<EmployeeSummary> esList = new List<EmployeeSummary>();

      string sqlCmd = "SELECT EmployeeID, Name, HireDate, Notes, ReportsTo, SupervisorName, CanBeDeleted FROM EmployeeSummaries";
      using (SqlCommand cmd = new SqlCommand(sqlCmd, conn, txn))
      {
        using (SqlDataReader rdr = cmd.ExecuteReader())
        {
          while (rdr.Read())
          {
            EmployeeSummary es = new EmployeeSummary
            {
              EmployeeID = rdr.GetInt32(rdr.GetOrdinal("EmployeeID")),
              Name = rdr.GetString(rdr.GetOrdinal("Name")),
              HireDate = rdr.IsDBNull(rdr.GetOrdinal("HireDate"))
                  ? (DateTime?)null : rdr.GetDateTime(rdr.GetOrdinal("HireDate")),
              Notes = rdr.IsDBNull(rdr.GetOrdinal("Notes")) ? "" : rdr.GetString(rdr.GetOrdinal("Notes")),
              ReportsTo = rdr.IsDBNull(rdr.GetOrdinal("ReportsTo"))
                  ? (int?)null : rdr.GetInt32(rdr.GetOrdinal("ReportsTo")),
              //SupervisorName = rdr.GetString(rdr.GetOrdinal("SupervisorName")),
              SupervisorName = rdr.IsDBNull(rdr.GetOrdinal("SupervisorName"))
                  ? null : rdr.GetString(rdr.GetOrdinal("SupervisorName")),
              CanBeDeleted = rdr.IsDBNull(rdr.GetOrdinal("CanBeDeleted"))
                  ? false : rdr.GetBoolean(rdr.GetOrdinal("CanBeDeleted")),
            };
            esList.Add(es);
          }
        }
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