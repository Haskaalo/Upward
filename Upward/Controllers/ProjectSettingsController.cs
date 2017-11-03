using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Upward.ActionFilters;

namespace Upward.Controllers
{
    public class ProjectSettingsController : Controller
    {
        // GET: /create
        [HttpGet("/create"), ValidApiKey(MustCheck = true)]
        public IActionResult Index()
        {
            return Ok("test");
        }
    }
}
