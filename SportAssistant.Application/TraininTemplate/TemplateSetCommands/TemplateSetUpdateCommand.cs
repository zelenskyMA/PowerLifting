using AutoMapper;
using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;

namespace SportAssistant.Application.TrainingTemplate.TemplateSetCommands
{
    /// <summary>
    /// Обновление данных тренировочного цикла
    /// </summary>
    public class TemplateSetUpdateCommand : ICommand<TemplateSetUpdateCommand.Param, bool>
    {
        private readonly ICrudRepo<TemplateSetDb> _templateSetRepository;
        private readonly IUserProvider _user;
        private readonly IMapper _mapper;

        public TemplateSetUpdateCommand(
            ICrudRepo<TemplateSetDb> templateSetRepository,
            IUserProvider user,
            IMapper mapper)
        {
            _templateSetRepository = templateSetRepository;
            _user = user;
            _mapper = mapper;
        }

        public async Task<bool> ExecuteAsync(Param param)
        {
            if (string.IsNullOrEmpty(param.TemplateSet.Name))
            {
                throw new BusinessException("Необходимо указать новое название цикла");
            }

            var duplicateSetDb = await _templateSetRepository.FindOneAsync(t => t.Name == param.TemplateSet.Name && t.CoachId == _user.Id);
            if (duplicateSetDb != null && duplicateSetDb.Id != param.TemplateSet.Id)
            {
                throw new BusinessException("Тренировочный цикл с указанным именем уже существует");
            }

            var templateSetDb = await _templateSetRepository.FindOneAsync(t => t.Id == param.TemplateSet.Id && t.CoachId == _user.Id);
            if (templateSetDb == null)
            {
                throw new BusinessException($"У вас нет тренировочного цикла с ид {param.TemplateSet.Id}");
            }

            templateSetDb = _mapper.Map<TemplateSetDb>(param.TemplateSet);
            templateSetDb.CoachId = _user.Id;

            _templateSetRepository.Update(templateSetDb);

            return true;
        }

        public class Param
        {
            public TemplateSet TemplateSet { get; set; }
        }
    }
}
