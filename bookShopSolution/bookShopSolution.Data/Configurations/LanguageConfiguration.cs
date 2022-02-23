using bookShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.Data.Configurations
{
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.ToTable("Languages");
            builder.HasKey(x => x.LanguageId);
            builder.Property(x => x.LanguageId).UseIdentityColumn();
            builder.Property(x => x.LanguageCOD).IsRequired().HasMaxLength(5).IsUnicode(false);
            builder.Property(x => x.LanguageName).IsRequired().HasMaxLength(30);
            builder.Property(x => x.IsDefault).HasDefaultValue(true);
        }
    }
}
