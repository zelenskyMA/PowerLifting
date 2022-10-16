namespace SportAssistant.Domain.Models.Coaching
{
    public class TrainingGroupInfo
    {
        public TrainingGroup Group { get; set; }

        public List<GroupUser> Users { get; set; }
    }
}
