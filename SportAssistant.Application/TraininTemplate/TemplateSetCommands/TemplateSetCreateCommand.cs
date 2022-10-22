using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TraininTemplate;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;

namespace SportAssistant.Application.TraininTemplate.TemplateSetCommands
{
    public class TemplateSetCreateCommand : ICommand<TemplateSetCreateCommand.Param, int>
    {
        private readonly ICrudRepo<TemplateSetDb> _templateSetRepository;
        private readonly IUserProvider _user;

        public TemplateSetCreateCommand(
            ICrudRepo<TemplateSetDb> templateSetRepository,
            IUserProvider user)
        {
            _templateSetRepository = templateSetRepository;
            _user = user;
        }

        public async Task<int> ExecuteAsync(Param param)
        {
            if (string.IsNullOrEmpty(param.Name))
            {
                throw new BusinessException("Необходимо указать название цикла");
            }

            var templateSetDb = await _templateSetRepository.FindOneAsync(t => t.Name == param.Name && t.CoachId == _user.Id);
            if (templateSetDb != null)
            {
                throw new BusinessException("Тренировочный цикл с указанным именем уже существует");
            }

            var templateSet = new TemplateSetDb()
            {
                Name = param.Name,
                CoachId = _user.Id,
            };

            await _templateSetRepository.CreateAsync(templateSet);

            return templateSet.Id;
        }

        public class Param
        {
            public string Name { get; set; }
        }
    }
}
