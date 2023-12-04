using AutoMapper;
using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repository;
using MagicVilla_API.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace MagicVilla_API.Controllers
{
    [Route("api/VillaNumberAPI")]
    [ApiController]
    public class VillaNumberAPIController : ControllerBase
    {
        private readonly ILogger<VillaNumberAPIController> _logger;
        private readonly IVillaNumberRepository _RepoVillaNumber;
        private readonly IMapper _mapper;
        private readonly IVillaRepository _RepoVilla;
        protected APIResponse _apiResponse;

        public VillaNumberAPIController(
            ILogger<VillaNumberAPIController> logger, IVillaNumberRepository RepoVillaNumber, IMapper mapper, 
            IVillaRepository RepoVilla)
        {
            _logger = logger;
            _RepoVillaNumber = RepoVillaNumber;
            _mapper = mapper;
            this._apiResponse = new APIResponse();
            _RepoVilla = RepoVilla;
        }

        [HttpGet]
        [ResponseCache(Duration = 30)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {
                _logger.LogInformation($"Getting all villa numbers");

                IEnumerable<VillaNumber> villaNumbersList = await _RepoVillaNumber.GetAllAsync(includeProperties: "Villa");
                _apiResponse.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumbersList);
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.ToString() };

                _logger.LogError($"Geting all villas fails - {ex.ToString()}");

                return _apiResponse;
            }

        }

        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ResponseCache(Duration = 30)]
        [ActionName(nameof(GetVillaNumber))]
        //[HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogError($"Villa Number error with id - {id}");
                    _apiResponse.StatusCode=HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                var villaNumber = await _RepoVillaNumber.GetAsync(u => u.VillaNo == id);
                if (villaNumber == null)
                {
                    _logger.LogError($"Villa Number error with id - {id}");
                    return NotFound();
                }
                _apiResponse.Result = villaNumber;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                _logger.LogInformation($"Geting villa Number - {id}");

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.ToString() };

                _logger.LogError($"Get villa Number with ID {id} fails - {ex.ToString()}");

                return _apiResponse;
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ActionName(nameof(CreateVillaNumber))]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO createDTO)
        {
            try
            {
                if (await _RepoVillaNumber.GetAsync(u => u.VillaNo == createDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa Number already exists!");
                    _logger.LogError($"Villa Number create error - VillaNumber already exists!");
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    //_apiResponse.Result = ModelState;
                    return BadRequest(ModelState);
                }
                if (await _RepoVilla.GetAsync(i=>i.Id == createDTO.VillaId)==null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa Number ID does not exists!");
                    _logger.LogError($"Villa Number create error - Villa Number ID does not exists!");
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Result = ModelState;
                    return BadRequest(ModelState);
                }
                if (createDTO == null)
                {
                    _logger.LogError($"Villa Number create error - null");
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Result = createDTO;
                    return BadRequest(_apiResponse);
                }

                VillaNumber villaNumber = _mapper.Map<VillaNumber>(createDTO);

                await _RepoVillaNumber.CreateAsync(villaNumber);

                _logger.LogInformation($"Villa Number created successful with ID - {villaNumber.VillaNo}");

                _apiResponse.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                _apiResponse.StatusCode = HttpStatusCode.Created;

                //return CreatedAtRoute("GetVillaNumber", new { villaNumber.VillaNo }, _apiResponse);
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.ToString() };

                _logger.LogError($"Create villa Number fails - {ex.ToString()}");

                return _apiResponse;
            }
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogError($"Villa Number delete unsuccessful - id error");
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.ErrorMessages = new List<string>() { "Villa Number delete unsuccessful - id error" };
                    return BadRequest(_apiResponse);
                }
                var villaNumber = await _RepoVillaNumber.GetAsync(v => v.VillaNo == id);

                if (villaNumber == null)
                {
                    _logger.LogError($"Villa Number delete unsuccessful - null");
                    return NotFound();
                }

                await _RepoVillaNumber.RemoveAsync(villaNumber);

                _logger.LogInformation($"Villa Number deleted successful");

                _apiResponse.StatusCode = HttpStatusCode.NoContent;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.ToString() };

                _logger.LogError($"Delete villa Number with ID {id} fails - {ex.ToString()}");

                return _apiResponse;
            }
        }

        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO.VillaNo != id || updateDTO == null)
                {
                    _logger.LogError($"Villa Number update unsuccessful - id error");
                    ModelState.AddModelError("ErrorMessages", "Villa Number ID does not exists!");
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.ErrorMessages = new List<string>() { "Villa Number update unsuccessful - id error" };
                    return BadRequest(ModelState);
                }

                if (await _RepoVilla.GetAsync(i => i.Id == updateDTO.VillaId) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa Number ID does not exists!");
                    _logger.LogError($"Villa Number create error - Villa Number ID does not exists!");
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Result = ModelState;
                    return BadRequest(ModelState);
                }

                var villaNumber = await _RepoVillaNumber.GetAsync(v => v.VillaNo == id, tracked: false);

                if (villaNumber == null)
                {
                    _logger.LogError($"Villa Number update unsuccessful - not found");
                    return NotFound();
                }
                VillaNumber model = _mapper.Map<VillaNumber>(updateDTO);

                await _RepoVillaNumber.UpdateAsync(model);

                _logger.LogInformation($"Villa Number update successful!");

                _apiResponse.StatusCode = HttpStatusCode.NoContent;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.ToString() };

                _logger.LogError($"Update villa Number with ID {id} fails - {ex.ToString()}");

                return _apiResponse;
            }
        }
    }
}
