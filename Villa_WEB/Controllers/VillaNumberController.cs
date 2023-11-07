using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using Villa_WEB.Models;
using Villa_WEB.Models.Dto;
using Villa_WEB.Models.VM;
using Villa_WEB.Services;
using Villa_WEB.Services.IServices;

namespace Villa_WEB.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVIllaNumberService _villaNumberService;
        private readonly IVIllaService _villaService;
        private readonly IMapper _mapper;

        public VillaNumberController(IVIllaNumberService vIllaNumberService, IMapper mapper, IVIllaService villaService)
        {
            _villaNumberService = vIllaNumberService;
            _mapper = mapper;
            _villaService = villaService;

        }

        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO> list = new();

            var response = await _villaNumberService.GetAllAsyc<APIResponse>();
            if (response != null && response.IsSuccess == true)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            }

            return View(list);
        }

        public async Task<IActionResult> CreateVillaNumber()
        {
            VillaNumberCreateVM villanumberVm = new VillaNumberCreateVM();
            var response = await _villaService.GetAllAsyc<APIResponse>();
            if (response != null && response.IsSuccess == true)
            {
                villanumberVm.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                    (Convert.ToString(response.Result)).Select(i=> new SelectListItem 
                    { 
                        Text = i.Name,
                        Value = i.Id.ToString(),
                    });
            }
            return View(villanumberVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaNumber(VillaCreateDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaService.CreateAsyn<APIResponse>(model);

                if (response != null && response.IsSuccess == true)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }

            }
            return View(model);
        }

        //public async Task<IActionResult> UpdateVilla(int villaId)
        //{
        //    var response = await _villaService.GetAsync<APIResponse>(villaId);

        //    if (response != null && response.IsSuccess == true)
        //    {
        //        VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
        //        return View(_mapper.Map<VillaUpdateDTO>(model));
        //    }
        //    return NotFound();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> UpdateVilla(VillaUpdateDTO model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var response = await _villaService.UpdateAsyn<APIResponse>(model);

        //        if (response != null && response.IsSuccess == true)
        //        {
        //            return RedirectToAction(nameof(IndexVilla));
        //        }

        //    }
        //    return View(model);
        //}
        //public async Task<IActionResult> DeleteVilla(int villaId)
        //{
        //    var response = await _villaService.GetAsync<APIResponse>(villaId);

        //    if (response != null && response.IsSuccess == true)
        //    {
        //        VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
        //        return View(model);
        //    }
        //    return NotFound();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteVillaNumber(VillaDTO model)
        //{

        //    var response = await _villaService.DeleteAsyn<APIResponse>(model.Id);

        //    if (response != null && response.IsSuccess == true)
        //    {
        //        return RedirectToAction(nameof(IndexVilla));
        //    }

        //    return View(model);
        //}
    }
}
