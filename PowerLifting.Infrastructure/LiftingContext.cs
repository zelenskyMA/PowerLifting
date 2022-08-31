using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLifting.Infrastructure
{
  public class LiftingContext : DbContext
  {
  /*  public DbSet<Herb> Herbs { get; set; }
    public DbSet<Potion> Potions { get; set; }
    public DbSet<Organ> Organs { get; set; }

    public DbSet<ConHerbToPotion> HerbsToPotions { get; set; }
    public DbSet<ConHerbToOrgan> HerbsToOrgans { get; set; }
    public DbSet<ConPotionToOrgan> PotionsToOrgans { get; set; }

    public DbSet<Book> Books { get; set; }
    public DbSet<BookPage> BookPages { get; set; }*/

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
