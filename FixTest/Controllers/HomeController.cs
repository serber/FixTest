using System.Threading.Tasks;
using FixTest.Entities;
using Microsoft.AspNetCore.Mvc;
using FixTest.Models;
using FixTest.Services.Interfaces;

namespace FixTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebSiteService _webSiteService;
        private readonly IScheduleService _scheduleService;

        public HomeController(IWebSiteService webSiteService, IScheduleService scheduleService)
        {
            _webSiteService = webSiteService;
            _scheduleService = scheduleService;
        }

        public IActionResult Index()
        {
            return View(_webSiteService.List());
        }

        [HttpGet("~/add")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost("~/add")]
        public async Task<IActionResult> Add(WebSiteViewModel model)
        {
            WebSite webSite = await _webSiteService.Add(model.Url, model.CheckInterval);

            _scheduleService.AddOrUpdate(webSite);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("~/edit/{id:long}")]
        public async Task<IActionResult> Edit(long id)
        {
            WebSite webSite = await _webSiteService.Get(id);
            if (webSite == null)
            {
                return NotFound();
            }

            WebSiteViewModel viewModel = new WebSiteViewModel
            {
                CheckInterval = webSite.CheckInterval,
                Url = webSite.Url
            };

            return View(viewModel);
        }

        [HttpPost("~/edit/{id:long}")]
        public async Task<IActionResult> Edit(long id, WebSiteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            WebSite webSite = await _webSiteService.Edit(id, model.Url, model.CheckInterval);
            
            _scheduleService.AddOrUpdate(webSite);

            return RedirectToAction("Index", "Home");
        }
    }
}