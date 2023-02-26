using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Interfaces.TrainingTemplate.Application;
using SportAssistant.Domain.Models.TrainingTemplate;

namespace SportAssistant.Application.TrainingTemplate.TemplateExerciseCommands
{
    /// <summary>
    /// Получение запланированного упражнения в шаблоне по его Ид.
    /// </summary>
    public class TemplateExerciseGetByIdQuery : ICommand<TemplateExerciseGetByIdQuery.Param, TemplateExercise>
    {
        private readonly IProcessTemplateExercise _processTemplateExercise;
        private readonly IProcessSetUserId _processSetUserId;
        private readonly ICrudRepo<TemplateExerciseDb> _templateExerciseRepository;
        private readonly IUserProvider _user;

        public TemplateExerciseGetByIdQuery(
            IProcessTemplateExercise processTemplateExercise,
            IProcessSetUserId processSetUserId,
            ICrudRepo<TemplateExerciseDb> templateExerciseRepository,
            IUserProvider user)
        {
            _processSetUserId = processSetUserId;
            _processTemplateExercise = processTemplateExercise;
            _templateExerciseRepository = templateExerciseRepository;
            _user = user;
        }

        public async Task<TemplateExercise> ExecuteAsync(Param param)
        {
            var templateExerciseDb = await _templateExerciseRepository.FindAsync(t => t.Id == param.Id);

            if (templateExerciseDb != null) // чужие данные смотреть нельзя
            {
                var setUserId = await _processSetUserId.GetByDayId(param.Id);
                if (setUserId != _user.Id)
                {
                    throw new DataException();
                }
            }

            var exercise = (await _processTemplateExercise.PrepareExerciseDataAsync(templateExerciseDb)).FirstOrDefault() ?? new TemplateExercise();
            return exercise;
        }

        public class Param
        {
            public int Id { get; set; }
        }
    }
}
