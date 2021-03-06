﻿using Microsoft.AspNetCore.Mvc;
using Registry.Models;
using ServiceGovernance.Registry.Stores;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Registry.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServiceStore _serviceStore;

        public HomeController(IServiceStore serviceStore)
        {
            _serviceStore = serviceStore ?? throw new ArgumentNullException(nameof(serviceStore));
        }

        public async Task<IActionResult> Index()
        {
            return View(await _serviceStore.GetAllAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
