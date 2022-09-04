using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels;
using PowerLifting.Infrastructure.Repositories.Common;

namespace PowerLifting.Infrastructure.Repositories
{
    public class DictionaryRepository : CrudRepo<DictionaryDb>
    {
        public DictionaryRepository(DbContextOptions<LiftingContext> provider) : base(provider) { }

        /*
          var items = Context.Dictionaries
                .Join(Context.DictionaryTypes, dict => dict.Id, dType => dType.Id,
                    (dict, dType) => new DictionaryItem()
                    {
                        Id = dict.Id,
                        Name = dict.Name,
                        Description = dict.Description,
                        TypeId = dict.TypeId,
                        TypeName = dType.Name,
                    })
                .Where(predicate)
                .ToListAsync();
         */
    }
}
