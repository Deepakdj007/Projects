using MailKit.Net.Smtp;
using MimeKit;
using Backend.Identity;
using OtpNet;
using Microsoft.Extensions.Configuration;
using System;

namespace Backend.Utility
{
    public class OtpService
    {
        private readonly IConfiguration _configuration;
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;

        public OtpService(IConfiguration configuration)
        {
            _configuration = configuration;
            _smtpHost = _configuration["SmtpSettings:Host"];
            _smtpPort = int.Parse(_configuration["SmtpSettings:Port"]);
            _smtpUsername = _configuration["SmtpSettings:Username"];
            _smtpPassword = _configuration["SmtpSettings:Password"];
        }

        // ✅ Generate OTP using base32 secret
        public string GenerateOtp(ApplicationUser user)
        {
            if (string.IsNullOrEmpty(user.TwoFactorSecret))
                throw new ArgumentException("2FA secret not configured for user.");

            var secretKey = Base32Encoding.ToBytes(user.TwoFactorSecret);
            var totp = new Totp(secretKey, step: 300); // 5-minute window
            return totp.ComputeTotp();
        }

        // ✅ Send OTP via email
        public void SendOtpToUser(ApplicationUser user, string otp)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_smtpUsername));
            message.To.Add(MailboxAddress.Parse($"{user.FullName} <{user.Email}>"));
            message.Subject = "Your One-Time Password (OTP)";

            message.Body = new TextPart("plain")
            {
                Text = $"Hello {user.FullName},\n\nYour OTP is: {otp}\nIt will expire in 5 minutes.\n\nThank you,\nYour App Team"
            };

            using var client = new SmtpClient();
            try
            {
                client.Connect(_smtpHost, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                client.Authenticate(_smtpUsername, _smtpPassword);
                client.Send(message);
                client.Disconnect(true);
            }
            catch (Exception ex)
            {
                // Log the exception or rethrow based on your needs
                Console.WriteLine($"[ERROR] OTP email failed: {ex.Message}");
                // Optionally: throw;
            }
        }
    }
}
