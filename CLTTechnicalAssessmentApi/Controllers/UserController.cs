using AutoMapper;
using CLTTechnicalAssessmentApi.Models;
using CLTTechnicalAssessmentApi.Models.Dtos;
using CLTTechnicalAssessmentApi.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using Utility;

namespace CLTTechnicalAssessmentApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IUserRepository _dbUser;
        private readonly IMapper _mapper;
        public UserController(IUserRepository dbUser, IMapper mapper)
        {
            _dbUser = dbUser;
            _mapper = mapper;
            _response = new APIResponse();
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetUsers([FromQuery(Name = "name")] string? nameFilter, int pageSize = 20, int pageNumber = 1)
        {
            try
            {

                IEnumerable<User> userList;

                if (!string.IsNullOrEmpty(nameFilter))
                {
                    userList = await _dbUser.GetAllAsync(u => u.Name.ToLower().Contains(nameFilter), pageSize: pageSize,
                      pageNumber: pageNumber);
                }
                else
                {
                    userList = await _dbUser.GetAllAsync(pageSize: pageSize,
                        pageNumber: pageNumber);
                }
                Pagination pagination = new Pagination() { PageNumber = pageNumber, PageSize = pageSize };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.BuildResponse(result: _mapper.Map<List<UserDto>>(userList),
                    statusCode: HttpStatusCode.OK,
                    isSuccess: true
                    );
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.BuildResponse(errors: new List<string>() { ex.ToString() },
                   statusCode: HttpStatusCode.InternalServerError,
                   isSuccess: false
                   );
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

        [HttpGet("{id:int}", Name = "GetUser")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetUser(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.BuildResponse(errors: new List<string>() { "Id must be greater than 0" },
                        statusCode: HttpStatusCode.BadRequest, isSuccess: false);
                    return BadRequest(_response);
                }
                var user = await _dbUser.GetAsync(u => u.Id == id);
                if (user == null)
                {
                    _response.BuildResponse(errors: new List<string>() { $"User with id {id} not found" },
                        statusCode: HttpStatusCode.NotFound, isSuccess: false);
                    return NotFound(_response);
                }

                _response.BuildResponse(result: _mapper.Map<UserDto>(user), statusCode: HttpStatusCode.OK, isSuccess: true);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.BuildResponse(errors: new List<string>() { ex.ToString() },
                   statusCode: HttpStatusCode.InternalServerError,
                   isSuccess: false
                   );
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateUser([FromBody] CreateUserDto createDTO)
        {
            try
            {
                if (await _dbUser.GetAsync(u => u.Document == createDTO.Document) != null)
                {
                    _response.BuildResponse(errors: new List<string>() { $"User with document number {createDTO.Document} already exists" },
                      statusCode: HttpStatusCode.BadRequest,
                      isSuccess: false
                      );
                    return BadRequest(_response);
                }

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }
                User user = _mapper.Map<User>(createDTO);
                await _dbUser.CreateAsync(user);
                _response.BuildResponse(result: _mapper.Map<UserDto>(user),
                    statusCode: HttpStatusCode.Created,
                    isSuccess: true
                    );
                return CreatedAtRoute("GetUser", new { id = user.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.BuildResponse(errors: new List<string>() { ex.ToString() },
                   statusCode: HttpStatusCode.InternalServerError,
                   isSuccess: false
                   );
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

        [HttpPut("{id:int}", Name = "UpdateUser")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> UpdateUser(int id, [FromBody] UpdateUserDto updateDTO)
        {
            try
            {
                var user = await _dbUser.GetAsync(u => u.Id == id, tracked: false);
                if (user == null)
                {
                    _response.BuildResponse(errors: new List<string>() { $"User with id {id} not found" }, statusCode: HttpStatusCode.NotFound, isSuccess: false);
                    return BadRequest(_response);
                }

                User model = _mapper.Map<User>(updateDTO);
                model.Id = id;

                await _dbUser.UpdateAsync(model);                

                _response.BuildResponse(result: _mapper.Map<UserDto>(model), statusCode: HttpStatusCode.OK, isSuccess: true);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.BuildResponse(errors: new List<string>() { ex.ToString() },
                   statusCode: HttpStatusCode.InternalServerError,
                   isSuccess: false
                   );
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteUser")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.BuildResponse(errors: new List<string>() { "Id must be greater than 0" },
                         statusCode: HttpStatusCode.BadRequest, isSuccess: false);
                    return BadRequest(_response);
                }   
                var user = await _dbUser.GetAsync(u => u.Id == id);
                if (user == null)
                {
                    _response.BuildResponse(errors: new List<string>() { $"User with id {id} not found" },
                        statusCode: HttpStatusCode.NotFound, isSuccess: false);
                    return NotFound(_response);
                }
                await _dbUser.RemoveAsync(user);

                _response.BuildResponse(statusCode: HttpStatusCode.OK, isSuccess: true);
                return Ok(_response);
                
            }
            catch (Exception ex)
            {
                _response.BuildResponse(errors: new List<string>() { ex.ToString() },
                   statusCode: HttpStatusCode.InternalServerError,
                   isSuccess: false
                   );
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }
    }
}
