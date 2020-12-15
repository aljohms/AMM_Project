using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AMM_Project.Frontend.Models
{
    public class EmployeeItem
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [Display(Name = "Document Title")]
        public string DocumentTitle { get; set; }
        public DateTime? ExpDate { get; set; }
        [Display(Name = "DocumentNumber")]
        public string DocumentNumber { get; set; }
        [Display(Name = "Renewal Fees")]
        public long? ItemAnnualExpectedCost { get; set; }
        public long EmployeeId { get; set; }
        public byte? Attachment { get; set; }
        public bool Notified { get; set; }
        [JsonIgnore]
        public virtual Employee Employee { get; set; }
    }
}
