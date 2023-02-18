using SportAssistant.Domain.DbModels.Basic;
using SportAssistant.Domain.DbModels.Coaching;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Models.TrainingPlan;
using SportAssistant.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestFramework.Presets;

public static class DbSeed
{
    public static void InitializeDbForTests(SportContext ctx, List<UserDb> users)
    {
        ExecuteInitScripts(ctx);
        UsersSetup(ctx, users);
        AddCoachGroupData(ctx, users);
    }

    public static List<PlanDay> CreatePlan(SportContext ctx, int userId, DateTime startDate)
    {
        startDate = startDate.Date;

        //план
        var plan = new PlanDb() { StartDate = startDate, UserId = userId };
        ctx.Plans.Add(plan);
        ctx.SaveChanges();

        //7 плановых дней
        var planDays = new List<PlanDayDb>() { };
        for (int i = 0; i < 7; i++)
        {
            planDays.Add(new PlanDayDb() { PlanId = plan.Id, ActivityDate = startDate.AddDays(i) });
        }
        ctx.PlanDays.AddRange(planDays);
        ctx.SaveChanges();

        //упражнения по дням
        var exercises = new List<PlanExerciseDb>() {
            new PlanExerciseDb(){ PlanDayId = planDays[0].Id, Order = 1, ExerciseId = 1 },
            new PlanExerciseDb(){ PlanDayId = planDays[0].Id, Order = 2, ExerciseId = 20},

            new PlanExerciseDb(){ PlanDayId = planDays[1].Id, Order = 1, ExerciseId = 91},
            new PlanExerciseDb(){ PlanDayId = planDays[1].Id, Order = 2, ExerciseId = 61},
        };
        ctx.PlanExercises.AddRange(exercises);
        ctx.SaveChanges();

        var exSettings = new List<PlanExerciseSettingsDb>() {
            new PlanExerciseSettingsDb(){ PlanExerciseId = exercises[0].Id, Iterations= 2, ExercisePart1=1, ExercisePart2 = 2, ExercisePart3 = 3, Weight = 100, PercentageId = 9, Completed = false  },
            new PlanExerciseSettingsDb(){ PlanExerciseId = exercises[0].Id, Iterations= 1, ExercisePart1=1, ExercisePart2 = 2, ExercisePart3 = 3, Weight = 80, PercentageId = 7, Completed = false  },
            new PlanExerciseSettingsDb(){ PlanExerciseId = exercises[1].Id, Iterations= 2, ExercisePart1=1, ExercisePart2 = 2, ExercisePart3 = 3, Weight = 100, PercentageId = 9, Completed = false  },
            new PlanExerciseSettingsDb(){ PlanExerciseId = exercises[1].Id, Iterations= 1, ExercisePart1=1, ExercisePart2 = 2, ExercisePart3 = 3, Weight = 80, PercentageId = 7, Completed = true  },

            new PlanExerciseSettingsDb(){ PlanExerciseId = exercises[2].Id, Iterations= 2, ExercisePart1=1, ExercisePart2 = 2, ExercisePart3 = 3, Weight = 100, PercentageId = 9, Completed = false  },
            new PlanExerciseSettingsDb(){ PlanExerciseId = exercises[2].Id, Iterations= 1, ExercisePart1=1, ExercisePart2 = 2, ExercisePart3 = 3, Weight = 80, PercentageId = 7, Completed = true  },
            new PlanExerciseSettingsDb(){ PlanExerciseId = exercises[3].Id, Iterations= 2, ExercisePart1=1, ExercisePart2 = 2, ExercisePart3 = 3, Weight = 100, PercentageId = 9, Completed = false  },
            new PlanExerciseSettingsDb(){ PlanExerciseId = exercises[3].Id, Iterations= 1, ExercisePart1=1, ExercisePart2 = 2, ExercisePart3 = 3, Weight = 80, PercentageId = 7, Completed = false  },
        };
        ctx.PlanExerciseSettings.AddRange(exSettings);
        ctx.SaveChanges();

        var result = new List<PlanDay>();
        foreach (var planDay in planDays)
        {
            var dayExercises = exercises.Where(t => t.PlanDayId == planDay.Id).ToList();
            var settings = exSettings.Where(t => dayExercises.Select(t => t.Id).Contains(t.PlanExerciseId)).ToList();

            result.Add(new PlanDay()
            {
                Id = planDay.Id,
                PlanId = plan.Id,
                Exercises = dayExercises.Count == 0 ? new List<PlanExercise>() :
                    new List<PlanExercise>() {
                        new PlanExercise() {
                            Id = dayExercises[0].Id,
                            Exercise = new Exercise(){ Id = dayExercises[0].ExerciseId, ExerciseTypeId = 2 },
                            Settings = settings.Count == 0 ? new List<PlanExerciseSettings>() : new List<PlanExerciseSettings>()
                            {
                                new PlanExerciseSettings() { Id = settings[0].Id }, new PlanExerciseSettings() { Id = settings[1].Id }
                            }
                        },
                        new PlanExercise() {
                            Id = dayExercises[1].Id,
                            Exercise = new Exercise(){ Id = dayExercises[1].ExerciseId, ExerciseTypeId = 1 },
                            Settings = settings.Count == 0 ? new List<PlanExerciseSettings>() : new List<PlanExerciseSettings>()
                            {
                                new PlanExerciseSettings() { Id = settings[2].Id }, new PlanExerciseSettings() { Id = settings[3].Id }
                            }
                    }
            }
            });
        }

        return result;
    }

