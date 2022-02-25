using bookShopSolution.Data.EF;
using bookShopSolution.ViewModels.Catalog.Products;
using bookShopSolution.ViewModels.common;
using Microsoft.EntityFrameworkCore;

namespace bookShopSolution.Application.Catalog.Products
{
    public class PublicProductService : IPublicProductService
    {
        private BookShopDbContext _context;
        public PublicProductService(BookShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductViewModel>> GetAll()
        {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.ProductId equals pt.ProductId
                        join pic in _context.ProductInCategories on p.ProductId equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.CategoryId
                        select new { p, pt, pic };
            
            var data = await query.Select(x => new ProductViewModel()
                {
                    ProductId = x.p.ProductId,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    ProductName = x.pt.ProductName,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();
            return data;
        }

        public async Task<PagedResult<ProductViewModel>> GetAllByCategory(GetPublicProductPagingRequest request)
        {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.ProductId equals pt.ProductId
                        join pic in _context.ProductInCategories on p.ProductId equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.CategoryId
                        select new { p, pt, pic };
            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(x => x.pic.CategoryId == request.CategoryId);
            }
            // paging
            var totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    ProductId = x.p.ProductId,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    ProductName = x.pt.ProductName,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();
            var pagedResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data
            };
            return pagedResult;
        }
    }
}
