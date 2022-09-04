using System.ComponentModel.DataAnnotations;

namespace PowerLifting.Domain.DbModels.Common
{
    public class EntityDb
    {
        [Key]
        public int Id { get; set; }
    }
}
