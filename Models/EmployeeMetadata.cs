using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
  public class EmployeeMetadata { }

  [MetadataType(typeof(EmployeeMetadata))]
  public partial class Employee
  {

  }
}
