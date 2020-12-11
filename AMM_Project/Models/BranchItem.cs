using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AMM_Project.Models
{
    public class BranchItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string DocumentTitle { get; set; }
        public DateTime? ExpDate { get; set; }
        public string DocumentNumber { get; set; }
        public int? AnnualCost { get; set; }
        public bool Notified { get; set; }
        public byte? Attachment { get; set; }
        public int BranchId { get; set; }
        [JsonIgnore]
        public virtual Branch Branch { get; set; }


    }
}
