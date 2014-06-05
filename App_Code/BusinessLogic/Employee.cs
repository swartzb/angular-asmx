using System.Collections.Generic;
using System.Linq;
using DA = DataAccess;
using System.Diagnostics;

namespace BusinessLogic
{
  public class EmployeeLite
  {
    public string DisplayName { get; set; }
    public int EmployeeID { get; set; }

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
  public class Employee : DA.Employee
  {
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
    }

    public List<EmployeeLite> CanReportTo { get; set; }

    public string DisplayName { get; set; }

    public Employee(DA.Employee e)
      : base(e)
    {
      this.CanReportTo = new List<EmployeeLite>();
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
      this.CanReportTo = new List<EmployeeLite>();
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

      for (int i = 0, len = blEmployeeList.Count; i < len; ++i)
      {
        Employee blEmp = blEmployeeList[i];
        Debug.WriteLine("blEmp: {0}, {1}", blEmp.EmployeeID, blEmp.DisplayName);
        for (int j = 0; j < len; ++j)
        {
          Employee candidate = blEmployeeList[j];
          Debug.WriteLine("  candidate: {0}, {1}", candidate.EmployeeID, candidate.DisplayName);
          for
          (
            Employee testEmpl = candidate;
            true;
            testEmpl = testEmpl.ReportsTo.HasValue ? blEmployeeList.Where(empl => empl.EmployeeID == testEmpl.ReportsTo.Value).Single() : null
          )
          {
            if (testEmpl == blEmp)
            {
              Debug.WriteLine("    testEmpl: {0}, {1}", testEmpl.EmployeeID, testEmpl.DisplayName);
              break;
            }
            else if (testEmpl == null)
            {
              Debug.WriteLine("    testEmpl: null");
              blEmp.CanReportTo.Add((EmployeeLite)candidate);
              break;
            }
            else
            {
              Debug.WriteLine("    testEmpl: {0}, {1}", testEmpl.EmployeeID, testEmpl.DisplayName);
            }
          }
        }
      }

      //foreach (Employee blEmp in blEmployeeList)
      //{
      //  foreach (Employee candidate in blEmployeeList)
      //  {
      //    for
      //    (
      //      Employee testEmpl = candidate;
      //      testEmpl == blEmp;
      //      testEmpl = testEmpl.ReportsTo.HasValue ? blEmployeeList.Where(empl => empl.EmployeeID == testEmpl.ReportsTo.Value).Single() : null
      //    )
      //    {
      //      if (testEmpl == null)
      //      {
      //        blEmp.CanReportTo.Add((EmployeeLite)candidate);
      //        break;
      //      }
      //    }
      //  }
      //}

      return blEmployeeList;
    }
  }
}