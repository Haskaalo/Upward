using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Upward.ActionFilters;
using Upward.Models;
using Microsoft.AspNetCore.Http;

namespace Upward.Controllers
{
    public class ProjectSettingsController : Controller
    {
        // GET: /create
        [HttpPost("/create"), ValidApiKey(MustCheck = true)]
        public IActionResult Create(ICollection<IFormFile> files)
        {
            var ErrorMessage = new ApiErrorModel();
            if (files.Count() == 0)
            {
                ErrorMessage.code = "NoFileAttached";
                ErrorMessage.message = "No file(s) was attached to the request.";

                HttpContext.Response.StatusCode = 400;
                return Json(ErrorMessage);
            }

            ErrorMessage.code = "ErrorHappened";
            ErrorMessage.message = "An error happened while creating object.";

            HttpContext.Response.StatusCode = 500;
            return Json(ErrorMessage);
        }
    }
}
