namespace Jcsms
{
    /// <summary>
    /// 短信发送结果返回
    /// </summary>
    public class SmsSentResult
    {
        /// <summary>
        /// 指示调用发送接口是否成功,(根据不同平台的状态码匹配的)
        /// </summary>
        public bool Succeed { get; set; }
        /// <summary>
        /// 接口返回的原始状态码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 接口返回的原始消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 发送回执ID,可根据该ID查询具体的发送状态
        /// </summary>
        public string BizId { get; set; }
        /// <summary>
        /// 请求ID
        /// </summary>
        public string RequestId { get; set; }
    }
}
