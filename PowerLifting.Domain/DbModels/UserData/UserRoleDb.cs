using System.ComponentModel.DataAnnotations.Schema;

namespace PowerLifting.Domain.DbModels.UserData
{
    [Table("UserRoles", Schema = "usr")]
    public class UserRoleDb
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }
    }
}
