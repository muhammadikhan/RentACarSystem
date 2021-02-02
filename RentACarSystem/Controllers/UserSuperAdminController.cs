using RCS_WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace RentACarSystem.Controllers
{
    public class UserSuperAdminController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Name"] = "ADMIN";
            UserLogin userLogin = HttpContext.Session.Get<UserLogin>("UserLogin");
            if (userLogin != null)
            {
                ViewData["Name"] = userLogin.FirstName.ToUpper() + " " + userLogin.LastName.ToUpper();
                return View();
            }
            else
                return Redirect("../Home/Index");
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
                    return PartialView(orderResponse.ListOrder);
                else
                    return PartialView();
            }
            else
                return Redirect("/UserClient/Index");

        }
       
        [HttpGet]
        public ActionResult UsersList()
        {
            ConsumeWebAPI.UserSuperAdmin userSuperAdminAPI = new ConsumeWebAPI.UserSuperAdmin();
            UserLoginResponse userLoginResponse = userSuperAdminAPI.UsersList();
            if (userLoginResponse.ResponseCode == ResponseCode.Success)
            {
                HttpContext.Session.Remove("ListUserLogin");
                HttpContext.Session.Set<List<UserLogin>>("ListUserLogin", userLoginResponse.ListUserLogin);
                return PartialView(userLoginResponse.ListUserLogin);
            }
            else
                return PartialView();
        }

        [HttpGet]
        public ActionResult UserInsert()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult UserInsert(UserLogin userLogin)
        {
            int UserID = 0;
            UserLogin _userLogin = HttpContext.Session.Get<UserLogin>("UserLogin");
            if (userLogin != null)
            {
                UserID = _userLogin.Id;

                ConsumeWebAPI.UserSuperAdmin userSuperAdminAPI = new ConsumeWebAPI.UserSuperAdmin();
                UserLoginResponse userLoginResponse = userSuperAdminAPI.UserInsert(UserID, userLogin);
                if (userLoginResponse.ResponseCode == ResponseCode.Success)
                    return Redirect("/UserSuperAdmin/UsersList");
                else
                    return PartialView(UserID);
            }
            else
                return Redirect("../Home/Index");
        }

        [HttpGet]
        public ActionResult UserEdit(int id)
        {
            List<UserLogin> listUserLogin = HttpContext.Session.Get<List<UserLogin>>("ListUserLogin");
            if (listUserLogin != null)
            {
                UserLogin userLogin = new UserLogin();
                userLogin = listUserLogin.Find(x => x.Id == id);
                return PartialView(userLogin);
            }
            else
                return Redirect("/UserSuperAdmin/UsersList");
        }

        [HttpPost]
        public ActionResult UserEdit(UserLogin userLogin)
        {
            int UserID = 0;
            UserLogin _userLogin = HttpContext.Session.Get<UserLogin>("UserLogin");
            if (userLogin != null)
            {
                UserID = _userLogin.Id;
                ConsumeWebAPI.UserSuperAdmin userSuperAdminAPI = new ConsumeWebAPI.UserSuperAdmin();
                UserLoginResponse userLoginResponse = userSuperAdminAPI.UserEdit(UserID,userLogin);
                if (userLoginResponse.ResponseCode == ResponseCode.Updated)
                    return Redirect("/UserSuperAdmin/UsersList");
                else
                    return PartialView(UserID);
            }
            else
                return Redirect("../Home/Index");
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

        [HttpGet]
        public ActionResult Dashboard()
        {
            return PartialView();
        }
    }
}