using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.CustomExceptions;
using SportAssistant.Domain.DbModels.TraininTemplate;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Interfaces.Common.Repositories;

namespace SportAssistant.Application.TraininTemplate.TemplateSetCommands
{
    /// <summary>
    /// Удаление тренировочного цикла
    /// </summary>
    public class TemplateSetDeleteCommand : ICommand<TemplateSetDeleteCommand.Param, bool>
    {
        private readonly ICrudRepo<TemplateSetDb> _templateSetRepository;
        private readonly IUserProvider _user;

        public TemplateSetDeleteCommand(
            ICrudRepo<TemplateSetDb> templateSetRepository,
            IUserProvider user)
        {
            _templateSetRepository = templateSetRepository;
            _user = user;
        }

        public async Task<bool> ExecuteAsync(Param param)
        {
            var templateSetDb = await _templateSetRepository.FindOneAsync(t => t.Id == param.Id && t.CoachId == _user.Id);
            if (templateSetDb == null)
            {
                throw new BusinessException($"У вас нет тренировочного цикла с ид {param.Id}");
            }

            _templateSetRepository.Delete(templateSetDb);

            return true;
        }

        public class Param
        {
            public int Id { get; set; }
        }
    }
}
