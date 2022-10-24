using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.TrainingTemplate.Application;
using SportAssistant.Domain.Models.TraininTemplate;

namespace SportAssistant.Application.TraininTemplate.TemplateExerciseCommands
{
    /// <summary>
    /// Получение запланированных упражнений по Ид дней в шаблоне.
    /// </summary>
    public class TemplateExerciseGetByDaysQuery : ICommand<TemplateExerciseGetByDaysQuery.Param, List<TemplateExercise>>
    {
        private readonly IProcessTemplateExercise _processTemplateExercise;

        public TemplateExerciseGetByDaysQuery(
            IProcessTemplateExercise processTemplateExercise)
        {
            _processTemplateExercise = processTemplateExercise;
        }

        public async Task<List<TemplateExercise>> ExecuteAsync(Param param)
        {
            var exercises = await _processTemplateExercise.GetByDaysAsync(param.DayIds);
            return exercises;
        }

        public class Param
        {
            public List<int> DayIds { get; set; }
        }
    }
}
