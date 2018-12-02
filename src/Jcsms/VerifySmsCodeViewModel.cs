using System.ComponentModel.DataAnnotations;

namespace Jcsms
{
    /// <summary>
    /// 验证短信验证码视图模型
    /// </summary>
    public class VerifySmsCodeViewModel
    {
        /// <summary>
        /// 使用范围
        /// </summary>
        [Required]
        public string Scope { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [Required]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 标记
        /// </summary>
        [Required]
        public string Token { get; set; }
        /// <summary>
        /// 要验证的短信验证码
        /// </summary>
        [Required]
        public string Code { get; set; }

    }
}