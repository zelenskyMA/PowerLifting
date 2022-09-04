using AutoMapper;
using PowerLifting.Domain.DbModels;
using PowerLifting.Domain.Interfaces;
using PowerLifting.Domain.Interfaces.Common.Repositories;
using PowerLifting.Domain.Models;

namespace PowerLifting.Application
{
    public class DictionaryCommands : IDictionaryCommands
    {
        private readonly ICrudRepo<DictionaryDb> _itemRepository;
        private readonly ICrudRepo<DictionaryTypeDb> _typeRepository;
        private readonly IMapper _mapper;

        public DictionaryCommands(
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

        public async Task<List<DictionaryItem>> GetItemsByTypeIdAsync(int typeId)
        {
            var items = await _itemRepository.FindAsync(t => t.TypeId == typeId);
            return items.Select(t => _mapper.Map<DictionaryItem>(t)).ToList();
        }

        public async Task<List<DictionaryType>> GetTypesAsync()
        {
            var itemTypes = await _typeRepository.GetAllAsync();
            return itemTypes.Select(t => _mapper.Map<DictionaryType>(t)).ToList();
        }

        public async Task AddItem(DictionaryItem item) => await _itemRepository.CreateAsync(_mapper.Map<DictionaryDb>(item));
        public async Task AddType(DictionaryType item) => await _typeRepository.CreateAsync(_mapper.Map<DictionaryTypeDb>(item));

        public async Task UpdateItem(DictionaryItem item) => await _itemRepository.UpdateAsync(_mapper.Map<DictionaryDb>(item));
        public async Task UpdateType(DictionaryType item) => await _typeRepository.UpdateAsync(_mapper.Map<DictionaryTypeDb>(item));

        public async Task DeleteItem(DictionaryItem item) => await _itemRepository.DeleteAsync(_mapper.Map<DictionaryDb>(item));
        public async Task DeleteType(DictionaryType item) => await _typeRepository.DeleteAsync(_mapper.Map<DictionaryTypeDb>(item));

    }
}
