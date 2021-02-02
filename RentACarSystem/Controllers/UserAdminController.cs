using RCS_WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACarSystem.Controllers
{
    public class UserAdminController : Controller
    {
      
        public ActionResult Index(string OrderNumber)
        {
            ViewData["Name"] = "ADMIN";
            UserLogin userLogin = HttpContext.Session.Get<UserLogin>("UserLogin");
            if (userLogin != null)
                ViewData["Name"] = userLogin.FirstName.ToUpper() + " " + userLogin.LastName.ToUpper();

            if (!string.IsNullOrEmpty(OrderNumber))
                if (OrderNumber.Length > 0)
                {
                    ViewData["OrderNumber"] = OrderNumber;
                }

            return View();
        }
        [HttpGet]
        public ActionResult OrdersList()
        {
            UserLogin userLogin = HttpContext.Session.Get<UserLogin>("UserLogin");
            if (userLogin != null)
            {
                ConsumeWebAPI.UserClient UserClientAPI = new ConsumeWebAPI.UserClient();
                OrderResponse orderResponse = UserClientAPI.OrderList(userLogin.Id);
                if (orderResponse.ResponseCode == ResponseCode.Success)
                    return PartialView(orderResponse.ListOrder);
                else
                    return PartialView();
            }
            else
                return Redirect("/UserAdmin/Index");

        }
        [HttpGet]
        public ActionResult Update(string OrderID)
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Update(int OrderID, string Status)
        {
            UserLogin userLogin = HttpContext.Session.Get<UserLogin>("UserLogin");
            if (userLogin != null)
            {
                ConsumeWebAPI.UserAdmin UserClientAPI = new ConsumeWebAPI.UserAdmin();
                OrderResponse orderResponse = UserClientAPI.Update(OrderID, Status);
                if (orderResponse.ResponseCode == ResponseCode.Success)
                    return PartialView(orderResponse.ListOrder);
                else
                    return PartialView();
            }
            else
                return Redirect("/UserAdmin/Index");
        }
        [HttpGet]
        public ActionResult UsersList()
        {
            ConsumeWebAPI.UserAdmin userAdminAPI = new ConsumeWebAPI.UserAdmin();
            UserLoginResponse userLoginResponse = userAdminAPI.UsersList();
            if (userLoginResponse.ResponseCode == ResponseCode.Success)
                return PartialView(userLoginResponse.ListUserLogin);
            else
                return PartialView();
        }
        [HttpPost]
        public ActionResult UsersList(IFormCollection formCollection)
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult Profile()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Profile(IFormCollection formCollection)
        {
            return PartialView();
        }
    }
}