using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Villa_WEB.Models;
using Villa_WEB.Models.Dto;
using Villa_WEB.Services;
using Villa_WEB.Services.IServices;

namespace Villa_WEB.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVIllaNumberService _villaNumberService;
        private readonly IMapper _mapper;

        public VillaNumberController(IVIllaNumberService vIllaNumberService, IMapper mapper)
        {
            _villaNumberService = vIllaNumberService;
            _mapper = mapper;
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
    }
}
