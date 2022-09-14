namespace PowerLifting.Domain.Models.TrainingPlan
{
    public class Plans
    {
        public List<Plan> ActivePlans { get; set; } = new List<Plan>();

        public List<Plan> ExpiredPlans { get; set; } = new List<Plan>();
    }
}
