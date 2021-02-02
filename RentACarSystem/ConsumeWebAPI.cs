using System;
using RCS_WebAPI.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace RentACarSystem
{
    public class ConsumeWebAPI
    {
        #region SignInSignUp

        public UserLoginResponse SignIn(UserSignInRequestModel signInRequestModel)
        {
            try
            {
                string URL = GlobalProperties.WebAPI_URL + "/UserLogin/SignIn";
                string Response = string.Empty;
                UserLoginResponse userLoginResponse = new UserLoginResponse();
                userLoginResponse = (UserLoginResponse)iWebServiceConsumer.ConsumeJsonWebService(URL, signInRequestModel, userLoginResponse, GlobalProperties.WebAPI_Timeout, out Response);
                return userLoginResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public UserLoginResponse SignUp(SignUpRequestModel signUpRequestModel)
        {
            try
            {
                string URL = GlobalProperties.WebAPI_URL + "/UserLogin/SignUp";
                string Response = string.Empty;
                UserLoginResponse userLoginResponse = new UserLoginResponse();
                userLoginResponse = (UserLoginResponse)iWebServiceConsumer.ConsumeJsonWebService(URL, signUpRequestModel, userLoginResponse, GlobalProperties.WebAPI_Timeout, out Response);
                return userLoginResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public UserLoginResponse VerifySignUp(string OTP, string EmailAddress)
        {
            try
            {
                string URL = GlobalProperties.WebAPI_URL + "/UserLogin/VerifySignUp?OTP=" + OTP + "&EmailAddress=" + EmailAddress;
                string Response = string.Empty;
                UserLoginResponse userLoginResponse = new UserLoginResponse();
                userLoginResponse = (UserLoginResponse)iWebServiceConsumer.ConsumeJsonWebService(URL, userLoginResponse, GlobalProperties.WebAPI_Timeout, out Response);
                return userLoginResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public UserLoginResponse ReVerifySignUp(SignUpRequestModel signUpRequestModel)
        {
            try
            {
                string URL = GlobalProperties.WebAPI_URL + "/UserLogin/ReVerifySignUp";
                string Response = string.Empty;
                UserLoginResponse userLoginResponse = new UserLoginResponse();
                userLoginResponse = (UserLoginResponse)iWebServiceConsumer.ConsumeJsonWebService(URL, signUpRequestModel, userLoginResponse, GlobalProperties.WebAPI_Timeout, out Response);
                return userLoginResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public UserLoginResponse ChangePassword(UserLogin userLogin)
        {
            try
            {
                string URL = GlobalProperties.WebAPI_URL + "/UserLogin/UpdateUserLoginEntity";
                string Response = string.Empty;
                UserLoginResponse userLoginResponse = new UserLoginResponse();
                userLoginResponse = (UserLoginResponse)iWebServiceConsumer.ConsumeJsonWebService(URL, userLogin, userLoginResponse, GlobalProperties.WebAPI_Timeout, out Response);
                return userLoginResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        public class UserClient
        {
            public OrderResponse OrderList(int UserID)
            {
                try
                {
                    string URL = GlobalProperties.WebAPI_URL + "/UserClient/OrderList?UserID=" + UserID;
                    string Response = string.Empty;
                    OrderResponse orderResponse = new OrderResponse();
                    orderResponse = (OrderResponse)iWebServiceConsumer.ConsumeJsonWebService(URL, orderResponse, GlobalProperties.WebAPI_Timeout, out Response);
                    return orderResponse;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            
        }
        public class UserAdmin
        {
            public UserLoginResponse UsersList()
            {
                try
                {
                    string URL = GlobalProperties.WebAPI_URL + "/UserAdmin/UserList";
                    string Response = string.Empty;
                    UserLoginResponse userLoginResponse = new UserLoginResponse();
                    userLoginResponse = (UserLoginResponse)iWebServiceConsumer.ConsumeJsonWebService(URL, userLoginResponse, GlobalProperties.WebAPI_Timeout, out Response);
                    return userLoginResponse;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public OrderResponse Create(OrderRequest orderRequest)
            {
                try
                {
                    string URL = GlobalProperties.WebAPI_URL + "/UserAdmin/CreateOrder";
                    string Response = string.Empty;
                    OrderResponse orderResponse = new OrderResponse();
                    orderResponse = (OrderResponse)iWebServiceConsumer.ConsumeJsonWebService(URL, orderRequest, orderResponse, GlobalProperties.WebAPI_Timeout, out Response);
                    return orderResponse;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public OrderResponse Update(int OrderID, string Status)
            {
                try
                {
                    string URL = GlobalProperties.WebAPI_URL + "/UserAdmin/UpdateOrder?OrderID=" + OrderID + "&Status=" + Status;
                    string Response = string.Empty;
                    OrderResponse orderResponse = new OrderResponse();
                    orderResponse = (OrderResponse)iWebServiceConsumer.ConsumeJsonWebService(URL, orderResponse, GlobalProperties.WebAPI_Timeout, out Response);
                    return orderResponse;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public class UserSuperAdmin
        {
            public UserLoginResponse UsersList()
            {
                try
                {
                    string URL = GlobalProperties.WebAPI_URL + "/UserSuperAdmin/UserList";
                    string Response = string.Empty;
                    UserLoginResponse userLoginResponse = new UserLoginResponse();
                    userLoginResponse = (UserLoginResponse)iWebServiceConsumer.ConsumeJsonWebService(URL, userLoginResponse, GlobalProperties.WebAPI_Timeout, out Response);
                    return userLoginResponse;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public UserLoginResponse UserInsert(int UserID, UserLogin userLogin)
            {
                try
                {
                    userLogin.CreatedBy = UserID;
                    userLogin.CreatedByIpaddress = GlobalProperties.IP_Address;
                    userLogin.CreatedDateTime = DateTime.Now;

                    string URL = GlobalProperties.WebAPI_URL + "/UserSuperAdmin/UserInsert";
                    string Response = string.Empty;
                    UserLoginResponse userLoginResponse = new UserLoginResponse();
                    userLoginResponse = (UserLoginResponse)iWebServiceConsumer.ConsumeJsonWebService(URL, userLogin, userLoginResponse, GlobalProperties.WebAPI_Timeout, out Response);
                    return userLoginResponse;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public UserLoginResponse UserEdit(int UserID, UserLogin userLogin)
            {
                try
                {
                    userLogin.ModifiedBy = UserID;
                    userLogin.ModifiedByIpaddress = GlobalProperties.IP_Address;
                    userLogin.ModifiedDateTime = DateTime.Now;

                    string URL = GlobalProperties.WebAPI_URL + "/UserLogin/UpdateUserLoginEntity";
                    string Response = string.Empty;
                    UserLoginResponse userLoginResponse = new UserLoginResponse();
                    userLoginResponse = (UserLoginResponse)iWebServiceConsumer.ConsumeJsonWebService(URL, userLogin, userLoginResponse, GlobalProperties.WebAPI_Timeout, out Response);
                    return userLoginResponse;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }

    public class iWebServiceConsumer
    {

        #region JSON Service Consumer

        /// <summary>
        /// Consume Json based WebService (External Service must take input object as JSON and return an Object). USED FOR GET METHOD
        /// </summary>
        /// <param name="_url">URL of external Service</param>
        /// <param name="_Output_Object">The resultant object that will be retuen back from service as result.</param> 
        /// <param name="jsonData">Json Response Data</param>
        public static object ConsumeJsonWebService(string _url, object _Output_Object, int request_timeout, out string jsonData)
        {
            try
            {
                jsonData = string.Empty;
                WebRequest request = WebRequest.Create(_url);

                request.Method = "GET";
                request.Timeout = request_timeout;
                request.ContentType = "Application/json";

                using (Stream s = request.GetResponse().GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        jsonData = sr.ReadToEnd();
                    }
                }
                return JsonConvert.DeserializeObject(jsonData, _Output_Object.GetType());
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Consume Json based WebService (External Service must take input object as JSON and return an Object). POST METHOD
        /// </summary>
        /// <param name="_url">URL of external Service</param>
        /// <param name="_Input_Object">The object that need to consume over the service.</param>
        /// <param name="_Output_Object">The resultant object that will be return back from service as result.</param> 
        /// <param name="jsonData">Json Response Data</param>
        public static object ConsumeJsonWebService(string _url, object _Input_Object, object _Output_Object, int request_timeout, out string jsonData)
        {
            try
            {
                string Json_String = iSerializer.ObjectToJson(_Input_Object);

                // corrected to WebRequest from HttpWebRequest
                WebRequest request = WebRequest.Create(_url);

                request.Method = "POST";
                request.Timeout = request_timeout;
                request.ContentType = "application/json";

                //get a reference to the request-stream, and write the postData to it
                using (Stream s = request.GetRequestStream())
                {
                    using (StreamWriter sw = new StreamWriter(s))
                        sw.Write(Json_String);
                }
                //get response-stream, and use a streamReader to read the content
                using (Stream s = request.GetResponse().GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        jsonData = sr.ReadToEnd();
                    }
                }
                return JsonConvert.DeserializeObject(jsonData, _Output_Object.GetType());
            }
            catch (Exception ex) { throw ex; }
        }

        #endregion

    }
    public class iSerializer
    {
        public static string ObjectToJson(object _Input_Object)
        {

            string Json_String = JsonConvert.SerializeObject(_Input_Object);
            return Json_String;
        }
    }
}
