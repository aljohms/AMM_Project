using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AMM_Project.Frontend.Models
{
    public class Branch
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Location { get; set; }
        public long BusinessId { get; set; }
        public virtual List<Employee> Employees { get; set; }
        public virtual List<BranchItem> branchItems { get; set; }
        [JsonIgnore]
        public  virtual Business Business { get; set; }
    }
}
