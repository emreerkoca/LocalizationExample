using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using LocalizationExample;
using LocalizationExample.Localize;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace LocalizationExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IStringLocalizer<Resource> _stringLocalizer;
        public HomeController(IStringLocalizer<Resource> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }

        [HttpGet]
        public string Get()
        {
            var value = _stringLocalizer["Hello"];

            return value;
        }
    }
}
