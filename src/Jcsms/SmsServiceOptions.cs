using EasyCaching.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jcsms
{
    /// <summary>
    /// 短信服务选项
    /// </summary>
    public class SmsServiceOptions
    {
        /// <summary>
        /// 验证码过期时间(单位:秒,默认5分钟)
        /// </summary>
        public int SaveCodeSeconds { get; set; } = 5 * 60;
        /// <summary>
        /// 验证码请求间隔时长(单位:秒,默认60秒)
        /// </summary>
        public int SendCodeIntervals { get; set; } = 60;
        /// <summary>
        /// 缓存
        /// </summary>
        public IEasyCachingProvider Cache { get; set; }
        /// <summary>
        /// 短信发送器
        /// </summary>
        public readonly List<ISmsSender> SmsSenders = new List<ISmsSender>();

    }
}
