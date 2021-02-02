using System;
using System.Collections.Generic;

#nullable disable

namespace RCS_WebAPI.Models
{
    public partial class UserOtp
    {
        public int UserLoginId { get; set; }
        public string Otp { get; set; }
        public bool? IsExpired { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public int? CreatedBy { get; set; }
        public string CreatedByIpaddress { get; set; }
    }
}
