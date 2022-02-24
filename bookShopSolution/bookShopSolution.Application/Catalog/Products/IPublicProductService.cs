using bookShopSolution.ViewModels.Catalog.Products;
using bookShopSolution.ViewModels.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.Application.Catalog.Products
{
    public interface IPublicProductService
    {
        Task<PagedResult<ProductViewModel>> GetAllByCategory(GetPublicProductPagingRequest request);
    }
}
