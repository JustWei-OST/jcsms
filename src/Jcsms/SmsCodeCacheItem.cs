using System;

namespace Jcsms
{
    /// <summary>
    /// 短信验证码缓存项
    /// </summary>
    public class SmsCodeCacheItem
    {
        /// <summary>
        /// 标记
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendAt { get; set; }
        /// <summary>
        /// 发送的验证码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 验证码作用标识
        /// </summary>
        public string Scope { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime Expire { get; set; }
    }
}