namespace promoCodeApp.Models
{
    using System.Data.Entity;

    public partial class promoCodeDbContext : DbContext
    {
        public promoCodeDbContext()
            : base("name=promoCodeDbContext")
        {
        }

        public virtual DbSet<game> games { get; set; }
        public virtual DbSet<gamePackage> gamePackages { get; set; }
        public virtual DbSet<promotion> promotions { get; set; }
        public virtual DbSet<registration> registrations { get; set; }
        public virtual DbSet<type> types { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<game>()
                .Property(e => e.gameName)
                .IsUnicode(false);

            modelBuilder.Entity<game>()
                .HasMany(e => e.gamePackages)
                .WithRequired(e => e.game)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<gamePackage>()
                .Property(e => e.packageName)
                .IsUnicode(false);

            modelBuilder.Entity<gamePackage>()
                .Property(e => e.secretKey)
                .IsUnicode(false);

            modelBuilder.Entity<gamePackage>()
                .HasMany(e => e.promotions)
                .WithRequired(e => e.gamePackage)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<promotion>()
                .Property(e => e.promotionCode)
                .IsUnicode(false);

            modelBuilder.Entity<registration>()
                .Property(e => e.registrationName)
                .IsUnicode(false);

            modelBuilder.Entity<registration>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<registration>()
                .Property(e => e.clientServerKey)
                .IsUnicode(false);

            modelBuilder.Entity<registration>()
                .HasMany(e => e.games)
                .WithRequired(e => e.registration)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<type>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<type>()
                .HasMany(e => e.promotions)
                .WithRequired(e => e.type)
                .WillCascadeOnDelete(false);
        }
    }
}
