using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Models;

namespace SportAssistant.Domain.Interfaces;

public interface IProcessDictionary
{
    /// <summary>
    /// Get dictionary items by id list
    /// </summary>
    /// <param name="ids">Id list</param>
    /// <returns>Dictionary items</returns>
    Task<List<DictionaryItem>> GetItemsAsync(List<int> ids);

    /// <summary>
    /// Get dictionary items by dictionary type id
    /// </summary>
    /// <param name="typeId">Type id</param>
    /// <returns>Dictionary items</returns>
    Task<List<DictionaryItem>> GetItemsByTypeIdAsync(DictionaryTypes typeId);

    /// <summary>
    /// Get full list of dictionary types
    /// </summary>
    /// <returns>Dictionary types</returns>
    Task<List<DictionaryType>> GetTypesAsync();

    /*
    /// <summary>
    /// Add new dictionary item
    /// </summary>
    /// <param name="item">Item to add</param>
    /// <returns></returns>
    Task AddItem(DictionaryItem item);

    /// <summary>
    /// Add new dictionary type item
    /// </summary>
    /// <param name="item">Item to add</param>
    /// <returns></returns>
    Task AddType(DictionaryType item);


    /// <summary>
    /// Update dictionary item
    /// </summary>
    /// <param name="item">Item to update</param>
    /// <returns></returns>
    Task UpdateItem(DictionaryItem item);

    /// <summary>
    /// Update dictionary type item
    /// </summary>
    /// <param name="item">Item to update</param>
    /// <returns></returns>
    Task UpdateType(DictionaryType item);


    /// <summary>
    /// Delete dictionary item
    /// </summary>
    /// <param name="item">Item to delete</param>
    /// <returns></returns>
    Task DeleteItem(DictionaryItem item);

    /// <summary>
    /// Delete dictionary type item
    /// </summary>
    /// <param name="item">Item to delete</param>
    /// <returns></returns>
    Task DeleteType(DictionaryType item);
    */
}
