using bookShopSolution.Application.Common;
using bookShopSolution.Data.EF;
using bookShopSolution.Data.Entities;
using bookShopSolution.Utilities.Exceptions;
using bookShopSolution.ViewModels.Catalog.ProductImages;
using bookShopSolution.ViewModels.Catalog.ProductRatings;
using bookShopSolution.ViewModels.Catalog.Products;
using bookShopSolution.ViewModels.common;
using bookShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace bookShopSolution.Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly BookShopDbContext _context;
        private readonly IStorageService _storageService;

        public ProductService(BookShopDbContext context, IStorageService storageService)
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
            if (request.Details == null)
            {
                request.Details = "";
            }
            if (request.SeoAlias == null)
            {
                request.SeoAlias = "";
            }
            if (request.SeoDescription == null)
            {
                request.SeoDescription = "";
            }
            if (request.SeoTitle == null)
            {
                request.SeoTitle = "";
            }
            var product = new Product()
            {
                Price = request.Price,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                IsFeatured = request.IsFeatured,
                DateModified = DateTime.Now,

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
            return product.Id;
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
            //var query = from p in _context.Products
            //            join pt in _context.ProductTranslations on p.Id equals pt.ProductId
            //            join pic in _context.ProductInCategories on p.Id equals pic.ProductId
            //            join c in _context.Categories on pic.CategoryId equals c.Id
            //            where pt.LanguageId == request.LanguageId
            //            select new { p, pt, c };
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        where pt.LanguageId == request.LanguageId && pi.IsDefault == true
                        select new { p, pt, pic, pi };

            //filter by keyword search and categoryid
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.pt.ProductName.Contains(request.Keyword));
            }
            if (request.CategoryId != null && request.CategoryId != 0)
            {
                query = query.Where(x => x.pic.CategoryId == request.CategoryId);
            }
            var a = await query.ToListAsync();
            // paging
            var totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    Name = x.pt.ProductName,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    Thumbnail = x.pi.ImagePath,
                    IsFeatured = x.p.IsFeatured,
                    LikeCount = x.p.LikeCount
                }).ToListAsync();
            // set result to PageResult and return
            var pageResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
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

        public async Task<PagedResult<ProductImageViewModel>> GetListImages(int productId)
        {
            var data = await _context.ProductImages.Where(x => x.ProductId == productId).
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
            var totalRow = data.Count();
            // set result to PageResult and return
            var pageResult = new PagedResult<ProductImageViewModel>()
            {
                TotalRecord = totalRow,
                Items = data
            };
            return pageResult;
        }

        public async Task<ProductViewModel> GetProductById(int productId, string languageId)
        {
            var product = await _context.Products.FindAsync(productId);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == productId && x.LanguageId == languageId);
            var categories = await (from c in _context.Categories
                                    join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                                    join pic in _context.ProductInCategories on c.Id equals pic.CategoryId
                                    where pic.ProductId == productId && ct.LanguageId == languageId
                                    select ct.CategoryName).ToListAsync();
            var image = _context.ProductImages.Where(x => x.ProductId == productId && x.IsDefault == true).FirstOrDefault();
            var productModel = new ProductViewModel()
            {
                Id = product.Id,
                DateCreated = product.DateCreated,
                Description = productTranslation.Description,
                Details = productTranslation.Details,
                OriginalPrice = product.OriginalPrice,
                Price = product.Price,
                Name = productTranslation.ProductName,
                SeoAlias = productTranslation.SeoAlias,
                SeoDescription = productTranslation.SeoDescription,
                SeoTitle = productTranslation.SeoTitle,
                Stock = product.Stock,
                ViewCount = product.ViewCount,
                Thumbnail = image.ImagePath,
                IsFeatured = product.IsFeatured,
                LikeCount = product.LikeCount,
                LanguageId = languageId
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

        public async Task<int> Update(int productId, ProductUpdateRequest request)
        {
            if (request.Details == null)
            {
                request.Details = "";
            }
            if (request.SeoDescription == null)
            {
                request.SeoDescription = "";
            }
            if (request.SeoTitle == null)
            {
                request.SeoTitle = "";
            }
            if (request.SeoAlias == null)
            {
                request.SeoAlias = "";
            }
            var product = await _context.Products.FindAsync(productId);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == productId && x.LanguageId == request.LanguageId);
            if (product == null || productTranslation == null) 
                return 0;
            productTranslation.ProductName = request.ProductName;
            productTranslation.Description = request.Description;
            productTranslation.Details = request.Details;
            productTranslation.SeoDescription = request.SeoDescription;
            productTranslation.SeoTitle = request.SeoTitle;
            productTranslation.SeoAlias = request.SeoAlias;

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

        public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request)
        {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        where pt.LanguageId == languageId
                        select new { p, pt, pic };
            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(x => x.pic.CategoryId == request.CategoryId);
            }
            // paging
            var totalRow = await query.CountAsync();
            var data = await query.Distinct().Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    Name = x.pt.ProductName,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();
            var pagedResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
            return pagedResult;
        }

        public async Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return new ApiErrorResult<bool>($"Can not find product with id {id}");
            }
            foreach (var category in request.Categories)
            {
                // check category
                var categoryDb = await _context.Categories.FindAsync(category.Id);
                if (categoryDb == null)
                {
                    return new ApiErrorResult<bool>($"Can not find category with id {category.Id}");
                }
                var productInCategory = await _context.ProductInCategories.FirstOrDefaultAsync(x => x.CategoryId == category.Id && x.ProductId == id);
                if (productInCategory != null && category.Selected == false)
                {
                    _context.ProductInCategories.Remove(productInCategory);
                }
                else if (productInCategory == null && category.Selected == true)
                {
                    await _context.ProductInCategories.AddAsync(new ProductInCategory()
                    {
                        CategoryId = category.Id,
                        ProductId = id
                    });
                }
            }
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<List<ProductViewModel>> GetFeaturedProduct(string languageId, int take)
        {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        where pt.LanguageId == languageId && p.IsFeatured && (pi == null || pi.IsDefault == true)
                        select new { p, pt, pi };
            var data = await query.Distinct().Take(take).Select(x => new ProductViewModel
            {
                Id = x.p.Id,
                Name = x.pt.ProductName,
                DateCreated = x.p.DateCreated,
                Description = x.pt.Description,
                Details = x.pt.Details,
                Price = x.p.Price,
                OriginalPrice = x.p.OriginalPrice,
                SeoAlias = x.pt.SeoAlias,
                SeoDescription = x.pt.SeoDescription,
                SeoTitle = x.pt.SeoTitle,
                Stock = x.p.Stock,
                ViewCount = x.p.ViewCount,
                IsFeatured = x.p.IsFeatured,
                LikeCount = x.p.LikeCount,
                Thumbnail = x.pi.ImagePath
            }).ToListAsync();
            return data;
        }

        public async Task<List<ProductViewModel>> GetLatestProduct(string languageId, int take)
        {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        where pt.LanguageId == languageId && (pi == null || pi.IsDefault == true)
                        select new { p, pt, pi };
            var data = await query.Distinct().OrderByDescending(i => i.p.DateCreated).Take(take).Select(x => new ProductViewModel
            {
                Id = x.p.Id,
                Name = x.pt.ProductName,
                DateCreated = x.p.DateCreated,
                Description = x.pt.Description,
                Details = x.pt.Details,
                Price = x.p.Price,
                OriginalPrice = x.p.OriginalPrice,
                SeoAlias = x.pt.SeoAlias,
                SeoDescription = x.pt.SeoDescription,
                SeoTitle = x.pt.SeoTitle,
                Stock = x.p.Stock,
                ViewCount = x.p.ViewCount,
                IsFeatured = x.p.IsFeatured,
                LikeCount = x.p.LikeCount,
                Thumbnail = x.pi.ImagePath
            }).ToListAsync();
            return data;
        }

        public async Task<List<ProductViewModel>> GetTopViewProduct(string languageId, int take)
        {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        where pt.LanguageId == languageId && (pi == null || pi.IsDefault == true)
                        select new { p, pt, pi };
            var data = await query.Distinct().OrderByDescending(i => i.p.ViewCount).Take(take).Select(x => new ProductViewModel
            {
                Id = x.p.Id,
                Name = x.pt.ProductName,
                DateCreated = x.p.DateCreated,
                Description = x.pt.Description,
                Details = x.pt.Details,
                Price = x.p.Price,
                OriginalPrice = x.p.OriginalPrice,
                SeoAlias = x.pt.SeoAlias,
                SeoDescription = x.pt.SeoDescription,
                SeoTitle = x.pt.SeoTitle,
                Stock = x.p.Stock,
                ViewCount = x.p.ViewCount,
                IsFeatured = x.p.IsFeatured,
                LikeCount = x.p.LikeCount,
                Thumbnail = x.pi.ImagePath
            }).ToListAsync();
            return data;
        }

        public async Task<ApiResult<bool>> Rating(int productId, RatingRequest request)
        {
            var review = new Review()
            {
                ProductId = productId,
                UserId = request.UserId,
                Star = request.Star,
                DateCreated = DateTime.Now,
                Comment = request.Comment,
            };
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<List<ProductRatingViewModel>> GetAllRating(int productId, int take)
        {
            var query = from r in _context.Reviews
                        join p in _context.Products on r.ProductId equals p.Id
                        join u in _context.Users on r.UserId equals u.Id
                        where p.Id == productId
                        select new { r, p, u };

            var data = await query.OrderByDescending(x => x.r.DateCreated).Take(take)
                .Select(x => new ProductRatingViewModel()
                {
                    UserName = x.u.UserName,
                    Comment = x.r.Comment,
                    Star = x.r.Star,
                    DateCreated = x.r.DateCreated
                }).ToListAsync();
            // set result to PageResult and return

            return data;
        }
    }
}