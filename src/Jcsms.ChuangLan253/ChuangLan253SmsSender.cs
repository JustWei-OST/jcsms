using System;
using System.Collections.Generic;

namespace Jcsms.ChuangLan253
{
    /// <summary>
    /// 创蓝253 短信发送器
    /// </summary>
    public class ChuangLan253SmsSender : ISmsSender
    {
        /// <summary>
        /// 提供商标识
        /// </summary>
        public string Provider => "创蓝253短信";

        /// <summary>
        /// 发送模板短信
        /// </summary>
        /// <param name="phoneNumber">接收号码集合</param>
        /// <param name="templateCode">使用的模板编码</param>
        /// <param name="smsData">短信数据包</param>
        public SmsSentResult Send(IEnumerable<string> phoneNumbers, string templateCode, object smsData)
        {
            throw new NotImplementedException();

        }

        public SmsSentResult SendCode(string phoneNumber, string code, string scope, object smsData)
        {
            throw new NotImplementedException();
        }
    }
}
