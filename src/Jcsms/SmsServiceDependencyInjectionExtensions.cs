using EasyCaching.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jcsms
{
    /// <summary>
    /// 短信服务的依赖注入扩展
    /// </summary>
    public static class SmsServiceDependencyInjectionExtensions
    {
        private static readonly SmsServiceOptions option = new SmsServiceOptions();

        /// <summary>
        /// 添加短信服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        public static void AddSmsService(this IServiceCollection services, Action<SmsServiceOptions> options)
        {
            options(option);

            services.AddSingleton<SmsService>(new SmsService(option));
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSmsService(this IApplicationBuilder app)
        {

            var ic = app.ApplicationServices.GetService<IEasyCachingProvider>();
            option.Cache = ic;
            return app;
        }
    }
}
