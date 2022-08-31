using PowerLifting.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerLifting.Domain.DbModels
{
  [Table("ExerciseTypes", Schema = "dbo")]
  public class ExerciseTypeDb : EntityModel
  {
    public string Name { get; set; }

    public string Description { get; set; }
  }
}
