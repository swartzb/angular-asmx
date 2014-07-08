using System;
using System.Collections.Generic;
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

    public Territory()
    {

    }

    public Territory(Territory t)
    {
      this.TerritoryDescription = t.TerritoryDescription;
      this.TerritoryID = t.TerritoryID;
    }
  }

}