using System.ComponentModel.DataAnnotations.Schema;

namespace SportAssistant.Domain.DbModels.UserData;

[Table("UserRoles", Schema = "usr")]
public class UserRoleDb
{
    public int UserId { get; set; }

    public int RoleId { get; set; }
}
