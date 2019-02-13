using MathCenter.Models;
using MathCenter.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MathCenter.Controllers
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
            if (db.Students.Find(VNum) != null)
            {
                return RedirectToAction("Done");
            }
            //Otherwise, redirect to create student page.
            return RedirectToAction("CreateStudent", new { Id = VNum });
        }

        public ActionResult Done()
        {

            return View();
        }

        /*
         * This method will finish the "creation" process of a student.
         */
        public ActionResult CreateStudent(int Id)
        {
            ViewBag.Id = Id;

            return View();
        }

        /*
         * This method will continue the "creation" process of a student.
         */
         [HttpPost]
         public ActionResult CreateStudent(int Id, string FirstName, string LastName)
        {
            Person person = new Person { VNum = Id, FirstName = FirstName, LastName = LastName };
            Debug.WriteLine(person.FirstName + ", " + person.LastName + ", " + person.VNum);
            if (person != null)
            {
                return RedirectToAction("SelectClass", new { Adult = person });
            }
            return View();
        }

        /*
         * This method will allow the student to select their class
         */
         public ActionResult SelectClass(Person Adult)
        {
            return View();
        }
    }
}