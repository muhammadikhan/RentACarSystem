using System;

#nullable disable

namespace RCS_WebAPI.Models
{
    public partial class UserLogin
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public UserLoginType Type { get; set; }
        public UserLoginStatus? Status { get; set; }
        public Gender? Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string Mobile { get; set; }
        public string Landline { get; set; }
        public bool? EmailAlerts { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public int? CreatedBy { get; set; }
        public string CreatedByIpaddress { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public int? ModifiedBy { get; set; }
        public string ModifiedByIpaddress { get; set; }
    }
}
