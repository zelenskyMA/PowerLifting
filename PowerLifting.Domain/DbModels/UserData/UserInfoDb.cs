using PowerLifting.Domain.DbModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLifting.Domain.DbModels.UserData
{
    [Table("UserInfo", Schema = "usr")]
    public class UserInfoDb : EntityDb
    {
        public int UserId { get; set; }

        public string? FirstName { get; set; }

        public string? Surname { get; set; }

        public string? Patronimic { get; set; }

        public int? Weight { get; set; } = 0;

        public int? Height { get; set; }

        public int? Age { get; set; }

        public int? CoachId { get; set; }
    }
}
