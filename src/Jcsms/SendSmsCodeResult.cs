namespace Jcsms
{
    /// <summary>
    /// 发送短信验证码结果
    /// </summary>
    public class SendSmsCodeResult
    {
        /// <summary>
        /// 指示是否调用成功
        /// </summary>
        public bool Succeed { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 缓存内容
        /// </summary>
        public SmsCodeCacheItem CacheItem { get; set; }
        /// <summary>
        /// 接口发送结果
        /// </summary>
        public SmsSentResult SmsSentResult { get; set; }
    }
}