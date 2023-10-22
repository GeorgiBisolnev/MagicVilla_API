using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogger<VillaAPIController> _logger;

        public VillaAPIController(ILogger<VillaAPIController> logger)  {
            _logger = logger;
        }


        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.LogInformation($"Geting all villas");
            return Ok(VillaStore.villaList);
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id <= 0)
            {
                _logger.LogError($"Villa error with id - {id}");
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                _logger.LogError($"Villa error with id - {id}");
                return NotFound();
            }
            _logger.LogInformation($"Geting villa - {id}");
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villa)
        {
            if (VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villa.Name.ToLower()) != null)
            {
                ModelState.AddModelError("Custom Error", "Villa already exists!");
                _logger.LogError($"Villa create error");
                return BadRequest(ModelState);
            }
            if (villa == null)
            {
                _logger.LogError($"Villa create error - null");
                return NotFound();
            }
            if (villa.Id > 0)
            {
                _logger.LogError($"Villa create error - ID error");
                return BadRequest();
            }

            villa.Id = VillaStore.villaList.OrderByDescending(villa => villa.Id).First().Id + 1;

            VillaStore.villaList.Add(villa);

            _logger.LogInformation($"Villa create successful");
            return CreatedAtRoute("GetVilla", new { villa.Id }, villa);
            //return Ok(villa);
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("id", Name = "DeleteVilla")]
        public IActionResult DeleteVilla(int id)
        {
            if (id <= 0)
            {
                _logger.LogError($"Villa delete unsuccessful - id error");
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            if (villa == null)
            {
                _logger.LogError($"Villa delete unsuccessful - null");
                return NotFound();
            }

            VillaStore.villaList.Remove(villa);

            _logger.LogInformation($"Villa delete successful");
            return NoContent();
        }

        [HttpPut ("{id:int}" , Name ="UpdateVilla")]
        [ProducesResponseType (StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound) ]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            if (villaDTO.Id != id || villaDTO==null) 
            {
                _logger.LogError($"Villa update unsuccessful - id error");
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault (v => v.Id == id);

            if (villa == null) {
                _logger.LogError($"Villa update unsuccessful - null");
                return NotFound();
            }

            villa.Name= villaDTO.Name;
            villa.Occupancy= villaDTO.Occupancy;
            villa.SquareFoot = villaDTO.SquareFoot;

            _logger.LogInformation($"Villa update successful!");
            return NoContent();
        }

        [HttpPatch ("id:int" , Name ="PatchVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> pacthDTO)
        {
            if (pacthDTO==null || id<=0)
            {
                _logger.LogError($"Villa patch unsuccessful - id error");
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            if (villa == null)
            {
                _logger.LogError($"Villa patch unsuccessful - null");
                return BadRequest();
            }
            
            pacthDTO.ApplyTo(villa, ModelState);

            if (!ModelState.IsValid)
            {
                _logger.LogError($"Villa patch unsuccessful - model state not valid");
                return BadRequest();
            }

            _logger.LogInformation($"Villa patch successful!");
            return Ok();
        }
    }
}
    