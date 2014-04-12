using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using NorthWest.Domain.Abstract;
using NorthWest.Domain.Entities;
using System.Net;

namespace NorthWest.Domain.Concrete
{
    public class EmailSettings {
         public string MailToAddress = "mingyuan7726@gmail.com";
         public string MailFromAddress = "mingyuan7725@gmail.com";
         public bool UseSsl = true;
         public string Username = "mingyuan7725@gmail.com";
         public string Password = "***********";
         public string ServerName = "smtp.o2.ie";
         public int ServerPort = 25;
         public bool WriteAsFile = false;
         public string FileLocation = @"e:\NorthWestOrders";
    }
    public class EmailOrderProcessor :IOrderProcessor {
         private EmailSettings emailSettings;
         public EmailOrderProcessor(EmailSettings settings) {
         emailSettings = settings;
    }
    public void ProcessOrder(Cart cart, ShippingDetails shippingInfo) {
         using (var smtpClient = new SmtpClient()) {
         smtpClient.EnableSsl = emailSettings.UseSsl;
         smtpClient.Host = emailSettings.ServerName;
         smtpClient.Port = emailSettings.ServerPort;
         smtpClient.UseDefaultCredentials = false;
         smtpClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);
         if (emailSettings.WriteAsFile) {
             smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
             smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
             smtpClient.EnableSsl = false;
         }
         StringBuilder body = new StringBuilder()
         .AppendLine("A new order has been submitted")
         .AppendLine("---")
         .AppendLine("Items:");
         foreach (var line in cart.Lines) {
             var subtotal = line.Product.Price * line.Quantity;
             body.AppendFormat("{0} x {1} (subtotal: {2:c}", line.Quantity,
             line.Product.Name,
             subtotal);
         }
         body.AppendFormat("Total order value: {0:c}", cart.ComputeTotalValue())
         .AppendLine("---")
         .AppendLine("Ship to:")
         .AppendLine(shippingInfo.Name)
         .AppendLine(shippingInfo.Line1)
         .AppendLine(shippingInfo.Line2 ?? "")
         .AppendLine(shippingInfo.Line3 ?? "")
         .AppendLine(shippingInfo.City)
         .AppendLine(shippingInfo.State ?? "")
         .AppendLine(shippingInfo.Country)
         .AppendLine(shippingInfo.Zip)
         .AppendLine("---")
         .AppendFormat("Gift wrap: {0}",
         shippingInfo.GiftWrap ? "Yes" : "No");
         MailMessage mailMessage = new MailMessage(
             emailSettings.MailFromAddress, // From
             emailSettings.MailToAddress, // To
             "New order submitted!", // Subject
             body.ToString()); // Body
         if (emailSettings.WriteAsFile)
         {
         mailMessage.BodyEncoding = Encoding.ASCII;
         }
         smtpClient.Send(mailMessage);
      }
   }
}
}
