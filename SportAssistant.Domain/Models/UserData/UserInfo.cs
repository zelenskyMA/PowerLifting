namespace SportAssistant.Domain.Models.UserData
{
    public class UserInfo
    {
        public string? FirstName { get; set; }

        public string? Surname { get; set; }

        public string? Patronimic { get; set; }

        public int? Weight { get; set; } = 0;

        public int? Height { get; set; }

        public int? Age { get; set; }

        public int? CoachId { get; set; }

        /// <summary>
        /// Отформатированное полное  имя, например: Иванов И.И.
        /// </summary>
        public string? LegalName { get; set; }

        public string? CoachLegalName { get; set; }

        /// <summary>
        /// Не тренирующийся тренер. У спортсмена нет личных тренировочных планов.
        /// </summary>
        public bool CoachOnly { get; set; }

        public RolesInfo RolesInfo { get; set; } = new RolesInfo();
    }
}
