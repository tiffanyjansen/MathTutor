using MCv1._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MCv1._0.Controllers
{
    public class HomeController : Controller
    {
        //Access to Database.
        MathContext db = new MathContext();


        /*
         * The "Home Page." The page for Tutors/Faculty to either access the 
         * sign in sheet or the data. (Depending on which you are.)
         */ 
        public ActionResult Index()
        {
            return View();
        }

        /*
         * The method for signing a student in. This page will just have the student
         * enter their V Number, then it will direct them to select their class and 
         * add them to the database.
         */
        [HttpGet]
        public ActionResult SignIn(int? VNum)
        {
            //If no input, refresh page.
            if (VNum == null)
            {
                return View();
            }
            //If student is in the database, redirect to done page.
            if(db.Students.Find(VNum) != null)
            {
                return RedirectToAction("Done");
            }
            //Otherwise, redirect to create student page.
            return RedirectToAction("CreateStudent");
        }

        public ActionResult Done()
        {
            
            return View();
        }
    }
}