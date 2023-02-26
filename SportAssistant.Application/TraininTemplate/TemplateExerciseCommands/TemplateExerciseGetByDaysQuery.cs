using SportAssistant.Application.TrainingPlan.PlanCommands;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingPlan.Application;
using SportAssistant.Domain.Interfaces.TrainingTemplate.Application;
using SportAssistant.Domain.Models.TrainingTemplate;

namespace SportAssistant.Application.TrainingTemplate.TemplateExerciseCommands
{
    /// <summary>
    /// Получение запланированных упражнений по Ид дней в шаблоне.
    /// </summary>
    public class TemplateExerciseGetByDaysQuery : ICommand<TemplateExerciseGetByDaysQuery.Param, List<TemplateExercise>>
    {
        private readonly IProcessTemplateExercise _processTemplateExercise;
        private readonly IProcessTemplateSet _processTemplateSet;
        private readonly IProcessSetUserId _processSetUserId;
        private readonly IUserProvider _user;

        public TemplateExerciseGetByDaysQuery(
            IProcessTemplateExercise processTemplateExercise,
            IProcessTemplateSet processTemplateSet,
            IProcessSetUserId processSetUserId,
            IUserProvider user)
        {
            _processTemplateExercise = processTemplateExercise;
            _processTemplateSet = processTemplateSet;
            _processSetUserId = processSetUserId;
            _user = user;
        }

        public async Task<List<TemplateExercise>> ExecuteAsync(Param param)
        {
            var exercises = await _processTemplateExercise.GetByDaysAsync(new List<int>() { param.DayId });

            if (exercises.Count > 0) // запрет просмотра чужих данных
            {
                var ownerId = await _processSetUserId.GetByDayId(param.DayId);
                await _processTemplateSet.ViewAllowedForDataOfUserAsync(ownerId);
            }

            return exercises;
        }

        public class Param
        {
            public int DayId { get; set; }
        }
    }
}