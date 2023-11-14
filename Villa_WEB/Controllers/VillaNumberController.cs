using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Villa_WEB.Models;
using Villa_WEB.Models.Dto;
using Villa_WEB.Models.VM;
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
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM  model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.CreateAsyn<APIResponse>(model.VillaNumber);

                if (response != null && response.IsSuccess == true)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                if (response.IsSuccess==false && response.ErrorMessages.Count>0)
                {
                    ModelState.AddModelError("ErrorMessages", "VillaNumber already exists");
                }

            }

            var res = await _villaService.GetAllAsyc<APIResponse>();
            if (res != null && res.IsSuccess == true)
            {
                model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                    (Convert.ToString(res.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString(),
                    });
            }
            return View(model);
        }

        public async Task<IActionResult> UpdateVillaNumber(int VillaNo)
        {
            VillaNumberUpdateVM villaNumberVM = new ();
            var response = await _villaNumberService.GetAsync<APIResponse>(VillaNo);

            if (response != null && response.IsSuccess == true)
            {
                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
                villaNumberVM.VillaNumber = _mapper.Map<VillaNumberUpdateDTO>(model);
            }

            response = await _villaService.GetAllAsyc<APIResponse>();
            if (response != null && response.IsSuccess == true)
            {
                villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString(),
                    });
                return View(villaNumberVM);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.UpdateAsyn<APIResponse>(model.VillaNumber);

                if (response != null && response.IsSuccess == true)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                if (response.IsSuccess == false && response.ErrorMessages.Count > 0)
                {
                    ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                }

            }

            var res = await _villaService.GetAllAsyc<APIResponse>();
            if (res != null && res.IsSuccess == true)
            {
                model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                    (Convert.ToString(res.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString(),
                    });
            }
            return View(model);
        }
        public async Task<IActionResult> DeleteVillaNumber(int villaNo)
        {
            VillaNumberDeleteVM villaNumberVM = new ();
            var response = await _villaNumberService.GetAsync<APIResponse>(villaNo);

            if (response != null && response.IsSuccess == true)
            {
                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
                villaNumberVM.VillaNumber = model;
            }

            response = await _villaService.GetAllAsyc<APIResponse>();
            if (response != null && response.IsSuccess == true)
            {
                villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString(),
                    });
                return View(villaNumberVM);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM model)
        {

            var response = await _villaNumberService.DeleteAsyn<APIResponse>(model.VillaNumber.VillaNo);

            if (response != null && response.IsSuccess == true)
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }

            return View(model);
        }
    }
}
