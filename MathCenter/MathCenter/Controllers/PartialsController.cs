using MathCenter.Models;
using MathCenter.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace MathCenter.Controllers
{
    public class PartialsController : Controller
    {
        public PartialViewResult _Errors(string error)
        {
            ViewBag.Error = error;
            return PartialView();
        }
    }
}