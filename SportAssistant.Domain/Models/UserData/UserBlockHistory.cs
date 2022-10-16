using SportAssistant.Domain.Models.Common;

namespace SportAssistant.Domain.Models.UserData
{
    public class UserBlockHistory : Entity
    {
        public int UserId { get; set; }

        public DateTime CreationDate { get; set; }

        public string Reason { get; set; }

        public int BlockerId { get; set; }
    }
}
