//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PTIStockMgmt.Models
{
    using System;
    using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
    
    public partial class location
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string location_string { get; set; }
        public string postcode { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string street { get; set; }
        public string number { get; set; }
        public string suburub { get; set; }
    }
}
