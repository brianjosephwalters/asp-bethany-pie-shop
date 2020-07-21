using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BethanysPieShop.Models;
using BethanysPieShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BethanysPieShop.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Authorize(Policy = "DeletePie")]
    public class PieManagementController : Controller
    {
        private readonly IPieRepository _pieRepository;
        private readonly ICategoryRepository _categoryRepository;

        public PieManagementController(IPieRepository pieRepository, ICategoryRepository categorgyRepository)
        {
            _pieRepository = pieRepository;
            _categoryRepository = categorgyRepository;
        }

        public ViewResult Index()
        {
            return View(_pieRepository.AllPies.OrderBy(p => p.PieId));
        }

        public IActionResult AddPie()
        {
            var categories = _categoryRepository.AllCategories;
            var pieEditViewModel = new PieEditViewModel()
            {
                Categories = categories.Select(c => new SelectListItem() { Text = c.CategoryName, Value = c.CategoryName }).ToList(),
                CategoryId = categories.FirstOrDefault().CategoryId
            };
            return View(pieEditViewModel);
        }

        [HttpPost]
        public IActionResult AddPie(PieEditViewModel pieEditViewModel)
        {
            if (ModelState.IsValid)
            {
                _pieRepository.CreatePie(pieEditViewModel.Pie);
                return RedirectToAction("Index");
            }
            return View(pieEditViewModel);
        }

        public IActionResult EditPie(int pieId)
        {
            var categories = _categoryRepository.AllCategories;

            var pie = _pieRepository.AllPies.FirstOrDefault(p => p.PieId == pieId);

            var pieEditViewModel = new PieEditViewModel()
            {
                Categories = categories.Select(c => new SelectListItem() { Text = c.CategoryName, Value = c.CategoryName }).ToList(),
                Pie = pie,
                CategoryId = categories.FirstOrDefault().CategoryId
            };

            var item = pieEditViewModel.Categories.FirstOrDefault(c => c.Value == pie.Category.CategoryName);
            item.Selected = true;
            return View(pieEditViewModel);
        }
    }
}
