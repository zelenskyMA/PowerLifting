using SportAssistant.Domain;
using SportAssistant.Domain.DbModels.Basic;
using SportAssistant.Domain.DbModels.Coaching;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Models.TrainingPlan;
using SportAssistant.Domain.Models.TrainingTemplate;
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
        for (int i = 0; i < AppConstants.DaysInPlan; i++)
        {
            planDays.Add(new PlanDayDb() { PlanId = plan.Id, ActivityDate = startDate.AddDays(i) });
        }
        ctx.PlanDays.AddRange(planDays);
        ctx.SaveChanges();

        //упражнения по дням
        var exercises = new List<PlanExerciseDb>() {
            new PlanExerciseDb(){ PlanDayId = planDays[0].Id, Order = 2, ExerciseId = 1 },
            new PlanExerciseDb(){ PlanDayId = planDays[0].Id, Order = 1, ExerciseId = 20},

            new PlanExerciseDb(){ PlanDayId = planDays[1].Id, Order = 2, ExerciseId = 91},
            new PlanExerciseDb(){ PlanDayId = planDays[1].Id, Order = 1, ExerciseId = 61},
        };
        ctx.PlanExercises.AddRange(exercises);
        ctx.SaveChanges();

        var exSettings = new List<PlanExerciseSettingsDb>() {
            new PlanExerciseSettingsDb(){ PlanExerciseId = exercises[0].Id, Iterations= 2, ExercisePart1=1, ExercisePart2 = 2, ExercisePart3 = 3, Weight = 100, PercentageId = 1, Completed = false  },
            new PlanExerciseSettingsDb(){ PlanExerciseId = exercises[0].Id, Iterations= 1, ExercisePart1=1, ExercisePart2 = 2, ExercisePart3 = 3, Weight = 80, PercentageId = 2, Completed = false  },
            new PlanExerciseSettingsDb(){ PlanExerciseId = exercises[1].Id, Iterations= 2, ExercisePart1=1, ExercisePart2 = 2, ExercisePart3 = 3, Weight = 100, PercentageId = 1, Completed = false  },
            new PlanExerciseSettingsDb(){ PlanExerciseId = exercises[1].Id, Iterations= 1, ExercisePart1=1, ExercisePart2 = 2, ExercisePart3 = 3, Weight = 80, PercentageId = 2, Completed = true  },

            new PlanExerciseSettingsDb(){ PlanExerciseId = exercises[2].Id, Iterations= 2, ExercisePart1=1, ExercisePart2 = 2, ExercisePart3 = 3, Weight = 100, PercentageId = 1, Completed = false  },
            new PlanExerciseSettingsDb(){ PlanExerciseId = exercises[2].Id, Iterations= 1, ExercisePart1=1, ExercisePart2 = 2, ExercisePart3 = 3, Weight = 80, PercentageId = 2, Completed = true  },
            new PlanExerciseSettingsDb(){ PlanExerciseId = exercises[3].Id, Iterations= 2, ExercisePart1=1, ExercisePart2 = 2, ExercisePart3 = 3, Weight = 100, PercentageId = 1, Completed = false  },
            new PlanExerciseSettingsDb(){ PlanExerciseId = exercises[3].Id, Iterations= 1, ExercisePart1=1, ExercisePart2 = 2, ExercisePart3 = 3, Weight = 80, PercentageId = 2, Completed = false  },
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
                ActivityDate = planDay.ActivityDate,
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

    public static TemplateSet CreateTemplateSet(SportContext ctx, int coachId)
    {
        //тренировочный цикл
        var templateSet = new TemplateSetDb() { Name = "test set", CoachId = coachId };
        ctx.TemplateSets.Add(templateSet);
        ctx.SaveChanges();

        //шаблон плана
        var plan = new TemplatePlanDb() { Name = "test tmplt 1", TemplateSetId = templateSet.Id };
        ctx.TemplatePlans.Add(plan);
        ctx.SaveChanges();

        //7 плановых дней в шаблоне
        var planDays = new List<TemplateDayDb>() { };
        for (int i = 0; i < AppConstants.DaysInPlan; i++)
        {
            planDays.Add(new TemplateDayDb() { TemplatePlanId = plan.Id, DayNumber = i + 1 });
        }
        ctx.TemplateDays.AddRange(planDays);
        ctx.SaveChanges();

        //упражнения по дням
        var exercises = new List<TemplateExerciseDb>() {
            new TemplateExerciseDb(){ TemplateDayId = planDays[0].Id, Order = 1, ExerciseId = 1 },
            new TemplateExerciseDb(){ TemplateDayId = planDays[0].Id, Order = 2, ExerciseId = 20},

            new TemplateExerciseDb(){ TemplateDayId = planDays[1].Id, Order = 1, ExerciseId = 91},
            new TemplateExerciseDb(){ TemplateDayId = planDays[1].Id, Order = 2, ExerciseId = 61},
        };
        ctx.TemplateExercises.AddRange(exercises);
        ctx.SaveChanges();

        var exSettings = new List<TemplateExerciseSettingsDb>() {
            new TemplateExerciseSettingsDb(){ TemplateExerciseId = exercises[0].Id, Iterations= 2, ExercisePart1=1, ExercisePart2 = 2, ExercisePart3 = 3, WeightPercentage = 100, PercentageId = 1 },
            new TemplateExerciseSettingsDb(){ TemplateExerciseId = exercises[0].Id, Iterations= 1, ExercisePart1=1, ExercisePart2 = 2, ExercisePart3 = 3, WeightPercentage = 80, PercentageId = 2  },
            new TemplateExerciseSettingsDb(){ TemplateExerciseId = exercises[1].Id, Iterations= 2, ExercisePart1=1, ExercisePart2 = 2, ExercisePart3 = 3, WeightPercentage = 100, PercentageId = 1 },
            new TemplateExerciseSettingsDb(){ TemplateExerciseId = exercises[1].Id, Iterations= 1, ExercisePart1=1, ExercisePart2 = 2, ExercisePart3 = 3, WeightPercentage = 80, PercentageId = 2, },

            new TemplateExerciseSettingsDb(){ TemplateExerciseId = exercises[2].Id, Iterations= 2, ExercisePart1=1, ExercisePart2 = 2, ExercisePart3 = 3, WeightPercentage = 100, PercentageId = 1 },
            new TemplateExerciseSettingsDb(){ TemplateExerciseId = exercises[2].Id, Iterations= 1, ExercisePart1=1, ExercisePart2 = 2, ExercisePart3 = 3, WeightPercentage = 80, PercentageId = 2, },
            new TemplateExerciseSettingsDb(){ TemplateExerciseId = exercises[3].Id, Iterations= 2, ExercisePart1=1, ExercisePart2 = 2, ExercisePart3 = 3, WeightPercentage = 100, PercentageId = 1 },
            new TemplateExerciseSettingsDb(){ TemplateExerciseId = exercises[3].Id, Iterations= 1, ExercisePart1=1, ExercisePart2 = 2, ExercisePart3 = 3, WeightPercentage = 80, PercentageId = 2  },
        };
        ctx.TemplateExerciseSettings.AddRange(exSettings);
        ctx.SaveChanges();

        var days = new List<TemplateDay>();
        foreach (var planDay in planDays)
        {
            var dayExercises = exercises.Where(t => t.TemplateDayId == planDay.Id).ToList();
            var settings = exSettings.Where(t => dayExercises.Select(t => t.Id).Contains(t.TemplateExerciseId)).ToList();

            days.Add(new TemplateDay()
            {
                Id = planDay.Id,
                TemplatePlanId = plan.Id,
                DayNumber = planDay.DayNumber,
                Exercises = dayExercises.Count == 0 ? new List<TemplateExercise>() :
                    new List<TemplateExercise>() {
                        new TemplateExercise() {
                            Id = dayExercises[0].Id,
                            Exercise = new Exercise(){ Id = dayExercises[0].ExerciseId, ExerciseTypeId = 2 },
                            Settings = settings.Count == 0 ? new List<TemplateExerciseSettings>() : new List<TemplateExerciseSettings>()
                            {
                                new TemplateExerciseSettings() { Id = settings[0].Id }, new TemplateExerciseSettings() { Id = settings[1].Id }
                            }
                        },
                        new TemplateExercise() {
                            Id = dayExercises[1].Id,
                            Exercise = new Exercise(){ Id = dayExercises[1].ExerciseId, ExerciseTypeId = 1 },
                            Settings = settings.Count == 0 ? new List<TemplateExerciseSettings>() : new List<TemplateExerciseSettings>()
                            {
                                new TemplateExerciseSettings() { Id = settings[2].Id }, new TemplateExerciseSettings() { Id = settings[3].Id }
                            }
                        }
                    }
            });
        }

        var set = new TemplateSet() { Id = templateSet.Id, CoachId = coachId, Name = templateSet.Name };
        set.Templates = new List<TemplatePlan>() {
            new TemplatePlan() {
                Id = plan.Id,
                Name = plan.Name,
                TrainingDays = days,
            }
        };

        return set;
    }

    private static void UsersSetup(SportContext ctx, List<UserDb> users)
    {
        ctx.Users.AddRange(users);
        ctx.SaveChanges();

        var mainUserId = users.First(t => t.Email == TestConstants.UserLogin).Id;
        var main2UserId = users.First(t => t.Email == TestConstants.User2Login).Id;
        var adminId = users.First(t => t.Email == TestConstants.AdminLogin).Id;
        var coachId = users.First(t => t.Email == TestConstants.CoachLogin).Id;

        //user info
        var noCoachUsers = new List<int>() {
            coachId,
            users.First(t => t.Email == TestConstants.NoCoachUserLogin).Id,
            users.First(t => t.Email == TestConstants.SecondCoachLogin).Id
        };

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
            });
        }

        // user roles
        ctx.UserRoles.Add(new UserRoleDb() { UserId = adminId, RoleId = 10 });
        ctx.UserRoles.Add(new UserRoleDb() { UserId = coachId, RoleId = 11 });
        ctx.UserRoles.Add(new UserRoleDb() { UserId = users.First(t => t.Email == TestConstants.SecondCoachLogin).Id, RoleId = 11 });
        ctx.UserRoles.Add(new UserRoleDb() { UserId = users.First(t => t.Email == TestConstants.ManagerLogin).Id, RoleId = 12 });
        ctx.UserRoles.Add(new UserRoleDb() { UserId = users.First(t => t.Email == TestConstants.OrgOwnerLogin).Id, RoleId = 13 });

        var achivements = new List<UserAchivementDb>() {
            new UserAchivementDb() { UserId = mainUserId, CreationDate = DateTime.Now.AddDays(-1), ExerciseTypeId = 1, Result = 20 },
            new UserAchivementDb() { UserId = main2UserId, CreationDate = DateTime.Now.AddDays(-1), ExerciseTypeId = 1, Result = 50 },
            new UserAchivementDb() { UserId = main2UserId, CreationDate = DateTime.Now.AddDays(-1), ExerciseTypeId = 2, Result = 120 },
        };
        ctx.UserAchivements.AddRange(achivements);



        //set blocked user
        ctx.UserBlockHistoryItems.Add(new UserBlockHistoryDb()
        {
            BlockerId = adminId,
            CreationDate = DateTime.Now,
            Reason = "test reason",
            UserId = users.First(t => t.Email == TestConstants.BlockedUserLogin).Id,
        });

        ctx.SaveChanges();
    }

    private static void AddCoachGroupData(SportContext ctx, List<UserDb> users)
    {
        var coachId = users.First(t => t.Email == TestConstants.CoachLogin).Id;
        var coach2Id = users.First(t => t.Email == TestConstants.SecondCoachLogin).Id;
        var userId = users.First(t => t.Email == TestConstants.UserLogin).Id;

        var groups = new List<TrainingGroupDb>()
        {
            new TrainingGroupDb() { CoachId = coachId, Name = TestConstants.GroupName },
            new TrainingGroupDb() { CoachId = coachId, Name = TestConstants.SecondGroupName },
            new TrainingGroupDb() { CoachId = coach2Id, Name = TestConstants.GroupName },
        };
        ctx.TrainingGroups.AddRange(groups);
        ctx.SaveChanges();

        ctx.TrainingGroupUsers.Add(new TrainingGroupUserDb() { GroupId = groups[0].Id, UserId = userId });
        ctx.TrainingGroupUsers.Add(new TrainingGroupUserDb() { GroupId = groups[1].Id, UserId = users.First(t => t.Email == TestConstants.User2Login).Id });
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
            new() { Id = 1, TypeId = 1, Name = "Толчковые", Description = "толчок штанги" },
            new() { Id = 2, TypeId = 1, Name = "Рывковые", Description = "рывок штанги" },
            new() { Id = 3, TypeId = 1, Name = "ОФП", Description = "общая физ. подготовка" },

            new() { Id = 10, TypeId = 3, Name = "Администратор", Description = "толчок штанги" },
            new() { Id = 11, TypeId = 3, Name = "Тренер", Description = "рывок штанги" },

            new() { Id = TestConstants.SubTypeId, TypeId = 2, Name = "Рывок классический", Description = string.Empty },
            new() { Id = 51, TypeId = 2, Name = "Толчок. Взятие на грудь", Description = string.Empty },
            new() { Id = 52, TypeId = 2, Name = "Толчок с груди", Description = string.Empty },
            new() { Id = 53, TypeId = 2, Name = "Толчок классический", Description = string.Empty },
            new() { Id = 54, TypeId = 2, Name = "XXX", Description = string.Empty },
            new() { Id = 55, TypeId = 2, Name = "ЯЯЯ", Description = string.Empty },
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
            new ExerciseDb(){ Id = TestConstants.ExType2Id, ExerciseTypeId = 2, ExerciseSubTypeId = 50, Name = "Рывок классический", Description = string.Empty},
            new ExerciseDb(){ Id = 2, ExerciseTypeId = 2, ExerciseSubTypeId = 50, Name = "Рывок без разброса ног", Description = string.Empty},
            new ExerciseDb(){ Id = 20, ExerciseTypeId = 1, ExerciseSubTypeId = 51, Name = "Взятие на грудь без разброса ног", Description = string.Empty},
            new ExerciseDb(){ Id = 21, ExerciseTypeId = 1, ExerciseSubTypeId = 51, Name = "Взятие на грудь с подставки", Description = string.Empty},
            new ExerciseDb(){ Id = TestConstants.ExType1Id, ExerciseTypeId = 1, ExerciseSubTypeId = 52, Name = "Приседания на груди + толчок", Description = string.Empty},
            new ExerciseDb(){ Id = 41, ExerciseTypeId = 1, ExerciseSubTypeId = 52, Name = "Толчок со стоек", Description = string.Empty},
            new ExerciseDb(){ Id = 60, ExerciseTypeId = 1, ExerciseSubTypeId = 53, Name = "Толчок классический", Description = string.Empty},
            new ExerciseDb(){ Id = 61, ExerciseTypeId = 1, ExerciseSubTypeId = 53, Name = "Взятие на грудь + толчок", Description = string.Empty},
            new ExerciseDb(){ Id = 90, ExerciseTypeId = 2, ExerciseSubTypeId = 54, Name = "Тяга рывковая", Description = string.Empty},
            new ExerciseDb(){ Id = 91, ExerciseTypeId = 2, ExerciseSubTypeId = 54, Name = "Тяга рывковая с подставки", Description = string.Empty},

            new ExerciseDb(){ Id = TestConstants.ExType3Id, ExerciseTypeId = 3, ExerciseSubTypeId = 55, Name = "Приседания", Description = string.Empty},
        });
    }
}