    private static void UsersSetup(SportContext ctx, List<UserDb> users)
    {
        ctx.Users.AddRange(users);
        ctx.SaveChanges();

        var adminId = users.First(t => t.Email == Constants.AdminLogin).Id;
        var coachId = users.First(t => t.Email == Constants.CoachLogin).Id;

        //user info
        var noCoachUsers = new List<int>() { coachId, users.First(t => t.Email == Constants.NoCoachUserLogin).Id,
            users.First(t => t.Email == Constants.SecondCoachLogin).Id};

        foreach (var user in users)
        {
            ctx.UsersInfo.Add(new UserInfoDb()
            {
                UserId = user.Id,
                Age = 45,
                FirstName = "Иван",
                Surname = "Иванов",
                Height = 150,
                Weight = 60,
                CoachId = noCoachUsers.Contains(user.Id) ? null : coachId
            }
            );
        }

        // user roles
        ctx.UserRoles.Add(new UserRoleDb() { UserId = adminId, RoleId = 10 });
        ctx.UserRoles.Add(new UserRoleDb() { UserId = coachId, RoleId = 11 });
        ctx.UserRoles.Add(new UserRoleDb() { UserId = users.First(t => t.Email == Constants.SecondCoachLogin).Id, RoleId = 11 });

        //set blocked user
        ctx.UserBlockHistoryItems.Add(new UserBlockHistoryDb()
        {
            BlockerId = adminId,
            CreationDate = DateTime.Now,
            Reason = "test reason",
            UserId = users.First(t => t.Email == Constants.BlockedUserLogin).Id,
        });

        ctx.SaveChanges();
    }

    private static void AddCoachGroupData(SportContext ctx, List<UserDb> users)
    {
        var coachId = users.First(t => t.Email == Constants.CoachLogin).Id;
        var userId = users.First(t => t.Email == Constants.UserLogin).Id;

        var groups = new List<TrainingGroupDb>()
        {
            new TrainingGroupDb() { CoachId = coachId, Name = Constants.GroupName },
            new TrainingGroupDb() { CoachId = coachId, Name = Constants.SecondGroupName },
        };
        ctx.TrainingGroups.AddRange(groups);
        ctx.SaveChanges();

        ctx.TrainingGroupUsers.Add(new TrainingGroupUserDb() { GroupId = groups[0].Id, UserId = userId });
        ctx.SaveChanges();
    }

