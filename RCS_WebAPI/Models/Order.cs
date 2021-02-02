using System;

#nullable disable

namespace RCS_WebAPI.Models
{
    public partial class Order
    {
        public int Id { get; set; }
        public int CustomerLoginID { get; set; }
        public string Car { get; set; }
        public string LocationFrom { get; set; }
        public string LocationTo { get; set; }
        public string BookingType { get; set; }
        public decimal BookingRate { get; set; }
        public DateTime BookingDateTime { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public int? CreatedBy { get; set; }
        public string CreatedByIpaddress { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public int? ModifiedBy { get; set; }
        public string ModifiedByIpaddress { get; set; }
    }
}
