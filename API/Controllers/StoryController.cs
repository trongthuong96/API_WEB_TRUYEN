using API.DataAccess.Repository.IRepository;
using API.Models;
using API.Models.Models;
using API.Models.Models.Dtos;
using API.Repository;
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
    public class StoryController : Controller
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryStoryRepository _categoryStoryRepository;
        private readonly ITickRepository _tickRepository;
        private readonly IMapper _mapper;

        public StoryController(IStoryRepository storyRepository, IMapper mapper, IAuthorRepository authorRepository, ICategoryRepository categoryRepository, ICategoryStoryRepository categoryStoryRepository, ITickRepository tickRepository)
        {
            _storyRepository = storyRepository;
            _mapper = mapper;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _categoryStoryRepository = categoryStoryRepository;
            _tickRepository = tickRepository;
        }

        /// <summary>
        /// Get list of story.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<StoryDto>))]
        public IActionResult GetStories()
        {
            var objList = _storyRepository.GetStories();

            var objDto = new List<StoryDto>();
            var storyDto = new StoryDto();

            foreach (var obj in objList)
            {
                storyDto = _mapper.Map<StoryDto>(obj);
                // author name
                storyDto.AuthorName = _authorRepository.GetAuthor(obj.AuthorId).pseudonym;

                // category name
                ICollection<CategoryStory> list = _categoryStoryRepository.GetCategories(storyDto.Id);
                storyDto.CategoryName = new List<String>();

                foreach (CategoryStory cs in list)
                {
                    storyDto.CategoryName.Add(_categoryRepository.GetCategory(cs.CategoryId).Name);
                }
                objDto.Add(storyDto);
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get individual story
        /// </summary>
        /// <param name="storyId">The id of the story</param>
        /// <returns></returns>
        [HttpGet("{storyId:Guid}", Name = "GetStory")]
        [ProducesResponseType(200, Type = typeof(StoryDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetStory(Guid storyId)
        {
            var obj = _storyRepository.GetStory(storyId);
            if(obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<StoryDto>(obj);
            objDto.AuthorName = _authorRepository.GetAuthor(obj.AuthorId).pseudonym;

            // category name
            ICollection<CategoryStory> list = _categoryStoryRepository.GetCategories(objDto.Id);
            objDto.CategoryName = new List<String>();

            foreach (CategoryStory cs in list)
            {
                objDto.CategoryName.Add(_categoryRepository.GetCategory(cs.CategoryId).Name);
            }

            return Ok(objDto);
        }

        //search name

        [HttpGet("search", Name = "search")]
        [ProducesResponseType(200, Type = typeof(StoryDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetStoriesName([FromQuery]string name)
        {
            var objList = _storyRepository.GetStoriesToName(name);

            var objDto = new List<StoryDto>();
            var storyDto = new StoryDto();

            foreach (var obj in objList)
            {
                storyDto = _mapper.Map<StoryDto>(obj);
                // author name
                storyDto.AuthorName = _authorRepository.GetAuthor(obj.AuthorId).pseudonym;

                // category name
                ICollection<CategoryStory> list = _categoryStoryRepository.GetCategories(storyDto.Id);
                storyDto.CategoryName = new List<String>();

                foreach (CategoryStory cs in list)
                {
                    storyDto.CategoryName.Add(_categoryRepository.GetCategory(cs.CategoryId).Name);
                }
                objDto.Add(storyDto);
            }

            return Ok(objDto);
        }

        // insert story
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(StoryDto))]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateStory([FromBody] StoryCreateDto storyCreateDto)
        {
            // check info null
            if(storyCreateDto == null)
            {
                return BadRequest(ModelState);
            }

            // check name story -- unique
            if (_storyRepository.StoryExists(storyCreateDto.Name))
            {
                ModelState.AddModelError("", "Đã tồn tại truyện này!");
                return StatusCode(404, ModelState);
            }

            // check model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            var authorDto = storyCreateDto.Author;

            // check author null
            if (authorDto == null)
            {
                return BadRequest(ModelState);
            }

            var storyObj = new Story();

            // check pseudonym author -- unique
            if (_authorRepository.AuthorExists(authorDto.pseudonym)) //true
            {
                StoryDto storyDto = new StoryDto();
                storyDto.AuthorId = _authorRepository.AuthorId(authorDto.pseudonym);
                storyDto.Name = storyCreateDto.Name;
                storyDto.Status = storyCreateDto.Status;
                storyDto.Description = storyCreateDto.Description;
                storyDto.UserId = storyCreateDto.UserId;
                storyDto.Image = storyCreateDto.Image;
                storyDto.Views = 0;

                storyObj = _mapper.Map<Story>(storyDto);
            }
            else //false
            {
                storyObj = _mapper.Map<Story>(storyCreateDto);
            }
            //add date
            storyObj.CreateDate = DateTime.Now;
            storyObj.UpdateDate = DateTime.Now;

            // create story
            if (!_storyRepository.CreateOrUpdateStory(storyObj))
            {
                ModelState.AddModelError("", $"Đã xảy ra sự cố khi lưu {storyObj.Name}");
                return StatusCode(500, ModelState);
            }

            // create category_story

            Guid storyId = storyObj.Id;

            ICollection<CategoryStoryDto> categoryStoryDtos = storyCreateDto.categoryStoryDtos;
            foreach (CategoryStoryDto categoryStory in categoryStoryDtos)
            {
                if (!_categoryRepository.CategoryExists(categoryStory.CategoryId))
                {
                    ModelState.AddModelError("", "Khong ton tai the loai nay");
                    return StatusCode(500, ModelState);
                }
                else
                {
                    categoryStory.StoryId = storyId;
                }
            }

            foreach (CategoryStoryDto categoryStory in categoryStoryDtos)
            {
                var categoryStoryObj = new CategoryStory();
                categoryStoryObj.Category = _categoryRepository.GetCategory(categoryStory.CategoryId);
                categoryStoryObj.Story = storyObj;

                // create categoryStory
                if (!_categoryStoryRepository.CreateOrUpdateCategoryStory(categoryStoryObj))
                {
                    ModelState.AddModelError("", $"Đã xảy ra sự cố khi lưu {storyObj.Name}");
                    return StatusCode(500, ModelState);
                }
            }

            return CreatedAtRoute("GetStory", new { storyId = storyObj.Id }, storyObj);
        }

        //edit story
        [HttpPatch("{storyId:Guid}", Name = "UpdateStory")]
        [ProducesResponseType(200, Type = typeof(StoryDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateStory(Guid storyId, [FromBody]StoryUpdateDto storyDto)
        {
            if (storyDto == null || storyId != storyDto.Id)
            {
                return BadRequest(ModelState);
            }

            //change
            var storyObj = _mapper.Map<Story>(storyDto);
            storyDto.UpdateDate = DateTime.Now;

            //change data
            if (!_storyRepository.CreateOrUpdateStory(storyObj))
            {
                ModelState.AddModelError("", $"Đã xảy ra sự cố khi cập nhập {storyObj.Name}");
                return StatusCode(500, ModelState);
            }

            return Ok(storyObj);
        }

        //delete story
        [HttpDelete("{storyId:Guid}", Name = "DeleteStory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteStory(Guid storyId)
        {
            if (!_storyRepository.StoryExists(storyId))
            {
                return NotFound();
            }

            var storyObj = _storyRepository.GetStory(storyId);

            if (!_storyRepository.DeleteStory(storyObj))
            {
                ModelState.AddModelError("", $"Đã xảy ra sự cố khi xóa {storyObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //post tick
        [HttpPost("Tick")]
        public IActionResult TickPost([FromBody] TickDto tickDto) 
        {
            if (!_storyRepository.StoryExists(tickDto.StoryId))
            {
                return NotFound();
            }

            Tick tick = _mapper.Map<Tick>(tickDto);
            if (!_tickRepository.CreateTick(tick))
            {
                return StatusCode(401, "Loi luu");
            }
            return Ok(tick);
        }

        //delete tick
        [HttpDelete("Tick")]
        public IActionResult TickDelete([FromBody] TickDto tickDto)
        {
            if (!_storyRepository.StoryExists(tickDto.StoryId))
            {
                return NotFound();
            }

            Tick tick = _mapper.Map<Tick>(tickDto);
            if (!_tickRepository.DeleteTick(tick))
            {
                return StatusCode(401, "Loi luu");
            }
            return Ok(tick);
        }

        //get tick
        [HttpGet("Tick")]
        public IActionResult TickGet(string userId)
        {
            ICollection<Tick> tick = _tickRepository.GetTick(userId);
            List<StoryDto> storyDtos = new List<StoryDto>(); 
            foreach(Tick t in tick)
            {
                var obj = _storyRepository.GetStory(t.StoryId);
                if (obj == null)
                {
                    return NotFound();
                }

                var objDto = _mapper.Map<StoryDto>(obj);
                objDto.AuthorName = _authorRepository.GetAuthor(obj.AuthorId).pseudonym;

                // category name
                ICollection<CategoryStory> list = _categoryStoryRepository.GetCategories(objDto.Id);
                objDto.CategoryName = new List<String>();

                foreach (CategoryStory cs in list)
                {
                    objDto.CategoryName.Add(_categoryRepository.GetCategory(cs.CategoryId).Name);
                }

                storyDtos.Add(objDto);
            }
            return Ok(storyDtos);
        }

        [HttpGet("ExistTick/{userId}/{storyId:Guid}")]
        public IActionResult ExistTick(string userId, Guid storyId)
        {
            if(_tickRepository.TickExists(userId, storyId))
            {
                return Ok(new TickDto(userId, storyId));
            }
            return NotFound();
        }
    }
}
