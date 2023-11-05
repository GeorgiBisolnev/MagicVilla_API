using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using Villa_WEB.Models;
using Villa_WEB.Models.Dto;
using Villa_WEB.Services.IServices;

namespace Villa_WEB.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVIllaService _villaService;
        private readonly IMapper _mapper;

        public VillaController(IVIllaService vIllaService, IMapper mapper)
        {
            _villaService = vIllaService;
            _mapper = mapper;
        }
        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDTO> list = new();

            var response = await _villaService.GetAllAsyc<APIResponse>();
            if (response != null && response.IsSuccess==true) 
            {
                list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }

            return View(list);
        }
        
        public async Task<IActionResult> CreateVilla()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVilla(VillaCreateDTO model)
        {
            if (ModelState.IsValid) 
            {
                var response = await _villaService.CreateAsyn<APIResponse>(model);

                if (response!=null && response.IsSuccess==true)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }
               
            }
            return View(model);
        }

        public async Task<IActionResult> UpdateVilla(int villaId)
        {
            var response = await _villaService.GetAsync<APIResponse>(villaId);

            if (response != null && response.IsSuccess == true)
            {
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
                return View(_mapper.Map<VillaUpdateDTO>(model));
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaService.UpdateAsyn<APIResponse>(model);

                if (response != null && response.IsSuccess == true)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }

            }
            return View(model);
        }
    }
}
