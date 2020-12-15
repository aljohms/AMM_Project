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
        public byte[] Attachment { get; set; }
        public string FileName { get; set; }

        public void SetImage(Microsoft.AspNetCore.Http.IFormFile file)
        {
            if (file == null)
                return;

            FileName = file.ContentType;

            using (var stream = new System.IO.MemoryStream())
            {
                file.CopyTo(stream);
                Attachment = stream.ToArray();
            }
        }
        public bool Notified { get; set; }
        [JsonIgnore]
        public virtual Employee Employee { get; set; }
    }
}
