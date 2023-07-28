using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PruebaTecnicaCoppel.Models
{
    public class Employee
    {
       [Key] 
       public long Id { get; set; }
       [Required]
       public string Name { get; set; } = "";
        [Required]
        public long EmployeeNumber { get; set; }
        [Required]
        public int role { get; set; }
        public bool Status { get; set; }

    }

    public class Role
    {
        public bool Driver  { get; set; }
        public bool Charger  { get; set; }
        public bool Assistant { get; set; }
    }
}
