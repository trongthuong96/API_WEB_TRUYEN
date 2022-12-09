using API.DataAccess.Repository;
using API.Models.Models;
using API.Models.Models.Dtos;
using API.Utility;
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
    public class ApplicationUserController : ControllerBase
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

        // add user
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ApplicationUserDto))]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateApplicationUserAsync([FromBody] ApplicationUserCreateDto userCreateDto)
        {
            if (userCreateDto == null)
            {
                return BadRequest(ModelState);
            }

            if (userCreateDto.UserName != null && _userRepository.UserExists(userCreateDto.UserName))
            {
                return StatusCode(404, "Đã có username này!");
            } else if (_userRepository.EmailExists(userCreateDto.Email))
            {
                return StatusCode(405, "Đã có email này!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userObj = _mapper.Map<ApplicationUser>(userCreateDto);

            var result = await _userManager.CreateAsync(userObj, userCreateDto.Password);

            await _userManager.AddToRoleAsync(userObj, SD.Role_User_Indi);

            await _userManager.UpdateAsync(userObj);
            
            return CreatedAtRoute("GetApplicationUser", new { userId = userObj.Id }, userObj);
        }

        // check user
        [HttpPost("CheckUser", Name = "CheckUser")]
        [ProducesResponseType(200, Type = typeof(ApplicationUserDto))]
        public async Task<ActionResult<ApplicationUser>> CheckUser([FromBody] ApplicationUserDto userCreateDto)
        {
            if (userCreateDto == null)
            {
                return BadRequest(ModelState);
            }

            var userObj = new ApplicationUser();

            //check username && pass
            if (userCreateDto.UserName != null)
            {
                userObj = await _userManager.FindByNameAsync(userCreateDto.UserName);

                if (userObj == null)
                {
                    return StatusCode(401, "Sai tài khoản đăng nhập!");
                }

                var passwordOK = await _userManager.CheckPasswordAsync(userObj, userCreateDto.Password);
                if (!passwordOK)
                {
                    return StatusCode(401, "Sai tài khoản đăng nhập!");
                }
            }    
            
            if(userCreateDto.Email != null)
            {
                //check email && pass
                userObj = await _userManager.FindByEmailAsync(userCreateDto.Email);
                if (userObj == null)
                {
                    return StatusCode(401, "Sai tài khoản đăng nhập!");
                }
                var passwordOK = await _userManager.CheckPasswordAsync(userObj, userCreateDto.Password);
                if (!passwordOK)
                {
                    return StatusCode(401, "Sai tài khoản đăng nhập!");
                }
            }

            ApplicationUserDto applicationUserDto = _mapper.Map<ApplicationUserDto>(userObj);
            IList<string> role = await _userManager.GetRolesAsync(userObj);
            applicationUserDto.Role = role;

            return Ok(applicationUserDto);
        }

        // create token
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] ApplicationUserDto applicationUser)
        {
            var user = _userRepository.Authenticate(applicationUser.UserName, applicationUser.Password);
            if(applicationUser == null)
            {
                return BadRequest(new { message = "Loi tai khoan" });
            }

            return Ok(user);
        }

        //edit user
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

        //delete user
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
