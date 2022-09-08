using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels;
using PowerLifting.Domain.DbModels.TrainingPlan;

namespace PowerLifting.Infrastructure
{
    public class LiftingContext : DbContext
    {
        public DbSet<PlanDb> TrainingPlans { get; set; }
        public DbSet<PlanDayDb> TrainingDays { get; set; }
        public DbSet<PlanExerciseDb> PlannedExercises { get; set; }
        public DbSet<PlanExerciseSettingsDb> ExerciseSettings { get; set; }
        public DbSet<ExerciseDb> Exercises { get; set; }
        public DbSet<PercentageDb> Percentages { get; set; }

        public DbSet<DictionaryDb> Dictionaries { get; set; }
        public DbSet<DictionaryTypeDb> DictionaryTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* modelBuilder.Entity<ConHerbToPotion>().HasKey(c => new { c.Herb, c.Potion });
             modelBuilder.Entity<ConHerbToOrgan>().HasKey(c => new { c.Herb, c.Organ });
             modelBuilder.Entity<ConPotionToOrgan>().HasKey(c => new { c.Potion, c.Organ });

             modelBuilder.Entity<BookPage>().HasKey(c => new { c.Book, c.PageNumber });*/
        }

        public LiftingContext(DbContextOptions<LiftingContext> options) : base(options) { }
    }
}
