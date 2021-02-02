using RCS_WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace RentACarSystem.Controllers
{
    public class SignInSignUpController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            UserLogin userLogin = HttpContext.Session.Get<UserLogin>("UserLogin");
            if (userLogin != null)
            {
                if (userLogin.Password.Contains("@DCL"))
                {
                    ViewData["Message"] = "It seems you have logged in with system generated password. Please change your password to avoid any security risk.";
                    return View("ChangePassword");
                }
                else if (userLogin.Type == UserLoginType.SuperAdmin)
                    return Redirect("/UserSuperAdmin/Index");
                else if (userLogin.Type == UserLoginType.Administrator)
                    return Redirect("/UserAdmin/Index");
                else if (userLogin.Type == UserLoginType.Customer)
                    return Redirect("/UserClient/Index");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(IFormCollection collection)
        {

            UserLogin userLogin = HttpContext.Session.Get<UserLogin>("UserLogin");
            if (userLogin != null)
            {
                if (userLogin.Password.Contains("@DCL"))
                {
                    ViewData["Message"] = "It seems you have logged in with system generated password. Please change your password to avoid any security risk.";
                    return View("ChangePassword");
                }
                else if (userLogin.Type == UserLoginType.SuperAdmin)
                    return Redirect("/UserSuperAdmin/Index");
                else if (userLogin.Type == UserLoginType.Administrator)
                {
                    string DATE = collection["DATE"].ToString();
                    string CAR = collection["CAR"].ToString();
                    string CITYFrom = collection["CITYFrom"].ToString();
                    string CITYTo = collection["CITYTo"].ToString();
                    string BOOKING = collection["BOOKING"].ToString();

                    if (DATE != null)
                    {
                        if (CITYFrom != "------" && CAR != "------")
                        {
                            OrderRequest orderRequest = new OrderRequest();
                            Order order = new Order();

                            order.LocationFrom = CITYFrom;
                            order.LocationTo = CITYTo;
                            order.BookingType = BOOKING;
                            order.BookingDateTime = System.DateTime.Parse(DATE);
                            order.Car = CAR;
                            order.CreatedBy = userLogin.Id;
                            order.CreatedByIpaddress = GlobalProperties.IP_Address;
                            orderRequest.Order = order;

                            ConsumeWebAPI.UserAdmin consumeWebAPI = new ConsumeWebAPI.UserAdmin();
                            OrderResponse orderResponse = consumeWebAPI.Create(orderRequest);
                            if (orderResponse.ResponseCode == ResponseCode.Success)
                            {
                                return Redirect("/UserAdmin/Index?OrderNumber=" + orderResponse.Order.order.Id);
                            }
                            else if (orderResponse.ResponseCode == ResponseCode.Duplicate)
                            {
                                TempData["Message"] = "Duplicate Booking Can't created in system. Same User can't book same Car for City at same Booking DateTime.";
                                return RedirectToAction("Index", "Home");
                            }
                        }

                    }
                    else if (userLogin.Type == UserLoginType.Customer)
                        return Redirect("/UserClient/Index");
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult SignIn(IFormCollection collection)
        {
            UserLogin userLogin = HttpContext.Session.Get<UserLogin>("UserLogin");
            if (userLogin != null)
            {
                return View("Index");
            }
            else
            {
                string LoginEmail = collection["LoginEmail"].ToString();
                string LoginPassword = collection["LoginPassword"].ToString();
                if (!string.IsNullOrEmpty(LoginEmail))
                    if (LoginEmail.Length > 3)
                    {
                        if (LoginEmail.Contains("@"))
                        {
                            UserSignInRequestModel signInRequestModel = new UserSignInRequestModel();
                            signInRequestModel.UserName = LoginEmail;
                            signInRequestModel.Password = LoginPassword;
                            signInRequestModel.ActionByIPAddress = GlobalProperties.IP_Address;

                            ConsumeWebAPI consumeWebAPI = new ConsumeWebAPI();
                            UserLoginResponse userLoginResponse = consumeWebAPI.SignIn(signInRequestModel);
                            if (userLoginResponse.ResponseCode == ResponseCode.Success)
                            {
                                HttpContext.Session.Set<UserLogin>("UserLogin", userLoginResponse.UserLogin);
                                if (userLoginResponse.UserLogin.Password.Contains("@DCL"))
                                {
                                    ViewData["Message"] = "It seems you have logged in with system generated password. Please change your password to avoid any security risk.";
                                    return View("ChangePassword");
                                }
                                else if (userLoginResponse.UserLogin.Type == UserLoginType.SuperAdmin)
                                    return Redirect("/UserSuperAdmin/Index");
                                else if (userLoginResponse.UserLogin.Type == UserLoginType.Administrator)
                                    return Redirect("/UserAdmin/Index");
                                else if (userLoginResponse.UserLogin.Type == UserLoginType.Customer)
                                    return Redirect("/UserClient/Index");
                                else
                                    ViewData["LoginMessage"] = "Redirection Failed. Invalid User Type.";
                            }
                            else
                                ViewData["LoginMessage"] = "Invalid User Name or Password.";
                        }
                        else
                            ViewData["LoginMessage"] = "User Name should be email address only.";
                    }
                    else
                        ViewData["LoginMessage"] = "User Name should be at least 3 characters long.";
            }
            return View("Index");
        }
        [HttpPost]
        public ActionResult SignUp(IFormCollection collection)
        {
            UserLogin userLogin = HttpContext.Session.Get<UserLogin>("UserLogin");
            if (userLogin != null)
            {
                return View("Index");
            }
            else
            {
                string FirstName = collection["FirstName"].ToString();
                string LastName = collection["LastName"].ToString();
                string Mobile = collection["Mobile"].ToString();
                string SignUpEmail = collection["SignUpEmail"].ToString();

                SignUpRequestModel signUpRequestModel = new SignUpRequestModel();
                signUpRequestModel.UserName = SignUpEmail;
                signUpRequestModel.FirstName = FirstName;
                signUpRequestModel.LastName = LastName;
                signUpRequestModel.Mobile = Mobile;

                ConsumeWebAPI consumeWebAPI = new ConsumeWebAPI();
                UserLoginResponse userLoginResponse = consumeWebAPI.SignUp(signUpRequestModel);
                if (userLoginResponse.ResponseCode == ResponseCode.Success)
                    ViewData["SignUpMessage"] = "Account has been created. Please check your email to verify your account. Link is valid for only three (3) hours.";
                else if (userLoginResponse.ResponseCode == ResponseCode.Inserted)
                    ViewData["SignUpMessage"] = "Failed to send email. Please contact administrator to verify and activate your account.";
            }
            return View("Index");
        }
        [HttpGet]
        public ActionResult VerifySignUp(string OTP, string EmailAddress)
        {
            UserLogin userLogin = HttpContext.Session.Get<UserLogin>("UserLogin");
            if (userLogin != null)
            {
                return View("Index");
            }
            else
            {
                if (!string.IsNullOrEmpty(OTP) || !string.IsNullOrEmpty(EmailAddress))
                {
                    if (OTP.Contains("@DCL") && OTP.Length == 12)
                    {
                        if (EmailAddress.Contains("@") || EmailAddress.Length < 3)
                        {
                            ConsumeWebAPI consumeWebAPI = new ConsumeWebAPI();
                            UserLoginResponse userLoginResponse = consumeWebAPI.VerifySignUp(OTP, EmailAddress);
                            if (userLoginResponse.ResponseCode == ResponseCode.Success)
                            {
                                if (userLoginResponse.UserLogin.Type == UserLoginType.SuperAdmin)
                                    return Redirect("/UserSuperAdmin/Index");
                                else if (userLoginResponse.UserLogin.Type == UserLoginType.Administrator)
                                    return Redirect("/UserAdmin/Index");
                                else if (userLoginResponse.UserLogin.Type == UserLoginType.Customer)
                                    return Redirect("/UserClient/Index");
                                else
                                    ViewData["Message"] = "Auto Redirection Failed. Invalid User Type.";
                            }
                            else if (userLoginResponse.ResponseCode == ResponseCode.Expired)
                                ViewData["Message"] = "Sorry, Your activation link has been expired. Please Enter Email Address and matched Mobile to re-send you an Account Activation Email.";
                            else
                                ViewData["Message"] = "Unknown Error Occurred. Please Enter Email Address and matched Mobile to re-send you an Account Activation Email.";
                        }
                        else ViewData["Message"] = "Invalid Email Address. Please Enter Email Address and matched Mobile to re-send you an Account Activation Email.";
                    }
                    else ViewData["Message"] = "Invalid OTP. Please Enter Email Address and matched Mobile to re-send you an Account Activation Email.";
                }
                else ViewData["Message"] = "Invalid OTP and EMAIL Link. Please Enter Email Address and matched Mobile to re-send you an Account Activation Email.";
            }
            return View();
        }
        [HttpPost]
        public ActionResult VerifySignUp(IFormCollection collection)
        {
            UserLogin userLogin = HttpContext.Session.Get<UserLogin>("UserLogin");
            if (userLogin != null)
            {
                return View("Index");
            }
            else
            {
                string LoginEmail = collection["Email"].ToString();
                string Mobile = collection["Mobile"].ToString();
                if (!string.IsNullOrEmpty(LoginEmail))
                    if (LoginEmail.Length > 3)
                    {
                        if (LoginEmail.Contains("@"))
                        {
                            SignUpRequestModel signUpRequestModel = new SignUpRequestModel();
                            signUpRequestModel.UserName = LoginEmail;
                            signUpRequestModel.Mobile = Mobile;
                            signUpRequestModel.ActionByIPAddress = GlobalProperties.IP_Address;

                            ConsumeWebAPI consumeWebAPI = new ConsumeWebAPI();
                            UserLoginResponse userLoginResponse = consumeWebAPI.ReVerifySignUp(signUpRequestModel);
                            if (userLoginResponse.ResponseCode == ResponseCode.Success)
                                ViewData["SignUpMessage"] = "Verification Email has been regenerated. Please check your email to verify your account. Link is valid for only three (3) hours.";
                            else if (userLoginResponse.ResponseCode == ResponseCode.Inserted)
                                ViewData["SignUpMessage"] = "Failed to send email. Please contact administrator to verify and activate your account.";
                        }
                    }
            }
            return View("Index");
        }
        [HttpGet]
        public ActionResult ChangePassword(string view_type)
        {
            if (string.IsNullOrEmpty(view_type))
                view_type = "FULL";

            TempData["view_type"] = view_type;

            if (view_type.ToUpper() == "PARTIAL")
                return PartialView();
            else
                return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(IFormCollection collection)
        {
            string view_type = "FINAL";
            if (TempData["view_type"] != null)
                view_type = TempData["view_type"].ToString();

            UserLogin userLogin = HttpContext.Session.Get<UserLogin>("UserLogin");
            if (userLogin != null)
            {
                string OldPassword = collection["OldPassword"].ToString();
                string NewPassword = collection["NewPassword"].ToString();
                string ConfirmPassword = collection["ConfirmPassword"].ToString();
                if (!string.IsNullOrEmpty(OldPassword) || !string.IsNullOrEmpty(NewPassword) || !string.IsNullOrEmpty(ConfirmPassword))
                {
                    if (userLogin.Password == OldPassword)
                    {
                        if (NewPassword.Length > 5)
                        {
                            userLogin.Password = NewPassword;
                            userLogin.ModifiedDateTime = System.DateTime.Now;
                            userLogin.ModifiedBy = userLogin.Id;
                            userLogin.ModifiedByIpaddress = GlobalProperties.IP_Address;

                            ConsumeWebAPI consumeWebAPI = new ConsumeWebAPI();
                            UserLoginResponse userLoginResponse = consumeWebAPI.ChangePassword(userLogin);
                            if (userLoginResponse.ResponseCode == ResponseCode.Updated)
                            {
                                userLoginResponse = consumeWebAPI.SignIn(new UserSignInRequestModel { UserName = userLogin.UserName, Password = userLogin.Password });
                                if (userLoginResponse.ResponseCode == ResponseCode.Success)
                                {
                                    HttpContext.Session.Remove("UserLogin");
                                    HttpContext.Session.Set<UserLogin>("UserLogin", userLoginResponse.UserLogin);
                                }
                                ViewData["Message"] = "Password has been successfully Changed.";
                                if (userLogin.Type == UserLoginType.SuperAdmin)
                                    ViewData["Redirect"] = "/UserSuperAdmin/Index";
                                else if (userLogin.Type == UserLoginType.Administrator)
                                    ViewData["Redirect"] = "/UserAdmin/Index";
                                else if (userLogin.Type == UserLoginType.Customer)
                                    ViewData["Redirect"] = "/UserClient/Index";
                            }
                            else
                                ViewData["Message"] = "Unable to Changed Password. Please contact Administrator.";
                        }
                        else
                            ViewData["Message"] = "Password must be at least 6 characters.";
                    }
                    else
                        ViewData["Message"] = "Old Password not matched.";
                }
                else
                    ViewData["Message"] = "Old, New or Confirm Password all can't be empty.";
            }

            if (view_type.ToUpper() == "PARTIAL")
                return PartialView();
            else
                return View();
        }
        [HttpGet]
        public ActionResult SignOut()
        {
            foreach (string session_key_name in HttpContext.Session.Keys)
                HttpContext.Session.Remove(session_key_name);
            return Redirect("/Home/Index");
        }
    }
}

//Dictionary<string, StringValues> keyValuePairs = new Dictionary<string, StringValues>();
//keyValuePairs.Add("LoginEmail", EmailAddress);
//keyValuePairs.Add("LoginPassword", userLoginResponse.UserLogin.Password);
//FormCollection keyValues = new FormCollection(keyValuePairs); //keyValuePairs;
