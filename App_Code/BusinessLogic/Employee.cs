﻿using System.Collections.Generic;
using System.Linq;
using DA = DataAccess;

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

    public EmployeeLite Supervisor { get; set; }

    public List<EmployeeLite> CanReportTo { get; set; }

    public string DisplayName { get; set; }

    public Employee()
    {
      this.Supervisor = null;
      this.CanReportTo = new List<EmployeeLite>();
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
        blEmp.DisplayName = blEmp.LastName + ", " + blEmp.FirstName;
        if (!string.IsNullOrWhiteSpace(blEmp.TitleOfCourtesy))
        {
          blEmp.DisplayName += ", " + blEmp.TitleOfCourtesy;
        }
        if (!string.IsNullOrWhiteSpace(blEmp.Title))
        {
          blEmp.DisplayName += ", " + blEmp.Title;
        }
      }

      foreach (Employee blEmp in blEmployeeList)
      {
        if (blEmp.ReportsTo.HasValue)
        {
          Employee e = blEmployeeList
            .Where(empl => empl.EmployeeID == blEmp.ReportsTo.Value)
            .Single();
          blEmp.Supervisor = (EmployeeLite)e;
        }
      }

      foreach (Employee blEmp in blEmployeeList)
      {
        foreach (Employee candidate in blEmployeeList)
        {
          for
          (
            Employee testEmpl = candidate;
            true;
            testEmpl = testEmpl.ReportsTo.HasValue ? blEmployeeList.Where(empl => empl.EmployeeID == testEmpl.ReportsTo.Value).Single() : null
          )
          {
            if (testEmpl == blEmp)
            {
              break;
            }
            else if (testEmpl == null)
            {
              blEmp.CanReportTo.Add((EmployeeLite)candidate);
              break;
            }
          }
        }
      }

      return blEmployeeList;
    }
  }
}