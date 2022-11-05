using Core.Dtos;
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
            var respone = new SearchUserResponseDto();
            var userModel = await _userClient.GetAll();
            respone.UserModels = userModel;
            respone.TotalCount = userModel.Count;
            return View("~/pages/Index.cshtml", respone);
        }

        [HttpGet]
        public ActionResult NotFound(SearchUserResponseDto searchUserResponseDto)
        {
           
            return View("~/pages/Index.cshtml", searchUserResponseDto);
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
            return View("~/Pages/Create.cshtml");
        }

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserModel userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _userClient.CreateUserAsync(userModel);
                    return RedirectToAction("Index");
                }
                return View("~/Pages/Create.cshtml", userModel);

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
            return View("~/Pages/Edit.cshtml", userModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserModel userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _userClient.UpdateAsync(userModel);
                    return RedirectToAction("Index");
                }
                return View("~/Pages/Edit.cshtml", userModel);
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


        [HttpGet]
        public async Task<ActionResult> Search(string SearchField)
        {
            var responseDto = new SearchUserResponseDto();
            try
            {
                var result = await _userClient.Search(SearchField);
                if(result != null && result.UserModels.Any())
                {
                    return RedirectToAction("Index", result);

                }
                return RedirectToAction("NotFound", responseDto);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}
