using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Jcsms.Example.AspNetCore.Controllers
{
    public class TestSmsController : Controller
    {
        private readonly SmsService _smsService;

        public TestSmsController(SmsService smsService)
        {
            _smsService = smsService;
        }


        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public IActionResult Send(string phoneNumber, string code)
        {
            var ret = _smsService.SendSmsCode("13997882568", "user:register");

            return View();
        }
    }
}