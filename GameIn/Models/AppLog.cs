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
    using System.Collections.Generic;
    
    public partial class AppLog
    {
        public int ID { get; set; }
        public string Error { get; set; }
        public string Method { get; set; }
        public string Class { get; set; }
        public string ErrorType { get; set; }
        public string StackTrace { get; set; }
        public string Source { get; set; }
        public int UserID { get; set; }
        public string IP { get; set; }
        public System.DateTime Date { get; set; }
    }
}
