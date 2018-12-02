using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jcsms
{
    /// <summary>
    /// 短信发送器接口
    /// </summary>
    public interface ISmsSender
    {
        /// <summary>
        /// 提供商标识
        /// </summary>
        string Provider { get; }
        /// <summary>
        /// 发送模板短信
        /// </summary>
        /// <param name="phoneNumber">接收号码集合</param>
        /// <param name="templateCode">使用的模板编码</param>
        /// <param name="smsData">短信数据包</param>
        SmsSentResult Send(IEnumerable<string> phoneNumbers, string templateCode, object smsData);
        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="phoneNumber">手机号码</param>
        /// <param name="code">要发送的验证码</param>
        /// <param name="scope">场景标识</param>
        /// <param name="smsData">短信数据包</param>
        /// <returns></returns>
        SmsSentResult SendCode(string phoneNumber, string code, string scope, object smsData);
    }
}