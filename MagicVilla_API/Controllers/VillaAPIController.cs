using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStore.villaList);
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0) 
            {
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villa) 
        {
            if (VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villa.Name.ToLower())!=null) 
                {
                ModelState.AddModelError("Custom Error", "Villa already exists!");               
                return BadRequest(ModelState);
                }
            if (villa==null)
            {
                return NotFound();
            }
            if (villa.Id>0)
            {
                return BadRequest();
            }

            villa.Id = VillaStore.villaList.OrderByDescending(villa => villa.Id).First().Id + 1 ;

            VillaStore.villaList.Add(villa);

            return CreatedAtRoute("GetVilla",  new { villa.Id }, villa);
            //return Ok(villa);
        }
    }
}
    