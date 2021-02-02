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
    public class UserAdminController : ControllerBase
    {
        [HttpGet("UserList")]
        public async Task<UserLoginResponse> UserList()
        {
            UserLoginResponse ObjCompleteUserLoginResponse = new UserLoginResponse();
            try
            {
                using (RCS_dbContext context = new RCS_dbContext())
                {
                    List<UserLogin> response = await context.UserLogins.AsNoTracking().ToListAsync();
                    ObjCompleteUserLoginResponse.ListUserLogin = response.FindAll(x=>x.Type == UserLoginType.Customer);
                    ObjCompleteUserLoginResponse.ResponseCode = ResponseCode.Success;
                }
            }
            catch (Exception ex)
            {
                ObjCompleteUserLoginResponse.ResponseCode = ResponseCode.Exception;
                ObjCompleteUserLoginResponse.Exception = ex;
            }
            return ObjCompleteUserLoginResponse;
        }
        [HttpPost("CreateOrder")]
        public async Task<OrderResponse> CreateOrder(OrderRequest orderRequest)
        {
            OrderResponse orderResponse = new OrderResponse();
            try
            {
                using (RCS_dbContext context = new RCS_dbContext())
                {
                    var Orders_EntityEntry = await context.Orders.AddAsync(orderRequest.Order);
                    int Orders_Response = await context.SaveChangesAsync();
                    if (Orders_Response > 0)
                    {
                        orderResponse.Order = new OrderModel();
                        orderResponse.Order.order = orderRequest.Order;
                        orderResponse.ResponseCode = ResponseCode.Success;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("duplicate"))
                    orderResponse.ResponseCode = ResponseCode.Duplicate;
                else
                {
                    orderResponse.ResponseCode = ResponseCode.Exception;
                    orderResponse.Exception = ex;
                }
            }
            return orderResponse;
        }
        [HttpPost("UpdateOrder")]
        public async Task<OrderResponse> UpdateOrder(int OrderID, string Status)
        {
            OrderResponse orderResponse = new OrderResponse();
            try
            {
                using (RCS_dbContext context = new RCS_dbContext())
                {
                    Order order = await context.Orders.FirstOrDefaultAsync(x => x.Id == OrderID);
                    order.Status = Status;
                    int Orders_Response = await context.SaveChangesAsync();
                    if (Orders_Response > 0)
                    {
                        orderResponse.Order = new OrderModel();
                        orderResponse.Order.order = order;
                        orderResponse.ResponseCode = ResponseCode.Updated;
                    }
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