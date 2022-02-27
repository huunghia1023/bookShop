using bookShopSolution.Application.Common;
using bookShopSolution.Data.EF;
using bookShopSolution.Data.Entities;
using bookShopSolution.Utilities.Exceptions;
using bookShopSolution.ViewModels.Catalog.ProductImages;
using bookShopSolution.ViewModels.Catalog.Products;
using bookShopSolution.ViewModels.common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace bookShopSolution.Application.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
        private readonly BookShopDbContext _context;
        private readonly IStorageService _storageService;

        public ManageProductService(BookShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                DateCreated = DateTime.Now,
                IsDefault = request.IsDefault,
                ProductId = productId,
                SortOrder = request.SortOrder
            };
            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();
            return productImage.ImageId;
        }

        public async Task AddViewCount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Price = request.Price,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation()
                    {
                        Description = request.Description,
                        Details = request.Details,
                        LanguageId = request.LanguageId,
                        ProductName = request.ProductName,
                        SeoAlias = request.SeoAlias,
                        SeoDescription = request.SeoDescription,
                        SeoTitle = request.SeoTitle
                    }
                }
            };
            // save image
            if (request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Thumbnail Image For Product",
                        DateCreated = DateTime.Now,
                        FileSize = request.ThumbnailImage.Length,
                        IsDefault = true,
                        SortOrder = 1,
                        ImagePath = await this.SaveFile(request.ThumbnailImage)
                    }
                };
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.ProductId;
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new BookShopException($"Can not find product {productId}");
            var images = _context.ProductImages.Where(i => i.ProductId == productId);
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.ImagePath);
            }

            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            // select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.ProductId equals pt.ProductId
                        join pic in _context.ProductInCategories on p.ProductId equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.CategoryId
                        select new { p, pt, c };

            //filter by keyword search and categoryid
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.pt.ProductName.Contains(request.Keyword));
            }
            if (request.CategoryIds.Count > 0)
            {
                query = query.Where(x => request.CategoryIds.Contains(x.c.CategoryId));
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
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    ProductName = x.pt.ProductName,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();
            // set result to PageResult and return
            var pageResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data
            };
            return pageResult;
        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
            {
                throw new BookShopException($"Can not find image with Id {imageId}");
            }
            var result = new ProductImageViewModel()
            {
                Caption = productImage.Caption,
                DateCreated = productImage.DateCreated,
                FileSize = productImage.FileSize,
                ImageId = imageId,
                ImagePath = productImage.ImagePath,
                IsDefault = productImage.IsDefault,
                ProductId = productImage.ProductId,
                SortOrder = productImage.SortOrder
            };
            return result;
        }

        public async Task<List<ProductImageViewModel>> GetListImages(int productId)
        {
            return await _context.ProductImages.Where(x => x.ProductId == productId).
                Select(i => new ProductImageViewModel()
                {
                    ProductId = productId,
                    Caption = i.Caption,
                    DateCreated = i.DateCreated,
                    FileSize = i.FileSize,
                    ImageId = i.ImageId,
                    ImagePath = i.ImagePath,
                    IsDefault = i.IsDefault,
                    SortOrder = i.SortOrder
                }).ToListAsync();
        }

        public async Task<ProductViewModel> GetProductById(int productId, int languageId)
        {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.ProductId equals pt.ProductId
                        select new { p, pt };
            var product = await query.FirstOrDefaultAsync(p => p.p.ProductId == productId && p.pt.LanguageId == languageId);
            if (product == null) throw new BookShopException($"Can not find product");
            var productModel = new ProductViewModel()
            {
                DateCreated = product.p.DateCreated,
                Description = product.pt.Description,
                Details = product.pt.Details,
                LanguageId = product.pt.LanguageId,
                OriginalPrice = product.p.OriginalPrice,
                Price = product.p.Price,
                ProductName = product.pt.ProductName,
                SeoAlias = product.pt.SeoAlias,
                SeoDescription = product.pt.SeoDescription,
                SeoTitle = product.pt.SeoTitle,
                Stock = product.p.Stock,
                ViewCount = product.p.ViewCount,
                ProductId = product.p.ProductId
            };
            return productModel;
        }

        public async Task<int> RemoveImage(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
            {
                throw new BookShopException($"Can not find image with id {imageId}");
            }
            _context.ProductImages.Remove(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.ProductId);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == request.ProductId);
            if (product == null || productTranslation == null) throw new BookShopException($"Can not find product with id {request.ProductId}");
            productTranslation.ProductName = request.ProductName;
            productTranslation.Description = request.Description;
            productTranslation.Details = request.Details;
            productTranslation.SeoDescription = request.SeoDescription;
            productTranslation.SeoTitle = request.SeoTitle;
            productTranslation.SeoAlias = request.SeoAlias;
            // save image
            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.IsDefault == true && i.ProductId == request.ProductId);
                if (thumbnailImage != null)
                {
                    thumbnailImage.FileSize = request.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    _context.ProductImages.Update(thumbnailImage);
                }
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
            {
                throw new BookShopException($"Can not find image with id {imageId}");
            }
            productImage.Caption = request.Caption;
            productImage.IsDefault = request.IsDefault;
            productImage.SortOrder = request.SortOrder;
            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Update(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new BookShopException($"Can not find product with id {productId}");
            product.Price = newPrice;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new BookShopException($"Can not find product with id {productId}");
            product.Stock += addedQuantity;
            return await _context.SaveChangesAsync() > 0;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
    }
}