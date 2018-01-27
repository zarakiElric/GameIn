//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GameIn.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using App_GlobalResources;

    public partial class Users
    {
        public long ID { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(500, MinimumLength = 3, ErrorMessageResourceName = "MaxPassed", ErrorMessageResourceType = typeof(Resources))]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(Resources))]
        public string Email { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Username")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(200, MinimumLength = 3, ErrorMessageResourceName = "MaxPassed", ErrorMessageResourceType = typeof(Resources))]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(200, MinimumLength = 5, ErrorMessageResourceName = "MaxPassed", ErrorMessageResourceType = typeof(Resources))]
        public string Password { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Resources), Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessageResourceName = "ConfirmMatch", ErrorMessageResourceType = typeof(Resources))]
        public string ConfirmPassword { get; set; }

        public byte Type { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public bool Status { get; set; }

        public byte? Country { get; set; }
        public int? State { get; set; }
        public Int64? Region { get; set; }

        [StringLength(200, MinimumLength = 0, ErrorMessageResourceName = "MaxPassed", ErrorMessageResourceType = typeof(Resources))]
        public string SubRegion { get; set; }
        public byte Lang { get; set; }
        public byte Theme { get; set; }
        public string TimeZone { get; set; }
        public DateTime CreateDate { get; set; }
        public Nullable<DateTime> LastLogin { get; set; }
        public string LastIP { get; set; }
        public string LastUserAgent { get; set; }

        public Users()
        {
            Type = (int)Enums.Users.Type.Standard;
            Lang = (int)Enums.Users.Lang.en_US;
        }


    }
}
