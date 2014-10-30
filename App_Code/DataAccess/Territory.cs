using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DataAccess
{
  /// <summary>
  /// Summary description for Territory
  /// </summary>
  public class Territory
  {
    public string TerritoryID { set; get; }
    public string TerritoryDescription { set; get; }
    public bool EmployeeCoversTerritory { get; set; }

    public Territory()
    {

    }

    public static EmployeeSummaryRetVal UpdateTerritoriesForEmployee(string connectionString, int id, List<string> territoryIDs)
    {
      EmployeeSummaryRetVal retVal = new EmployeeSummaryRetVal();

      using (SqlConnection conn = new SqlConnection(connectionString))
      {
        conn.Open();
        using (SqlTransaction txn = conn.BeginTransaction())
        {
          string delCmdStr = "DELETE FROM EmployeeTerritories WHERE (EmployeeID = @id)";
          using (SqlCommand cmd = new SqlCommand(delCmdStr, conn, txn))
          {
            cmd.Parameters.AddWithValue("@id", id);
            retVal.numRows = cmd.ExecuteNonQuery();
          }

          foreach (string tId in territoryIDs)
          {
            string insCmdStr = "INSERT INTO EmployeeTerritories (EmployeeID, TerritoryID) VALUES (@eId, @tId)";
            using (SqlCommand cmd = new SqlCommand(insCmdStr, conn, txn))
            {
              cmd.Parameters.AddWithValue("@eId", id);
              cmd.Parameters.AddWithValue("@tId", tId);
              retVal.numRows = cmd.ExecuteNonQuery();
            }
          }

          retVal.employees = EmployeeSummary.SelectAll(conn, txn);

          foreach (EmployeeSummary es in retVal.employees)
          {
            es.Territories = EmployeeSummary.GetTerritories(conn, txn, es.EmployeeID);
          }

          txn.Commit();
        }
      }

      return retVal;
    }

    public static List<Territory> GetTerritoriesForEmployee(string connectionString, int id)
    {
      List<Territory> tList;

      using (SqlConnection conn = new SqlConnection(connectionString))
      {
        conn.Open();
        using (SqlTransaction txn = conn.BeginTransaction())
        {
          tList = TerritoriesForEmployee(conn, txn, id);
          txn.Commit();
        }
      }

      return tList;
    }

    static List<Territory> TerritoriesForEmployee(SqlConnection conn, SqlTransaction txn, int? id)
    {
      List<Territory> tList = new List<Territory>();

      string sqlCmd = "SELECT TerritoryID, TerritoryDescription, dbo.EmployeeCoversTerritory(@id, TerritoryID) AS EmployeeCoversTerritory FROM Territories ORDER BY TerritoryDescription";
      using (SqlCommand cmd = new SqlCommand(sqlCmd, conn, txn))
      {
        cmd.Parameters.AddWithValue("@id", id);
        using (SqlDataReader rdr = cmd.ExecuteReader())
        {
          while (rdr.Read())
          {
            Territory t = new Territory
            {
              TerritoryID = rdr.GetString(rdr.GetOrdinal("TerritoryID")),
              TerritoryDescription = rdr.GetString(rdr.GetOrdinal("TerritoryDescription")),
              EmployeeCoversTerritory = rdr.IsDBNull(rdr.GetOrdinal("EmployeeCoversTerritory"))
                  ? false : rdr.GetBoolean(rdr.GetOrdinal("EmployeeCoversTerritory")),
            };
            tList.Add(t);
          }
        }
      }

      return tList;
    }
  }
}