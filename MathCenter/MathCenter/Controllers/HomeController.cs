using MathCenter.Models;
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

        [HttpPost]
        public ActionResult Index(string button, string tutorPwd, string facultyPwd)
        {
            //Get the passwords from the outside file so the passwords are not hard-coded.
            string tutorPass = "Math42";
                //System.Configuration.ConfigurationManager.AppSettings["TutorPass"];
            string facultyPass = "Math42";
                //System.Configuration.ConfigurationManager.AppSettings["FacultyPass"];

            Debug.WriteLine(button);
            if (button == "tutor")
            {
                Debug.WriteLine(tutorPwd);
                Debug.WriteLine(tutorPass);
                if (tutorPwd == tutorPass)
                {
                    return RedirectToAction("SignIn");
                }
                else
                {
                    ViewBag.Error = "You typed in the wrong password. Please Try Again.";
                    return View();
                }
            }
            if(button == "faculty")
            {
                Debug.WriteLine(facultyPwd);
                Debug.WriteLine(facultyPass);
                if (facultyPwd == facultyPass)
                {
                    return RedirectToAction("FacultyStuff");
                }
                else
                {
                    ViewBag.Error = "Please Try Again. You typed in the wrong password.";
                    return View();
                }
            }
            return View();
        }

        /*
         * The method for signing a student in. This page will just have the student
         * enter their V Number, then it will direct them to select their class and 
         * add them to the database.
         */
        [HttpGet]
        public ActionResult SignIn(string VNum)
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
    }
}