using bookShopSolution.Application.Catalog.Products;
using bookShopSolution.ViewModels.Catalog.ProductImages;
using bookShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bookShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("paging")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetManageProductPagingRequest request)
        {
            var products = await _productService.GetAllPaging(request);
            return Ok(products);
        }

        //[HttpGet("{languageId}")]
        //public async Task<IActionResult> GetAllPaging(string languageId, [FromQuery] GetPublicProductPagingRequest request)
        //{
        //    var products = await _productService.GetAllByCategoryId(languageId, request);
        //    return Ok(products);
        //}

        [HttpGet("{productId}/{languageId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var product = await _productService.GetProductById(productId, languageId);
            if (product == null)
            {
                return BadRequest("Can not find product");
            }
            return Ok(product);
        }

        [HttpGet("featured/{languageId}/{take}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFeatured(int take, string languageId)
        {
            var products = await _productService.GetFeaturedProduct(languageId, take);
            return Ok(products);
        }

        [HttpGet("viewest/{languageId}/{take}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTopView(int take, string languageId)
        {
            var products = await _productService.GetTopViewProduct(languageId, take);
            return Ok(products);
        }

        [HttpGet("latest/{languageId}/{take}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLatest(int take, string languageId)
        {
            var products = await _productService.GetLatestProduct(languageId, take);
            return Ok(products);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var productId = await _productService.Create(request);
            if (productId == 0) return BadRequest("Create failed");
            var product = await _productService.GetProductById(productId, request.LanguageId);
            return CreatedAtAction(nameof(GetById), new { id = productId });
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> Update(int productId, [FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var updatedCount = await _productService.Update(productId, request);
            if (updatedCount == 0)
                return BadRequest("Update failed");

            return Ok();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var result = await _productService.Delete(productId);
            if (result == 0)
                return BadRequest("Delete failed");

            return Ok();
        }

        //update 1 phan
        [HttpPatch("{productId}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var isUpdateSuccess = await _productService.UpdatePrice(productId, newPrice);
            if (isUpdateSuccess)
                return Ok();
            return BadRequest();
        }

        [HttpGet("{productId}/images")]
        public async Task<IActionResult> GetAllImage(int productId)
        {
            var images = await _productService.GetListImages(productId);
            return Ok(images);
        }

        [HttpGet("{productId}/images/{imageId}")]
        public async Task<IActionResult> GetImageById(int productId, int imageId)
        {
            var image = _productService.GetImageById(imageId);
            if (image.Result == null)
            {
                return BadRequest();
            }
            return Ok(image.Result);
        }

        [HttpPost("{productId}/images")]
        public async Task<IActionResult> CreateImage(int productId, [FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var imageIdCreated = await _productService.AddImage(productId, request);
            if (imageIdCreated == 0)
            {
                return BadRequest();
            }
            var imageCreated = await _productService.GetImageById(imageIdCreated);
            return CreatedAtAction(nameof(GetImageById), new { id = imageIdCreated }, imageCreated);
        }

        [HttpPost("{productId}/rating")]
        public async Task<IActionResult> Rating(int productId, [FromBody] RatingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productService.Rating(productId, request);

            return Ok(result);
        }

        [HttpGet("{productId}/rating/{take}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRatingById(int productId, int take)
        {
            var reviews = await _productService.GetAllRating(productId, take);
            return Ok(reviews);
        }

        [HttpPut("{productId}/images/{imageId}")]
        public async Task<IActionResult> UpdateImage(int imageId, [FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var updatedCount = await _productService.UpdateImage(imageId, request);
            if (updatedCount == 0)
                return BadRequest("Update failed");

            return Ok();
        }

        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<IActionResult> RemoveImage(int imageId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _productService.RemoveImage(imageId);
            if (result == 0)
                return BadRequest("Delete failed");

            return Ok();
        }

        [HttpPut("{id}/categories")]
        public async Task<IActionResult> CategoryAssign(int id, [FromBody] CategoryAssignRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productService.CategoryAssign(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("{id}/addview")]
        public async Task<IActionResult> AddViewCount(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _productService.AddViewCount(id);

            return Ok();
        }
    }
}