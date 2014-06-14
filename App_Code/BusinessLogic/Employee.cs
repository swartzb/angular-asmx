using System.Collections.Generic;
using System.Linq;
using DA = DataAccess;
using System.Diagnostics;
using System.Xml.Serialization;

namespace BusinessLogic
{
  public class EmployeeLite
  {
    public string DisplayName { get; set; }
    public int EmployeeID { get; set; }

    public EmployeeLite()
    {

    }

    public EmployeeLite(Employee e)
    {
      this.EmployeeID = e.EmployeeID;
      this.DisplayName = e.DisplayName;
    }

    public static explicit operator EmployeeLite(Employee e)
    {
      EmployeeLite el = new EmployeeLite(e);
      return el;
    }
  }

  /// <summary>
  /// Summary description for Employee
  /// </summary>
  [XmlType("DataAccessEmployee")]
  public class Employee : DA.Employee
  {
    public new class Details
    {
      public List<EmployeeLite> canReportTo { get; set; }

      public Details()
      {

      }

      public Details(DA.Employee.Details daDetails, int? id)
      {
        canReportTo = new List<EmployeeLite>();
        foreach (DA.Employee candidate in daDetails.employees)
        {
          if (id.HasValue)
          {
            DA.Employee tmp = candidate;
            bool done = false;
            do
            {
              if (tmp.EmployeeID == id.Value)
              {
                done = true;
              }
              else if (!tmp.ReportsTo.HasValue)
              {
                Employee blEmp = new Employee(candidate);
                canReportTo.Add((EmployeeLite)blEmp);
                done = true;
              }
              else
              {
                tmp = daDetails.employees.
                  Where(e => e.EmployeeID == tmp.ReportsTo.Value).
                  Single();
              }
            } while (!done);
          }
          else
          {
            Employee blEmp = new Employee(candidate);
            canReportTo.Add((EmployeeLite)blEmp);
          }
        }
      }
    }

    public new class ReturnVal
    {
      public int? id { get; set; }
      public int? numRows { get; set; }
      public List<Employee> employees { get; set; }

      public ReturnVal(DA.Employee.ReturnVal daRetVal)
      {
        id = daRetVal.id;
        numRows = daRetVal.numRows;
        employees = FromDaList(daRetVal.employees);
      }

      public ReturnVal()
      {

      }
    }

    public EmployeeLite Supervisor { get; set; }

    public string DisplayName { get; set; }

    public Employee(DA.Employee e)
      : base(e)
    {
      DisplayName = LastName + ", " + FirstName;
      if (!string.IsNullOrWhiteSpace(TitleOfCourtesy))
      {
        DisplayName += ", " + TitleOfCourtesy;
      }
      if (!string.IsNullOrWhiteSpace(Title))
      {
        DisplayName += ", " + Title;
      }
    }

    public Employee()
    {

    }

    public static new Details GetDetails(string connectionString, int? id)
    {
      DA.Employee.Details daDetails = DA.Employee.GetDetails(connectionString, id);

      return new Details(daDetails, id);
    }

    public static new ReturnVal Edit(string connectionString, DA.Employee emp)
    {
      DA.Employee.ReturnVal daRetVal = DA.Employee.Edit(connectionString, emp);
      ReturnVal retVal = new ReturnVal(daRetVal);
      return retVal;
    }

    public static new ReturnVal Add(string connectionString, DA.Employee emp)
    {
      DA.Employee.ReturnVal daRetVal = DA.Employee.Add(connectionString, emp);
      ReturnVal retVal = new ReturnVal(daRetVal);
      return retVal;
    }

    public static new ReturnVal Remove(string connectionString, int id)
    {
      DA.Employee.ReturnVal daRetVal = DA.Employee.Remove(connectionString, id);
      ReturnVal retVal = new ReturnVal(daRetVal);
      return retVal;
    }

    public static new ReturnVal GetAll(string connectionString)
    {
      DA.Employee.ReturnVal daRetVal = DA.Employee.GetAll(connectionString);
      ReturnVal retVal = new ReturnVal(daRetVal);
      return retVal;
    }

    static List<Employee> FromDaList(List<DA.Employee> daEmployeeList)
    {
      List<Employee> blEmployeeList = daEmployeeList
        .Select(daEmp => { Employee blEmp = new Employee(daEmp); return blEmp; })
        .ToList();

      foreach (Employee blEmp in blEmployeeList)
      {
        if (blEmp.ReportsTo.HasValue)
        {
          Employee supervisor = blEmployeeList.
            Where(e => e.EmployeeID == blEmp.ReportsTo.Value).
            Single();
          blEmp.Supervisor = (EmployeeLite)supervisor;
        }
      }

      return blEmployeeList;
    }
  }
}