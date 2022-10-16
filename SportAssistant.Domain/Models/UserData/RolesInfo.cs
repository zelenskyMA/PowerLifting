namespace SportAssistant.Domain.Models.UserData
{
    public class RolesInfo
    {
        /// <summary>
        /// UI supplies this field for role update
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// True if user har administration role
        /// </summary>
        public bool IsAdmin { get; set; } = false;

        /// <summary>
        /// True if user har coach role
        /// </summary>
        public bool IsCoach { get; set; } = false;
    }
}
