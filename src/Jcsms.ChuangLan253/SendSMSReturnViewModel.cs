using System;

namespace Jcsms.ChuangLan253
{
    public class SendSMSReturnViewModel
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 失败条数
        /// </summary>
        public int failNum { get; set; }

        /// <summary>
        /// 成功条数
        /// </summary>
        public int successNum { get; set; }


        /// <summary>
        /// 消息Id
        /// </summary>
        public string msgId { get; set; }

        /// <summary>
        /// 响应时间
        /// </summary>
        public string time { get; set; }

        /// <summary>
        /// 错误消息(成功为空)
        /// </summary>
        public string errorMsg { get; set; }
    }
}