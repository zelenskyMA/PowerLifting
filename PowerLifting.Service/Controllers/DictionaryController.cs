using Microsoft.AspNetCore.Mvc;
using PowerLifting.Domain.Enums;
using PowerLifting.Domain.Interfaces;
using PowerLifting.Domain.Models;

namespace PowerLifting.Service.Controllers
{
    [Route("dictionary")]
    public class DictionaryController : BaseController
    {
        private readonly IProcessDictionary _dictionaryCommands;

        public DictionaryController(IProcessDictionary dictionaryCommands)
        {
            _dictionaryCommands = dictionaryCommands;
        }

        [HttpGet]
        [Route("getListByType")]
        public async Task<List<DictionaryItem>> GetListByTypeAsync(DictionaryTypes id)
        {
            var result = await _dictionaryCommands.GetItemsByTypeIdAsync(id);
            return result;
        }
    }
}
