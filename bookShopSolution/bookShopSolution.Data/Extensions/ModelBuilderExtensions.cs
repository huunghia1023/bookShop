using bookShopSolution.Data.Entities;
using bookShopSolution.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace bookShopSolution.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppConfig>().HasData(
                new AppConfig() { Key = "HomeTitle", Value = "This is home page of bookShopSolution" },
                new AppConfig() { Key = "HomeKeyword", Value = "This is keyword of bookShopSolution" },
                new AppConfig() { Key = "HomeDescription", Value = "This is description of bookShopSolution" });
            modelBuilder.Entity<Language>().HasData(
                new Language() { Id = "vi", LanguageName = "Tiếng Việt", IsDefault = true },
                new Language() { Id = "en", LanguageName = "English", IsDefault = false });
            modelBuilder.Entity<Category>().HasData(
                new Category() { Id = 1, IsShowOnHome = true, ParentId = null, SortOrder = 0, Status = Status.Active },
                new Category() { Id = 2, IsShowOnHome = true, ParentId = null, SortOrder = 0, Status = Status.Active });
            modelBuilder.Entity<CategoryTranslation>().HasData(
                new CategoryTranslation() { Id = 1, CategoryId = 1, CategoryName = "Văn Học", LanguageId = "vi", SeoAlias = "van-hoc", SeoDescription = "Sách thể loại văn học", SeoTitle = "Sách văn học" },
                new CategoryTranslation() { Id = 2, CategoryId = 1, CategoryName = "Literary", LanguageId = "en", SeoAlias = "literary", SeoDescription = "literary", SeoTitle = "Literary Book" },
                new CategoryTranslation() { Id = 3, CategoryId = 2, CategoryName = "Kinh Tế", LanguageId = "vi", SeoAlias = "kinh-te", SeoDescription = "Sách thuộc thể loại kinh tế", SeoTitle = "Sách kinh tế" },
                new CategoryTranslation() { Id = 4, CategoryId = 2, CategoryName = "Economic", LanguageId = "en", SeoAlias = "economic", SeoDescription = "economic", SeoTitle = "Economic Book" });
            // role and user
            var roleId = new Guid("6A768150-5DE9-48D0-97DF-9D1542314334");
            var adminId = new Guid("BAB47F6A-CA90-4FC2-A18D-484060A1332B");
            modelBuilder.Entity<AppRole>().HasData(
                new AppRole() { Id = roleId, Name = "admin", NormalizedName = "admin", Description = "Adminstrator role" }
                );
            var hasher = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(
                new AppUser() { Id = adminId, FirstName = "Nghia", LastName = "Gia", Email = "nguyengiahuunghia118@gmail.com", BirthDay = new DateTime(1999, 6, 8), EmailConfirmed = true, NormalizedEmail = "nguyengiahuunghia118@gmail.com", UserName = "admin", PasswordHash = hasher.HashPassword(null, "N123456@"), NormalizedUserName = "admin", SecurityStamp = "" }
                );
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid> { RoleId = roleId, UserId = adminId }
                );
            // role and user
            var roleCustomerId = new Guid("7297BE39-5977-40AF-9AAF-4B57B21B24C1");
            var customerId = new Guid("5F9EC3C0-6F07-4103-AB0A-413F961C8B06");
            modelBuilder.Entity<AppRole>().HasData(
                new AppRole() { Id = roleCustomerId, Name = "customer", NormalizedName = "customer", Description = "Customer role" }
                );

            modelBuilder.Entity<AppUser>().HasData(
                new AppUser() { Id = customerId, FirstName = "Nghia", LastName = "Dev", Email = "nghiadev@gmail.com", BirthDay = new DateTime(1999, 6, 8), EmailConfirmed = true, NormalizedEmail = "nghiadev@gmail.com", UserName = "customer", PasswordHash = hasher.HashPassword(null, "N1234@"), NormalizedUserName = "customer", SecurityStamp = "" }
                );
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid> { RoleId = roleCustomerId, UserId = customerId }
                );
        }
    }
}