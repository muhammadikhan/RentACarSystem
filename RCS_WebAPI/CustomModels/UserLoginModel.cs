using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RCS_WebAPI.Models
{
    public class UserLoginResponse : BaseResponseModel
    {
        public List<UserLogin> ListUserLogin { get; set; }
        public UserLogin UserLogin { get; set; }
    }

    //public class UserInsertRequestModel : BaseResquestModel
    //{
    //    public UserLogin UserLogin { get; set; }
    //}


    #region Sign IN -- Model Region

    public class UserSignInRequestModel : BaseResquestModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    #endregion

    #region Sign UP -- Model Region

    public class SignUpRequestModel : BaseResquestModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public UserLoginType Type { get; set; }
    }

    #endregion
    public enum Gender
    {
        Male = 1,
        Female = 0,
    }
    public enum UserLoginType
    {
        SuperAdmin = 1,
        Administrator = 2,
        //Default DB Value
        Customer = 3
    }
    public enum UserLoginStatus
    {
        ///Not Approved or Rejected or Deactivated
        InActive = 0,
        ///Approved or Active
        Active = 1,
        ///In process for Approval from sAdmin or Administrator
        ApprovalInProcess = 3,
        ///If SignUp Email Responded
        Verified = 4,
        ///Default DB Value 
        NotVerified = 9
    }
}
