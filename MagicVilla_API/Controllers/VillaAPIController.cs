﻿using AutoMapper;
using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repository;
using MagicVilla_API.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
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
        private readonly IVillaRepository _RepoVilla;
        private readonly IMapper _mapper;

        public VillaAPIController(
            ILogger<VillaAPIController> logger, IVillaRepository RepoVilla, IMapper mapper)  
        {
            _logger = logger;
            _RepoVilla=RepoVilla;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            _logger.LogInformation($"Geting all villas");

            IEnumerable<Villa> villaList = await _RepoVilla.GetAllAsync();
            return Ok(_mapper.Map<List<VillaDTO>>(villaList));
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

            var villa = await _RepoVilla.GetAsync(u => u.Id == id);
            if (villa == null)
            {
                _logger.LogError($"Villa error with id - {id}");
                return NotFound();
            }
            _logger.LogInformation($"Geting villa - {id}");
            return Ok(_mapper.Map<VillaDTO>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO createDTO)
        {
            if (await _RepoVilla.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("Custom Error", "Villa already exists!");
                _logger.LogError($"Villa create error");
                return BadRequest(ModelState);
            }
            if (createDTO == null)
            {
                _logger.LogError($"Villa create error - null");
                return BadRequest(createDTO);
            }

            Villa model = _mapper.Map<Villa>(createDTO);

            await _RepoVilla.CreateAsync(model);

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
            var villa = await _RepoVilla.GetAsync(v => v.Id == id);

            if (villa == null)
            {
                _logger.LogError($"Villa delete unsuccessful - null");
                return NotFound();
            }

            _RepoVilla.RemoveAsync(villa);
            _logger.LogInformation($"Villa delete successful");
            return NoContent();
        }

        [HttpPut ("{id:int}" , Name ="UpdateVilla")]
        [ProducesResponseType (StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound) ]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
        {
            if (updateDTO.Id != id || updateDTO == null) 
            {
                _logger.LogError($"Villa update unsuccessful - id error");
                return BadRequest();
            }

            var villa = await _RepoVilla.GetAsync(v => v.Id == id,tracked:false);

            if (villa == null) {
                _logger.LogError($"Villa update unsuccessful - null");
                return NotFound();
            }
            Villa model = _mapper.Map<Villa>(updateDTO);

            model.CreatedDate= villa.CreatedDate;

            await _RepoVilla.UpdateAsync(model);

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

            var villa = await _RepoVilla.GetAsync(v => v.Id == id, tracked:false);

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
            return Ok();

        }
    }
}
    