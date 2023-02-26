using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingTemplate.Application;
using SportAssistant.Domain.Models.TrainingTemplate;

namespace SportAssistant.Application.TrainingTemplate.TemplateExerciseCommands
{
    /// <summary>
    /// Обновление упражнения в сете для шаблона дня. Создание поднятий (template exercise settings)
    /// </summary>
    public class TemplateExerciseUpdateCommand : ICommand<TemplateExerciseUpdateCommand.Param, bool>
    {
        private readonly IProcessTemplateExerciseSettings _processTemplateExerciseSettings;
        private readonly IProcessTemplateSet _processTemplateSet;
        private readonly IProcessSetUserId _processSetUserId;
        private readonly ICrudRepo<TemplateExerciseDb> _templateExerciseRepository;

        public TemplateExerciseUpdateCommand(
            IProcessTemplateExerciseSettings processTemplateExerciseSettings,
            IProcessTemplateSet processTemplateSet,
            IProcessSetUserId processSetUserId,
            ICrudRepo<TemplateExerciseDb> templateExerciseRepository)
        {
            _processTemplateExerciseSettings = processTemplateExerciseSettings;
            _processTemplateSet = processTemplateSet;
            _processSetUserId = processSetUserId;
            _templateExerciseRepository = templateExerciseRepository;
        }

        public async Task<bool> ExecuteAsync(Param param)
        {
            var templateExerciseDb = (await _templateExerciseRepository.FindAsync(t => t.Id == param.TemplateExercise.Id)).FirstOrDefault();

            await VerifyRequestAsync(templateExerciseDb, param.TemplateExercise.Id);

            if (templateExerciseDb.Comments != param.TemplateExercise.Comments)
            {
                templateExerciseDb.Comments = param.TemplateExercise.Comments;
                _templateExerciseRepository.Update(templateExerciseDb);
            }

            await _processTemplateExerciseSettings.UpdateAsync(param.TemplateExercise.Id, param.TemplateExercise.Settings);
            return true;
        }

        private async Task VerifyRequestAsync(TemplateExerciseDb templateExerciseDb, int exerciseId)
        {
            if (templateExerciseDb == null)
            {
                throw new BusinessException("Не найдено упражнение для обновления");
            }

            var ownerId = await _processSetUserId.GetByPlanExerciseId(exerciseId);
            await _processTemplateSet.ChangingAllowedForUserAsync(ownerId);
        }

        public class Param
        {
            /// <summary>
            /// Упражнение, для которого планируются поднятия
            /// </summary>
            public TemplateExercise TemplateExercise { get; set; }
        }
    }
}
