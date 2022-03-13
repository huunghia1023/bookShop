using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.Data.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int Star { get; set; }
        public string Comment { get; set; }
        public int ProductId { get; set; }
        public Guid UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public Product Product { get; set; }
        public AppUser AppUser { get; set; }
    }
}