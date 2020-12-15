using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AMM_Project.Frontend.Models
{
    public class BranchItem
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [Display(Name = "Document Title")]
        public string DocumentTitle { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Expiry Date")]
        public DateTime ExpDate { get; set; }
        [Display(Name = "Document Number")]
        public string DocumentNumber { get; set; }
        [Display(Name = "Renewal Fees")]
        public long? AnnualCost { get; set; }
        public bool Notified { get; set; }
        public byte[] Attachment { get; set; }
        public string FileName { get; set; }

//        public string GetInlineImageSrc()
//        {
//            if (Attachment == null || ImageContentType == null)
//                return null;
//            string mimeType = "application/octet-stream"
//;
//            return new FileContentResult(Attachment, mimeType)
//            {
                
//            };
//           // var base64Image = System.Convert.ToBase64String(Attachment);
//            //return $"data:{ImageContentType};base64,{base64Image}";
//        }

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
        public long BranchId { get; set; }
        [JsonIgnore]
        public virtual Branch Branch { get; set; }


    }
}
