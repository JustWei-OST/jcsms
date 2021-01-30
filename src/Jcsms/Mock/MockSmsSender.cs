using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jcsms.Mock
{
    public class MockSmsSender : ISmsSender
    {
        public string Provider => "模拟短信服务";
        private int v;
        public MockSmsSender(int code)
        {
            this.v = code;
        }
        public ResultBase<QueryBalanceResult> GetBalance()
        {
            return new ResultBase<QueryBalanceResult>()
            {
                Code = 0,
                Message = "OK",
                Succeed = true,
                Data = new QueryBalanceResult()
                {
                    Balance = 9999999,
                    Time = DateTime.Now
                },
            };
        }

        public SmsSentResult Send(IEnumerable<string> phoneNumbers, string templateCode, object smsData)
        {

            return new SmsSentResult()
            {
                Succeed = true
            };

        }

        public SmsSentResult SendCode(string phoneNumber, string code, string scope, object smsData)
        {
            return new SmsSentResult()
            {
                Succeed = true
            };
        }

        public SmsSentResult SendCode(string phoneNumber, string code, string scope, string content, object smsData)
        {
            return new SmsSentResult()
            {
                Succeed = true
            };
        }

        public SmsSentResult SendFreeMessage(IEnumerable<string> phoneNumbers, string message)
        {
            return new SmsSentResult()
            {
                Succeed = true
            };
        }
    }
}
