namespace PowerLifting.Domain.Models.TrainingPlan
{
    public class RequestPlanCreation
    {
        public DateTime CreationDate { get; set; }

        public int UserId { get; set; } = 0;
    }
}
