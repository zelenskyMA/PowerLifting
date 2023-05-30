using System.ComponentModel.DataAnnotations.Schema;

namespace SportAssistant.Domain.DbModels.UserData;

[Table("UserAchivements", Schema = "usr")]
public class UserAchivementDb
{
    public int UserId { get; set; }

    public int ExerciseTypeId { get; set; }

    public DateTime CreationDate { get; set; }

    public int Result { get; set; }
}
