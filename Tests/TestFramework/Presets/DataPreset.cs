using SportAssistant.Domain.DbModels.TrainingTemplate;
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

    public TemplateSet TemplateSet { get; set; }

    public DataPreset(SportContext ctx)
    {
        Users = new List<UserDb>() {
            new UserDb() { Id = 1, Email = TestConstants.AdminLogin, Password = TestConstants.EncriptedPwd, Salt = TestConstants.Salt, },
            new UserDb() { Id = 2, Email = TestConstants.CoachLogin, Password = TestConstants.EncriptedPwd, Salt = TestConstants.Salt, },
            new UserDb() { Id = 3, Email = TestConstants.SecondCoachLogin, Password = TestConstants.EncriptedPwd, Salt = TestConstants.Salt, },
            new UserDb() { Id = 4, Email = TestConstants.UserLogin, Password = TestConstants.EncriptedPwd, Salt = TestConstants.Salt, },
            new UserDb() { Id = 5, Email = TestConstants.User2Login, Password = TestConstants.EncriptedPwd, Salt = TestConstants.Salt, },
            new UserDb() { Id = 6, Email = TestConstants.BlockedUserLogin, Password = TestConstants.EncriptedPwd, Salt = TestConstants.Salt, Blocked = true },
            new UserDb() { Id = 7, Email = TestConstants.NoCoachUserLogin, Password = TestConstants.EncriptedPwd, Salt = TestConstants.Salt, },
            new UserDb() { Id = 8, Email = TestConstants.ManagerLogin, Password = TestConstants.EncriptedPwd, Salt = TestConstants.Salt, },
            new UserDb() { Id = 9, Email = TestConstants.OrgOwnerLogin, Password = TestConstants.EncriptedPwd, Salt = TestConstants.Salt, },
        };

        DbSeed.InitializeDbForTests(ctx, Users);
        PlanDays = DbSeed.CreatePlan(ctx, GetUserId(TestConstants.UserLogin), DateTime.Now); //Active plan
        DbSeed.CreatePlan(ctx, GetUserId(TestConstants.UserLogin), DateTime.Now.AddDays(-10)); // 1 old plan
        DbSeed.CreatePlan(ctx, GetUserId(TestConstants.UserLogin), DateTime.Now.AddDays(-20)); // 2 old plan

        TemplateSet = DbSeed.CreateTemplateSet(ctx, GetUserId(TestConstants.CoachLogin)); // template set
    }

    public int GetUserId(string login) => Users.First(t => t.Email == login).Id;
}
