using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Jcsms.Aliyun
{
    /// <summary>
    /// 阿里云 短信发送器
    /// </summary>
    public class AliyunSmsSender : ISmsSender
    {
        const string product = "Dysmsapi";//产品名称:云通信短信API产品,开发者无需替换
        const string domain = "dysmsapi.aliyuncs.com";//产品域名,开发者无需替换

        /// <summary>
        ///
        /// </summary>
        public string AccessKeyId { get; set; }
        /// <summary>
        ///
        /// </summary>
        public string AccessKeySecret { get; set; }
        /// <summary>
        /// 短信签名
        /// </summary>
        public string SignName { get; set; }

        /// <summary>
        /// 提供商标识
        /// </summary>
        public string Provider => "阿里云短信服务";


        /// <summary>
        /// 发送模板短信
        /// </summary>
        /// <param name="phoneNumbers">接收号码集合</param>
        /// <param name="templateCode">使用的模板编码</param>
        /// <param name="smsData">短信数据包 类型:Dictionary`string,string`</param>
        public SmsSentResult Send(IEnumerable<string> phoneNumbers, string templateCode, object smsData)
        {

            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", AccessKeyId, AccessKeySecret);
            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsResponse response = null;
            try
            {
                SendSmsRequest request = new SendSmsRequest
                {
                    SignName = "中税华通",//必填:短信签名-可在短信控制台中找到

                    //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为1000个手机号码,
                    // --- 批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                    PhoneNumbers = string.Join(',', phoneNumbers),
                    TemplateCode = templateCode, //必填:短信模板-可在短信控制台中找到
                    OutId = Guid.NewGuid().ToString(),//可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                    TemplateParam = smsData == null ? "" : JsonConvert.SerializeObject(smsData),
                };

                //请求失败这里会抛ClientException异常
                response = acsClient.GetAcsResponse(request);

                SmsSentResult sentResult = new SmsSentResult()
                {
                    Succeed = response.Code == "OK",
                    Code = response.Code,
                    BizId = response.BizId,
                    Message = response.Message,
                    RequestId = response.RequestId
                };
                return sentResult;
            }
            catch (Exception e)
            {
                return new SmsSentResult()
                {
                    Succeed = false,
                    Code = e.HResult.ToString(),
                    Message = e.Message,
                };
            }

        }

        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="phoneNumber">手机号码</param>
        /// <param name="vcode">要发送的验证码</param>
        /// <param name="scope">场景标识</param>
        /// <param name="smsData">短信数据包</param>
        /// <returns></returns>
        public SmsSentResult SendCode(string phoneNumber, string vcode, string scope, object smsData)
        {
            var token = Guid.NewGuid().ToString();
            var sentRet = Send(new List<string> { phoneNumber }, "SMS_103345009", new { code = vcode });
            //TODO: 如果要根据不同场景发送不同的模板短信,在这里处理
            return sentRet;

            //switch (scope.ToLower())
            //{

            //    //TODO:作用范围标识对应的短信模板,要可以配置才行.具体业务里要求使用哪个标识怎么处理?
            //    case "user:register":
            //        var token = Guid.NewGuid().ToString();
            //        var sentRet = Send(new List<string> { phoneNumber }, "SMS_103345009", new { code = vcode });
            //        if (sentRet.Succeed)
            //        {
            //            ret = SimpleStatusResult.Succeeded(new
            //            {
            //                Token = token,
            //                Expire = DateTime.Now.AddMinutes(5)
            //            });

            //            SetSmsCodeCache(new SmsCodeCacheItem
            //            {
            //                PhoneNumber = phoneNumber,
            //                Code = vcode,
            //                Scope = scope,
            //                SendAt = DateTime.Now,
            //                Token = token
            //            });
            //        }
            //        else
            //        {
            //            // ret.Code = sentRet.Code;
            //            ret.Text = sentRet.Message;
            //            ret.Data = sentRet;
            //        }
            //        break;
            //    //case "user:maketrade":
            //    //    break;
            //    default:
            //        ret = SimpleStatusResult.Error(null, "不被支持的短信目标");
            //        break;
            //}

        }
    }
}
