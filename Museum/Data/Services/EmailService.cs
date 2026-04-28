using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Museum.Models;

namespace Museum.Data.Services
{
    public interface IEmailService
    {
        Task SendOrderConfirmationAsync(OrderModel order, string userEmail, string userName, List<(string ticketName, int quantity, byte[] qrCode)> tickets);
        Task SendOrderStatusChangeAsync(OrderModel order, string userEmail, string userName, OrderStatus newStatus);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendOrderConfirmationAsync(
            OrderModel order,
            string userEmail,
            string userName,
            List<(string ticketName, int quantity, byte[] qrCode)> tickets)
        {
            var smtpSettings = _configuration.GetSection("EmailSettings");
            var smtpHost = smtpSettings["SmtpHost"];
            var smtpPort = int.Parse(smtpSettings["SmtpPort"] ?? "587");
            var smtpUsername = smtpSettings["SmtpUsername"];
            var smtpPassword = smtpSettings["SmtpPassword"];
            var fromEmail = smtpSettings["FromEmail"];
            var fromName = smtpSettings["FromName"];

            using (var client = new SmtpClient(smtpHost, smtpPort))
            {
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                client.EnableSsl = true;

                using (var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = $"Order Confirmation - {order.OrderCode}",
                    Body = GetOrderConfirmationHtml(order, userName),
                    IsBodyHtml = true
                })
                {
                    mailMessage.To.Add(userEmail);

                    // Attach QR codes
                    for (int i = 0; i < tickets.Count; i++)
                    {
                        var stream = new MemoryStream(tickets[i].qrCode);
                        var attachment = new Attachment(stream, $"qr_code_{order.OrderCode}_{i + 1}.png", "image/png");
                        mailMessage.Attachments.Add(attachment);
                    }

                    try
                    {
                        await client.SendMailAsync(mailMessage);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException("Failed to send order confirmation email", ex);
                    }
                }
            }
        }

        public async Task SendOrderStatusChangeAsync(
            OrderModel order,
            string userEmail,
            string userName,
            OrderStatus newStatus)
        {
            var smtpSettings = _configuration.GetSection("EmailSettings");
            var smtpHost = smtpSettings["SmtpHost"];
            var smtpPort = int.Parse(smtpSettings["SmtpPort"] ?? "587");
            var smtpUsername = smtpSettings["SmtpUsername"];
            var smtpPassword = smtpSettings["SmtpPassword"];
            var fromEmail = smtpSettings["FromEmail"];
            var fromName = smtpSettings["FromName"];

            using (var client = new SmtpClient(smtpHost, smtpPort))
            {
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                client.EnableSsl = true;

                using (var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = $"Order Status Update - {order.OrderCode}",
                    Body = GetOrderStatusChangeHtml(order, userName, newStatus),
                    IsBodyHtml = true
                })
                {
                    mailMessage.To.Add(userEmail);

                    try
                    {
                        await client.SendMailAsync(mailMessage);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException("Failed to send status change email", ex);
                    }
                }
            }
        }

        private string GetOrderConfirmationHtml(OrderModel order, string userName)
        {
            return $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #1a1a1a; color: white; padding: 20px; text-align: center; border-radius: 5px; }}
                        .content {{ margin: 20px 0; }}
                        .order-details {{ background-color: #f5f5f5; padding: 15px; border-radius: 5px; margin: 15px 0; }}
                        .detail-row {{ display: flex; justify-content: space-between; padding: 8px 0; border-bottom: 1px solid #ddd; }}
                        .detail-row:last-child {{ border-bottom: none; }}
                        .qr-section {{ text-align: center; margin: 20px 0; }}
                        .footer {{ text-align: center; margin-top: 30px; color: #666; font-size: 12px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h2>Order Confirmation</h2>
                        </div>
                        
                        <div class='content'>
                            <p>Dear {userName},</p>
                            <p>Thank you for your purchase! Your order has been confirmed.</p>
                            
                            <div class='order-details'>
                                <div class='detail-row'>
                                    <strong>Order Code:</strong>
                                    <span>{order.OrderCode}</span>
                                </div>
                                <div class='detail-row'>
                                    <strong>Museum:</strong>
                                    <span>{order.Museum?.Name}</span>
                                </div>
                                <div class='detail-row'>
                                    <strong>Visit Date:</strong>
                                    <span>{order.VisitDate:yyyy-MM-dd}</span>
                                </div>
                                <div class='detail-row'>
                                    <strong>Order Date:</strong>
                                    <span>{order.CreatedAt:yyyy-MM-dd HH:mm}</span>
                                </div>
                                <div class='detail-row'>
                                    <strong>Status:</strong>
                                    <span>Confirmed</span>
                                </div>
                            </div>
                            
                            <div class='qr-section'>
                                <p><strong>Your QR codes are attached to this email.</strong></p>
                                <p>Please keep them safe and show them at the museum entrance on your visit date.</p>
                            </div>
                            
                            <p>If you have any questions, please don't hesitate to contact us.</p>
                        </div>
                        
                        <div class='footer'>
                            <p>&copy; 2026 Museum Booking System. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>";
        }

        private string GetOrderStatusChangeHtml(OrderModel order, string userName, OrderStatus status)
        {
            var statusText = status switch
            {
                OrderStatus.Confirmed => "Confirmed",
                OrderStatus.Cancelled => "Cancelled",
                OrderStatus.Used => "Completed",
                _ => status.ToString()
            };

            var statusMessage = status switch
            {
                OrderStatus.Confirmed => "Your order has been confirmed. Your QR codes are ready to use.",
                OrderStatus.Cancelled => "Your order has been cancelled. If this was unexpected, please contact us.",
                OrderStatus.Used => "Thank you for visiting! Your order has been marked as used.",
                _ => "Your order status has been updated."
            };

            var backgroundColor = status switch
            {
                OrderStatus.Confirmed => "#4CAF50",
                OrderStatus.Cancelled => "#f44336",
                OrderStatus.Used => "#2196F3",
                _ => "#1a1a1a"
            };

            return $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: {backgroundColor}; color: white; padding: 20px; text-align: center; border-radius: 5px; }}
                        .content {{ margin: 20px 0; }}
                        .order-details {{ background-color: #f5f5f5; padding: 15px; border-radius: 5px; margin: 15px 0; }}
                        .detail-row {{ display: flex; justify-content: space-between; padding: 8px 0; border-bottom: 1px solid #ddd; }}
                        .detail-row:last-child {{ border-bottom: none; }}
                        .footer {{ text-align: center; margin-top: 30px; color: #666; font-size: 12px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h2>Order Status Update</h2>
                        </div>
                        
                        <div class='content'>
                            <p>Dear {userName},</p>
                            <p>{statusMessage}</p>
                            
                            <div class='order-details'>
                                <div class='detail-row'>
                                    <strong>Order Code:</strong>
                                    <span>{order.OrderCode}</span>
                                </div>
                                <div class='detail-row'>
                                    <strong>Museum:</strong>
                                    <span>{order.Museum?.Name}</span>
                                </div>
                                <div class='detail-row'>
                                    <strong>Visit Date:</strong>
                                    <span>{order.VisitDate:yyyy-MM-dd}</span>
                                </div>
                                <div class='detail-row'>
                                    <strong>New Status:</strong>
                                    <span>{statusText}</span>
                                </div>
                            </div>
                            
                            <p>If you have any questions, please don't hesitate to contact us.</p>
                        </div>
                        
                        <div class='footer'>
                            <p>&copy; 2026 Museum Booking System. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>";
        }
    }
}
