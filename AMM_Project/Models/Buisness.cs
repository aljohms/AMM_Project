using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AMM_Project.Models
{
    public class Buisness
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name="Buisness Name")]
        public string Name { get; set; }
        [Display(Name = "Activity Field")]
        public string ActivityField { get; set; }
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }
        public virtual  List<Branch> Branches { get; set; }
    }
}
