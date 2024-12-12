using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    internal class EmailLogic
    {
        public static void SendCriticalLogEmail(string recipientEmail, string username, string reactivationLink)
        {
            string fromEmail = "sddrouet.dev@gmail.com"; // Tu dirección de correo electrónico
            string subject = "Alerta Crítica: Cuenta Bloqueada por Intentos Fallidos";
            string body = $"<h3>Alerta Crítica</h3>" +
                          $"<p>Se ha detectado que la cuenta con el nombre de usuario <strong>{username}</strong> ha sido bloqueada debido a múltiples intentos fallidos de inicio de sesión.</p>" +
                          "<p>Por favor, revise la situación y tome las medidas necesarias.</p>" +
                          $"<br/><br/>Por favor, haz clic en el siguiente enlace para reactivar la cuenta: <a href='{reactivationLink}'>Reactivar cuenta</a><br/><br/>" +
                          "<p>Gracias por tu atención.</p>";

            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com") // Configura tu servidor SMTP
                {
                    Port = 587, // Puerto SMTP (depende del servidor)
                    Credentials = new NetworkCredential("sddrouet.dev@gmail.com", "kbuj kiah amsl pqij"), // Credenciales del correo
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(recipientEmail);
                mailMessage.To.Add("sddrouet.dev@gmail.com"); // Agregar tu correo para recibir copia

                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                // Manejo de errores, podrías registrar el error si no puedes enviar el correo
                Console.WriteLine("Error al enviar correo: " + ex.Message);
            }
        }

        public static void SendConfirmationEmail(string recipientEmail, string confirmationLink)
        {
            string fromEmail = "sddrouet.dev@gmail.com"; // Tu dirección de correo electrónico
            string subject = "Confirmación de Activación de Cuenta";
            string body = $"Hola, <br/><br/>Por favor, haz clic en el siguiente enlace para activar tu cuenta de Sales2024: <a href='{confirmationLink}'>Activar cuenta</a><br/><br/>Gracias.";

            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com") // Configura tu servidor SMTP
                {
                    Port = 587, // Puerto SMTP (depende del servidor)
                    Credentials = new NetworkCredential("sddrouet.dev@gmail.com", "kbuj kiah amsl pqij"), // Credenciales del correo
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(recipientEmail);

                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                // Manejo de errores, podrías registrar el error si no puedes enviar el correo
                Console.WriteLine("Error al enviar correo: " + ex.Message);
            }
        }
    }
}
