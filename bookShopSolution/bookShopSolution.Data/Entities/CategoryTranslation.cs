using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.Data.Entities
{
    public class CategoryTranslation
    {
        public int CategoryTranslationId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string SeoDescription { get; set; }
        public string SeoTitle { get; set; }
        public int LanguageId { get; set; }
        public string SeoAlias { get; set; }
        public Category Category { get; set; }
        public Language Language { get; set; }

    }
}
