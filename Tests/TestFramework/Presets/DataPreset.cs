using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Models.TrainingPlan;
using SportAssistant.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace TestFramework.Presets;

/// <summary>
/// Заполняем новую БД с помощью DbSeed.
/// Открываем доступ к Ид сущностей, созданных в БД.
/// </summary>
[ExcludeFromCodeCoverage]
public class DataPreset
{
    /// <summary> Предсозданные пользователи для использования в тестах</summary>
    public List<UserDb> Users { get; }

    /// <summary> Предсозданный план для тестов</summary>
    public List<PlanDay> PlanDays { get; }

    public DataPreset(SportContext ctx)
    {
        Users = new List<UserDb>() {
            new UserDb() { Id = 1, Email = Constants.AdminLogin, Password = Constants.EncriptedPwd, Salt = Constants.Salt, },
            new UserDb() { Id = 2, Email = Constants.CoachLogin, Password = Constants.EncriptedPwd, Salt = Constants.Salt, },
            new UserDb() { Id = 3, Email = Constants.SecondCoachLogin, Password = Constants.EncriptedPwd, Salt = Constants.Salt, },
            new UserDb() { Id = 4, Email = Constants.UserLogin, Password = Constants.EncriptedPwd, Salt = Constants.Salt, },
            new UserDb() { Id = 5, Email = Constants.BlockedUserLogin, Password = Constants.EncriptedPwd, Salt = Constants.Salt, Blocked = true },
            new UserDb() { Id = 6, Email = Constants.NoCoachUserLogin, Password = Constants.EncriptedPwd, Salt = Constants.Salt, },
        };

        DbSeed.InitializeDbForTests(ctx, Users);
        PlanDays = DbSeed.CreatePlan(ctx, GetUserId(Constants.UserLogin), DateTime.Now);
    }

    public int GetUserId(string login) => Users.First(t => t.Email == login).Id;
}
