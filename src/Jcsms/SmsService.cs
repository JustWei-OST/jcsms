using EasyCaching.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jcsms
{
    /// <summary>
    /// 短信服务
    /// </summary>
    public class SmsService
    {
        private IEasyCachingProvider Cache => options.Cache;
        private readonly SmsServiceOptions options;
        private ISmsSender _defaulfSmsSender;
        /// <summary>
        /// 默认短信发送器
        /// </summary>
        private ISmsSender DefaulfSmsSender
        {
            get
            {
                if (_defaulfSmsSender == null)
                {
                    _defaulfSmsSender = options.SmsSenders.FirstOrDefault() ?? throw new Exception("没有配置任何短信发送器");
                }
                return _defaulfSmsSender;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="serviceOptions"></param>
        public SmsService(
            SmsServiceOptions serviceOptions
            )
        {
            options = serviceOptions;

        }
        /// <summary>
        /// 发送短信验证码 (使用默认短信发送器)
        /// </summary>
        /// <param name="phoneNumber">手机号码</param>
        /// <param name="scope">使用范围</param>
        /// <returns></returns>
        public SendSmsCodeResult SendSmsCode(string phoneNumber, string scope)
            => SendSmsCode(phoneNumber, scope, DefaulfSmsSender);

        /// <summary>
        /// 发送短信验证码 (使用指定的短信发送器)
        /// </summary>
        /// <param name="phoneNumber">手机号码</param>
        /// <param name="scope">使用范围</param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public SendSmsCodeResult SendSmsCode(string phoneNumber, string scope, string provider)
            => SendSmsCode(phoneNumber, scope, options.SmsSenders.Where(p => p.Provider == provider).Single());

        /// <summary>
        /// 发送短信验证码 (使用指定的短信发送器)
        /// </summary>
        /// <param name="phoneNumber">手机号码</param>
        /// <param name="scope">使用范围</param>
        /// <param name="smsSender"></param>
        /// <returns></returns>
        public SendSmsCodeResult SendSmsCode(string phoneNumber, string scope, ISmsSender smsSender)
        {
            Random rad = new Random();
            var vcode = rad.Next(1000, 9999).ToString();

            if (CheckFrequency(phoneNumber, scope))//限速
            {
                return new SendSmsCodeResult()
                {
                    Succeed = false,
                    Message = "短信请求频率过快,请稍候再试"
                };
            }
            else
            {
                var ret = new SendSmsCodeResult();
                var token = Guid.NewGuid().ToString();
                var sentRet = smsSender.SendCode(phoneNumber, vcode, scope, null);
                var now = DateTime.Now;
                SmsCodeCacheItem cacheItem = null;
                if (sentRet.Succeed)
                {
                    ret = new SendSmsCodeResult()
                    {
                        Succeed = true,
                        Message = "OK",
                    };

                    cacheItem = new SmsCodeCacheItem
                    {
                        PhoneNumber = phoneNumber,
                        Code = vcode,
                        Scope = scope,
                        SendAt = now,
                        Token = token,
                        Expire = now.AddMinutes(options.SaveCodeSeconds)
                    };

                    //设置缓存
                    SetSmsCodeCache(cacheItem);
                }
                else
                {
                    ret.Message = sentRet.Message;
                }

                ret.SmsSentResult = sentRet;
                ret.CacheItem = cacheItem;
                return ret;
            }

        }

        /// <summary>
        /// 向指定的号码集合发送自写内容短信,注意,自写注意可能会被运营商屏蔽
        /// </summary>
        /// <param name="phoneNumbers"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public SmsSentResult SendFreeMessage(IEnumerable<string> phoneNumbers, string message)
         => DefaulfSmsSender.SendFreeMessage(phoneNumbers, message);
        /// <summary>
        /// 检查短信发送频率,是否被流控限制发送了
        /// </summary>
        /// <param name="phoneNumber">手机号码</param>
        /// <param name="scope">使用范围</param>
        /// <returns>被限制了 返回true,否则返回false</returns>
        private bool CheckFrequency(string phoneNumber, string scope)
        {
            if (Cache == null)
            {
                throw new Exception("没有配置缓存服务");
            }
            var ce = Cache.Get<SmsCodeCacheItem>(options.GetSmsCodeCacheKey(phoneNumber, scope));
            var ret = ce.HasValue && (DateTime.Now - ce.Value.SendAt).TotalSeconds <= options.SendCodeIntervals;
            return ret;
        }

        /// <summary>
        /// 验证短信验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool VerifySmsCode(VerifySmsCodeViewModel model)
        {
            var ce = Cache.Get<SmsCodeCacheItem>(options.GetSmsCodeCacheKey(model.PhoneNumber, model.Scope));
            if (ce.HasValue)
            {
                SmsCodeCacheItem smsCodeCacheItem = ce.Value;
                var timeSpan = DateTime.Now - smsCodeCacheItem.SendAt;

                if (timeSpan.TotalMinutes <= options.SaveCodeSeconds && model.Code == smsCodeCacheItem.Code)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 设置验证码缓存
        /// </summary>
        /// <param name="cacheItem">缓存值</param>
        private void SetSmsCodeCache(SmsCodeCacheItem cacheItem)
        {
            var key = options.GetSmsCodeCacheKey(cacheItem.PhoneNumber, cacheItem.Scope);

            Cache.Set(key, cacheItem, TimeSpan.FromSeconds(options.SaveCodeSeconds));

        }

        /// <summary>
        /// 移除短信验证码缓存
        /// </summary>
        /// <param name="model"></param>
        public void RemoveSmsCodeCache(VerifySmsCodeViewModel model)
        {
            Cache.Remove(options.GetSmsCodeCacheKey(model.PhoneNumber, model.Scope));
        }


    }
}
