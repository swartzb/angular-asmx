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

    public static List<string> GetForEmployee(SqlConnection conn, SqlTransaction txn, int id)
    {
      List<string> territories = new List<string>();

      string sqlCmd = "SELECT TerritoryDescription FROM Territories WHERE (dbo.EmployeeCoversTerritory(@id, TerritoryID) = 1) ORDER BY TerritoryDescription";
      using (SqlCommand cmd = new SqlCommand(sqlCmd, conn, txn))
      {
        cmd.Parameters.AddWithValue("@id", id);
        using (SqlDataReader rdr = cmd.ExecuteReader())
        {
          while (rdr.Read())
          {
            string desc = rdr.GetString(rdr.GetOrdinal("TerritoryDescription"));
            territories.Add(desc.Trim());
          }
        }
      }

      return territories;
    }

    public static EmployeeSummaryRetVal Update(string connectionString, int id, List<string> territoryIDs)
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

          txn.Commit();
        }
      }

      return retVal;
    }

    public static List<Territory> Get(string connectionString, int id)
    {
      List<Territory> tList = new List<Territory>();

      using (SqlConnection conn = new SqlConnection(connectionString))
      {
        conn.Open();
        using (SqlTransaction txn = conn.BeginTransaction())
        {
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

          txn.Commit();
        }
      }

      return tList;
    }
  }
}