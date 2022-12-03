using API.DataAccess.Repository.IRepository;
using API.Models.Models;
using API.Models.Models.Dtos;
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
    public class ChapterController : ControllerBase
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly IMapper _mapper;
        private readonly IStoryRepository _storyRepository;

        public ChapterController(IChapterRepository chapterRepository, IMapper mapper, IStoryRepository storyRepository)
        {
            _chapterRepository = chapterRepository;
            _mapper = mapper;
            _storyRepository = storyRepository;
        }

        /// <summary>
        /// Get list of chapter.
        /// </summary>
        /// <param name="storyId"></param>
        /// <returns></returns>
        [HttpGet("{storyId:Guid}", Name = "GetListChapter")]
        [ProducesResponseType(200, Type = typeof(List<ChapterDto>))]
        public IActionResult GetChapters(Guid storyId)
        {
            var objList = _chapterRepository.GetChapters(storyId);

            var objDto = new List<ChapterDto>();
            var chapterDto = new ChapterDto();

            foreach (var obj in objList)
            {
                chapterDto = _mapper.Map<ChapterDto>(obj);
                chapterDto.StoryName = _storyRepository.GetStory(storyId).Name;

                objDto.Add(chapterDto);
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get individual chapter chapter
        /// </summary>
        /// <param name="storyId"></param>
        /// <param name="chapterId">The id of the chapter</param>
        /// <returns></returns>
        [HttpGet("{chapterId:Guid},{storyId:Guid}", Name = "GetChapter")]
        [ProducesResponseType(200, Type = typeof(ChapterDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetChapter(Guid storyId, Guid chapterId)
        {
            var obj = _chapterRepository.GetChapter(storyId, chapterId);
            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<ChapterDto>(obj);
            objDto.StoryName = _storyRepository.GetStory(storyId).Name;

            return Ok(objDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ChapterDto))]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateChapter([FromBody] ChapterCreateDto chapterCreateDto)
        {
            // check info null
            if (chapterCreateDto == null)
            {
                return BadRequest(ModelState);
            }

            // check story
            if (!_storyRepository.StoryExists(chapterCreateDto.StoryId)) //true
            {
                return NotFound("Truyện này chưa được tạo!");
            }

            // check name chapter -- unique
            if (_chapterRepository.ChapterExists(chapterCreateDto.Name, chapterCreateDto.StoryId))
            {
                ModelState.AddModelError("", "Đã tồn tại chương này!");
                return StatusCode(404, ModelState);
            }

            // check model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var chapterObj = new Chapter();

            chapterObj = _mapper.Map<Chapter>(chapterCreateDto);

            //add date
            chapterObj.CreateDate = DateTime.Now;
            chapterObj.UpdateDate = DateTime.Now;
            chapterObj.Id = Guid.NewGuid();
            

            // create chapter
            if (!_chapterRepository.CreateChapter(chapterObj))
            {
                ModelState.AddModelError("", $"Đã xảy ra sự cố khi lưu {chapterObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetChapter", new { chapterId = chapterObj.Id, storyId = chapterObj.StoryId}, chapterObj);
        }

        [HttpPatch("{storyId:Guid}/{chapterId:Guid}", Name = "UpdateChapter")]
        [ProducesResponseType(200, Type = typeof(ChapterDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateChapter(Guid storyId, Guid chapterId, [FromBody] ChapterUpdateDto chapterDto)
        {
            if (chapterDto == null || chapterId != chapterDto.Id)
            {
                return BadRequest(ModelState);
            }

            // check story
            if (!_storyRepository.StoryExists(storyId)) //true
            {
                ModelState.AddModelError("", $"Không tồn tại truyện này!");
                return StatusCode(500, ModelState);
            }

            //change
            var chapterObj = _mapper.Map<Chapter>(chapterDto);
            chapterObj.UpdateDate = DateTime.Now;

            //change data
            if (!_chapterRepository.UpdateChapter(chapterObj))
            {
                ModelState.AddModelError("", $"Đã xảy ra sự cố khi cập nhập {chapterObj.Name}");
                return StatusCode(500, ModelState);
            }

            return Ok(chapterObj);
        }

        [HttpDelete("{chapterId:Guid},{storyId:Guid}", Name = "DeleteChapter")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteChapter(Guid chapterId, Guid storyId)
        {
            if (!_chapterRepository.ChapterExists(chapterId, storyId))
            {
                return NotFound();
            }

            var chapterObj = _chapterRepository.GetChapter(storyId, chapterId);

            if(chapterObj == null)
            {
                return NotFound("Không tồn tại chương này!");
            }

            if (!_chapterRepository.DeleteChapter(chapterObj))
            {
                ModelState.AddModelError("", $"Đã xảy ra sự cố khi xóa {chapterObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
