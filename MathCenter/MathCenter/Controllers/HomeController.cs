using MathCenter.Models;
using MathCenter.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
        public ActionResult SignIn(string VNum, int Week)
        {
            //If no input, refresh page.
            if (VNum == null || VNum.Length != 8)
            {
                ViewBag.Error = "Your V Number is invalid. It must be 8 characters. Please do not include the V.";
                return View();
            }

            //If student is in the database, redirect to done page.
            if (db.Students.Find(VNum) != null)
            {
                return RedirectToAction("Done", new { VNum, Week });
            }
            //Otherwise, redirect to create student page.            
            return RedirectToAction("NameInput", new { VNum, Week });
        }

        /*
        * This is the method for inputting their name. It will allow the user
        * to input their name and get to adding their class.
        * 
        * It will also add the student to the database and provide a placeholder
        * class so we don't have to carry around all the information.
        */
        [HttpGet]
        public ActionResult NameInput(string VNum, int Week)
        {
            //Use the VNum and Week in the View.
            ViewBag.VNum = VNum;
            ViewBag.Week = Week;

            //Return the View
            return View();
        }
        [HttpPost]
        public ActionResult NameInput(PersonWeek pWeek)
        {
            //Create a student and set the class to the "placeholder class" created in the up script.
            Student student = new Student { VNum = pWeek.VNum, FirstName = pWeek.FirstName, LastName = pWeek.LastName, Class = -1 };

            //Add student to database, we will change the class later.
            try
            {
                db.Students.Add(student);
                db.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
            }

            //Redirect to the select department method and slowly select the class.
            return RedirectToAction("ClassDept", new { NumWeek = pWeek.Week, Id = student.VNum});
        }

        /*
         * This is the method for selecting the class prefix for the class the
         * user is taking. (ex: MTH, FYS, PSY, etc.) It will also have an "other"
         * category for people not in any of the available classes.
         */
         [HttpGet]
         public ActionResult ClassDept(int NumWeek, string Id )
        {
            //Find all of the distict Class Prefixes and use that for the drop down.
            var ClassDepts = db.Classes
                .GroupBy(c => c.DeptPrefix)
                .Select(c => c.FirstOrDefault())
                .ToList();

            //Remove the placeholder class from the list of options.
            var remClass = ClassDepts.Where(c => c.DeptPrefix == "NO").Select(c => c).FirstOrDefault();
            ClassDepts.Remove(remClass);

            //Keep these floating around so we can easily have the stuff working. 
            ViewBag.Id = Id;
            ViewBag.NumWeek = NumWeek;

            //Return the View so students can select their DeptPrefix.
            return View(ClassDepts);
        }
        [HttpPost]
        public ActionResult ClassDept(int NumWeek, string Id, string dept)
        {
            //Check if the dept selected was other.
            if (dept == "other"){
                //Redirect to the Page to Type in the "other" info.
                return RedirectToAction("Other", new { WeekNum = NumWeek, Id });
            }
            else
            {
                //Redirect to the Page to select the Class Number
                return RedirectToAction("ClassNum", new { WeekNum = NumWeek, Id, Dept = dept });
            }
        }

        /*
         * This is the method for selecting the class number for the class the
         * user is taking. (ex: 111, 102, 344, etc.) It will also have an "other"
         * category for people not in any of the available classes.
         */
        [HttpGet]
        public ActionResult ClassNum(int WeekNum, string Id, string Dept)
        {
            Debug.WriteLine("Dept = " + Dept);
            
            //Find all of the distict Class Numbers in relation to the Prefix Given and use that for the drop down.
            var ClassNums = db.Classes
                .Where(c => c.DeptPrefix == Dept)
                .GroupBy(c => c.ClassNum)
                .Select(c => c.FirstOrDefault())
                .ToList();

            //Remove the placeholder class from the list of options.
            var remClass = ClassNums.Where(c => c.DeptPrefix == "NO").Select(c => c).FirstOrDefault();
            ClassNums.Remove(remClass);

            //Keep these floating around so we can easily have the stuff working. 
            ViewBag.Id = Id;
            ViewBag.WeekNum = WeekNum;

            // Return the View so students can select their Class Number.
            return View(ClassNums);
        }
    }
}