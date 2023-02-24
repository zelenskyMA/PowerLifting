using Microsoft.EntityFrameworkCore;
using SportAssistant.Domain.DbModels.Basic;
using SportAssistant.Domain.DbModels.Coaching;
using SportAssistant.Domain.DbModels.TrainingPlan;
using SportAssistant.Domain.DbModels.TrainingTemplate;
using SportAssistant.Domain.DbModels.UserData;

namespace SportAssistant.Infrastructure.DataContext
{
    public class SportContext : DbContext
    {
        public DbSet<PlanDb> Plans { get; set; }
        public DbSet<PlanDayDb> PlanDays { get; set; }
        public DbSet<PlanExerciseDb> PlanExercises { get; set; }
        public DbSet<PlanExerciseSettingsDb> PlanExerciseSettings { get; set; }
        public DbSet<ExerciseDb> Exercises { get; set; }
        public DbSet<PercentageDb> Percentages { get; set; }

        public DbSet<TemplateSetDb> TemplateSets { get; set; }
        public DbSet<TemplatePlanDb> TemplatePlans { get; set; }
        public DbSet<TemplateDayDb> TemplateDays { get; set; }
        public DbSet<TemplateExerciseDb> TemplateExercises { get; set; }
        public DbSet<TemplateExerciseSettingsDb> TemplateExerciseSettings { get; set; }

        public DbSet<DictionaryDb> Dictionaries { get; set; }
        public DbSet<DictionaryTypeDb> DictionaryTypes { get; set; }
        public DbSet<SettingsDb> Settings { get; set; }
        public DbSet<EmailMessageDb> EmailMessages { get; set; }
                
        public DbSet<UserDb> Users { get; set; }
        public DbSet<UserInfoDb> UsersInfo { get; set; }
        public DbSet<UserAchivementDb> UserAchivements { get; set; }
        public DbSet<UserRoleDb> UserRoles { get; set; }
        public DbSet<UserBlockHistoryDb> UserBlockHistoryItems { get; set; }

        public DbSet<TrainingRequestDb> TrainingRequests { get; set; }
        public DbSet<TrainingGroupDb> TrainingGroups { get; set; }
        public DbSet<TrainingGroupUserDb> TrainingGroupUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAchivementDb>().HasKey(c => new { c.UserId, c.ExerciseTypeId, c.CreationDate });
            modelBuilder.Entity<UserRoleDb>().HasKey(c => new { c.UserId, c.RoleId });
            modelBuilder.Entity<UserInfoDb>().HasKey(c => new { c.UserId });

            modelBuilder.Entity<TrainingGroupUserDb>().HasKey(c => new { c.UserId, c.GroupId });
        }

        public SportContext(DbContextOptions<SportContext> options) : base(options) { }
    }
}
