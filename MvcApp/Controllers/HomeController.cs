using Core.Interfaces.IClients;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserClient _userClient;
        public HomeController(IUserClient userClient)
        {
            _userClient = userClient;
        }
        // GET: HomeController
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var userModel = await _userClient.GetAll();
            return View("~/pages/Index.cshtml", userModel);
        }

        // GET: HomeController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var result = await _userClient.FindOneAsync(id);
            return View(result);
        }

        // GET: HomeController/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View("~/pages/Create.cshtml");
        }

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserModel userModel)
        {
            try
            {
                var result = await _userClient.CreateUserAsync(userModel);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var userModel = await _userClient.FindOneAsync(id);
            return View(userModel);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, UserModel userModel)
        {
            try
            {
                await _userClient.UpdateAsync(id, userModel);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _userClient.DeleteAsync(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}
