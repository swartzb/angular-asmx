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

    public static List<string> GetNames(SqlConnection conn, SqlTransaction txn, int id)
    {
      List<string> names = new List<string>();

      string sqlCmd = "SELECT TerritoryDescription FROM Territories WHERE (dbo.EmployeeCoversTerritory(@id, TerritoryID) = 1) ORDER BY TerritoryDescription";
      using (SqlCommand cmd = new SqlCommand(sqlCmd, conn, txn))
      {
        cmd.Parameters.AddWithValue("@id", id);
        using (SqlDataReader rdr = cmd.ExecuteReader())
        {
          while (rdr.Read())
          {
            string name = rdr.GetString(rdr.GetOrdinal("TerritoryDescription"));
            names.Add(name.Trim());
          }
        }
      }

      return names;
    }
  }
}
