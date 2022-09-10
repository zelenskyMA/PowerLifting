using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels;
using PowerLifting.Domain.DbModels.TrainingPlan;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Models.UserData;

namespace PowerLifting.Infrastructure
{
    public class LiftingContext : DbContext
    {
        public DbSet<PlanDb> Plans { get; set; }
        public DbSet<PlanDayDb> PlanDays { get; set; }
        public DbSet<PlanExerciseDb> PlanExercises { get; set; }
        public DbSet<PlanExerciseSettingsDb> PlanExerciseSettings { get; set; }
        public DbSet<ExerciseDb> Exercises { get; set; }
        public DbSet<PercentageDb> Percentages { get; set; }

        public DbSet<DictionaryDb> Dictionaries { get; set; }
        public DbSet<DictionaryTypeDb> DictionaryTypes { get; set; }

        public DbSet<UserDb> Users { get; set; }
        public DbSet<UserAchivementDb> UserAchivements { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAchivementDb>().HasKey(c => new { c.UserId, c.ExerciseTypeId, c.CreationDate });
        }

        public LiftingContext(DbContextOptions<LiftingContext> options) : base(options) { }
    }
}
