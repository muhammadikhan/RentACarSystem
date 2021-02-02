using RCS_WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RentACarSystem.Controllers
{
    public class UserClientController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Name"] = "Customer";
            UserLogin userLogin = HttpContext.Session.Get<UserLogin>("UserLogin");
            if (userLogin != null)
                ViewData["Name"] = userLogin.FirstName.ToUpper() + " " + userLogin.LastName.ToUpper();
            return View();
        }
        [HttpGet]
        public ActionResult OrdersList()
        {
            UserLogin userLogin = HttpContext.Session.Get<UserLogin>("UserLogin");
            if (userLogin != null)
            {
                ConsumeWebAPI.UserClient UserClientAPI = new ConsumeWebAPI.UserClient();
                OrderResponse orderResponse = UserClientAPI.OrderList(0);
                if (orderResponse.ResponseCode == ResponseCode.Success)
                {
                    return PartialView(orderResponse.ListOrder);
                }
                else
                    return PartialView();
            }
            else
                return Redirect("/UserClient/Index");
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