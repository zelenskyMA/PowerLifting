using SportAssistant.Domain.DbModels.TraininTemplate;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingTemplate.Application;
using SportAssistant.Domain.Models.TraininTemplate;

namespace SportAssistant.Application.TraininTemplate.TemplateExerciseCommands
{
    /// <summary>
    /// Получение запланированного упражнения в шаблоне по его Ид.
    /// </summary>
    public class TemplateExerciseGetByIdQuery : ICommand<TemplateExerciseGetByIdQuery.Param, TemplateExercise>
    {
        private readonly IProcessTemplateExercise _processTemplateExercise;
        private readonly ICrudRepo<TemplateExerciseDb> _templateExerciseRepository;

        public TemplateExerciseGetByIdQuery(
            IProcessTemplateExercise processTemplateExercise,
            ICrudRepo<TemplateExerciseDb> templateExerciseRepository)
        {
            _processTemplateExercise = processTemplateExercise;
            _templateExerciseRepository = templateExerciseRepository;
        }

        public async Task<TemplateExercise> ExecuteAsync(Param param)
        {
            var templateExerciseDb = await _templateExerciseRepository.FindAsync(t => t.Id == param.Id);
            var exercise = (await _processTemplateExercise.PrepareExerciseDataAsync(templateExerciseDb)).FirstOrDefault() ?? new TemplateExercise();
            return exercise;
        }

        public class Param
        {
            public int Id { get; set; }
        }
    }
}
