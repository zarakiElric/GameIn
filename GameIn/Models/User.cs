namespace GameIn
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        public long ID { get; set; }

        [Required]
        [StringLength(500)]
        public string Email { get; set; }

        [Required]
        [StringLength(200)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public byte Type { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public bool Gender { get; set; }

        public bool Status { get; set; }

        public byte Country { get; set; }

        public byte? State { get; set; }

        public byte? Region { get; set; }

        [StringLength(200)]
        public string SubRegion { get; set; }

        public byte Lang { get; set; }

        [StringLength(100)]
        public string TimeZone { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? LastLogin { get; set; }

        [StringLength(15)]
        public string LastIP { get; set; }

        [StringLength(500)]
        public string LastUserAgent { get; set; }
    }
}
