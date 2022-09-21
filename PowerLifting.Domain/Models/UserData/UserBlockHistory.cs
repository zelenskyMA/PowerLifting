using PowerLifting.Domain.Models.Common;

namespace PowerLifting.Domain.Models.UserData
{
    public class UserBlockHistory : Entity
    {
        public int UserId { get; set; }

        public DateTime CreationDate { get; set; }

        public string Reason { get; set; }

        public int BlockerId { get; set; }
    }
}
