using bookShopSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.Data.Entities
{
    public class Promotion
    {
        public int PromotionId { get; set; }
        public DateTime FromDay { get; set; }
        public DateTime Today { get; set; }
        public bool ApplyForAll { get; set; }
        public int? DiscountPercent { get; set; }
        public decimal? DiscountAmount { get; set; }
        public string ProductIds { get; set; }
        public string ProductCategoryIds { get; set; }
        public Status Status { get; set; }
        public string PromotionName { get; set; }
        public string Description { get; set; }

    }
}
