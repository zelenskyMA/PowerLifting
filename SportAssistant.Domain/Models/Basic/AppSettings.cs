using SportAssistant.Domain.Models.Common;

namespace SportAssistant.Domain.Models.Basic
{
    public class AppSettings
    {
        /// <summary>
        /// Максимум упражнений в день
        /// </summary>
        public int MaxExercises { get; set; }

        /// <summary>
        /// Максимум поднятий в упражнении
        /// </summary>
        public int MaxLiftItems { get; set; }
    }
}
