using RCS_WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RCS_WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserClientController : ControllerBase
    {
        [HttpGet("OrderList")]
        public async Task<OrderResponse> OrderList(int UserID)
        {
            OrderResponse orderResponse = new OrderResponse();
            try
            {
                using (RCS_dbContext context = new RCS_dbContext())
                {
                    List<Order> response = new List<Order>();
                    if (UserID > 0)
                        response = context.Orders.AsNoTracking().ToListAsync().Result.FindAll(x => x.CreatedBy == UserID);
                    else
                        response = await context.Orders.AsNoTracking().ToListAsync();

                    List<OrderModel> orderModels = new List<OrderModel>();
                    foreach (Order item in response)
                    {
                        OrderModel orderModel = new OrderModel();
                        orderModel.order = item;
                        orderModel.driver_userLogin = await context.UserLogins.AsNoTracking().SingleOrDefaultAsync(x => x.Id == item.CreatedBy);
                        orderModel.customer_userLogin = await context.UserLogins.AsNoTracking().SingleOrDefaultAsync(x => x.Id == item.CustomerLoginID);
                        orderModels.Add(orderModel);
                    }
                    orderResponse.ListOrder = orderModels;
                    orderResponse.ResponseCode = ResponseCode.Success;
                }
            }
            catch (Exception ex)
            {
                orderResponse.ResponseCode = ResponseCode.Exception;
                orderResponse.Exception = ex;
            }
            return orderResponse;
        }
        
    }
}