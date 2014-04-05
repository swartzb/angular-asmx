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

    public static new List<Employee> GetAll(string connectionString)
    {
      List<DA.Employee> daEmployeeList = DA.Employee.GetAll(connectionString);
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