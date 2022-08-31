using System.ComponentModel.DataAnnotations;

namespace PowerLifting.Domain.DbModels.Common
{
  public class EntityModel
  {
    [Key]
    public int Id { get; set; }
  }
}
