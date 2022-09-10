namespace PowerLifting.Domain.Models.UserData
{
    public class UserAchivement
    {
        public int UserId { get; set; }

        public int ExerciseTypeId { get; set; }

        public DateTime CreationDate { get; set; }

        public int Result { get; set; }
    }
}
