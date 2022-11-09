using API.DataAccess.Repository.IRepository;
using API.Models.Models;
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
    public class CityController : Controller
    {
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;

        public CityController(ICityRepository cityRepository, IMapper mapper)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of city.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CityDto>))]
        public IActionResult GetCities()
        {
            var objList = _cityRepository.GetCities();

            var objDto = new List<CityDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<CityDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get individual city
        /// </summary>
        /// <param name="cityId">The Id of the city</param>
        /// <returns></returns>
        [HttpGet("{cityId:int}", Name = "GetCity")]
        [ProducesResponseType(200, Type = typeof(CityDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetCity(int cityId)
        {
            var obj = _cityRepository.GetCity(cityId);
            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<CityDto>(obj);
            return Ok(objDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CityDto))]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateCity([FromBody] CityDto cityCreateDto)
        {
            if (cityCreateDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_cityRepository.CityExists(cityCreateDto.Name))
            {
                ModelState.AddModelError("", "Đã tồn tại thể loại này!");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cityObj = _mapper.Map<City>(cityCreateDto);

            if (!_cityRepository.CreateOrUpdateCity(cityObj))
            {
                ModelState.AddModelError("", $"Đã xảy ra sự cố khi lưu {cityObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCity", new { cityId = cityObj.Id }, cityObj);
        }

        [HttpPatch("{cityId:int}", Name = "UpdateCity")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateCity(int cityId, [FromBody] CityDto cityDto)
        {
            if (cityDto == null || cityId != cityDto.Id)
            {
                return BadRequest(ModelState);
            }

            var cityObj = _mapper.Map<City>(cityDto);

            if (!_cityRepository.CreateOrUpdateCity(cityObj))
            {
                ModelState.AddModelError("", $"Đã xảy ra sự cố khi cập nhập {cityObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{cityId:int}", Name = "DeleteCity")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteCity(int cityId)
        {
            if (!_cityRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var cityObj = _cityRepository.GetCity(cityId);

            if (!_cityRepository.DeleteCity(cityObj))
            {
                ModelState.AddModelError("", $"Đã xảy ra sự cố khi xóa {cityObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
