using API.Models;
using API.Repository;
using API.Repository.IRepository;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public StoryController(IStoryRepository storyRepository, IMapper mapper, IAuthorRepository authorRepository)
        {
            _storyRepository = storyRepository;
            _mapper = mapper;
            _authorRepository = authorRepository;
        }

        /// <summary>
        /// Get list of story.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<StoryDto>))]
        public IActionResult GetStories()
        {
            var objList = _storyRepository.GetStoriesToAuthor("string");

            var objDto = new List<StoryDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<StoryDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get individual story story
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
            return Ok(objDto);
        }

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
                storyDto.Views = 0;

                storyObj = _mapper.Map<Story>(storyDto);
            }
            else //false
            {
                storyObj = _mapper.Map<Story>(storyCreateDto);
            }

            // create story
            if (!_storyRepository.CreateOrUpdateStory(storyObj))
            {
                ModelState.AddModelError("", $"Đã xảy ra sự cố khi lưu {storyObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetStory", new { storyId = storyObj.Id}, storyObj);
        }

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

            var storyObj = _mapper.Map<Story>(storyDto);

            if (!_storyRepository.CreateOrUpdateStory(storyObj))
            {
                ModelState.AddModelError("", $"Đã xảy ra sự cố khi cập nhập {storyObj.Name}");
                return StatusCode(500, ModelState);
            }

            return Ok(storyObj);
        }

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
    }
}
