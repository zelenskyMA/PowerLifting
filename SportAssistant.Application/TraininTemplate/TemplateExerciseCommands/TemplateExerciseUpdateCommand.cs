using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TraininTemplate;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingTemplate.Application;
using SportAssistant.Domain.Models.TraininTemplate;

namespace SportAssistant.Application.TraininTemplate.TemplateExerciseCommands
{
    /// <summary>
    /// Обновление упражнения в сете для шаблона дня. Создание поднятий (template exercise settings)
    /// </summary>
    public class TemplateExerciseUpdateCommand : ICommand<TemplateExerciseUpdateCommand.Param, bool>
    {
        private readonly IProcessTemplateExerciseSettings _processTemplateExerciseSettings;
        private readonly ICrudRepo<TemplateExerciseDb> _templateExerciseRepository;

        public TemplateExerciseUpdateCommand(
            IProcessTemplateExerciseSettings processTemplateExerciseSettings,
            ICrudRepo<TemplateExerciseDb> templateExerciseRepository)
        {
            _processTemplateExerciseSettings = processTemplateExerciseSettings;
            _templateExerciseRepository = templateExerciseRepository;
        }

        public async Task<bool> ExecuteAsync(Param param)
        {
            var templateExerciseDb = (await _templateExerciseRepository.FindAsync(t => t.Id == param.TemplateExercise.Id)).FirstOrDefault();
            if (templateExerciseDb == null)
            {
                throw new BusinessException("Не найдено упражнение для обновления");
            }

            if (templateExerciseDb.Comments != param.TemplateExercise.Comments)
            {
                templateExerciseDb.Comments = param.TemplateExercise.Comments;
                _templateExerciseRepository.Update(templateExerciseDb);
            }

            await _processTemplateExerciseSettings.UpdateAsync(param.TemplateExercise.Id, param.TemplateExercise.Settings);

            return true;
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
