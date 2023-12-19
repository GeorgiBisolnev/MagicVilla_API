using AutoMapper;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace MagicVilla_API.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogger<VillaAPIController> _logger;
        private readonly IVillaRepository _RepoVilla;
        private readonly IMapper _mapper;
        protected APIResponse _apiResponse;

        public VillaAPIController(
            ILogger<VillaAPIController> logger, IVillaRepository RepoVilla, IMapper mapper)
        {
            _logger = logger;
            _RepoVilla = RepoVilla;
            _mapper = mapper;
            this._apiResponse = new APIResponse();
        }

        [HttpGet]
        [ResponseCache(Duration = 30)]
        //[ResponseCache(CacheProfileName = "Default30")]
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> GetVillas([FromQuery(Name ="filterOccupancy")]int? occupancy,
            [FromQuery]string? search, int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                _logger.LogInformation($"Geting all villas");

                IEnumerable<Villa> villaList;

                if ( occupancy != null )
                {
                    villaList = await _RepoVilla.GetAllAsync(o=> o.Occupancy == occupancy,pageSize:pageSize,pageNumber:pageNumber);
                }
                else
                {
                    villaList = await _RepoVilla.GetAllAsync(pageSize: pageSize, pageNumber: pageNumber);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    villaList = await _RepoVilla.GetAllAsync(s => s.Name.ToLower().Contains(search.ToLower()));
                }

                Pagination pagination = new Pagination { PageSize = pageSize, PageCount = pageNumber };
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _apiResponse.Result = villaList;
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

        [HttpGet("{id:int}", Name = "GetVilla")]
        //[ResponseCache(Duration = 30)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles ="admin")]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogError($"Villa error with id - {id}");
                    _apiResponse.StatusCode=HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                var villa = await _RepoVilla.GetAsync(u => u.Id == id);
                if (villa == null)
                {
                    _logger.LogError($"Villa error with id - {id}");
                    return NotFound();
                }
                _apiResponse.Result = villa;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                _logger.LogInformation($"Geting villa - {id}");

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.ToString() };

                _logger.LogError($"Get villa with ID {id} fails - {ex.ToString()}");

                return _apiResponse;
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromForm] VillaCreateDTO createDTO)
        {
            try
            {
                if (await _RepoVilla.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa already exists!");
                    _logger.LogError($"Villa create error - Villa already exists!");
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Result = ModelState;
                    return BadRequest(_apiResponse);
                }
                if (createDTO == null)
                {
                    _logger.LogError($"Villa create error - null");
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Result = createDTO;
                    return BadRequest(_apiResponse);
                }

                Villa villa = _mapper.Map<Villa>(createDTO);

                villa.CreatedDate = DateTime.Now;

                await _RepoVilla.CreateAsync(villa);

                if (createDTO.Image != null)
                {
                    string fileName = villa.Id + Path.GetExtension(createDTO.Image.FileName);
                    string filePath = @"wwwroot\ProductImage\" + fileName;

                    var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                    FileInfo file = new FileInfo(directoryLocation);

                    if (file.Exists)
                    {
                        file.Delete();
                    }

                    using (var fileStream = new FileStream(directoryLocation, FileMode.Create))
                    {
                        createDTO.Image.CopyTo(fileStream);
                    }

                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    villa.ImageURL = baseUrl + "/ProductImage/" + fileName;
                    villa.ImageLocalPath = filePath;


                }
                else
                {
                    villa.ImageURL = "https://placehold.co/600x400";
                }

                await _RepoVilla.UpdateAsync(villa);

                _logger.LogInformation($"Villa create successful with ID - {villa.Id}");

                _apiResponse.Result = _mapper.Map<VillaDTO>(villa);
                _apiResponse.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVilla", new { villa.Id }, _apiResponse);
                //return Ok(villa);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.ToString() };

                _logger.LogError($"Create villa fails - {ex.ToString()}");

                return _apiResponse;
            }
        }
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogError($"Villa delete unsuccessful - id error");
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.ErrorMessages = new List<string>() { "Villa delete unsuccessful - id error" };
                    return BadRequest(_apiResponse);
                }
                var villa = await _RepoVilla.GetAsync(v => v.Id == id);

                if (villa == null)
                {
                    _logger.LogError($"Villa delete unsuccessful - null");
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(villa.ImageLocalPath))
                {
                    var oldFilepathDir = Path.Combine(Directory.GetCurrentDirectory(), villa.ImageLocalPath);
                    FileInfo file = new FileInfo(oldFilepathDir);

                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }

                await _RepoVilla.RemoveAsync(villa);

                _logger.LogInformation($"Villa delete successful with ID - {id}");

                _apiResponse.StatusCode = HttpStatusCode.NoContent;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.ToString() };

                _logger.LogError($"Delete villa with ID {id} fails - {ex.ToString()}");

                return _apiResponse;
            }
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromForm] VillaUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO.Id != id || updateDTO == null)
                {
                    _logger.LogError($"Villa update unsuccessful - id error");
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.ErrorMessages = new List<string>() { "Villa update unsuccessful - id error" };
                    return BadRequest(_apiResponse);
                }

                var villa = await _RepoVilla.GetAsync(v => v.Id == id, tracked: false);

                if (villa == null)
                {
                    _logger.LogError($"Villa update unsuccessful - not found");
                    return NotFound();
                }
                Villa model = _mapper.Map<Villa>(updateDTO);

                model.CreatedDate=villa.CreatedDate;
                model.UpdatedDate = DateTime.Now;

                if (updateDTO.Image != null)
                {
                    if (!string.IsNullOrEmpty(model.ImageLocalPath))
                    {
                        var oldFilepathDir = Path.Combine(Directory.GetCurrentDirectory(), model.ImageLocalPath);
                        FileInfo file = new FileInfo(oldFilepathDir);

                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }

                    string fileName = villa.Id + Path.GetExtension(updateDTO.Image.FileName);
                    string filePath = @"wwwroot\ProductImage\" + fileName;

                    var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);



                    using (var fileStream = new FileStream(directoryLocation, FileMode.Create))
                    {
                        updateDTO.Image.CopyTo(fileStream);
                    }

                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    model.ImageURL = baseUrl + "/ProductImage/" + fileName;
                    model.ImageLocalPath = filePath;


                }
                else
                {
                    model.ImageURL = "https://placehold.co/600x400";
                }

                await _RepoVilla.UpdateAsync(model);

                _logger.LogInformation($"Villa update successful with ID - {id}!");

                _apiResponse.StatusCode = HttpStatusCode.NoContent;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.ToString() };

                _logger.LogError($"Update villa with ID {id} fails - {ex.ToString()}");

                return _apiResponse;
            }
        }

        [HttpPatch("{id:int}", Name = "PatchVilla")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> pacthDTO)
        {
            try
            {
                if (pacthDTO == null || id <= 0)
                {
                    _logger.LogError($"Villa patch unsuccessful - id error");
                    return BadRequest();
                }

                var villa = await _RepoVilla.GetAsync(v => v.Id == id, tracked: false);

                var villaUpdateDTO = _mapper.Map<VillaUpdateDTO>(villa);

                if (villa == null)
                {
                    _logger.LogError($"Villa patch unsuccessful - null");
                    return BadRequest();
                }

                pacthDTO.ApplyTo(villaUpdateDTO, ModelState);

                if (!ModelState.IsValid)
                {
                    _logger.LogError($"Villa patch unsuccessful - model state not valid");
                    return BadRequest();
                }

                var model = _mapper.Map<Villa>(villaUpdateDTO);

                await _RepoVilla.UpdateAsync(model);

                _logger.LogInformation($"Villa patch successful!");

                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.ToString() };

                _logger.LogError($"Update partial villa with ID {id} fails - ERROR: {ex.ToString()} " +
                    $"\nJSON patch document {JsonSerializer.Serialize(pacthDTO)}");

                return _apiResponse;
            }
        }
    }
}