    private static void ExecuteInitScripts(SportContext ctx)
    {
        ctx.DictionaryTypes.AddRange(new List<DictionaryTypeDb>() {
            new() { Id = 1, Name = "Тип упражнений", Description = "Базовые типы упражнений" },
            new() { Id = 2, Name = "Категория упражнений", Description = "Подраздел базового типа упражнений" },
            new() { Id = 3, Name = "Роль пользователя", Description = "Роли пользователей" }
        });

        ctx.Dictionaries.AddRange(new List<DictionaryDb>() {
            new() { Id = 1,TypeId = 1, Name = "Толчковые", Description = "толчок штанги" },
            new() { Id = 2,TypeId = 1, Name = "Рывковые", Description = "рывок штанги" },

            new() { Id = 10,TypeId = 3, Name = "Администратор", Description = "толчок штанги" },
            new() { Id = 11,TypeId = 3, Name = "Тренер", Description = "рывок штанги" },

            new() { Id = 50,TypeId = 2, Name = "Рывок классический", Description = string.Empty },
            new() { Id = 51,TypeId = 2, Name = "Толчок. Взятие на грудь", Description = string.Empty },
            new() { Id = 52,TypeId = 2, Name = "Толчок с груди", Description = string.Empty },
            new() { Id = 53,TypeId = 2, Name = "Толчок классический", Description = string.Empty },
            new() { Id = 54,TypeId = 2, Name = "ОФП", Description = string.Empty },
        });

        ctx.Settings.AddRange(new List<SettingsDb>() {
            new SettingsDb(){ Id = 1, Name = "Максимум активных планов", Value = "30", Description = "Предел" },
            new SettingsDb(){ Id = 2, Name = "Максимум упражнений в день", Value = "10", Description = "Предел" },
            new SettingsDb(){ Id = 3, Name = "Максимум поднятий в упражнении", Value = "10", Description = "Предел" },
        });

        ctx.EmailMessages.AddRange(new List<EmailMessageDb>() {
            new EmailMessageDb(){ Id = 1, Subject = "Подтверждение регистрации", Body = "<h4>Body 1</h4>" },
            new EmailMessageDb(){ Id = 2, Subject = "Сброс пароля", Body = "<h4>Body 2</h4>" },
        });

        ctx.Percentages.AddRange(new List<PercentageDb>() {
            new PercentageDb(){ Id = 1, Name = " < 30%", Description="Меньше 30%", MinValue = 0, MaxValue = 29 },
            new PercentageDb(){ Id = 2, Name = "30 - 40%", Description=string.Empty, MinValue = 30, MaxValue = 39 },
            new PercentageDb(){ Id = 3, Name = "40 - 50%", Description=string.Empty, MinValue = 40, MaxValue = 49 },
            new PercentageDb(){ Id = 4, Name = "50 - 60%", Description=string.Empty, MinValue = 50, MaxValue = 59 },
            new PercentageDb(){ Id = 5, Name = "60 - 70%", Description=string.Empty, MinValue = 60, MaxValue = 69 },
            new PercentageDb(){ Id = 6, Name = "70 - 80%", Description=string.Empty, MinValue = 70, MaxValue = 79 },
            new PercentageDb(){ Id = 7, Name = "80 - 90%", Description=string.Empty, MinValue = 80, MaxValue = 89 },
            new PercentageDb(){ Id = 8, Name = "90 - 100%", Description=string.Empty, MinValue = 90, MaxValue = 99 },
            new PercentageDb(){ Id = 9, Name = "100 - 110%", Description=string.Empty, MinValue = 100, MaxValue = 109 },
            new PercentageDb(){ Id = 10, Name = "110 - 120%", Description=string.Empty, MinValue = 110, MaxValue = 119 },
            new PercentageDb(){ Id = 11, Name = "120 - 130%", Description=string.Empty, MinValue = 120, MaxValue = 129 },
            new PercentageDb(){ Id = 12, Name = "130 - 140%", Description=string.Empty, MinValue = 130, MaxValue = 139 },
            new PercentageDb(){ Id = 13, Name = "140 - 150%", Description=string.Empty, MinValue = 140, MaxValue = 149 },
            new PercentageDb(){ Id = 14, Name = "> 150%", Description="Больше 150%", MinValue = 150, MaxValue = 999 },
        });

        ctx.Exercises.AddRange(new List<ExerciseDb>() {
            new ExerciseDb(){ Id = 1, ExerciseTypeId = 2, ExerciseSubTypeId = 50, Name = "Рывок классический", Description = string.Empty},
            new ExerciseDb(){ Id = 2, ExerciseTypeId = 2, ExerciseSubTypeId = 50, Name = "Рывок без разброса ног", Description = string.Empty},
            new ExerciseDb(){ Id = 20, ExerciseTypeId = 1, ExerciseSubTypeId = 51, Name = "Взятие на грудь без разброса ног", Description = string.Empty},
            new ExerciseDb(){ Id = 21, ExerciseTypeId = 1, ExerciseSubTypeId = 51, Name = "Взятие на грудь с подставки", Description = string.Empty},
            new ExerciseDb(){ Id = 40, ExerciseTypeId = 1, ExerciseSubTypeId = 52, Name = "Приседания на груди + толчок", Description = string.Empty},
            new ExerciseDb(){ Id = 41, ExerciseTypeId = 1, ExerciseSubTypeId = 52, Name = "Толчок со стоек", Description = string.Empty},
            new ExerciseDb(){ Id = 60, ExerciseTypeId = 1, ExerciseSubTypeId = 53, Name = "Толчок классический", Description = string.Empty},
            new ExerciseDb(){ Id = 61, ExerciseTypeId = 1, ExerciseSubTypeId = 53, Name = "Взятие на грудь + толчок", Description = string.Empty},
            new ExerciseDb(){ Id = 90, ExerciseTypeId = 2, ExerciseSubTypeId = 54, Name = "Тяга рывковая", Description = string.Empty},
            new ExerciseDb(){ Id = 91, ExerciseTypeId = 2, ExerciseSubTypeId = 54, Name = "Тяга рывковая с подставки", Description = string.Empty},
        });
    }
}
