using Microsoft.AspNetCore.Mvc;
using PowerLifting.Application.Dictionaryies;
using PowerLifting.Domain.Interfaces.Common.Actions;
using PowerLifting.Domain.Models;

namespace PowerLifting.Service.Controllers
{
    [Route("dictionary")]
    public class DictionaryController : BaseController
    {       
        [HttpGet]
        [Route("getListByType")]
        public async Task<List<DictionaryItem>> GetListByTypeAsync([FromServices] ICommand<DictionaryGetByTypeQuery.Param, List<DictionaryItem>> command, int typeId)
        {
            var result = await command.ExecuteAsync(new DictionaryGetByTypeQuery.Param() {  TypeId = typeId });
            return result;
        }
    }
}
