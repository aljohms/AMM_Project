using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AMM_Project.Models
{
    public class EmployeeItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string DocumentTitle { get; set; }
        public DateTime? ExpDate { get; set; }
        public string DocumentNumber { get; set; }
        public int? ItemAnnualExpectedCost { get; set; }
        public int EmployeeId { get; set; }
        public byte? Attachment { get; set; }
        public bool Notified { get; set; }
        [JsonIgnore]
        public virtual Employee Employee { get; set; }
    }
}
