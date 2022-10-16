using SportAssistant.Domain.Models.Common;

namespace SportAssistant.Domain.Models.Coaching
{
    public class TrainingGroup : Entity
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public int ParticipantsCount { get; set; }

        public int CoachId { get; set; }
    }
}
