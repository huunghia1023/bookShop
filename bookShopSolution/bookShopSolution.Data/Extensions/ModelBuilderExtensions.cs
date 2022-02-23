using bookShopSolution.Data.Entities;
using bookShopSolution.Data.Enums;
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
                new Language() { LanguageId = 1, LanguageCOD = "vi-VN", LanguageName = "Tiếng Việt", IsDefault = true },
                new Language() { LanguageId = 2, LanguageCOD = "en-US", LanguageName = "English", IsDefault = false });
            modelBuilder.Entity<Category>().HasData(
                new Category() { CategoryId = 1, IsShowOnHome = true, ParentId = null, SortOrder = 0, Status = Status.Active },
                new Category() { CategoryId = 2, IsShowOnHome = true, ParentId = null, SortOrder = 0, Status = Status.Active });
            modelBuilder.Entity<CategoryTranslation>().HasData(
                new CategoryTranslation() { CategoryTranslationId = 1, CategoryId = 1, CategoryName = "Văn Học", LanguageId = 1, SeoAlias = "van-hoc", SeoDescription = "Sách thể loại văn học", SeoTitle = "Sách văn học" },
                new CategoryTranslation() { CategoryTranslationId = 2, CategoryId = 1, CategoryName = "Literary", LanguageId = 2, SeoAlias = "literary", SeoDescription = "literary", SeoTitle = "Literary Book" },
                new CategoryTranslation() { CategoryTranslationId = 3, CategoryId = 2, CategoryName = "Kinh Tế", LanguageId = 1, SeoAlias = "kinh-te", SeoDescription = "Sách thuộc thể loại kinh tế", SeoTitle = "Sách kinh tế" },
                new CategoryTranslation() { CategoryTranslationId = 4, CategoryId = 2, CategoryName = "Economic", LanguageId = 2, SeoAlias = "economic", SeoDescription = "economic", SeoTitle = "Economic Book" });
        }
    }
}
