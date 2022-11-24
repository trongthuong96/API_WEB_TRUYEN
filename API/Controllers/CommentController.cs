using API.DataAccess.Repository;
using API.DataAccess.Repository.IRepository;
using API.Models.Models;
using API.Models.Models.Dtos;
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
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IMapper _mapper;

        public CommentController(ICommentRepository commentRepository, IMapper mapper, IApplicationUserRepository applicationUserRepository)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _applicationUserRepository = applicationUserRepository;
        }

        /// <summary>
        /// Get list of comment story.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CommentDto>))]
        public IActionResult GetComments(Guid storyId)
        {
            var objList = _commentRepository.GetComments(storyId);

            var objDtos = new List<CommentDto>();
            var objDto = new CommentDto();

            foreach (var obj in objList)
            {
                objDto = _mapper.Map<CommentDto>(obj);
                objDto.UserName = _applicationUserRepository.GetUser(obj.UserId).UserName;
                objDtos.Add(objDto);
            }

            return Ok(objDtos);
        }

        [HttpPost]
        public IActionResult PostComment([FromBody] CommentCreateDto commentCreateDto)
        {
            if (commentCreateDto == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var objComment = _mapper.Map<Comment>(commentCreateDto);
            objComment.CreateDate = DateTime.Now;
            objComment.UpdateDate = DateTime.Now;

            if (!_commentRepository.CreateComment(objComment))
            {
                return StatusCode(404, "Lỗi lưu");
            }
            return Ok(objComment);
        }

        [HttpDelete]
        public IActionResult DeleteComment(Guid id)
        {
            if(id == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_commentRepository.DeleteComment(id))
            {
                return StatusCode(404, "Lỗi xóa");
            }
            return Ok(id);
        }
    }
}
