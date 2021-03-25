using Flurl.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jcsms.Mas
{
    /// <summary>
    /// 移动云MAS短信发送器
    /// </summary>
    public class MasSender : ISmsSender
    {
        /// <summary>
        /// 
        /// </summary>
        public string Provider => "移动云MAS";
        /// <summary>
        /// 帐号用户名
        /// </summary>
        public string ApId { get; set; }
        /// <summary>
        /// 企业名称
        /// </summary>
        public string EcName { get; set; }
        /// <summary>
        /// 签名编码
        /// </summary>
        public string Sign { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 发送接口地址
        /// </summary>
        public string SubmitUrl { get; set; } = "http://112.35.1.155:1992/sms/norsubmit";

        public ResultBase<QueryBalanceResult> GetBalance()
        {
            return new ResultBase<QueryBalanceResult>()
            {
                Code = -1,
                Succeed = false,
                Message = "暂不支持余额查询",
                Data = new QueryBalanceResult()
                {
                    Balance = -1
                }
            };
        }

        public SmsSentResult Send(IEnumerable<string> phoneNumbers, string templateCode, object smsData)
        {
            throw new NotImplementedException();
        }

        public SmsSentResult SendCode(string phoneNumber, string code, string scope, object smsData)
        {
            var content = $"亲爱的用户，您本次的验证码为{code}，五分钟内有效，若非本人操作请忽略。";
            return SendCode(phoneNumber, code, scope, content, smsData);
        }

        public SmsSentResult SendCode(string phoneNumber, string code, string scope, string content, object smsData)
        {
            return SendFreeMessage(new List<string>() { phoneNumber }, content);
        }

        public SmsSentResult SendFreeMessage(IEnumerable<string> phoneNumbers, string message)
        {
            var phones = string.Join(",", phoneNumbers);

            var _mac = ToMd5($"{EcName}{ApId}{Password}{phones}{message}{Sign}").ToLower();
            var model = new
            {
                ecName = EcName,
                apId = ApId,
                mobiles = phones,
                content = message,
                sign = Sign,
                addSerial = "",
                mac = _mac
            };

            var data = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model)));
            try
            {
                var ret = SubmitUrl
                        .PostJsonAsync(data)
                        .ReceiveJson<MasSendRetModel>()
                        .Result;


                return new SmsSentResult()
                {
                    Succeed = ret.success,
                    BizId = ret.mgsGroup,
                    Code = ret.rspcod,
                    Message = ret.message,
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

        private string ToMd5(string str)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = Encoding.UTF8.GetBytes(str);
            bs = md5.ComputeHash(bs);
            StringBuilder s = new StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToUpper());
            }
            string HashCode = s.ToString();
            return HashCode;
        }


    }

    internal class MasSendRetModel
    {
#pragma warning disable IDE1006 // 命名样式
        /// <summary>
        /// 响应状态
        /// </summary>
        public string rspcod { get; set; }
        /// <summary>
        /// 消息批次号，由云MAS平台生成，用于关联短信发送请求与状态报告，注：若数据验证不通过，该参数值为空
        /// </summary>
        public string mgsGroup { get; set; }
        /// <summary>
        /// 数据校验结果
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// 返回消息 （对应文档解析的，非接口直接返回）
        /// </summary>
        public string message
        {
            get
            {
                switch (rspcod)
                {
                    case "IllegalMac":
                        return "mac校验不通过";
                    case "IllegalSignId":
                        return "无效的签名编码";
                    case "InvalidMessage":
                        return "非法消息，请求数据解析失败";
                    case "InvalidUsrOrPwd":
                        return "未匹配到对应的签名信息";
                    case "success":
                        return "数据验证通过";
                    case "TooManyMobiles":
                        return "手机号数量超限";
                    default:
                        return "未知";
                }
            }
        }

#pragma warning restore IDE1006 // 命名样式

    }
}