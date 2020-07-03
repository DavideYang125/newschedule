using System;
using System.Collections.Generic;
using System.Text;
using MailKit.Net.Smtp;
using MimeKit;

namespace Schedule.ToolKit
{
    public class MailTool
    {
        public static string _host;
        public static int _port;
        public static string _userName;
        public static string _password;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MailTool(string host, int port, string userName, string password)
        {
            _host = host;
            _port = port;
            _userName = userName;
            _password = password;
        }

        public MailTool()
        {
            _host = "smtp.163.com";
            _port = 25;
            _userName = "dayang_techmail@163.com";
            _password = "";
        }

        public void Send(string msg)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("yangww", "dayang_techmail@163.com"));
            mimeMessage.To.Add(new MailboxAddress("yangww", "1483722265@qq.com"));
            mimeMessage.Body = new TextPart("plain") { Text = msg };
            using (SmtpClient client = new SmtpClient())
            {
                //Smtp服务器
                client.Connect(_host, _port, false);

                //登录，发送
                client.Authenticate(_userName, _password);

                client.Send(mimeMessage);

                //断开
                client.Disconnect(true);
            }
        }
    }
}
