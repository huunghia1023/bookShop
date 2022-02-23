using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.Data.Entities
{
    public class Language
    {
        public int LanguageId { get; set; }
        public string LanguageCOD { get; set; }
        public string LanguageName { get; set; }
        public bool IsDefault { get; set; }
        public List<CategoryTranslation> CategoryTranslations { get; set; }
        public List<ProductTranslation> ProductTranslations { get; set; }

    }
}
