using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Jcsms.ChuangLan253
{
    /// <summary>
    /// 短信帮助类
    /// <para>官方文档地址: https://zz.253.com/api_doc </para>
    /// </summary>
    public class SmsHelper
    {

        /// <summary>
        ///
        /// </summary>
        /// <param name="url"></param>
        /// <param name="jsonBody"></param>
        /// <returns></returns>
        public static SmsSentResult SendSMS(string url, string jsonBody)
        {
            try
            {
                SendSMSReturnViewModel result;
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                // Create NetworkCredential Object
                NetworkCredential admin_auth = new NetworkCredential("username", "password");

                // Set your HTTP credentials in your request header
                httpWebRequest.Credentials = admin_auth;

                // callback for handling server certificates
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonBody);
                    streamWriter.Flush();
                    streamWriter.Close();
                    HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = JsonConvert.DeserializeObject<SendSMSReturnViewModel>(streamReader.ReadToEnd());
                    }
                }


                return new SmsSentResult()
                {
                    Succeed = result.code == "0",
                    Code = result.code,
                    BizId = result.msgId,
                    Message = result.errorMsg
                };
            }
            catch (Exception ex)
            {
                return new SmsSentResult()
                {
                    Succeed = false,
                    //Code = "-10",
                    Message = ex.Message
                };

            }
        }
    }

}