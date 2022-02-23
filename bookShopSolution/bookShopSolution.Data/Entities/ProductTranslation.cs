﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.Data.Entities
{
    public class ProductTranslation
    {
        public int ProductTranslationId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public string SeoDescription { get; set; }
        public string SeoTitle { get; set; }
        public int LanguageId { get; set; }
        public string SeoAlias { get; set; }
        public Product Product { get; set; }
        public Language Language { get; set; }

    }
}
