//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Solution.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class Login
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Användarnamn")]
        public string Name { get; set; }
        [DataType(DataType.Password)]
        [Required]
        [DisplayName("Lösenord")]
        public string Password { get; set; }
    }
}
