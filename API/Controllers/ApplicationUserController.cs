﻿using API.DataAccess.Repository;
using API.Models.Models;
using API.Models.Models.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : Controller
    {
        private readonly IApplicationUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUserController(IApplicationUserRepository userRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        /// <summary>
        /// Get list of user story.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ApplicationUserDto>))]
        public IActionResult GetCategories()
        {
            var objList = _userRepository.GetUsers();

            var objDto = new List<ApplicationUserDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<ApplicationUserDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get individual user story
        /// </summary>
        /// <param name="userId">The Id of the user story</param>
        /// <returns></returns>
        [HttpGet("{userId}", Name = "GetApplicationUser")]
        [ProducesResponseType(200, Type = typeof(ApplicationUserDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetApplicationUser(string userId)
        {
            var obj = _userRepository.GetUser(userId);
            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<ApplicationUserDto>(obj);
            return Ok(objDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ApplicationUserDto))]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateApplicationUser([FromBody] ApplicationUserCreateDto userCreateDto)
        {
            if (userCreateDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_userRepository.UserExists(userCreateDto.UserName))
            {
                return StatusCode(404, "Đã có username này!");
            } else if (_userRepository.EmailExists(userCreateDto.Email))
            {
                return StatusCode(404, "Đã có email này!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userObj = _mapper.Map<ApplicationUser>(userCreateDto);

            var result = _userManager.CreateAsync(userObj, userCreateDto.Password).GetAwaiter();


            if (result.IsCompleted)
            {
                if (!_userRepository.CreateUser(userObj))
                {
                    ModelState.AddModelError("", $"Đã xảy ra sự cố khi lưu {userObj.UserName}");
                    return StatusCode(500, ModelState);
                }
            }

            

            return CreatedAtRoute("GetApplicationUser", new { userId = userObj.Id }, userObj);
        }

        [HttpPatch("{userId}", Name = "UpdateApplicationUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateApplicationUser(string userId, [FromBody] ApplicationUserDto userDto)
        {
            if (userDto == null || userId != userDto.Id)
            {
                return BadRequest(ModelState);
            }

            var userObj = _mapper.Map<ApplicationUser>(userDto);

            if (!_userRepository.UpdateUser(userObj))
            {
                ModelState.AddModelError("", $"Đã xảy ra sự cố khi cập nhập {userObj.UserName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{userId}", Name = "DeleteApplicationUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteApplicationUser(string userId)
        {
            if (!_userRepository.UserExists(userId))
            {
                return NotFound();
            }

            var userObj = _userRepository.GetUser(userId);

            if (!_userRepository.DeleteUser(userObj))
            {
                ModelState.AddModelError("", $"Đã xảy ra sự cố khi xóa {userObj.UserName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}