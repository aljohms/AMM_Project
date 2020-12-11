using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AMM_Project.Frontend.Models
{
    public class Employee
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public long BranchId { get; set; }
        [JsonIgnore]
        public virtual Branch Branch { get; set; }
        public virtual List<EmployeeItem> EmployeeItems { get; set; }
    }
}
