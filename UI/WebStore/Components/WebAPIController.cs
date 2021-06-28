﻿using Microsoft.AspNetCore.Mvc;
using WebStore.Inerfaces.TestAPI;

namespace WebStore.Components
{
    public class WebAPIController : Controller
    {
        private readonly IValuesService _ValuesService;

        public WebAPIController(IValuesService ValuesService)
        {
            _ValuesService = ValuesService;
        }
        public IActionResult Index()
        {
            var value = _ValuesService.GetAll();
            return View(value);
        }
    }
}
