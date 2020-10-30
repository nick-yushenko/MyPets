using Microsoft.AspNet.Identity;
using MrSimonAcademy2.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace MrSimonAcademy2.Controllers
{
    public class Service
    {
        public void SendEmail(ContactViewModel customer)
        {
            // Метод для отправки Email сообщений
            // Сбор заявок на обучение. Получатель и отправитель не меняются 

            // Отправитель - адрес почты и имя отправителя
            MailAddress From = new MailAddress(ConfigurationManager.AppSettings["Email"].ToString(), "Mr. Simon Academy");

            // Получатель
            MailAddress To = new MailAddress("mr.simon.academy@mail.ru");

            // Объект сообщения
            MailMessage msg = new MailMessage(From, To);

            // Тема сообщения 
            msg.Subject = "Заявка с сайта Mr. Simon Academy";

            // Разрешение на использование HTML-тегов в письме
            msg.IsBodyHtml = true;

            // Текст письма 
            msg.Body = "Новая заявка на сайте Mr. Simon Academy " + "<br> Имя: " + customer.Name + "<br> Телефон: " + customer.Phone 
                + "<br> Язык: " + customer.Language + "<br> Курс: " + customer.CourseName;

            // адресс SMTP-сервера и порт, с которого идет отправка 
            SmtpClient smtpClient = new SmtpClient("mail.hosting.reg.ru", Convert.ToInt32(587));

            smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Email"].ToString(),
                ConfigurationManager.AppSettings["Password"].ToString());
            smtpClient.EnableSsl = true;
            smtpClient.Send(msg);

        }


        public Task SendVerifyCodeAsync(string email, string subject, string code)
        {
            // Ассинхронный метод для отправки Email сообщений
            // Подтверждение Email адреса

            return Task.Factory.StartNew(() =>
            {
                IdentityMessage message = new IdentityMessage();
                message.Destination = email;
                message.Body = code;
                message.Subject = subject;

                SendVerifyCode(message);
            });



        }

        private void SendVerifyCode(IdentityMessage message)
        {
            // Метод для отправки Email сообщений
            // Подтверждение Email адреса

            // Отправитель - адрес почты и имя отправителя
            MailAddress From = new MailAddress(ConfigurationManager.AppSettings["Email"].ToString(), "Mr. Simon Academy");

            // Получатель
            MailAddress To = new MailAddress(message.Destination);

            // Объект сообщения
            MailMessage msg = new MailMessage(From, To);

            // Тема сообщения 
            msg.Subject = message.Subject;

            // Разрешение на использование HTML-тегов в письме
            msg.IsBodyHtml = true;


            // Текст письма 
            //msg.Body = "";

            //msg.Body = "Вы были добавлены на сайт Mr. Simon Academy. Перейдите по <a href=\"" + message.Body + "\"> этой ссылке</a>, " +
            //    "чтобы попасть в личный кабинет <br> Если ссылка не работает, скопируйте ее и вставьте в адресную строку Вашего браузер: "
            //    + "<a href =\"" + message.Body + "\"> \"" + message.Body + "\"  </a>, ";


            msg.Body = "<!DOCTYPE html>" +
                "<html xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" style=\"width:100%;font-family:arial," +
                "'helvetica neue', helvetica, sans-serif;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;padding:0;Margin:0\">" +
                "<head><meta charset=\"UTF-8\">" +
                "<meta content=\"width=device-width, initial-scale=1\" name=\"viewport\">" +
                "<meta name=\"x-apple-disable-message-reformatting\" >" +
                "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">" +
                "<link href=\"https://fonts.googleapis.com/css?family=Open+Sans:400,400i\" rel=\"stylesheet\"> </head> " +

                "<body>"  +
                " <div style=\"width: 100%; max-width: 600px; padding: 0 20px; margin: auto;\">" +
                "<h2 style=\"font-family: 'Open Sans'; font-size: 24px; font-weight: normal;\"> Поздравляем! Вы были зарегистрированы на сайте mrsimon.ru и теперь у Вас есть неограниченный доступ к знаниям. </h2>" +
                "<img src=\"https://mrsimon.ru/e_img/logo.jpg\" alt=\"Mr. Simon Academy\" style=\"width: 100%;\"> " +
                "<p style=\"font-family: 'Open Sans'; font-size: 16px; font-weight: normal; margin: 15px 0;\"> Пожалуйста, перейдите по ссылке, чтобы подтвердить E-mail.</p> " +
                "<a href=\"" + message.Body + "\" target = \"_blank\" style =\"margin: auto;text-decoration:none;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;" +
                "mso-line-height-rule:exactly;font-family:'open sans';font-size:20px;color:#FFFFFF;border-style:solid;border-color:#106E83;border-width:10px 40px;" +
                "display:block;background:#106E83;border-radius:40px;font-weight:normal;font-style:normal;line-height:24px;width:max-content;text-align:center\"> Ссылка </a>" +


                "<p style=\"font-family: 'Open Sans'; font-size: 16px; font-weight: normal; margin: 15px 0;\"> Если Вы получили письмо ошибочно, просто проигнорируйте его. </p></div></body></html>";

            // адресс SMTP-сервера и порт, с которого идет отправка 
            SmtpClient smtpClient = new SmtpClient("mail.hosting.reg.ru", Convert.ToInt32(587));
            smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Email"].ToString(),
                ConfigurationManager.AppSettings["Password"].ToString());
            smtpClient.EnableSsl = true;
            smtpClient.Send(msg);
        }
    }
}