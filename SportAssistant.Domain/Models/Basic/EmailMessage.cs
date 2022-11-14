using SportAssistant.Domain.Models.Common;

namespace SportAssistant.Domain.Models.Basic
{
    public class EmailMessage : Entity
    {
        public string Address { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}
