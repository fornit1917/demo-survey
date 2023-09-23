using DemoSurvey.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoSurvey.Web.Db;

public class SurveyDbContext : DbContext
{
    public DbSet<VoteModel> Votes { get; set; }

    public SurveyDbContext(DbContextOptions<SurveyDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VoteModel>().ToTable("votes");

        modelBuilder.Entity<VoteModel>()
            .HasKey(x => new { x.UserId, x.SurveyItemId });

        modelBuilder.Entity<VoteModel>()
            .Property(x => x.UserId)
            .HasColumnName("user_id");

        modelBuilder.Entity<VoteModel>()
            .Property(x => x.SurveyItemId)
            .HasColumnName("survey_item_id");
    }
}