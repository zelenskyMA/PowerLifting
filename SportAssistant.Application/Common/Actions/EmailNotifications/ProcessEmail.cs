using AutoMapper;
using Microsoft.Extensions.Configuration;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.Basic;
using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Interfaces.Actions.EmailNotifications;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Models.Basic;
using System.Net;
using System.Net.Mail;

namespace SportAssistant.Application.Common.Actions.EmailNotifications
{
    public class ProcessEmail : IProcessEmail
    {
        private readonly ICrudRepo<EmailMessageDb> _emailMessageRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public ProcessEmail(
            ICrudRepo<EmailMessageDb> emailMessageRepository,
            IConfiguration configuration,
            IMapper mapper)
        {
            _emailMessageRepository = emailMessageRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task SendAsync(EmailMessage message)
        {
            var smtpClient = SetupClient();
            var mailMessage = CreateMessage(message);

            await smtpClient.SendMailAsync(mailMessage);
        }

        /// <inheritdoc />
        public async Task<EmailMessage> GetMessage(EmailMessageTypes messageId)
        {
            var messageDb = await _emailMessageRepository.FindOneAsync(t => t.Id == (int)messageId);
            if (messageDb == null)
            {
                throw new BusinessException($"Почтовое сообщение с Ид {messageId} не найдено");
            }

            return _mapper.Map<EmailMessage>(messageDb);
        }

        private SmtpClient SetupClient()
        {
            var smtpClient = new SmtpClient(_configuration["Smtp:Host"])
            {
                Port = int.Parse(_configuration["Smtp:Port"]),
                Credentials = new NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]),
                EnableSsl = false,
            };

            return smtpClient;
        }

        private MailMessage CreateMessage(EmailMessage message)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Smtp:From"]),
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(message.Address);

            return mailMessage;
        }

    }
}