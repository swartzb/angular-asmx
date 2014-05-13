using System.Collections.Generic;
using System.Linq;
using DA = DataAccess;

namespace BusinessLogic
{
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

    public Employee Supervisor { get; set; }

    public Employee()
    {
      this.Supervisor = null;
    }

    public void Init(DA.Employee daEmployee)
    {
      this.EmployeeID = daEmployee.EmployeeID;
      this.LastName = daEmployee.LastName;
      this.FirstName = daEmployee.FirstName;
      this.Title = daEmployee.Title;
      this.TitleOfCourtesy = daEmployee.TitleOfCourtesy;
      this.BirthDate = daEmployee.BirthDate;
      this.HireDate = daEmployee.HireDate;
      this.Address = daEmployee.Address;
      this.City = daEmployee.City;
      this.Region = daEmployee.Region;
      this.PostalCode = daEmployee.PostalCode;
      this.Country = daEmployee.Country;
      this.HomePhone = daEmployee.HomePhone;
      this.Extension = daEmployee.Extension;
      this.Notes = daEmployee.Notes;
      this.ReportsTo = daEmployee.ReportsTo;
      this.PhotoPath = daEmployee.PhotoPath;
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
        .Select(daEmp => { Employee blEmp = new Employee(); blEmp.Init(daEmp); return blEmp; })
        .ToList();
      foreach (Employee blEmp in blEmployeeList)
      {
        if (blEmp.ReportsTo.HasValue)
        {
          blEmp.Supervisor = blEmployeeList
            .Where(empl => empl.EmployeeID == blEmp.ReportsTo.Value)
            .Single();
        }
      }

      return blEmployeeList;
    }
  }
}