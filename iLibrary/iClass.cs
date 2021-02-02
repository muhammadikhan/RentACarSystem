using System;
using System.Net;
using System.IO;
// To remove this error add reference as : System.Web.Extensions , Version : 4.0.0.0 , Description : System.Web.Extensions.dll
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using System.Xml;
using System.Text;

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
            JavaScriptSerializer JSS = new JavaScriptSerializer();
            // corrected to WebRequest from HttpWebRequest
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
            return JSS.Deserialize(jsonData, _Output_Object.GetType());
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
            JavaScriptSerializer JSS = new JavaScriptSerializer();
            //The maximum length of JSON strings. The default is 2097152 characters, which is equivalent to 4 MB of Unicode string data. 
            //2097152 = 4MB.  (2097152 * 5 as 4MB*5) = 20MB. So 10485760 = 20MB
            JSS.MaxJsonLength = 10485760; // 20MB Max data travel;
            string Json_String = JSS.Serialize(_Input_Object);

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
            return JSS.Deserialize(jsonData, _Output_Object.GetType());
        }
        catch (Exception ex) { throw ex; }
    }

    /// <summary>
    /// Consume Json based WebService (External Service must take input object as JSON and return an Object).
    /// </summary>
    /// <param name="_url">URL of external Service</param>
    /// <param name="_Input_Object">The object that need to consume over the service.</param>
    /// <param name="_Output_Object">The resultant object that will be retuen back from service as result.</param> 
    /// <param name="jsonData">Json Response Data</param>
    public static object ConsumeJsonWebService(string _url, string proxy_ip, string proxy_port, bool ServicePointManagerPass, object _Input_Object, object _Output_Object, int request_timeout, out string jsonData)
    {
        try
        {
            jsonData = string.Empty;
            if (SetProxy(proxy_ip, proxy_port) && !string.IsNullOrEmpty(_url))
            {
                if (ServicePointManagerPass)
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                JavaScriptSerializer JSS = new JavaScriptSerializer();
                string Json_String = JSS.Serialize(_Input_Object);

                // corrected to WebRequest from HttpWebRequest
                WebRequest request = WebRequest.Create(_url);

                request.Method = "POST";
                request.Timeout = request_timeout;
                request.ContentType = "application/json";

                ////get a reference to the request-stream, and write the postData to it
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
                return JSS.Deserialize(jsonData, _Output_Object.GetType());
            }
        }
        catch (Exception ex) { throw ex; }
        return null;
    }

    #endregion

    #region XML Service Consumer

    public static string ConsumeXMLWebService_GET(string url, string remote_user, string remote_pwd, string proxy_ip, string proxy_port, int request_timeout, out HttpStatusCode StatusCode)
    {
        try
        {
            StatusCode = HttpStatusCode.NoContent;
            String result = string.Empty;
            if (SetProxy(proxy_ip, proxy_port) && !string.IsNullOrEmpty(url))
            {
                HttpWebRequest _Request = (HttpWebRequest)HttpWebRequest.Create(url);
                _Request.Method = "GET";
                _Request.Timeout = request_timeout;
                _Request.Credentials = new NetworkCredential(remote_user, remote_pwd);
                HttpWebResponse _Response = (HttpWebResponse)_Request.GetResponse();
                XmlDocument _XMLDocument = new XmlDocument();
                XmlTextReader _XMLReader = new XmlTextReader(_Response.GetResponseStream());
                _XMLDocument.Load(_XMLReader);
                return _XMLDocument.ToString();
            }
        }
        catch (Exception ex) { throw ex; }
        return string.Empty;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="url">URL</param>
    /// <param name="remote_user">Remote System User Name</param>
    /// <param name="remote_pwd">Remote System Password</param>
    /// <param name="proxy_ip">User Name or IP for Proxy. (if any)</param>
    /// <param name="proxy_port">Port for Proxy. (if any)</param>
    /// <param name="objToDeSerialize">Object in which you want to DeSerialize yor XML.</param>
    /// <param name="ServicePointManagerPass">Use this as true if error e.g(Could not establish trust relationship for SSL/TLS secure channel) occured.</param>
    /// <param name="XMLContent">string XML content</param>
    /// <param name="StatusCode">Output.HttpStatusCode.(e.g Created/Success)</param>
    /// <returns>Object</returns>
    public static Object ConsumeXMLWebService_GET(string url, string remote_user, string remote_pwd, string proxy_ip, string proxy_port, Object objToDeSerialize, bool ServicePointManagerPass, int request_timeout, out string XML_Content, out HttpStatusCode StatusCode)
    {
        try
        {
            StatusCode = HttpStatusCode.NoContent;
            XML_Content = string.Empty;
            String result = string.Empty;
            if (SetProxy(proxy_ip, proxy_port) && !string.IsNullOrEmpty(url))
            {
                if (ServicePointManagerPass)
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                HttpWebRequest _Request = (HttpWebRequest)HttpWebRequest.Create(url);
                _Request.Method = "GET";
                _Request.Timeout = request_timeout;
                _Request.Credentials = new NetworkCredential(remote_user, remote_pwd);
                HttpWebResponse _Response = (HttpWebResponse)_Request.GetResponse();
                XmlDocument _XMLDocument = new XmlDocument();
                XmlTextReader _XMLReader = new XmlTextReader(_Response.GetResponseStream());
                _XMLDocument.Load(_XMLReader);
                XML_Content = _XMLDocument.InnerXml.ToString();
                return iSerializer.DeSerialize_XML_to_OBJECT(XML_Content, objToDeSerialize);
            }
        }
        catch (Exception ex) { throw ex; }
        return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="url">URL</param>
    /// <param name="remote_user">Remote System User Name</param>
    /// <param name="remote_pwd">Remote System Password</param>
    /// <param name="objToDeSerialize">Object in which you want to DeSerialize yor XML.</param>
    /// <param name="ServicePointManagerPass">Use this as true if error e.g(Could not establish trust relationship for SSL/TLS secure channel) occured.</param>
    /// <param name="XMLContent">string XML content</param>
    /// <param name="StatusCode">Output.HttpStatusCode.(e.g Created/Success)</param>
    /// <returns>Object</returns>
    public static Object ConsumeXMLWebService_GET(string url, string remote_user, string remote_pwd, Object objToDeSerialize, bool ServicePointManagerPass, int request_timeout, out string XML_Content, out HttpStatusCode StatusCode)
    {
        try
        {
            StatusCode = HttpStatusCode.NoContent;
            XML_Content = string.Empty;
            String result = string.Empty;
            if (!string.IsNullOrEmpty(url))
            {
                if (ServicePointManagerPass)
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                HttpWebRequest _Request = (HttpWebRequest)HttpWebRequest.Create(url);
                _Request.Method = "GET";
                _Request.Timeout = request_timeout;
                _Request.Credentials = new NetworkCredential(remote_user, remote_pwd);
                HttpWebResponse _Response = (HttpWebResponse)_Request.GetResponse();
                XmlDocument _XMLDocument = new XmlDocument();
                XmlTextReader _XMLReader = new XmlTextReader(_Response.GetResponseStream());
                _XMLDocument.Load(_XMLReader);
                XML_Content = _XMLDocument.InnerXml.ToString();
                return iSerializer.DeSerialize_XML_to_OBJECT(XML_Content, objToDeSerialize);
            }
        }
        catch (Exception ex) { throw ex; }
        return null;
    }


    /// <summary>
    /// ConsumeXMLWebService POST ONLY
    /// </summary>
    /// <param name="url">URL</param>
    /// <param name="xml">XML Content</param>
    /// <param name="StatusCode">Output.HttpStatusCode.(e.g Created/Success)</param>
    /// <returns>Object</returns>
    public static string ConsumeXMLWebService_POST(string url, string xml, int request_timeout, out HttpStatusCode StatusCode)
    {
        try
        {
            StatusCode = HttpStatusCode.NoContent;
            String result = string.Empty;
            if (!string.IsNullOrEmpty(xml) || !string.IsNullOrEmpty(url))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(xml);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Timeout = request_timeout;
                request.ContentLength = bytes.Length;
                using (Stream requestStream = request.GetRequestStream())
                    requestStream.Write(bytes, 0, bytes.Length);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    StreamReader loResponseStream = new StreamReader(response.GetResponseStream());
                    result = loResponseStream.ReadToEnd();
                    loResponseStream.Close();
                    StatusCode = response.StatusCode;
                    return result;
                }
            }
        }
        catch (Exception ex) { throw ex; }
        return null;
    }

    /// <summary>
    /// ConsumeXMLWebService POST ONLY
    /// </summary>
    /// <param name="url">URL</param>
    /// <param name="xml">XML Content</param>
    /// <param name="remote_user">Remote System User Name(if not required pass -> null)</param>
    /// <param name="remote_pwd">Remote System Password(if not required pass -> null)</param>
    /// <param name="StatusCode">Output.HttpStatusCode.(e.g Created/Success)</param>
    /// <returns>Object</returns>
    public static string ConsumeXMLWebService_POST(string url, string xml, string remote_user, string remote_pwd, int request_timeout, out HttpStatusCode StatusCode)
    {
        try
        {
            StatusCode = HttpStatusCode.NoContent;
            String result = string.Empty;
            if (!string.IsNullOrEmpty(xml) || !string.IsNullOrEmpty(url))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(xml);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Timeout = request_timeout;
                if (remote_user != null && remote_pwd != null)
                    request.Credentials = new NetworkCredential(remote_user, remote_pwd);
                request.ContentLength = bytes.Length;
                using (Stream requestStream = request.GetRequestStream())
                    requestStream.Write(bytes, 0, bytes.Length);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    StreamReader loResponseStream = new StreamReader(response.GetResponseStream());
                    result = loResponseStream.ReadToEnd();
                    loResponseStream.Close();
                    StatusCode = response.StatusCode;
                    return result;
                }
            }
        }
        catch (Exception ex) { throw ex; }
        return null;
    }

    /// <summary>
    /// ConsumeXMLWebService POST ONLY
    /// </summary>
    /// <param name="url">URL</param>
    /// <param name="xml">XML Content</param>
    /// <param name="remote_user">Remote System User Name</param>
    /// <param name="remote_pwd">Remote System Password</param>
    /// <param name="proxy_ip">User Name or IP for Proxy. (if any)</param>
    /// <param name="proxy_port">Port for Proxy. (if any)</param>
    /// <param name="StatusCode">Output.HttpStatusCode.(e.g Created/Success)</param>
    /// <returns>Object</returns>
    public static string ConsumeXMLWebService_POST(string url, string xml, string remote_user, string remote_pwd, string proxy_ip, string proxy_port, int request_timeout, out HttpStatusCode StatusCode)
    {
        try
        {
            StatusCode = HttpStatusCode.NoContent;
            String result = string.Empty;
            if (SetProxy(proxy_ip, proxy_port) && (!string.IsNullOrEmpty(xml) || !string.IsNullOrEmpty(url)))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(xml);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Timeout = request_timeout;
                request.Credentials = new NetworkCredential(remote_user, remote_pwd);
                request.ContentLength = bytes.Length;
                using (Stream requestStream = request.GetRequestStream())
                    requestStream.Write(bytes, 0, bytes.Length);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    StreamReader loResponseStream = new StreamReader(response.GetResponseStream());
                    result = loResponseStream.ReadToEnd();
                    loResponseStream.Close();
                    StatusCode = response.StatusCode;
                    return result;
                }
            }
        }
        catch (Exception ex) { throw ex; }
        return null;
    }

    static bool SetProxy(string ip_addr, string port_no)
    {
        try
        {
            if (!string.IsNullOrEmpty(ip_addr) && !string.IsNullOrEmpty(port_no))
            {
                WebProxy proxyObject = new WebProxy(ip_addr, int.Parse(port_no));
                proxyObject.BypassProxyOnLocal = true;
                proxyObject.Credentials = CredentialCache.DefaultCredentials;
                //GlobalProxySelection.Select = proxyObject;
                return true;
            }
            return false;
        }
        catch (Exception exx) { throw exx; }
    }

    #endregion

}
public class iSerializer
{
    /// <summary>
    /// Serialize_OBJECT_to_XML
    /// </summary>
    /// <param name="_Object">Input Object that need to transform into XML</param>
    /// <returns>String XML Content</returns>
    public static String Serialize_OBJECT_to_XML(Object _Object)
    {
        try
        {
            var output = new MemoryStream();
            var settings = new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true };

            using (var xmlWriter = XmlWriter.Create(output, settings))
            {
                var serializer = new XmlSerializer(_Object.GetType());
                var namespaces = new XmlSerializerNamespaces();
                xmlWriter.WriteStartDocument();
                // INPUT  ->   xmlWriter.WriteDocType("Field1", null, "someObject.dtd", null);
                // OUTPUT ->   <!DOCTYPE Field1 SYSTEM "someObject.dtd">
                namespaces.Add(string.Empty, string.Empty);
                serializer.Serialize(xmlWriter, _Object, namespaces);
            }
            output.Seek(0L, SeekOrigin.Begin);
            var reader = new StreamReader(output);
            return reader.ReadToEnd();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="XMLContent">string XML content</param>
    /// <param name="objToDeSerialize">Object in which you want to DeSerialize yor XML.</param>
    /// <returns></returns>
    public static Object DeSerialize_XML_to_OBJECT(string XMLContent, Object objToDeSerialize)
    {
        try
        {
            var serializer = new XmlSerializer(objToDeSerialize.GetType());

            object result;
            using (TextReader reader = new StringReader(XMLContent))
                result = serializer.Deserialize(reader);
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static string ObjectToJson(object _Input_Object)
    {
        JavaScriptSerializer JSS = new JavaScriptSerializer();
        //The maximum length of JSON strings. The default is 2097152 characters, which is equivalent to 4 MB of Unicode string data. 
        //2097152 = 4MB.  (2097152 * 5 as 4MB*5) = 20MB. So 10485760 = 20MB
        JSS.MaxJsonLength = 10485760; // 20MB Max data travel;
        string Json_String = JSS.Serialize(_Input_Object);
        return Json_String;
    }

}