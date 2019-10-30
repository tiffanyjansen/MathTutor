using MathCenter.Models;
using MathCenter.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace MathCenter.Controllers
{
    public class HomeController : Controller
    {
        //Access to Database.
        private readonly MathContext db = new MathContext();

        /// <summary>
        /// The "Home Page." The page for Tutors/Faculty to either access the 
        /// sign in sheet or the data. (Depending on which you are.) It will 
        ///  check passwords and return the view necessary for who signed in.
        /// </summary>
        /// <returns>The View</returns>
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string tutorPwd, int Week) //When tutor form has been submitted.
        {
            //password
            string tutorPass = "Math42"; 

            //Check the password and make sure there is a week input.
            if (tutorPwd == tutorPass && Week != -1)
            {
                return RedirectToAction("Welcome", new { Week });
            }
            //Return specific errors if the input is not valid.
            else if (Week == -1)
            {
                ViewBag.Error = "Please select a week number.";
            }            
            else
            {
                ViewBag.Error = "Incorrect password. Please try again.";                
            }
            return View();
        }
        [HttpPost]
        public ActionResult Faculty(string facultyPwd) //When faculty form has been submitted.
        {
            //password
            string facultyPass = "Math42";

            //Check the password.
            if (facultyPwd == facultyPass)
            {
                return RedirectToAction("Index", "Faculty");
            }
            //Return specific errors if the input is not valid.
            else
            {
                ViewBag.Error = "Incorrect password. Please try again.";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        ///  The method for signing a student in. This page will just have the student
        ///  enter their V Number, then it will direct them to select their class and 
        ///  add them to the database
        /// </summary>
        /// <param name="Week">The Week Number</param>
        /// <returns>The View</returns>
        [HttpGet]
        public ActionResult Welcome(int? Week)
        {
            //Check for no input. This just adds extra error-handling.
            if (Week == null)
            { 
                return RedirectToAction("Index");
            }
            ViewBag.Week = Week;
            return View();
        }
        [HttpPost]
        public ActionResult Welcome(string VNum, int Week)
        {
            //Check if V Number is all numbers and the correct length
            Regex rx = new Regex(@"^\d{8}$", RegexOptions.IgnoreCase);
            if (rx.IsMatch(VNum))
            {
                //If student is in the database, redirect to 'done' page.
                if (db.Students.Find(VNum) != null)
                {
                    return RedirectToAction("Done", new { VNum, Week });
                }
                //Otherwise, redirect to create student page.            
                return RedirectToAction("Name", new { VNum, Week });
            }
            else
            {
                ViewBag.Error = "Your V Number is invalid. It must be 8 characters. Please do not include the V.";
                ViewBag.Num = Week;
                return View();
            }           
        }

        /// <summary>
        /// This is the method for inputting their name. It will allow the user
        /// to input their name and get to adding their class.
        /// 
        /// It will also add the student to the database.
        /// </summary>
        /// <param name="VNum">The Student's V-Number</param>
        /// <param name="Week">The Week Number</param>
        /// <returns>The View</returns>
        [HttpGet]
        public ActionResult Name(string VNum, int? Week)
        {
            //Check for no input. This just adds extra error-handling.
            if (Week == null || VNum == null)
            {
                if (Week == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Welcome", new { Week });
                }
            }

            //Use the VNum and Week in the View.
            ViewBag.VNum = VNum;
            ViewBag.Week = Week;

            //Return the View
            return View();
        }
        [HttpPost]
        public ActionResult Name(PersonWeek pWeek)
        {
            Regex rx = new Regex(@"^[a-zA-Z-\.\s]+$");
            if (rx.IsMatch(pWeek.FirstName) && rx.IsMatch(pWeek.LastName))
            {
                try
                {
                    string firstName = CapitalizeName(pWeek.FirstName);                    
                    string lastName = CapitalizeName(pWeek.LastName);
                    
                    if (db.Students.Find(pWeek.VNum) != null) //Check if Student is already in DB.
                    {
                        Student student = db.Students.Find(pWeek.VNum); //If so, update them.
                        student.FirstName = firstName; 
                        student.LastName = lastName;
                    }
                    else
                    {                       
                        Student student = new Student { VNum = pWeek.VNum, FirstName = firstName, LastName = lastName }; //Create a student.
                        db.Students.Add(student);  //Add the student to the database.
                    }                
                               
                    db.SaveChanges(); //Save the changes to the database. 
                }
                catch (Exception)
                {
                    ViewBag.Error = "There was an error adding you to the database. Please ask a tutor for help.";
                    return View(pWeek);
                }
            }
            else
            {
                ViewBag.Error = "Please input your name.";
                return View(pWeek);
            }

            //Redirect to the select department method and slowly select the class.
            return RedirectToAction("SelectClass", new { pWeek.Week, pWeek.VNum });
        }
        
         /// <summary>
         /// The method for selecting a class. It's a table that is filterable. 
         /// There is some Javascript, for functionality, but no Ajax calls.
         /// </summary>
         /// <param name="Week">The Week Number</param>
         /// <param name="VNum">The Student's V-Number</param>
         /// <returns>The View</returns>
        [HttpGet]
        public ActionResult SelectClass(int? Week, string VNum)
        {
            //TO DO - Update Comments and line spacing.

            //get all the query strings
            var queryStrings = new Dictionary<string, string>();
            foreach (var key in Request.QueryString.AllKeys)
            {
                queryStrings.Add(key, Request.QueryString[key]);
            }

            //Check for no input. This just adds extra error-handling.
            if (Week == null || VNum == null)
            {
                VNum = queryStrings.ContainsKey("VNum") ? queryStrings["VNum"] : VNum;
                Week = queryStrings.ContainsKey("Week") ? Int32.Parse(queryStrings["Week"]) : Week;
                
                if (Week == null)
                {
                    return RedirectToAction("Index");
                }
                else if(VNum == null)
                {
                    return RedirectToAction("Welcome", new { Week });
                }
            }          

            //Keep these floating around so we can easily have the stuff working. 
            ViewBag.Id = VNum;
            ViewBag.Week = Week;

            //remove them so we just have the filters.
            queryStrings.Remove("VNum");
            queryStrings.Remove("Week");

            int[] IdArray = GetClassIdArray(queryStrings);
            if(IdArray.Length > 0)
            {
                ViewBag.SelectedIds = queryStrings["ClassIds"]; //send the thing we got to the server back.
                ViewBag.ClassIds = IdArray;
            }            
            ViewBag.SelectedCount = IdArray.Length;

            //Get all the classes
            var filteredQuery = GetFilteredClassQuery(queryStrings, IdArray);
            var Classes = filteredQuery.ToList(); 
            
            //The select filters.
            ViewBag.Departments = GetClassDepts(filteredQuery); //gets the possible class departments
            ViewBag.ClassNumbers = GetClassNumbers(filteredQuery); //gets the possible class numbers
            ViewBag.Instructors = GetInstructors(filteredQuery); //gets the possible instructors
            ViewBag.Days = GetClassDays(filteredQuery); //gets the possible days
            ViewBag.Times = GetClassTimes(filteredQuery); //gets the possible start times   
            
            if(queryStrings.ContainsKey("SelectedFilter"))
            {
                ViewBag.Filtered = queryStrings["SelectedFilter"];
            }

            return View(Classes);
        }
        [HttpPost]
        public ActionResult SelectClass(string VNum, int? Week, int[] Classes)
        {
            if (Week == null || VNum == null)
            {
                if (Week == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Welcome", new { Week });
                }
            }

            if (Classes == null)
            {
                ViewBag.Id = VNum;
                ViewBag.Week = Week;
                var query = GetFilteredClassQuery(null, null);
                //The select filters.
                ViewBag.Departments = GetClassDepts(query); //gets the possible class departments
                ViewBag.ClassNumbers = GetClassNumbers(query); //gets the possible class numbers
                ViewBag.Instructors = GetInstructors(query); //gets the possible instructors
                ViewBag.Days = GetClassDays(query); //gets the possible days
                ViewBag.Times = GetClassTimes(query); //gets the possible start times
                return View(query.ToList());
            }
            else
            {
                Student currentStudent = db.Students.Find(VNum);
                foreach (var ClassID in Classes)
                {
                    db.StudentClasses.Add(new StudentClass { VNum = currentStudent.VNum, ClassID = (int)ClassID });
                }                
                try
                { 
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    //There was an error.                        
                    ViewBag.Id = VNum;
                    ViewBag.Week = Week;
                    ViewBag.Error = "There was an error with the database. Please try again.";
                    var query = GetFilteredClassQuery(null, null);
                    //The select filters.
                    ViewBag.Departments = GetClassDepts(query); //gets the possible class departments
                    ViewBag.ClassNumbers = GetClassNumbers(query); //gets the possible class numbers
                    ViewBag.Instructors = GetInstructors(query); //gets the possible instructors
                    ViewBag.Days = GetClassDays(query); //gets the possible days
                    ViewBag.Times = GetClassTimes(query); //gets the possible start times
                    return View(query.ToList());
                }

                //Add the SignIn to the Database
                try
                {
                    foreach(var ClassID in Classes)
                    {
                        db.SignIns.Add(new SignIn { Week = (int)Week, Date = DateTime.Today, Hour = DateTime.Now.Hour, Min = DateTime.Now.Minute, StudentID = VNum, ClassID = (int)ClassID }); //add a sign in for every class.
                    }
                    
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    //There was an error
                    ViewBag.Error = "There was an error with the database. Please try again.";
                    ViewBag.Id = VNum;
                    ViewBag.Week = Week;
                    return View(GetFilteredClassQuery(null, null).ToList());
                }
                return RedirectToAction("Finish", new { Week });
            }                     
        }        

        /*
         * The method for when the student selects Other.
         */
        [HttpGet]
        public ActionResult Other(int? Week, string VNum)
        {
            //Check for no input. This just adds extra error-handling.
            if (Week == null || VNum == null)
            {
                if (Week == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Welcome", new { Week });
                }
            }

            //Keep the data floating.
            ViewBag.Week = Week;
            ViewBag.Id = VNum;

            //Return the View.
            return View();
        }
        [HttpPost]
        public ActionResult Other(int Week, string VNum, string other)
        {
            Regex rx = new Regex(@"^[A-Z]{1,3}\s\d{2,3}", RegexOptions.IgnoreCase);
            if (rx.IsMatch(other))
            {
                try
                {
                    //Create the class to be connected to the student.
                    db.Classes.Add(new Class { Other = other });

                    //Save the class into the database.
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    //There was an error.
                    ViewBag.Error = "There was an error with the database. Please try again.";
                    ViewBag.Week = Week;
                    ViewBag.Id = VNum;
                    return View();
                }

                //Get the class again.
                Class sClass = db.Classes
                    .Where(c => c.Other == other)
                    .Select(c => c).FirstOrDefault();

                //Add the class to the current student.
                Student currentStudent = db.Students.Find(VNum);
                db.StudentClasses.Add(new StudentClass { VNum = VNum, ClassID = sClass.ClassID });

                try
                {
                    //Create the Sign In to be added to the db.
                    db.SignIns.Add(new SignIn { Week = Week, Date = DateTime.Today, Hour = DateTime.Now.TimeOfDay.Hours, Min = DateTime.Now.TimeOfDay.Minutes, StudentID = VNum, ClassID = sClass.ClassID });

                    //Save the Changes to the db.
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    //There was an error.
                    ViewBag.Error = "There was an error with the database. Please try again.";
                    ViewBag.Week = Week;
                    ViewBag.Id = VNum;
                    return View();
                }

                //Redirect to the finish page.
                return RedirectToAction("Finish", new { VNum, Week });
            }
            else
            {
                ViewBag.Error = "Please input an actual class.";
                ViewBag.Week = Week;
                ViewBag.Id = VNum;
                return View();
            }
        }
        /*
         * The method for when you are already in the DB and just need to approve the sign in.
         */
        [HttpGet]
        public ActionResult Done(string VNum, int? Week)
        {
            //Check for no input. This just adds extra error-handling.
            if (Week == null || VNum == null)
            {
                if (Week == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Welcome", new { Week });
                }
            }

            //Get the info from the Database about the current student.
            Student currentStudent = db.Students.Find(VNum);

            //Keep the data floating
            ViewBag.VNum = VNum;
            ViewBag.Week = Week;

            //Return the View with the current student.
            return View(currentStudent);
        }
        [HttpPost]
        public ActionResult Done(string VNum, int? Week, int approved, int? classID)
        {
            if (approved == 1 && classID != null)
            {
                try
                {
                    //Create the Sign In and add it to the database.
                    db.SignIns.Add(new SignIn { Week = (int)Week, Date = DateTime.Today, Hour = DateTime.Now.TimeOfDay.Hours, Min = DateTime.Now.TimeOfDay.Minutes, StudentID = VNum, ClassID = (int)classID });
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    //There was an error.
                    ViewBag.Error = "There was an error with the database. Please try again.";
                    //Keep the data floating
                    ViewBag.VNum = VNum;
                    ViewBag.Week = Week;

                    //Return the View with the current student.
                    return View(db.Students.Find(VNum));
                }

                //Redirect to the "finish" page.
                return RedirectToAction("Finish", new { Week });
            }
            else if (approved == 0)
            {
                //If it's not you redirect to Sign In page.
                return RedirectToAction("Name", new { Week, VNum });
            }
            else if (approved == 3)
            {
                //Redirect back to the page to add more classes
                return RedirectToAction("SelectClass", new { VNum, Week });
            }
            else if (approved == 2)
            {
                //Redirect back to the page to input your V-Number
                return RedirectToAction("Welcome", new { Week });
            }
            else
            {
                ViewBag.VNum = VNum;
                ViewBag.Week = Week;
                ViewBag.ClassError = "Please select the class you would like to Sign In for.";
                return View(db.Students.Find(VNum));
            }
        }
        /*
         * The last page that gives a good message.
         */
        [HttpGet]
        public ActionResult Finish(int? Week)
        {
            //Check for no input. This just adds extra error-handling.
            if (Week == null)
            {
                return RedirectToAction("Index");
            }
            //The Week Continues to float.
            ViewBag.Week = Week;

            //Return the View.
            return View();
        }       

        /// <summary>
        /// Capitalize the string given. Basically just formats it so it looks nice everywhere. :)
        /// </summary>
        /// <param name="name">The name we want capitalized</param>
        /// <returns>The name capitalized</returns>        
        protected string CapitalizeName(string name)
        {
            name = char.ToUpper(name[0]) + name.Substring(1);
            int index = 0;
            while (name.IndexOf(' ', index) != -1)
            {
                index = name.IndexOf(' ', index) + 1;
                name = name.Substring(0, index) + char.ToUpper(name[index]) + name.Substring(index + 1);
            }
            if (name.IndexOf('-') != -1)
            {
                index = name.IndexOf('-', index) + 1;
                name = name.Substring(0, index) + char.ToUpper(name[index]) + name.Substring(index + 1);
            }
            return name;
        }


        /// <summary>
        /// Either removes the "Other" classes from the list or gets the other
        /// classes.
        /// </summary>
        /// <param name="Classes">List of classes or null</param>
        /// <param name="remove">true or false</param>
        /// <returns>The new and improved list of classes</returns>
        private List<Class> RemoveOthers(List<Class> Classes = null, bool remove = true)
        {
            List<Class> Others = db.Classes
                .Where(c => c.Other != null)
                .ToList();

            if (remove) //if we want to remove them, then do so.
            {
                foreach (var Other in Others)
                {
                    Classes.Remove(Other); //Remove them from the list passed in.
                }

                return Classes;
            }

            return Others; //otherwise, return the others only

        }

        /// <summary>
        /// Either removes Community College classes or get the list of 
        /// Community College classes
        /// </summary>
        /// <param name="Classes">The list of classes or null</param>
        /// <param name="remove">true or false</param>
        /// <returns>The new and improved list of classes</returns>
        private List<Class> RemoveCCInstructors(List<Class> Classes = null, bool remove = true)
        {
            string[] CCInstructors = { "Portland", "Chemeketa", "Clackamas", "Mt. Hood", "Linn-Benton" }; //All the possible CCIntstructors

            List<Class> CCClasses = db.Classes
                .Where(c => CCInstructors.Contains(c.Instructor)) //Get all the ones in the DB with them. 
                .ToList();

            if (remove) //remove them if we want them removed
            {
                foreach (var CCClass in CCClasses)
                {
                    Classes.Remove(CCClass); //Remove them from the list passed in.
                }

                return Classes; //return the new list. :)
            }

            return CCClasses; //return the cc list only
        }

        /// <summary>
        /// Get the list of distinct class departments with the 
        /// given filters.
        /// </summary>
        /// <param name="filters">The dictionary of filters</param>
        /// <returns>the list of distinct class departments</returns>
        public List<Class> GetClassDepts(IQueryable<Class> filteredQuery)
        {
            List<Class> ClassDepts = filteredQuery
                .GroupBy(c => c.DeptPrefix)
                .Select(c => c.FirstOrDefault())
                .ToList();

            ClassDepts = RemoveOthers(ClassDepts);
            ClassDepts = RemoveCCInstructors(ClassDepts);

            return ClassDepts;
        }

        /// <summary>
        /// Get the list of distinct class numbers with the 
        /// given filters.
        /// </summary>
        /// <param name="filters">The dictionary of filters</param>
        /// <returns>the list of distinct class numbers</returns>
        private List<Class> GetClassNumbers(IQueryable<Class> filteredQuery)
        {
            //Find all of the distict Class Numbers.
            List<Class> ClassNums = filteredQuery
                .GroupBy(c => c.ClassNum)
                .Select(c => c.FirstOrDefault())
                .ToList();

            ClassNums = RemoveOthers(ClassNums);
            ClassNums = RemoveCCInstructors(ClassNums);

            return ClassNums;
        }

        /// <summary>
        /// Get the list of distinct instructors with the 
        /// given filters.
        /// </summary>
        /// <param name="filters">The dictionary of filters</param>
        /// <returns>the list of distinct instructors</returns>
        private List<Class> GetInstructors(IQueryable<Class> filteredQuery)
        {
            List<Class> Instructors = filteredQuery
                .GroupBy(c => c.Instructor)
                .Select(c => c.FirstOrDefault())
                .ToList();

            Instructors = RemoveOthers(Instructors);
            Instructors = RemoveCCInstructors(Instructors);

            return Instructors;
        }

        /// <summary>
        /// Get the list of distinct class days with the 
        /// given filters.
        /// </summary>
        /// <param name="filters">The dictionary of filters</param>
        /// <returns>the list of distinct class days</returns>
        private List<Class> GetClassDays(IQueryable<Class> filteredQuery)
        {
            List<Class> Days = filteredQuery
                .GroupBy(c => c.Days)
                .Select(c => c.FirstOrDefault())
                .ToList();

            Days = RemoveOthers(Days);
            Days = RemoveCCInstructors(Days);

            return Days;
        }

        /// <summary>
        /// Get the list of distinct class times with the 
        /// given filters.
        /// </summary>
        /// <param name="filters">The dictionary of filters</param>
        /// <returns>the list of distinct class times</returns>
        private List<Class> GetClassTimes(IQueryable<Class> filteredQuery)
        {
            //Find all possible start times
            List<Class> startTimes = filteredQuery
                .GroupBy(c => c.Time)
                .Select(c => c.FirstOrDefault())
                .ToList();

            startTimes = RemoveOthers(startTimes);
            startTimes = RemoveCCInstructors(startTimes);

            return startTimes;
        }

        /// <summary>
        /// Get the query for classes with the given filters with the 
        /// given filters.
        /// </summary>
        /// <param name="filters">The dictionary of filters</param>
        /// <returns>the query</returns>
        private IQueryable<Class> GetFilteredClassQuery(Dictionary<string, string> filters, int[] classIds)
        {
            var query = db.Classes.Where(c => c == c);

            if (filters != null && filters.ContainsKey("DepartmentFilter"))
            {
                string dept = filters["DepartmentFilter"] != "all" ? filters["DepartmentFilter"] : null;
                int? num = 0;
                if (filters["ClassNumberFilter"] != "all")
                {
                    num = Int32.Parse(filters["ClassNumberFilter"]);
                }
                string instructor = filters["InstructorFilter"] != "all" ? filters["InstructorFilter"] : null;
                string days = filters["DaysFilter"] != "all" ? filters["DaysFilter"] : null;
                string time = filters["TimeFilter"] != "all" ? filters["TimeFilter"] : null;

                query = dept != null ? query.Where(c => c.DeptPrefix == dept) : query;
                query = num != 0 ? query.Where(c => c.ClassNum == num) : query;
                query = instructor != null ? query.Where(c => c.Instructor == instructor) : query;
                query = days != null ? query.Where(c => c.Days == days) : query;
                query = time != null ? query.Where(c => c.Time == time) : query;
            }

            //TO DO - Fix this part of the function.
            if (filters != null && filters.ContainsKey("SelectedFilter"))
            {
                switch(filters["SelectedFilter"])
                {
                    case "selected":
                        query = query.Where(c => classIds.Contains(c.ClassID)); 
                        break;
                    case "not_selected":
                        query = query.Where(c => !classIds.Contains(c.ClassID));
                        break;
                    default:
                        break;
                }
            }

            return query;
        }

        private int[] GetClassIdArray(Dictionary<string, string> filters)
        {
            int[] IdArray;
            
            if (filters.ContainsKey("ClassIds") && filters["ClassIds"] != null && filters["ClassIds"] != "")
            {
                String[] Ids = filters["ClassIds"].Split(',');
                IdArray = new int[Ids.Length];
                for (int i = 0; i < Ids.Length; i++)
                {
                    IdArray[i] = Int32.Parse(Ids[0]);
                }                
            }
            else
            {
                IdArray = new int[0];
            }

            return IdArray;
        }
    }    
}