using PowerLifting.Domain.Enums;
using PowerLifting.Domain.Interfaces;
using PowerLifting.Domain.Interfaces.Common.Operations;
using PowerLifting.Domain.Models;

namespace PowerLifting.Application.Dictionaryies
{
    /// <summary>
    /// Получение записей из справочника по Ид типа справочника.
    /// </summary>
    public class DictionaryGetByTypeQuery : ICommand<DictionaryGetByTypeQuery.Param, List<DictionaryItem>>
    {
        private readonly IProcessDictionary _processDictionary;

        public DictionaryGetByTypeQuery(IProcessDictionary processDictionary)
        {
            _processDictionary = processDictionary;
        }

        public async Task<List<DictionaryItem>> ExecuteAsync(Param param)
        {
            var entries = await _processDictionary.GetItemsByTypeIdAsync((DictionaryTypes)param.TypeId);
            return entries;
        }

        public class Param
        {
            public int TypeId { get; set; }
        }
    }
}
