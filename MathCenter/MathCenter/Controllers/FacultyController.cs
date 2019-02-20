using MathCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MathCenter.Controllers
{
    public class FacultyController : Controller
    {
        MathContext db = new MathContext();
        // GET: Faculty
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Data()
        {
            return View(db.SignIns.ToList());
        }
    }
}