using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.Models
{
  public class EmployeeMetadata { }

  [MetadataType(typeof(EmployeeMetadata))]
  public partial class Employee
  {
    public static Employee Create(EmployeeContext db, Employee employee)
    {
      employee.CreateDate = DateTime.Now;
      employee.UpdateDate = DateTime.Now;
      employee.IsDelete = false;
      db.Employees.Add(employee);
      db.SaveChanges();

      return employee;
    }

    public static List<Employee> GetAll(EmployeeContext db)
    {
      List<Employee> employees = db.Employees.Where(q => q.IsDelete != true).ToList();
      return employees;
    }

    public static Employee GetById(EmployeeContext db, int id)
    {
      Employee? result = db.Employees.Where(q => q.Id == id && q.IsDelete != true).FirstOrDefault();
      return result ?? new Employee();
    }

    public static Employee Update(EmployeeContext db, Employee employee)
    {
      employee.UpdateDate = DateTime.Now;
      db.Entry(employee).State = EntityState.Modified;
      db.SaveChanges();

      return employee;
    }

    public static Employee Delete(EmployeeContext db, int id)
    {
      Employee employee = GetById(db, id);

      employee.IsDelete = true;
      db.Entry(employee).State = EntityState.Modified;
      db.SaveChanges();

      return employee;
    }

    public static List<Employee> Search(EmployeeContext db, string keyword)
    {
      List<Employee> result = db.Employees.Where(q => q.FirstName.Contains(keyword) || q.LastName.Contains(keyword) && q.IsDelete != true).ToList();
      return result;
    }
  }
}
