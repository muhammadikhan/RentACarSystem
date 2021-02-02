using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using RCS_WebAPI.Models;

namespace RCS_WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserLoginController : ControllerBase
    {
        [HttpPost("SignIn")]
        public async Task<UserLoginResponse> SignIn(UserSignInRequestModel ObjSignInRequestModel)
        {
            UserLoginResponse ObjUserLoginResponse = new UserLoginResponse();
            try
            {
                using (RCS_dbContext context = new RCS_dbContext())
                {
                    UserLogin userLogin = await context.UserLogins.AsNoTracking().FirstOrDefaultAsync(a => a.UserName == ObjSignInRequestModel.UserName && a.Password == ObjSignInRequestModel.Password.ToUpper());
                    if (userLogin != null)
                    {
                        ObjUserLoginResponse.UserLogin = userLogin;
                        if (ObjUserLoginResponse.UserLogin.Id > 0)
                        {
                            MailOperations mailOperations = new MailOperations();
                            string name = userLogin.FirstName + " " + userLogin.LastName.ToUpper();
                            if (mailOperations.SendSignInEmail(new System.Net.Mail.MailAddress(userLogin.UserName, name)))
                                ObjUserLoginResponse.ResponseCode = ResponseCode.Success;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                ObjUserLoginResponse.ResponseCode = ResponseCode.Exception;
                ObjUserLoginResponse.Exception = ex;
            }
            return ObjUserLoginResponse;
        }

        [HttpPost("SignUp")]
        public async Task<UserLoginResponse> SignUp(SignUpRequestModel ObjSignUpRequestModel)
        {
            UserLoginResponse ObjUserLoginResponse = new UserLoginResponse();
            try
            {
                using (RCS_dbContext context = new RCS_dbContext())
                {
                    UserLogin userLogin = new UserLogin();
                    userLogin.UserName = ObjSignUpRequestModel.UserName;
                    userLogin.Password = GlobalProperties.RandomOTP;
                    userLogin.FirstName = ObjSignUpRequestModel.FirstName;
                    userLogin.LastName = ObjSignUpRequestModel.LastName;
                    userLogin.Mobile = ObjSignUpRequestModel.Mobile;
                    userLogin.Type = ObjSignUpRequestModel.Type;
                    userLogin.CreatedByIpaddress = ObjSignUpRequestModel.ActionByIPAddress;
                    userLogin.CreatedBy = ObjSignUpRequestModel.ActionByUserID;

                    var UserLogins_EntityEntry = await context.UserLogins.AddAsync(userLogin);
                    int UserLogins_Response = await context.SaveChangesAsync();
                    if (UserLogins_Response > 0)
                    {
                        ObjUserLoginResponse.UserLogin = await context.UserLogins.AsNoTracking().FirstOrDefaultAsync(a => a.Id == userLogin.Id);
                        UserOtp userOtp = new UserOtp();
                        userOtp.UserLoginId = ObjUserLoginResponse.UserLogin.Id;
                        userOtp.Otp = userLogin.Password;
                        userOtp.CreatedByIpaddress = ObjSignUpRequestModel.ActionByIPAddress;
                        userOtp.CreatedBy = ObjSignUpRequestModel.ActionByUserID;

                        var UserOTP_EntityEntry = await context.AddAsync(userOtp);
                        int UserOTP_Response = await context.SaveChangesAsync();
                        if (UserOTP_Response > 0)
                        {
                            ObjUserLoginResponse.ResponseCode = ResponseCode.Inserted;
                            MailOperations mailOperations = new MailOperations();
                            string name = userLogin.FirstName + " " + userLogin.LastName.ToUpper();
                            if (mailOperations.SendSignUpEmail(new System.Net.Mail.MailAddress(userLogin.UserName, name), userOtp.Otp))
                                ObjUserLoginResponse.ResponseCode = ResponseCode.Success;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                ObjUserLoginResponse.ResponseCode = ResponseCode.Exception;
                ObjUserLoginResponse.Exception = ex;
            }
            return ObjUserLoginResponse;
        }

        [HttpGet("VerifySignUp")]
        public async Task<UserLoginResponse> VerifySignUp(string OTP, string EmailAddress)
        {
            UserLoginResponse ObjUserLoginResponse = new UserLoginResponse();
            try
            {
                using (RCS_dbContext context = new RCS_dbContext())
                {
                    UserLogin userLogin = await context.UserLogins.FirstOrDefaultAsync(x => x.UserName.ToUpper().Equals(EmailAddress.ToUpper()));
                    if (userLogin != null)
                    {
                        userLogin.Status = UserLoginStatus.Verified;
                        if (userLogin.Id > 0)
                        {
                            UserOtp userOtp = await context.UserOtps.FirstOrDefaultAsync(x => x.UserLoginId == userLogin.Id && x.Otp == OTP && x.IsExpired == false);
                            if (userOtp != null)
                            {
                                userOtp.IsExpired = true;
                                context.SaveChanges();

                                if (userOtp.CreatedDateTime > DateTime.Now.AddMinutes(int.Parse("-" + GlobalProperties.OTPExpiryMinutes)))
                                {
                                    userLogin.Status = UserLoginStatus.ApprovalInProcess;
                                    ObjUserLoginResponse.UserLogin = userLogin;

                                    MailOperations mailOperations = new MailOperations();
                                    string name = userLogin.FirstName + " " + userLogin.LastName.ToUpper();
                                    if (mailOperations.SendVerificationEmail(new System.Net.Mail.MailAddress(userLogin.UserName, name), userOtp.Otp))
                                        ObjUserLoginResponse.ResponseCode = ResponseCode.Success;

                                }
                                else
                                    ObjUserLoginResponse.ResponseCode = ResponseCode.Expired;
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                ObjUserLoginResponse.ResponseCode = ResponseCode.Exception;
                ObjUserLoginResponse.Exception = ex;
            }
            return ObjUserLoginResponse;
        }

        [HttpPost("ReVerifySignUp")]
        public async Task<UserLoginResponse> ReVerifySignUp(SignUpRequestModel ObjSignUpRequestModel)
        {
            UserLoginResponse ObjUserLoginResponse = new UserLoginResponse();
            try
            {
                using (RCS_dbContext context = new RCS_dbContext())
                {
                    UserLogin userLogin = await context.UserLogins.FirstOrDefaultAsync(x => x.UserName.ToUpper().Equals(ObjSignUpRequestModel.UserName.ToUpper()));
                    if (userLogin != null)
                    {
                        userLogin.Status = UserLoginStatus.Verified;
                        if (userLogin.Id > 0)
                        {
                            UserOtp userOtp = await context.UserOtps.FirstOrDefaultAsync(x => x.UserLoginId == userLogin.Id && x.IsExpired == false);
                            if (userOtp != null)
                            {
                                //string OTP = Guid.NewGuid().ToString("n").Substring(0, 8) + "@DCL";
                                UserOtp _userOtp = new UserOtp();
                                userOtp.IsExpired = true;

                                userLogin.Password = GlobalProperties.RandomOTP;
                                //Add new UserOtp 
                                _userOtp.UserLoginId = userLogin.Id;
                                _userOtp.Otp = userLogin.Password;
                                _userOtp.IsExpired = false;
                                _userOtp.CreatedByIpaddress = ObjSignUpRequestModel.ActionByIPAddress;
                                _userOtp.CreatedBy = ObjSignUpRequestModel.ActionByUserID;
                                var UserOTP_EntityEntry = await context.AddAsync(_userOtp);
                                int Response = await context.SaveChangesAsync();
                                if (Response > 0)
                                {
                                    MailOperations mailOperations = new MailOperations();
                                    string name = userLogin.FirstName + " " + userLogin.LastName.ToUpper();
                                    if (mailOperations.SendSignUpEmail(new System.Net.Mail.MailAddress(userLogin.UserName, name), _userOtp.Otp))
                                        ObjUserLoginResponse.ResponseCode = ResponseCode.Success;
                                }
                                else
                                    ObjUserLoginResponse.ResponseCode = ResponseCode.Inserted;
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                ObjUserLoginResponse.ResponseCode = ResponseCode.Exception;
                ObjUserLoginResponse.Exception = ex;
            }
            return ObjUserLoginResponse;
        }

        [HttpPost("UpdateUserLoginEntity")]
        public async Task<UserLoginResponse> UpdateUserLoginEntity(UserLogin userLogin)
        {
            UserLoginResponse userLoginResponse = new UserLoginResponse();
            try
            {
                using (RCS_dbContext context = new RCS_dbContext())
                {
                    UserLogin response = await context.UserLogins.FirstOrDefaultAsync(a => a.Id == userLogin.Id);

                    if (response.Password != userLogin.Password)
                        response.Password = userLogin.Password;

                    if (response.Type != userLogin.Type)
                        response.Type = userLogin.Type;

                    if (response.Type != userLogin.Type)
                        response.Type = userLogin.Type;

                    if (response.Status != userLogin.Status)
                        response.Status = userLogin.Status;

                    if (response.Location != userLogin.Location)
                        response.Location = userLogin.Location;

                    if (response.Mobile != userLogin.Mobile)
                        response.Mobile = userLogin.Mobile;

                    if (response.Landline != userLogin.Landline)
                        response.Landline = userLogin.Landline;

                    response.ModifiedBy = userLogin.ModifiedBy;
                    response.ModifiedByIpaddress = userLogin.ModifiedByIpaddress;
                    response.ModifiedDateTime = userLogin.ModifiedDateTime;

                    int updated = await context.SaveChangesAsync();
                    if (updated > 0)
                        userLoginResponse.ResponseCode = ResponseCode.Updated;
                }
            }
            catch (Exception ex)
            {
                userLoginResponse.ResponseCode = ResponseCode.Exception;
                userLoginResponse.Exception = ex;
            }
            return userLoginResponse;
        }
    }
}

//        //[HttpPost("ResetPassword")]
//        //public async Task<UserLoginResponseModel> ResetPassword(UserLoginRequestModel ObjUserLoginModel)
//        //{
//        //    UserLoginResponseModel ObjUserLoginResponseModel = new UserLoginResponseModel();
//        //    try
//        //    {
//        //        using (RCS_dbContext context = new RCS_dbContext())
//        //        {
//        //            ObjUserLoginResponseModel.UserLogin = await context.UserLogin.AsNoTracking().SingleOrDefaultAsync(a => a.UserName== ObjUserLoginModel.UserName&& a.Password== ObjUserLoginModel.Password.ToUpper());
//        //            ObjUserLoginResponseModel.ResponseCode = ResponseCode.Success;
//        //            if (ObjUserLoginResponseModel.UserLogin != null)
//        //                ObjUserLoginResponseModel.Status = true;
//        //        }
//        //    }
//        //    catch (System.Exception ex)
//        //    {
//        //        ObjUserLoginResponseModel.ResponseCode = ResponseCode.Exception;
//        //        ObjUserLoginResponseModel.Exception = ex;
//        //    }
//        //    return ObjUserLoginResponseModel;
//        //}
//        //[HttpPost("ForgetPassword")]
//        //public async Task<UserLoginResponseModel> ForgetPassword(UserLoginRequestModel ObjUserLoginModel)
//        //{
//        //    UserLoginResponseModel ObjUserLoginResponseModel = new UserLoginResponseModel();
//        //    try
//        //    {
//        //        using (RCS_dbContext context = new RCS_dbContext())
//        //        {
//        //            ObjUserLoginResponseModel.UserLogin = await context.UserLogin.AsNoTracking().SingleOrDefaultAsync(a => a.UserName== ObjUserLoginModel.UserName&& a.Password== ObjUserLoginModel.Password.ToUpper());
//        //            ObjUserLoginResponseModel.ResponseCode = ResponseCode.Success;
//        //            if (ObjUserLoginResponseModel.UserLogin != null)
//        //                ObjUserLoginResponseModel.Status = true;
//        //        }
//        //    }
//        //    catch (System.Exception ex)
//        //    {
//        //        ObjUserLoginResponseModel.ResponseCode = ResponseCode.Exception;
//        //        ObjUserLoginResponseModel.Exception = ex;
//        //    }
//        //    return ObjUserLoginResponseModel;
//        //}

//    }
//}

/*
 
        //[HttpGet("GetUserLoginList")]
        //public async Task<UserLoginResponse> GetUserLoginList()
        //{
        //    UserLoginResponse userLoginResponse = new UserLoginResponse();
        //    try
        //    {
        //        using (RCS_dbContext context = new RCS_dbContext())
        //        {
        //            userLoginResponse.ListUserLogin = await context.UserLogins.AsNoTracking().ToListAsync();
        //            userLoginResponse.ResponseCode = ResponseCode.Success;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        userLoginResponse.ResponseCode = ResponseCode.Exception;
        //        userLoginResponse.Exception = ex;
        //    }
        //    return userLoginResponse;
        //}

        //[HttpGet("GetUserLoginListByUserTypes")]
        //public async Task<UserLoginResponse> GetUserLoginListByUserTypes(List<UserLoginType> userLoginType)
        //{
        //    UserLoginResponse userLoginResponse = new UserLoginResponse();
        //    try
        //    {
        //        using (RCS_dbContext context = new RCS_dbContext())
        //        {
        //            List<UserLogin> response = await context.UserLogins.AsNoTracking().ToListAsync();
        //            foreach (UserLoginType _userLoginType in userLoginType)
        //                userLoginResponse.ListUserLogin = response.FindAll(a => a.Type == _userLoginType);
        //            userLoginResponse.ResponseCode = ResponseCode.Success;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        userLoginResponse.ResponseCode = ResponseCode.Exception;
        //        userLoginResponse.Exception = ex;
        //    }
        //    return userLoginResponse;
        //}

        //[HttpGet("GetUserLoginListByUserStatuses")]
        //public async Task<UserLoginResponse> GetUserLoginListByUserStatuses(List<UserLoginStatus> userLoginStatus)
        //{
        //    UserLoginResponse userLoginResponse = new UserLoginResponse();
        //    try
        //    {
        //        using (RCS_dbContext context = new RCS_dbContext())
        //        {
        //            List<UserLogin> response = await context.UserLogins.AsNoTracking().ToListAsync();
        //            foreach (UserLoginStatus _userLoginStatus in userLoginStatus)
        //                userLoginResponse.ListUserLogin = response.FindAll(a => a.Status == _userLoginStatus);
        //            userLoginResponse.ResponseCode = ResponseCode.Success;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        userLoginResponse.ResponseCode = ResponseCode.Exception;
        //        userLoginResponse.Exception = ex;
        //    }
        //    return userLoginResponse;
        //}*/