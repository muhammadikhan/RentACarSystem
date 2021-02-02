using System;
using System.ComponentModel.DataAnnotations;

namespace RCS_WebAPI.Models
{
    public class BaseResponseModel
    {
        public ResponseCode ResponseCode { get; set; }
        public string ResponseDisplayMessage { get; set; }
        public string ResponseErrorMessage { get; set; }
        public Exception Exception { get; set; }
    }
    public enum ResponseCode
    {
        Exception = 0,
        Success = 1,
        Inserted = 2,
        Updated = 3,
        Deleted = 4,
        Expired = 5,
        Duplicate = 6,
    }
    public class BaseResquestModel
    {
        public int ActionByUserID { get; set; }
        public string ActionByIPAddress { get; set; }
    }
}