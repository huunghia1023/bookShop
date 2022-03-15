using bookShopSolution.Data.Configurations;
using bookShopSolution.Data.Entities;
using bookShopSolution.Data.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace bookShopSolution.Data.EF
{
    public class BookShopDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public BookShopDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure using  fluent  api
            modelBuilder.ApplyConfiguration(new AppConfigConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductInCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryTranslationConfiguration());
            modelBuilder.ApplyConfiguration(new ContactConfiguration());
            modelBuilder.ApplyConfiguration(new LanguageConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new ProductImageConfiguration());
            modelBuilder.ApplyConfiguration(new ProductTranslationConfiguration());
            modelBuilder.ApplyConfiguration(new PromotionConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
            modelBuilder.ApplyConfiguration(new AppUserConfiguration());
            modelBuilder.ApplyConfiguration(new AppRoleConfiguration());
            modelBuilder.ApplyConfiguration(new ReviewConfiguration());

            // config identity
            modelBuilder.Entity<AppUser>(b =>
            {
                //// Primary key
                //b.HasKey(u => u.Id);

                //// Indexes for "normalized" username and email, to allow efficient lookups
                //b.HasIndex(u => u.NormalizedUserName).HasName("UserNameIndex").IsUnique();
                //b.HasIndex(u => u.NormalizedEmail).HasName("EmailIndex");

                //// Maps to the AspNetUsers table
                //b.ToTable("AppUsers");

                //// A concurrency token for use with the optimistic concurrency checking
                //b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

                //// Limit the size of columns to use efficient database types
                //b.Property(u => u.UserName).HasMaxLength(256);
                //b.Property(u => u.NormalizedUserName).HasMaxLength(256);
                //b.Property(u => u.Email).HasMaxLength(256);
                //b.Property(u => u.NormalizedEmail).HasMaxLength(256);

                // The relationships between User and other entity types
                // Note that these relationships are configured with no navigation properties

                // Each User can have many UserClaims
                b.HasMany<IdentityUserClaim<Guid>>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();

                // Each User can have many UserLogins
                b.HasMany<IdentityUserLogin<Guid>>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

                // Each User can have many UserTokens
                b.HasMany<IdentityUserToken<Guid>>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany<IdentityUserRole<Guid>>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
            });

            modelBuilder.Entity<IdentityUserClaim<Guid>>(b =>
            {
                // Primary key
                b.HasKey(uc => uc.Id);

                // Maps to the AspNetUserClaims table
                b.ToTable("AppUserClaims");
            });

            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

            //modelBuilder.Entity<IdentityUserLogin<Guid>>(b =>
            //{
            //    // Composite primary key consisting of the LoginProvider and the key to use
            //    // with that provider
            //    b.HasKey(l => new { l.LoginProvider, l.ProviderKey });

            //    // Limit the size of the composite key columns due to common DB restrictions
            //    b.Property(l => l.LoginProvider).HasMaxLength(128);
            //    b.Property(l => l.ProviderKey).HasMaxLength(128);

            //    // Maps to the AspNetUserLogins table
            //    b.ToTable("AspNetUserLogins");
            //});

            modelBuilder.Entity<IdentityUserToken<Guid>>(b =>
            {
                // Composite primary key consisting of the UserId, LoginProvider and Name
                b.HasKey(x => x.UserId);

                // Limit the size of the composite key columns due to common DB restrictions
                //b.Property(t => t.LoginProvider).HasMaxLength(200);
                //b.Property(t => t.Name).HasMaxLength(200);

                // Maps to the AspNetUserTokens table
                b.ToTable("AppUserTokens");
            });

            modelBuilder.Entity<AppRole>(b =>
            {
                //// Primary key
                //b.HasKey(r => r.Id);

                //// Index for "normalized" role name to allow efficient lookups
                //b.HasIndex(r => r.NormalizedName).HasName("RoleNameIndex").IsUnique();

                //// Maps to the AspNetRoles table
                //b.ToTable("AppRoles");

                //// A concurrency token for use with the optimistic concurrency checking
                //b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

                //// Limit the size of columns to use efficient database types
                //b.Property(u => u.Name).HasMaxLength(256);
                //b.Property(u => u.NormalizedName).HasMaxLength(256);

                // The relationships between Role and other entity types
                // Note that these relationships are configured with no navigation properties

                // Each Role can have many entries in the UserRole join table
                b.HasMany<IdentityUserRole<Guid>>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();

                // Each Role can have many associated RoleClaims
                b.HasMany<IdentityRoleClaim<Guid>>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
            });

            modelBuilder.Entity<IdentityRoleClaim<Guid>>(b =>
            {
                // Primary key
                b.HasKey(rc => rc.Id);

                // Maps to the AspNetRoleClaims table
                b.ToTable("AppRoleClaims");
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>(b =>
            {
                // Primary key
                b.HasKey(r => new { r.UserId, r.RoleId });

                // Maps to the AspNetUserRoles table
                b.ToTable("AppUserRoles");
            });

            //modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
            //modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.UserId, x.RoleId });

            //modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
            //modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => x.UserId);
            // seeding data
            modelBuilder.Seed();

            // base.OnModelCreating(modelBuilder);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductTranslation> ProductTranslations { get; set; }
        public DbSet<AppConfig> AppConfigs { get; set; }
        public DbSet<ProductInCategory> ProductInCategories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}