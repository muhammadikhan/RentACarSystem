using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RCS_WebAPI.Models
{
    public class OrderResponse : BaseResponseModel
    {
        public List<OrderModel> ListOrder { get; set; }
        public OrderModel Order { get; set; }
    }
    public class OrderRequest: BaseResquestModel
    {
        public Order Order { get; set; }
    }
    public class OrderModel
    {
        public Order order { get; set; }
        public UserLogin driver_userLogin { get; set; }
        public UserLogin customer_userLogin { get; set; }
    }
}
