using FixTest.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FixTest
{
    public class FixDbContext : DbContext
    {
        public FixDbContext(DbContextOptions<FixDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(BuildUserModel);
            modelBuilder.Entity<WebSite>(BuildWebSiteModel);
        }

        private void BuildWebSiteModel(EntityTypeBuilder<WebSite> builder)
        {
            builder.ToTable("WEB_SITE");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("ID").IsRequired();

            builder.Property(e => e.Url).HasColumnName("URL").IsRequired().HasMaxLength(128);
            builder.Property(e => e.LastCheckedAt).HasColumnName("LAST_CHECKED_AT").IsRequired(false);
            builder.Property(e => e.IsAvailable).HasColumnName("IS_AVAILABLE").IsRequired(false);
            builder.Property(e => e.CheckInterval).HasColumnName("CHECK_INTERVAL").IsRequired();
        }

        private void BuildUserModel(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("USER");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("ID").IsRequired();
            builder.Property(e => e.Login).HasColumnName("LOGIN").IsRequired().HasMaxLength(64);
            builder.Property(e => e.Password).HasColumnName("PASSWORD").IsRequired().HasMaxLength(256);
        }
    }
}