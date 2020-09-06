using System;

namespace Jcsms
{
    /// <summary>
    /// 通用返回
    /// </summary>
    public class ResultBase<T>
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
        /// 状态码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; set; }
    }
    /// <summary>
    /// 发送短信验证码结果
    /// </summary>
    public class SendSmsCodeResult : ResultBase<object>
    {
        /// <summary>
        /// 缓存内容
        /// </summary>
        public SmsCodeCacheItem CacheItem { get; set; }
        /// <summary>
        /// 接口发送结果
        /// </summary>
        public SmsSentResult SmsSentResult { get; set; }
    }
    /// <summary>
    /// 查询短信余额结果
    /// </summary>
    public class QueryBalanceResult
    {
        /// <summary>
        /// 余额
        /// </summary>
        public int Balance { get; set; }
        /// <summary>
        /// 查询时间
        /// </summary>
        public DateTime? Time { get; set; }
    }
}