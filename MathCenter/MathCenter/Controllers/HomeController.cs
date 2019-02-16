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
         * sign in sheet or the data. (Depending on which you are.) It will 
         * check passwords and return the view necessary for who signed in.
         */
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string button, string tutorPwd, string facultyPwd, int? Week)
        {
            //Get the passwords from the outside file so the passwords are not hard-coded.
            //Hard code passwords since the outside file was being dumb.
            string tutorPass = "Math42";
                //System.Configuration.ConfigurationManager.AppSettings["TutorPass"];
            string facultyPass = "Math42";
                //System.Configuration.ConfigurationManager.AppSettings["FacultyPass"];

            //Check which button was pressed.
            if (button == "tutor")
            {
                //Check the password and make sure there is a week input.
                if (tutorPwd == tutorPass && Week != -1)
                {
                    return RedirectToAction("SignIn", new {Week});
                }
                //Return specific errors if the input is not valid.
                else if(tutorPwd != tutorPass)
                {
                    ViewBag.Error = "You typed in the wrong password. Please Try Again.";
                    return View();
                }
                else
                {
                    ViewBag.Error = "You did not select a week, please select a week number.";
                    return View();
                }
            }
            else
            {
                //Check the password.
                if (facultyPwd == facultyPass)
                {
                    return RedirectToAction("FacultyStuff");
                }
                //Return specific errors if the input is not valid.
                else
                {
                    ViewBag.Error = "Please Try Again. You typed in the wrong password.";
                    return View();
                }
            }
        }

        /*
        * The method for signing a student in. This page will just have the student
        * enter their V Number, then it will direct them to select their class and 
        * add them to the database.
        */
        [HttpGet]
        public ActionResult SignIn(int Week)
        {
            ViewBag.Num = Week;
            return View();
        }             
        [HttpPost]
        public ActionResult SignIn()
        {                
            ////If no input, refresh page.
            //if (VNum == null || VNum.Length != 8)
            //{
            //    ViewBag.Error = "Your V Number is invalid. It must be 8 characters. Please do not include the V.";
            //    return View();
            //}            

            ////If student is in the database, redirect to done page.
            //if (db.Students.Find(VNum) != null)
            //{
            //    return RedirectToAction("Done", new { VNum, Week });
            //}
            ////Otherwise, redirect to create student page.            
            //return RedirectToAction("NameInput", new { VNum, Week });
            return View();
        }

        /*
        * This is the method for inputting their name. It will allow the user
        * to input their name and get to adding their class.
        */
        [HttpGet]
        public ActionResult NameInput(string VNum, int Week)
        {
            ViewBag.VNum = VNum;
            ViewBag.Week = Week;

            return View();
        }
        [HttpPost]
        public ActionResult NameInput(PersonWeek pWeek)
        {
            return RedirectToAction("ClassDept", new { Num = pWeek.VNum, NumWeek = pWeek.Week, FName = pWeek.FirstName, LName = pWeek.LastName });
        }

        /*
         * This is the method for selecting the class prefix for the class the
         * user is taking. (ex: MTH, FYS, PSY, etc.) It will also have an "other"
         * category for people not in any of the available classes.
         */
         [HttpGet]
         public ActionResult ClassDept(string Num, int NumWeek, string FName, string LName)
        {
            PersonWeek pWeek = new PersonWeek { VNum = Num, Week = NumWeek, FirstName = FName, LastName = LName};

            return View();
        }
    }
}