using Flurl.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jcsms.ChuangLan253
{
    /// <summary>
    /// 创蓝253 短信发送器
    /// </summary>
    public class ChuangLan253SmsSender : ISmsSender
    {
        /// <summary>
        /// 帐号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 发送短信的接口地址
        /// </summary>
        public string ApiUrl_Send { get; set; } = "http://smssh1.253.com/msg/variable/json";
        /// <summary>
        /// 提供商标识
        /// </summary>
        public string Provider => "创蓝253短信";
        /// <summary>
        /// 短信验名
        /// </summary>
        public string SignName { get; set; }
        /// <summary>
        /// 获取短信余额
        /// </summary>
        /// <returns></returns>
        public ResultBase<QueryBalanceResult> GetBalance()
        {
            ResultBase<QueryBalanceResult> r;
            if (Account != null && Password != null)
            {
                var ret = "http://smssh1.253.com/msg/balance/json"
                        .PostJsonAsync(new
                        {
                            account = Account,
                            password = Password
                        })
                        .ReceiveJson<ClGetBalanceRetViewModel>()
                        .Result;

                r = new ResultBase<QueryBalanceResult>()
                {
                    Succeed = ret.code == 0,
                    Code = ret.code,
                    Message = ret.errorMsg,
                    Data = new QueryBalanceResult()
                    {
                        Balance = int.Parse(ret.balance),
                        Time = PrTime(ret.time)
                    }
                };

            }
            else
            {
                r = new ResultBase<QueryBalanceResult>()
                {
                    Succeed = false,
                    Code = -1,
                    Message = "未配置短信帐号"
                };
            }
            return r;
        }

        private DateTime? PrTime(string time)
        {
            try
            {
                return new DateTime(
                    int.Parse(time.Substring(0, 4)),
                    int.Parse(time.Substring(4, 2)),
                    int.Parse(time.Substring(6, 2)),
                    int.Parse(time.Substring(8, 2)),
                    int.Parse(time.Substring(10, 2)),
                    int.Parse(time.Substring(12, 2))
                    );
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 发送模板短信
        /// </summary>
        /// <param name="phoneNumbers">接收号码集合</param>
        /// <param name="templateCode">使用的模板编码</param>
        /// <param name="smsData">短信数据包</param>
        public SmsSentResult Send(IEnumerable<string> phoneNumbers, string templateCode, object smsData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="phoneNumber">手机号码</param>
        /// <param name="code">要发送的验证码</param>
        /// <param name="scope">场景标识</param>
        /// <param name="smsData">短信数据包</param>
        /// <returns></returns>
        public SmsSentResult SendCode(string phoneNumber, string code, string scope, object smsData)
        {
            //文档: https://zz.253.com/v5.html#/api_doc
            // 此接口一次可提交不超过1000个手机号码。

            var request = new
            {
                //用户在253云通讯平台上申请的API账号
                account = Account,
                //用户在253云通讯平台上申请的API账号对应的API密钥
                password = Password,
                //是否需要状态报告（默认为false）（选填参数）
                report = "true",
                //短信内容。长度不能超过536个字符
                msg = $"【{SignName}】" + "亲爱的用户，您的短信验证码为{$var},五分钟内有效，若非本人操作请忽略。",
                @params = $"{phoneNumber},{code}",
            };

            string jsonBody = JsonConvert.SerializeObject(request);

            var result = SmsHelper.SendSMS(ApiUrl_Send, jsonBody);

            return result;

        }

        public SmsSentResult SendCode(string phoneNumber, string code, string scope, string content, object smsData)
        {
            return SendFreeMessage(new List<string> { phoneNumber }, content);
        }

        /// <summary>
        /// 向指定的号码集合发送自写内容短信,注意,自写注意可能会被运营商屏蔽
        /// </summary>
        /// <param name="phoneNumbers"></param>
        /// <param name="message"></param>

        public SmsSentResult SendFreeMessage(IEnumerable<string> phoneNumbers, string message)
        {
            //检查内容长度
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new Exception("短信内容不能为空");
            }
            else if (message.Length + 2 + SignName.Length > 536)
            {
                throw new Exception("短信内容。长度不能超过536个字符");
            }
            else if (phoneNumbers.Count() == 0)
            {
                throw new Exception("最少需要指定一个手机号码");
            }
            else if (phoneNumbers.Count() > 1000)
            {
                throw new Exception("不可超过1000个手机号码");
            }

            var request = new
            {
                //用户在253云通讯平台上申请的API账号
                account = Account,
                //用户在253云通讯平台上申请的API账号对应的API密钥
                password = Password,
                //是否需要状态报告（默认为false）（选填参数）
                report = "true",
                //手机号码。多个手机号码使用英文逗号分隔
                phone = string.Join(',', phoneNumbers),
                //短信内容。长度不能超过536个字符
                msg = $"【{SignName}】{message}"
            };

            string jsonBody = JsonConvert.SerializeObject(request);

            var url = "http://smssh1.253.com/msg/send/json";
            var result = SmsHelper.SendSMS(url, jsonBody);

            return result;
        }
    }

    /// <summary>
    /// 创蓝查询短信余额返回视图模型
    /// </summary>
    internal class ClGetBalanceRetViewModel
    {
        public int code { get; set; }
        public string balance { get; set; }
        public string time { get; set; }
        public string errorMsg { get; set; }
    }

}
