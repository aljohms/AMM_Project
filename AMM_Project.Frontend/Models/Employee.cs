﻿using System;
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
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Name")]
        public string ContactNumber { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        public long BranchId { get; set; }
        [JsonIgnore]
        public virtual Branch Branch { get; set; }
        public virtual List<EmployeeItem> EmployeeItems { get; set; }
    }
}
