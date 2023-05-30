using SportAssistant.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportAssistant.Domain.DbModels.UserData;

[Table("UserBlockHistory", Schema = "usr")]
public class UserBlockHistoryDb : EntityDb
{
    public int UserId { get; set; }

    public DateTime CreationDate { get; set; }

    public string Reason { get; set; }

    public int BlockerId { get; set; }
}