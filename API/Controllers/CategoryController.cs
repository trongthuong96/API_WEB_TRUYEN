using API.Models;
using API.Repository.IRepository;
using API.Utility;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of category story.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CategoryDto>))]
        public IActionResult GetCategories()
        {
            var objList = _categoryRepository.GetCategories();

            var objDto = new List<CategoryDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<CategoryDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get individual category story
        /// </summary>
        /// <param name="categoryId">The Id of the category story</param>
        /// <returns></returns>
        [HttpGet("{categoryId:Guid}", Name = "GetCategory")]
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetCategory(Guid categoryId)
        {
            var obj = _categoryRepository.GetCategory(categoryId);
            if(obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<CategoryDto>(obj);
            return Ok(objDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CategoryDto))]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateCategory([FromBody] CategoryCreateDto categoryCreateDto)
        {
            if(categoryCreateDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_categoryRepository.CategoryExists(categoryCreateDto.Name))
            {
                ModelState.AddModelError("", "Đã tồn tại thể loại này!");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryObj = _mapper.Map<Category>(categoryCreateDto);

            if (!_categoryRepository.CreateOrUpdateCategory(categoryObj))
            {
                ModelState.AddModelError("", $"Đã xảy ra sự cố khi lưu {categoryObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategory", new { categoryId = categoryObj.Id}, categoryObj);
        }

        [HttpPatch("{categoryId:Guid}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateCategory(Guid categoryId, [FromBody]CategoryDto categoryDto)
        {
            if (categoryDto == null || categoryId != categoryDto.Id)
            {
                return BadRequest(ModelState);
            }

            var categoryObj = _mapper.Map<Category>(categoryDto);

            if (!_categoryRepository.CreateOrUpdateCategory(categoryObj))
            {
                ModelState.AddModelError("", $"Đã xảy ra sự cố khi cập nhập {categoryObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{categoryId:Guid}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteCategory(Guid categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var categoryObj = _categoryRepository.GetCategory(categoryId);

            if (!_categoryRepository.DeleteCategory(categoryObj))
            {
                ModelState.AddModelError("", $"Đã xảy ra sự cố khi xóa {categoryObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
