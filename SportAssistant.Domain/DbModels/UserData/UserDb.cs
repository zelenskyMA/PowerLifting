﻿using SportAssistant.Domain.DbModels.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportAssistant.Domain.DbModels.UserData;

[Table("Users", Schema = "usr")]
public class UserDb : EntityDb
{
    public string Email { get; set; }

    public string Password { get; set; }

    public string Salt { get; set; }

    public bool Blocked { get; set; }
}