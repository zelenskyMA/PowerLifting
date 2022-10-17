using AutoMapper;
using SportAssistant.Domain.DbModels.Basic;
using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Interfaces;
using SportAssistant.Domain.Interfaces.Common.Repositories;
using SportAssistant.Domain.Models;

namespace SportAssistant.Application.Dictionaryies
{
    public class ProcessDictionary : IProcessDictionary
    {
        private readonly ICrudRepo<DictionaryDb> _itemRepository;
        private readonly ICrudRepo<DictionaryTypeDb> _typeRepository;
        private readonly IMapper _mapper;

        public ProcessDictionary(
         ICrudRepo<DictionaryDb> itemRepository,
         ICrudRepo<DictionaryTypeDb> typeRepository,
         IMapper mapper)
        {
            _itemRepository = itemRepository;
            _typeRepository = typeRepository;
            _mapper = mapper;
        }

        public async Task<List<DictionaryItem>> GetItemsAsync(List<int> ids)
        {
            var items = await _itemRepository.FindAsync(t => ids.Contains(t.Id));
            return items.Select(t => _mapper.Map<DictionaryItem>(t)).ToList();
        }

        public async Task<List<DictionaryItem>> GetItemsByTypeIdAsync(DictionaryTypes typeId)
        {
            var items = await _itemRepository.FindAsync(t => t.TypeId == (int)typeId);
            return items.Select(t => _mapper.Map<DictionaryItem>(t)).ToList();
        }

        public async Task<List<DictionaryType>> GetTypesAsync()
        {
            var itemTypes = await _typeRepository.GetAllAsync();
            return itemTypes.Select(t => _mapper.Map<DictionaryType>(t)).ToList();
        }
    }
}
