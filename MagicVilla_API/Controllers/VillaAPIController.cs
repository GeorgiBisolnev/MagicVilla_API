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
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.LogInformation($"Geting all villas");
            return Ok(_db.Villas.ToList());
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

            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
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
            if (_db.Villas.FirstOrDefault(u => u.Name.ToLower() == villa.Name.ToLower()) != null)
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

            var model = new Villa()
            {
                Name = villa.Name,
                Details = villa.Details,
                IageURL = villa.IageURL,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                SquareFoot = villa.SquareFoot,
                Id = villa.Id,
                Amenity = villa.Amenity,
            };

            _db.Villas.Add(model);
            _db.SaveChanges();

            _logger.LogInformation($"Villa create successful");
            return CreatedAtRoute("GetVilla", new { villa.Id }, villa);
            //return Ok(villa);
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public IActionResult DeleteVilla(int id)
        {
            if (id <= 0)
            {
                _logger.LogError($"Villa delete unsuccessful - id error");
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(v => v.Id == id);

            if (villa == null)
            {
                _logger.LogError($"Villa delete unsuccessful - null");
                return NotFound();
            }

            _db.Villas.Remove(villa);
            _db.SaveChanges();
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

            var villa = _db.Villas.AsNoTracking().FirstOrDefault (v => v.Id == id);

            if (villa == null) {
                _logger.LogError($"Villa update unsuccessful - null");
                return NotFound();
            }

            var model = new Villa()
            {
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                IageURL = villaDTO.IageURL,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                SquareFoot = villaDTO.SquareFoot,
                Id = villaDTO.Id,
                Amenity = villaDTO.Amenity,
            };

            _db.Villas.Update (model);
            _db.SaveChanges();

            _logger.LogInformation($"Villa update successful!");
            return NoContent();
        }

        [HttpPatch ("{id:int}" , Name ="PatchVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> pacthDTO)
        {
            if (pacthDTO==null || id<=0)
            {
                _logger.LogError($"Villa patch unsuccessful - id error");
                return BadRequest();
            }

            var villa = _db.Villas.AsNoTracking().FirstOrDefault(v => v.Id == id);

            var villaDTO = new VillaDTO()
            {
                Amenity = villa.Amenity,
                Name = villa.Name,
                Details = villa.Details,
                IageURL = villa.IageURL,
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
            
            pacthDTO.ApplyTo(villaDTO, ModelState);

            if (!ModelState.IsValid)
            {
                _logger.LogError($"Villa patch unsuccessful - model state not valid");
                return BadRequest();
            }

            var model = new Villa()
            {
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                IageURL = villaDTO.IageURL,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                SquareFoot = villaDTO.SquareFoot,
                Id = villaDTO.Id,
                Amenity = villaDTO.Amenity,
            };

            _db.Villas.Update(model);
            _db.SaveChanges();

            _logger.LogInformation($"Villa patch successful!");
            return Ok();
        }
    }
}
    