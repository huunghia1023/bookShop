using bookShopSolution.Application.Catalog.Categories;
using bookShopSolution.ViewModels.Catalog.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bookShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="admin")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(string languageId)
        {
            var category = await _categoryService.GetAll(languageId);
            return Ok(category);
        }

        [HttpGet("{categoryId}/{languageId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int categoryId, string languageId)
        {
            var category = await _categoryService.GetCategoryById(categoryId, languageId);
            if (category == null)
            {
                return BadRequest("Can not find category");
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CategoryCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var categoryId = await _categoryService.Create(request);
            if (categoryId == 0) return BadRequest("Create failed");
            var category = await _categoryService.GetCategoryById(categoryId, request.LanguageId);
            return CreatedAtAction(nameof(GetById), new { id = categoryId });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] CategoryUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _categoryService.Update(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPatch]
        public async Task<IActionResult> DeleteMultiple([FromForm] List<int> ids)
        {
            var result = await _categoryService.DeleteMultiple(ids);
            if (!result.IsSuccessed)
                return BadRequest(result);
            return Ok(result.Results);
        }
    }
}