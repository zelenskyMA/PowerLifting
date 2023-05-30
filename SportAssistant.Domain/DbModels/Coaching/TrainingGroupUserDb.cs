using System.ComponentModel.DataAnnotations.Schema;

namespace SportAssistant.Domain.DbModels.Coaching;

[Table("TrainingGroupUsers", Schema = "trn")]
public class TrainingGroupUserDb
{
    public int UserId { get; set; }

    public int GroupId { get; set; }
}