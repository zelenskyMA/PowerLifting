using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingTemplate.Application;

namespace SportAssistant.Application.TrainingPlan.PlanCommands
{
    /// <summary>
    /// Получение пользователя, которому принадлежит тренировочный цикл.
    /// </summary>
    public class ProcessSetUserId : IProcessSetUserId
    {
        private readonly ICrudRepo<TemplateSetDb> _templateSetRepository;
        private readonly ICrudRepo<TemplatePlanDb> _templatePlanRepository;
        private readonly ICrudRepo<TemplateDayDb> _templateDayRepository;
        private readonly ICrudRepo<TemplateExerciseDb> _templateExerciseRepository;

        public ProcessSetUserId(
            ICrudRepo<TemplateSetDb> templateSetRepository,
            ICrudRepo<TemplatePlanDb> templatePlanRepository,
            ICrudRepo<TemplateDayDb> templateDayRepository,
            ICrudRepo<TemplateExerciseDb> templateExerciseRepository)
        {
            _templateSetRepository = templateSetRepository;
            _templatePlanRepository = templatePlanRepository;
            _templateDayRepository = templateDayRepository;
            _templateExerciseRepository = templateExerciseRepository;
        }

        /// <inheritdoc />
        public async Task<int> GetByPlanId(int id)
        {
            var plan = await _templatePlanRepository.FindOneAsync(t => t.Id == id);
            if (plan == null)
            {
                return 0;
            }

            var set = await _templateSetRepository.FindOneAsync(t => t.Id == plan.TemplateSetId);

            return set?.CoachId ?? 0;
        }

        /// <inheritdoc />
        public async Task<int> GetByDayId(int id)
        {
            var day = await _templateDayRepository.FindOneAsync(t => t.Id == id);
            if (day == null)
            {
                return 0;
            }

            return await GetByPlanId(day.TemplatePlanId);
        }

        /// <inheritdoc />
        public async Task<int> GetByPlanExerciseId(int id)
        {
            var planExercise = await _templateExerciseRepository.FindOneAsync(t => t.Id == id);
            if (planExercise == null)
            {
                return 0;
            }

            return await GetByDayId(planExercise.TemplateDayId);
        }
    }
}
