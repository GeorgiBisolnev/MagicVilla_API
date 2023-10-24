using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogger<VillaAPIController> _logger;
        private readonly AppDbContext _db;

        public VillaAPIController(ILogger<VillaAPIController> logger, AppDbContext db)  {
            _logger = logger;
            _db=db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            _logger.LogInformation($"Geting all villas");
            return Ok(await _db.Villas.ToListAsync());
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if (id <= 0)
            {
                _logger.LogError($"Villa error with id - {id}");
                return BadRequest();
            }

            var villa = await _db.Villas.FirstOrDefaultAsync(u => u.Id == id);
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
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO villa)
        {
            if (await _db.Villas.FirstOrDefaultAsync(u => u.Name.ToLower() == villa.Name.ToLower()) != null)
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
            //if (villa.Id > 0)
            //{
            //    _logger.LogError($"Villa create error - ID error");
            //    return BadRequest();
            //}

            var model = new Villa()
            {
                Name = villa.Name,
                Details = villa.Details,
                ImageURL = villa.ImageURL,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                SquareFoot = villa.SquareFoot,
                Amenity = villa.Amenity,
                CreatedDate = DateTime.Now
            };

            await _db.Villas.AddAsync(model);
            await _db.SaveChangesAsync();

            _logger.LogInformation($"Villa create successful");
            return CreatedAtRoute("GetVilla", new { model.Id }, model);
            //return Ok(villa);
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id <= 0)
            {
                _logger.LogError($"Villa delete unsuccessful - id error");
                return BadRequest();
            }
            var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);

            if (villa == null)
            {
                _logger.LogError($"Villa delete unsuccessful - null");
                return NotFound();
            }

            _db.Villas.Remove(villa);
            await _db.SaveChangesAsync();
            _logger.LogInformation($"Villa delete successful");
            return NoContent();
        }

        [HttpPut ("{id:int}" , Name ="UpdateVilla")]
        [ProducesResponseType (StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound) ]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaDTO)
        {
            if (villaDTO.Id != id || villaDTO==null) 
            {
                _logger.LogError($"Villa update unsuccessful - id error");
                return BadRequest();
            }

            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync (v => v.Id == id);

            if (villa == null) {
                _logger.LogError($"Villa update unsuccessful - null");
                return NotFound();
            }

            var model = new Villa()
            {
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                ImageURL = villaDTO.ImageURL,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                SquareFoot = villaDTO.SquareFoot,
                Id = villaDTO.Id,
                Amenity = villaDTO.Amenity,
            };

            _db.Villas.Update (model);
            await _db.SaveChangesAsync();

            _logger.LogInformation($"Villa update successful!");
            return NoContent();
        }

        [HttpPatch ("{id:int}" , Name ="PatchVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> pacthDTO)
        {
            if (pacthDTO==null || id<=0)
            {
                _logger.LogError($"Villa patch unsuccessful - id error");
                return BadRequest();
            }

            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);

            var villaUpdateDTO = new VillaUpdateDTO()
            {
                Amenity = villa.Amenity,
                Name = villa.Name,
                Details = villa.Details,
                ImageURL = villa.ImageURL,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                SquareFoot = villa.SquareFoot,
                Id = villa.Id,
                
            };

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

            var model = new Villa()
            {
                Name = villaUpdateDTO.Name,
                Details = villaUpdateDTO.Details,
                ImageURL = villaUpdateDTO.ImageURL,
                Occupancy = villaUpdateDTO.Occupancy,
                Rate = villaUpdateDTO.Rate,
                SquareFoot = villaUpdateDTO.SquareFoot,
                Id = villaUpdateDTO.Id,
                Amenity = villaUpdateDTO.Amenity,
            };

            _db.Villas.Update(model);
            await _db.SaveChangesAsync();

            _logger.LogInformation($"Villa patch successful!");
            return Ok();
        }
    }
}
    