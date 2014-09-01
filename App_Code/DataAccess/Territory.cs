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

    public bool EmployeeHas { get; set; }

    public Territory()
    {

    }

    public Territory(Territory t)
    {
      this.TerritoryDescription = t.TerritoryDescription;
      this.TerritoryID = t.TerritoryID;
      this.EmployeeHas = t.EmployeeHas;
    }

    public static List<Territory> SelectAll(SqlConnection conn, SqlTransaction txn)
    {
      List<Territory> tList = new List<Territory>();

      string sqlCmd = "SELECT TerritoryID, TerritoryDescription FROM Territories";
      using (SqlCommand cmd = new SqlCommand(sqlCmd, conn, txn))
      {
        using (SqlDataReader rdr = cmd.ExecuteReader())
        {
          while (rdr.Read())
          {
            Territory t = new Territory
            {
              TerritoryID = rdr.GetString(rdr.GetOrdinal("TerritoryID")),
              TerritoryDescription = rdr.GetString(rdr.GetOrdinal("TerritoryDescription")),
              EmployeeHas = false
            };
            tList.Add(t);
          }
        }
      }

      return tList;
    }

    public static List<string> SelectIdsForEmployee(SqlConnection conn, SqlTransaction txn, int id)
    {
      List<string> idList = new List<string>();

      string sqlCmd = "SELECT TerritoryID FROM EmployeeTerritories WHERE (EmployeeID = @EmployeeID)";
      using (SqlCommand cmd = new SqlCommand(sqlCmd, conn, txn))
      {
        cmd.Parameters.AddWithValue("@EmployeeID", id);
        using (SqlDataReader rdr = cmd.ExecuteReader())
        {
          while (rdr.Read())
          {
            string territoryId = rdr.GetString(rdr.GetOrdinal("TerritoryID"));
            idList.Add(territoryId);
          }
        }
      }

      return idList;
    }
  }
}