using Microsoft.AspNetCore.Mvc;
using SportAssistant.Application.Dictionaryies;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Models;

namespace SportAssistant.Service.Controllers
{
    [Route("dictionary")]
    public class DictionaryController : BaseController
    {
        [HttpGet]
        [Route("getListByType/{typeId}")]
        public async Task<List<DictionaryItem>> GetListByTypeAsync([FromServices] ICommand<DictionaryGetByTypeQuery.Param, List<DictionaryItem>> command, int typeId)
        {
            var result = await command.ExecuteAsync(new DictionaryGetByTypeQuery.Param() { TypeId = typeId });
            return result;
        }
    }
}
