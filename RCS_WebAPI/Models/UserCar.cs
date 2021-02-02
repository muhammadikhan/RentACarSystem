using System;
using System.Collections.Generic;

#nullable disable

namespace RCS_WebAPI.Models
{
    public partial class UserCar
    {
        public int UserLoginId { get; set; }
        public string Name { get; set; }
        public bool? IsAvailable { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public int? CreatedBy { get; set; }
        public string CreatedByIpaddress { get; set; }
    }
}
