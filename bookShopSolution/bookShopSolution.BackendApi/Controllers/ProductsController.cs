using bookShopSolution.Application.Catalog.Products;
using bookShopSolution.ViewModels.Catalog.ProductImages;
using bookShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bookShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IPublicProductService _publicProductService;
        private readonly IManageProductService _manageProductService;

        public ProductsController(IPublicProductService publicProductService, IManageProductService manageProductService)
        {
            _publicProductService = publicProductService;
            _manageProductService = manageProductService;
        }

        [HttpGet("{languageId}")]
        public async Task<IActionResult> GetAllPaging(int languageId, [FromQuery] GetPublicProductPagingRequest request)
        {
            var products = await _publicProductService.GetAllByCategoryId(languageId, request);
            return Ok(products);
        }

        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId, int languageId)
        {
            var product = await _manageProductService.GetProductById(productId, languageId);
            if (product == null)
            {
                return BadRequest("Can not find product");
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var productId = await _manageProductService.Create(request);
            if (productId == 0) return BadRequest("Create failed");
            var product = await _manageProductService.GetProductById(productId, request.LanguageId);
            return CreatedAtAction(nameof(GetById), new { id = productId, product });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var updatedCount = await _manageProductService.Update(request);
            if (updatedCount == 0)
                return BadRequest("Update failed");

            return Ok();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var result = await _manageProductService.Delete(productId);
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
            var isUpdateSuccess = await _manageProductService.UpdatePrice(productId, newPrice);
            if (isUpdateSuccess)
                return Ok();
            return BadRequest();
        }

        [HttpGet("{productId}/images/{imageId}")]
        public async Task<IActionResult> GetImageById(int productId, int imageId)
        {
            var image = _manageProductService.GetImageById(imageId);
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
            var imageIdCreated = await _manageProductService.AddImage(productId, request);
            if (imageIdCreated == 0)
            {
                return BadRequest();
            }
            var imageCreated = await _manageProductService.GetImageById(imageIdCreated);
            return CreatedAtAction(nameof(GetImageById), new { id = imageIdCreated }, imageCreated);
        }

        [HttpPut("{productId}/images/{imageId}")]
        public async Task<IActionResult> UpdateImage(int imageId, [FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var updatedCount = await _manageProductService.UpdateImage(imageId, request);
            if (updatedCount == 0)
                return BadRequest("Update failed");

            return Ok();
        }

        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<IActionResult> RemoveImage(int imageId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _manageProductService.RemoveImage(imageId);
            if (result == 0)
                return BadRequest("Delete failed");

            return Ok();
        }
    }
}