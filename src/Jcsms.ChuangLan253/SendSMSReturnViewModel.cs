using System;

namespace Jcsms.ChuangLan253
{
    public class SendSMSReturnViewModel
    {
        /// <summary>
        /// ״̬��
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// ʧ������
        /// </summary>
        public int failNum { get; set; }

        /// <summary>
        /// �ɹ�����
        /// </summary>
        public int successNum { get; set; }


        /// <summary>
        /// ��ϢId
        /// </summary>
        public string msgId { get; set; }

        /// <summary>
        /// ��Ӧʱ��
        /// </summary>
        public string time { get; set; }

        /// <summary>
        /// ������Ϣ(�ɹ�Ϊ��)
        /// </summary>
        public string errorMsg { get; set; }
    }
}