using MathCenter.DAL;
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
        private readonly MathContext db = new MathContext(); //Access to Database
        private static int WeekNumber;
        private static string VNumber;

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
                WeekNumber = Week;
                return RedirectToAction("Welcome");
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
        /// <returns>The View</returns>
        [HttpGet]
        public ActionResult Welcome()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Welcome(WelcomeViewModel model)
        {
            if (ModelState.IsValid)
            {
                VNumber = model.VNum;
                if (db.Students.Find(VNumber) != null)
                {
                    //Probably should make a ViewModel for this page too... Maybe with a student in it?
                    return RedirectToAction("Done");
                }
                return RedirectToAction("Name");
            }
            else
            {
                ViewBag.Error = "Your V Number is invalid. It must be 8 characters. Please do not include the V.";
                return View();
            }           
        }

        /// <summary>
        /// This is the method for inputting their name. It will allow the user
        /// to input their name and get to adding their class.
        /// 
        /// It will also add the student to the database.
        /// <returns>The View</returns>
        [HttpGet]
        public ActionResult Name()
        {
            Student student = db.Students.Find(VNumber);
            if(student == null)
            {
                student = new Student { VNum = VNumber };
            }
            return View(student);
        }
        [HttpPost]
        public ActionResult Name(Student student)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    student.FirstName = student.CapitalizeName(student.FirstName);
                    student.LastName = student.CapitalizeName(student.LastName);
                    db.Students.Add(student);  //Add the student to the database.          

                    db.SaveChanges(); //Save the changes to the database. 
                }
                catch (Exception)
                {
                    ViewBag.Error = "There was an error adding you to the database. Please ask a tutor for help.";
                    return View(student);
                }

                //Redirect to the select department method and slowly select the class.
                return RedirectToAction("SelectClass");
            }
            else
            {
                ViewBag.Error = "Looks like you inputted something that does not look like a name. Please try again.";
                return View(student);
            }
        }
        
         /// <summary>
         /// The method for selecting a class. It's a table that is filterable. 
         /// There is some Javascript, for functionality, but no Ajax calls.
         /// </summary>
         /// <returns>The View</returns>
        [HttpGet]
        public ActionResult SelectClass()
        {
            //get all the query strings
            var queryStrings = new Dictionary<string, string>();
            foreach (var key in Request.QueryString.AllKeys)
            {
                queryStrings.Add(key, Request.QueryString[key]);
            }
            
            //get the classes and filters. 
            var filteredQuery = GetFilteredClassQuery(queryStrings);//Get the filtered class query 
            var Classes = filteredQuery.ToList(); //Get all the classes  
            ViewBag.Departments = GetClassDepts(filteredQuery); //gets the possible class departments
            ViewBag.ClassNumbers = GetClassNumbers(filteredQuery); //gets the possible class numbers
            ViewBag.Instructors = GetInstructors(filteredQuery); //gets the possible instructors
            ViewBag.Days = GetClassDays(filteredQuery); //gets the possible days
            ViewBag.Times = GetClassTimes(filteredQuery); //gets the possible start times 

            //get the array of selected class ids.
            int[] IdArray = GetClassIdArray(queryStrings);
            if (IdArray.Length > 0)
            {
                ViewBag.SelectedIds = queryStrings["ClassIds"]; //send the thing we got to the server back.
                ViewBag.ClassIds = IdArray;
            }
            ViewBag.SelectedCount = IdArray.Length; //the number of selected classes

            return View(Classes);
        }
        [HttpPost]
        public ActionResult SelectClass(int[] Classes)
        {
            if (Classes == null)
            {
                //get the classes and filters. 
                var filteredQuery = GetFilteredClassQuery(null);//Get the filtered class query 
                var AllClasses = filteredQuery.ToList(); //Get all the classes
                ViewBag.Departments = GetClassDepts(filteredQuery); //gets the possible class departments
                ViewBag.ClassNumbers = GetClassNumbers(filteredQuery); //gets the possible class numbers
                ViewBag.Instructors = GetInstructors(filteredQuery); //gets the possible instructors
                ViewBag.Days = GetClassDays(filteredQuery); //gets the possible days
                ViewBag.Times = GetClassTimes(filteredQuery); //gets the possible start times
                ViewBag.SelectedCount = 0;

                return View(AllClasses);
            }
            else
            {
                try
                {
                    Student currentStudent = db.Students.Find(VNumber);
                    foreach (var ClassID in Classes)
                    {                        
                        db.StudentClasses.Add(new StudentClass { VNum = currentStudent.VNum, ClassID = (int)ClassID });
                        db.SignIns.Add(new SignIn { Week = WeekNumber, Date = DateTime.Today, Hour = DateTime.Now.Hour, Min = DateTime.Now.Minute, StudentID = VNumber, ClassID = (int)ClassID }); //add a sign in for every class.
                    }
                    
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    //There was an error
                    ViewBag.Error = "There was an error with the database. Please try again.";

                    //get the classes and filters. 
                    var filteredQuery = GetFilteredClassQuery(null);//Get the filtered class query 
                    var AllClasses = filteredQuery.ToList(); //Get all the classes          
                    ViewBag.Departments = GetClassDepts(filteredQuery); //gets the possible class departments
                    ViewBag.ClassNumbers = GetClassNumbers(filteredQuery); //gets the possible class numbers
                    ViewBag.Instructors = GetInstructors(filteredQuery); //gets the possible instructors
                    ViewBag.Days = GetClassDays(filteredQuery); //gets the possible days
                    ViewBag.Times = GetClassTimes(filteredQuery); //gets the possible start times
                    ViewBag.SelectedCount = 0;

                    return View(AllClasses);
                }

                return RedirectToAction("Finish");
            }                     
        }
        [HttpPost]
        public ActionResult CommunityCollegeClasses(int[] Classes)
        {
            if(Classes != null)
            {
                try
                {
                    Student currentStudent = db.Students.Find(VNumber);
                    foreach (var ClassID in Classes)
                    {
                        db.StudentClasses.Add(new StudentClass { VNum = currentStudent.VNum, ClassID = ClassID });
                        db.SignIns.Add(new SignIn { Week = WeekNumber, Date = DateTime.Today, Hour = DateTime.Now.Hour, Min = DateTime.Now.Minute, StudentID = VNumber, ClassID = ClassID }); //add a sign in for every class.
                    }

                    db.SaveChanges();
                }
                catch (Exception)
                {
                    //There was an error
                    ViewBag.Error = "There was an error with the database. Please try again.";                   
                    return RedirectToAction("SelectClass");
                }               
            }
            //Redirect to CommunityCollege
            return RedirectToAction("CommunityCollege");
        }
        [HttpPost]
        public ActionResult OtherClasses(int[] Classes)
        {
            if(Classes != null)
            {
                try
                {
                    Student currentStudent = db.Students.Find(VNumber);
                    foreach (var ClassID in Classes)
                    {
                        db.StudentClasses.Add(new StudentClass { VNum = currentStudent.VNum, ClassID = ClassID });
                        db.SignIns.Add(new SignIn { Week = WeekNumber, Date = DateTime.Today, Hour = DateTime.Now.Hour, Min = DateTime.Now.Minute, StudentID = VNumber, ClassID = ClassID }); //add a sign in for every class.
                    }

                    db.SaveChanges();
                }
                catch (Exception)
                {
                    //There was an error
                    ViewBag.Error = "There was an error with the database. Please try again.";
                    return RedirectToAction("SelectClass");
                }                
            }
            //Redirect to Other
            return RedirectToAction("Other");
        }

        //HERE IS WHERE COMMUNITYCOLLEGE WILL GO
        
       /// <summary>
       /// The method for when a student wants to select a different class ("Other")
       /// </summary>
       /// <param name="Week">The Week Number</param>
       /// <param name="VNum">The Student's V-Number</param>
       /// <returns>The View</returns>
        [HttpGet]
        public ActionResult Other()
        {
            var OtherClasses = RemoveOthers(null, false);

            //get all the query strings
            var queryStrings = new Dictionary<string, string>();
            foreach (var key in Request.QueryString.AllKeys)
            {
                queryStrings.Add(key, Request.QueryString[key]);
            }

            if(queryStrings.ContainsKey("other"))
            {
                Regex rx = new Regex(@"^[A-Z]{1,3}\s\d{2,3}$", RegexOptions.IgnoreCase);
                if (rx.IsMatch(queryStrings["other"]))
                {
                    try
                    {
                        //Create the class to be connected to the student.
                        db.Classes.Add(new Class { Other = queryStrings["other"] });

                        //Save the class into the database.
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        //There was an error.
                        ViewBag.Error = "There was an error with the database. Please try again.";
                        return View(OtherClasses);
                    }

                    OtherClasses = RemoveOthers(null, false);
                    Class studentClass = db.Classes
                        .Where(c => c.Other == queryStrings["other"])
                        .Select(c => c).FirstOrDefault();

                    try
                    {
                        Student currentStudent = db.Students.Find(VNumber);            
                        db.StudentClasses.Add(new StudentClass { VNum = currentStudent.VNum, ClassID = studentClass.ClassID });
                        db.SignIns.Add(new SignIn { Week = WeekNumber, Date = DateTime.Today, Hour = DateTime.Now.Hour, Min = DateTime.Now.Minute, StudentID = VNumber, ClassID = studentClass.ClassID }); //add a sign in for every class.                       

                        db.SaveChanges();
                    }
                    catch(Exception)
                    {
                        //There was an error.
                        ViewBag.Error = "There was an error with the database. Please try again.";
                        return View(OtherClasses);
                    }

                    return RedirectToAction("Finish");
                }
            }

            //Return the View.
            return View(OtherClasses);
        }
        [HttpPost]
        public ActionResult Other(string other)
        {
            Regex rx = new Regex(@"^[A-Z]{1,3}\s\d{2,3}$", RegexOptions.IgnoreCase);
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
                    return View();
                }

                //Get the class again.
                Class sClass = db.Classes
                    .Where(c => c.Other == other)
                    .Select(c => c).FirstOrDefault();

                //Add the class to the current student.
                Student currentStudent = db.Students.Find(VNumber);
                db.StudentClasses.Add(new StudentClass { VNum = VNumber, ClassID = sClass.ClassID });

                try
                {
                    //Create the Sign In to be added to the db.
                    db.SignIns.Add(new SignIn { Week = WeekNumber, Date = DateTime.Today, Hour = DateTime.Now.TimeOfDay.Hours, Min = DateTime.Now.TimeOfDay.Minutes, StudentID = VNumber, ClassID = sClass.ClassID });

                    //Save the Changes to the db.
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    //There was an error.
                    ViewBag.Error = "There was an error with the database. Please try again.";
                    return View();
                }

                //Redirect to the finish page.
                return RedirectToAction("Finish");
            }
            else
            {
                ViewBag.Error = "Please input an actual class.";

                return View();
            }
        }
        /*
         * The method for when you are already in the DB and just need to approve the sign in.
         */
        [HttpGet]
        public ActionResult Done()
        {
            //Get the info from the Database about the current student.
            Student currentStudent = db.Students.Find(VNumber);

            //Return the View with the current student.
            return View(currentStudent);
        }
        [HttpPost]
        public ActionResult Done(int approved, int? classID)
        {
            if (approved == 1 && classID != null)
            {
                try
                {
                    //Create the Sign In and add it to the database.
                    db.SignIns.Add(new SignIn { Week = WeekNumber, Date = DateTime.Today, Hour = DateTime.Now.TimeOfDay.Hours, Min = DateTime.Now.TimeOfDay.Minutes, StudentID = VNumber, ClassID = (int)classID });
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    //There was an error.
                    ViewBag.Error = "There was an error with the database. Please try again.";

                    //Return the View with the current student.
                    return View(db.Students.Find(VNumber));
                }

                //Redirect to the "finish" page.
                return RedirectToAction("Finish");
            }
            else if (approved == 0)
            {
                //If it's not you redirect to Sign In page.
                return RedirectToAction("Name");
            }
            else if (approved == 3)
            {
                //Redirect back to the page to add more classes
                return RedirectToAction("SelectClass");
            }
            else if (approved == 2)
            {
                //Redirect back to the page to input your V-Number
                return RedirectToAction("Welcome");
            }
            else
            {
                ViewBag.ClassError = "Please select the class you would like to Sign In for.";
                return View(db.Students.Find(VNumber));
            }
        }
        /*
         * The last page that gives a good message.
         */
        [HttpGet]
        public ActionResult Finish()
        {

            //Return the View.
            return View();
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
        private IQueryable<Class> GetFilteredClassQuery(Dictionary<string, string> filters)
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

            return query;
        }

        /// <summary>
        /// Get the class ids of the selected classes
        /// </summary>
        /// <param name="filters">The dictionary of filters</param>
        /// <returns>the array of class ids</returns>
        private int[] GetClassIdArray(Dictionary<string, string> filters)
        {
            int[] IdArray;
            
            if (filters.ContainsKey("ClassIds") && filters["ClassIds"] != null && filters["ClassIds"] != "")
            {
                String[] Ids = filters["ClassIds"].Split(',');
                IdArray = new int[Ids.Length];
                for (int i = 0; i < Ids.Length; i++)
                {
                    IdArray[i] = Int32.Parse(Ids[i]);
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