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
    public class UserSuperAdminController : ControllerBase
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
                    ObjCompleteUserLoginResponse.ListUserLogin = response;
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

        [HttpPost("UserInsert")]
        public async Task<UserLoginResponse> UserInsert(UserLogin userLogin)
        {
            UserLoginResponse ObjUserLoginResponse = new UserLoginResponse();
            try
            {
                using (RCS_dbContext context = new RCS_dbContext())
                {
                    var UserLogins_EntityEntry = await context.UserLogins.AddAsync(userLogin);
                    int UserLogins_Response = await context.SaveChangesAsync();
                    if (UserLogins_Response > 0)
                    {
                        ObjUserLoginResponse.UserLogin = await context.UserLogins.AsNoTracking().FirstOrDefaultAsync(a => a.Id == userLogin.Id);
                        UserOtp userOtp = new UserOtp();
                        userOtp.UserLoginId = ObjUserLoginResponse.UserLogin.Id;
                        userOtp.Otp = userLogin.Password;
                        userOtp.CreatedByIpaddress = userLogin.CreatedByIpaddress;
                        userOtp.CreatedBy = userLogin.CreatedBy;

                        var UserOTP_EntityEntry = await context.AddAsync(userOtp);
                        int UserOTP_Response = await context.SaveChangesAsync();
                        if (UserOTP_Response > 0)
                        {
                            ObjUserLoginResponse.ResponseCode = ResponseCode.Inserted;
                            MailOperations mailOperations = new MailOperations();
                            string name = userLogin.FirstName + " " + userLogin.LastName.ToUpper();
                            if (mailOperations.SendCreateUserEmail(new System.Net.Mail.MailAddress(userLogin.UserName, name), userOtp.Otp))
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
    }
}