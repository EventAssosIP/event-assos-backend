using EventAssos.Domain.Entities;
using MailKit.Net.Smtp;
using MimeKit;

namespace EventAssos.Application.Interfaces.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public SmtpEmailService(
            string smtpHost,
            int smtpPort,
            string smtpUser,
            string smtpPass,
            string fromEmail,
            string fromName)
        {
            _smtpHost = smtpHost;
            _smtpPort = smtpPort;
            _smtpUser = smtpUser;
            _smtpPass = smtpPass;
            _fromEmail = fromEmail;
            _fromName = fromName;
        }

        public async Task SendEventUpdateNotificationAsync(string toEmail, Event updatedEvent)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_fromName, _fromEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = $"Event '{updatedEvent.Name}' Updated";

            message.Body = new TextPart("plain")
            {
                Text = $@"Hello,

                The event '{updatedEvent.Name}' has been updated.

                Start Date: {updatedEvent.StartDate}
                End Date: {updatedEvent.EndDate}
                Location: {updatedEvent.Location}

                Please check the event details."
            };

            await SendAsync(message);
        }

        public async Task SendWaitingListPromotionNotificationAsync(string toEmail, Event eventPromoted)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_fromName, _fromEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = $"You are now registered for '{eventPromoted.Name}'!";

            message.Body = new TextPart("plain")
            {
                Text = $@"Hello,

                A spot opened up! You are now officially registered for the event '{eventPromoted.Name}' starting on {eventPromoted.StartDate}.

                See you there!"
            };

            await SendAsync(message);
        }

        private async Task SendAsync(MimeMessage message)
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpHost, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpUser, _smtpPass);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}

