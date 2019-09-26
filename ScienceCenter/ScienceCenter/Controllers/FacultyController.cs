using OfficeOpenXml;
using ScienceCenter.Excel;
using ScienceCenter.Models;
using ScienceCenter.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScienceCenter.Controllers
{
    public class FacultyController : Controller
    {
        //Database Connection
        private readonly ScienceContext db = new ScienceContext();

        /*
         * This method returns a welcome page for Faculty users.
         */
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(int? download)
        {
            //if you press the download button, the excel sheet will be created.
            if (download == 1)
            {
                EntireExcel();
            }
            //If you press the reset button, it will redirect you to another page.
            if (download == 2)
            {
                return RedirectToAction("Reset");
            }
            //If you press the Go Back Button, it will redirect you to another page.
            if (download == 3)
            {
                return RedirectToAction("Index", "Home");
            }
            //If you press the download by professor button, the excel sheet for it will be created.
            if(download == 4)
            {
                ProfExcel();
            }
            //If you press the download by class button, the excel sheet for it will be created.
            if(download == 5)
            {
                ClassExcel();
            }
            if(download >= 6)
            {
                int num = (int)download - 5;
                return RedirectToAction("Extra", new { num });
            }
            return View();
        }

        /*
         * This method will do the work of downloading the excel file with 'hopefully'
         * all the data.
         */
        private void EntireExcel()
        {
            DataExcel excel = new DataExcel();
            Response.ClearContent();
            Response.BinaryWrite(excel.GenerateExcel(GetData()));
            Response.AddHeader("content-disposition", "attachment; filename=ScienceCenterData.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
        }

        /*
         * This method creates and downloads the Sort by Professor excel sheet.
         */ 
         private void ProfExcel()
        {
            ProfessorExcel excel = new ProfessorExcel();
            Response.ClearContent();
            Response.BinaryWrite(excel.GenerateExcel(GetProfData()));
            Response.AddHeader("content-disposition", "attachment; filename=ScienceCenterDataByProf.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
        }

        /*
         * This method creates and downloads the Sort by Class Level excel sheet.
         */
        private void ClassExcel()
        {
            ClassLevelExcel excel = new ClassLevelExcel();
            Response.ClearContent();
            Response.BinaryWrite(excel.GenerateExcel(GetClassData()));
            Response.AddHeader("content-disposition", "attachment; filename=ScienceCenterDataByClass.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
        }

        /*
         * This method gets all of the data for the "Data Excel"
         */
        private List<Data> GetData()
        {
            //Create an empty list.
            List<Data> datas = new List<Data>();

            //Go through the list of sign ins and add all the data to the list.
            foreach (var SignIn in db.SignIns.ToList())
            {
                Data data = new Data { Week = SignIn.Week, Date = SignIn.Date, Hour = SignIn.Hour, Min = SignIn.Min, VNum = SignIn.Student.VNum, FirstName = SignIn.Student.FirstName, LastName = SignIn.Student.LastName, CRN = SignIn.Class.CRN, DeptPrefix = SignIn.Class.DeptPrefix, ClassNum = SignIn.Class.ClassNum, Days = SignIn.Class.Days, Instructor = SignIn.Class.Instructor, StartTime = SignIn.Class.Time };

                datas.Add(data);
            }

            //Remove the Placeholder class.
            var remClass = datas.Where(d => d.CRN == 0).Select(d => d).FirstOrDefault();
            datas.Remove(remClass);

            //Return the list of the data.
            return datas;
        }

        /*
         * This method gets all the data in no particular order for the 2 extra excel sheets.
         */
        private List<ProfData> GetProfs()
        {
            //Create an empty list to be filled after the queries.
            List<ProfData> pData = new List<ProfData>();

            //Going through all the studentClasses so we don't 'double' count
            foreach(var studentClass in db.StudentClasses.ToList())
            {
                int numTimes = db.SignIns
                    .Where(s => s.StudentID == studentClass.VNum)
                    .Where(s => s.ClassId == studentClass.ClassId)
                    .Count();

                //make sure we have at least 1
                if (numTimes != 0)
                {
                    //Get the student and the class
                    Student student = studentClass.Student;
                    Class @class = studentClass.Class;

                    //set the data
                    ProfData data = new ProfData { FirstName = student.FirstName, LastName = student.LastName, CRN = @class.CRN, DeptPrefix = @class.DeptPrefix, Instructor = @class.Instructor, Days = @class.Days, ClassNum = @class.ClassNum, StartTime = @class.Time, TimesIn = numTimes };

                    //add it to the list
                    pData.Add(data);
                }
            }

            //return the created list.
            return pData;
        }


    /*
     * This method gets all the ProfData for the "Prof" excel sheet
     */
        private List<ProfData> GetProfData()
        {
            //Create the list.
            List<ProfData> pData = GetProfs();

            //Sort the list by instructor
            pData = pData.OrderBy(p => p.Instructor).ToList();

            //return the ordered list.
            return pData;
        }

        /*
         * This method gets all the ClassData for the "Class Level" excel sheet
         */
        private List<ProfData> GetClassData()
        {
            //Create the list.
            List<ProfData> pData = GetProfs();

            //Sort the list by instructor
            pData = pData
                .OrderBy(p => p.DeptPrefix)
                .OrderBy(p => p.ClassNum)
                .ToList();

            //return the ordered list.
            return pData;
        }

        /*
        * This method returns a table with all of the data from the Database.
        */
        [HttpGet]
        public ActionResult Data()
        {
            return View(db.SignIns.ToList());
        }

        /*
        * This method returns a table with all of the data sorted by professor from the 
        * Database.
        */
        [HttpGet]
        public ActionResult ProfData()
        {
            return View(GetProfData());
        }

        /*
        * This method returns a table with all of the data sorted by class level from the 
        * Database.
        */
        [HttpGet]
        public ActionResult ClassData()
        {
            return View(GetClassData());
        }

        /*
         * This method returns a page with a box where users can add classes to the database.
         * It will allow users to add classes whenever they want to and will always have the 
         * placeholder class automatically get added to the db.
         */
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    Stream fs = file.InputStream;
                    ExcelPackage package = new ExcelPackage(fs);
                    foreach (ExcelWorksheet worksheet in package.Workbook.Worksheets)
                    {
                        Class @class = new Models.Class();
                        for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                        {
                            if (Int32.Parse(worksheet.Cells[i, 1].Value.ToString()) != 0)
                            {
                                @class.CRN = Int32.Parse(worksheet.Cells[i, 1].Value.ToString());
                                @class.DeptPrefix = worksheet.Cells[i, 2].Value.ToString();
                                @class.ClassNum = worksheet.Cells[i, 3].Value.ToString();
                                @class.Time = worksheet.Cells[i, 4].Value.ToString();
                                @class.Days = worksheet.Cells[i, 5].Value.ToString();
                                @class.Instructor = worksheet.Cells[i, 6].Value.ToString();
                                db.Classes.Add(@class);
                                db.SaveChanges();
                            }                            
                        }
                    }
                    return RedirectToAction("Class");
                }
                else
                {
                    ViewBag.Error = "The file you uploaded has no data in it, please upload another one."; //return error message
                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "The data you inputted was not added to the database, please try again."; //return error message
                return View();
            }
        }

        /*
         * The class method returns a table with all the Classes in the database.
         */
        [HttpGet]
        public ActionResult Class()
        {
            //Get the entire list of classes.
            var Classes = db.Classes.ToList();

            //Remove the Placeholder Class
            var remClass = Classes.Where(c => c.CRN == 0).Select(c => c).FirstOrDefault();
            Classes.Remove(remClass);

            //Return the View with only the classes you actually want.
            return View(Classes);
        }

        /*
         * This is the method for the 5 extra questions that Hamid wanted for the 
         * Science Center.
         */
         [HttpGet]
         public ActionResult Extra(int num)
        {
            ViewBag.Id = num;
            if (num == 1)
            {
                return View(DailyCount());
            }
            else if(num == 2)
            {
                return View(WeeklyCount());
            }
            else if(num == 3)
            {
                return View(AverageByWeek());
            }
            else if(num == 4)
            {
                return View(AverageByDay());
            }
            else
            {
                return View(HourlyCount());                
            }
        }

        /*
         * This answers the first "extra" question.
         */ 
         private List<CountDay> DailyCount()
        {
            //Create an empty list
            List<CountDay> daily = new List<CountDay>();

            //Get all the days in the tables.
            List<SignIn> days = db.SignIns
                .GroupBy(s => s.Date)
                .Select(s => s.FirstOrDefault())
                .ToList();

            //ID Numbers
            int i = 1;

            //Go through all the days and count how many students there were.
            foreach (SignIn day in days)
            {
                int dailyCount = db.SignIns
                    .Where(s => s.Date == day.Date)
                    .Count();

                //Add it to the list.
                daily.Add(new CountDay { Date = day.Date, NumStudents = dailyCount, ID = 1 });

                //Increment the ID number
                i++;
            }

            return daily;
        }

        /*
         * This answers the second "extra" question.
         */
        private List<CountDay> WeeklyCount()
        {
            //Create an empty list
            List<CountDay> weekly = new List<CountDay>();

            //Get all the days in the tables.
            List<SignIn> weeks = db.SignIns
                .GroupBy(s => s.Week)
                .Select(s => s.FirstOrDefault())
                .ToList();

            //ID Numbers
            int i = 1;

            //Go through all the weeks and count how many students there were.
            foreach (SignIn week in weeks)
            {
                int weeklyCount = db.SignIns
                    .Where(s => s.Week == week.Week)
                    .Count();

                //Add it to the list.
                weekly.Add(new CountDay { WeekNum = week.Week, NumStudents = weeklyCount, ID = i });

                //Increment the ID number
                i++;
            }

            return weekly;
        }

        /*
         * This answers the third "extra" question.
         */ 
         private List<CountDay> HourlyCount()
        {
            //create an empty list.
            List<CountDay> hourly = new List<CountDay>();

            //Get the list of all the hours on the specific date.
            List<SignIn> hours = db.SignIns
                .GroupBy(s => new { s.Hour, s.Date })               
                .Select(s => s.FirstOrDefault())
                .ToList();

            //Id Numbers
            int i = 1;

            //Go through each one and count the number of students.
            foreach (SignIn hour in hours)
            {
                int hourlyCount = db.SignIns
                    .Where(s => s.Date == hour.Date)
                    .Where(s => s.Hour == hour.Hour)
                    .Count();

                //Add the stuff to the list.
                hourly.Add(new CountDay { Date = hour.Date, Hour = hour.Hour, NumStudents = hourlyCount, ID = i });

                //increment the id number
                i++;
            }       

            //return the list.
            return hourly;
        }

        /*
         * This answers the fourth "extra" question.
         */
        private List<CountDay> AverageByDay()
        {
            //get the total number of students per day.
            List<CountDay> daysCount = DailyCount();

            //create an empty number to be used to get the total number of students.
            int total = 0;
            
            //go through the list and add all the students.
            foreach (var day in daysCount)
            {
                total = total + day.NumStudents;
            }

            //get the average.
            int average = (total / daysCount.Count());

            //create a list and add the one number to it.
            List<CountDay> averageStudents = new List<CountDay>();
            averageStudents.Add(new CountDay { AverageNum = average });

            //return the list.
            return averageStudents;
        }

        /*
         * This answers the fifth "extra" question.
         */
        private List<CountDay> AverageByWeek()
        {
            //get the total number of students per day.
            List<CountDay> weekCount = WeeklyCount();

            //create an empty number to be used to get the total number of students.
            int total = 0;

            //go through the list and add all the students.
            foreach (var week in weekCount)
            {
                total = total + week.NumStudents;
            }

            //get the average.
            int average = (total / weekCount.Count());

            //create a list and add the one number to it.
            List<CountDay> averageStudents = new List<CountDay>();
            averageStudents.Add(new CountDay { AverageNum = average });

            //return the list.
            return averageStudents;
        }

        /*
         * This method takes you to a page to reset the data. 
         * (That way the button doesn't feel so scary)
         */
        [HttpGet]
        public ActionResult Reset()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Reset(int? reset)
        {
            if (reset == 1)
            {
                EntireExcel();
                ClearDB();
                return RedirectToAction("Complete");
            }
            else if (reset == 2)
            {
                ClearDB();
                return RedirectToAction("Complete");
            }
            else if (reset == 3)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        /*
         * This is the reset db method.
         */ 
        private void ClearDB()
        {
            //Delete all the SignIns from the DB.
            foreach (var SignIn in db.SignIns.ToList())
            {
                db.SignIns.Remove(SignIn);
            }
            //Delete all the Students from the DB.
            foreach (var Student in db.Students.ToList())
            {
                db.Students.Remove(Student);
            }
            //Delete all the Classes from the DB.
            foreach (var Class in db.Classes.ToList())
            {
                db.Classes.Remove(Class);
            }
            //Delete all the StudentClasses from the DB.
            foreach (var StudentClass in db.StudentClasses.ToList())
            {
                db.StudentClasses.Remove(StudentClass);
            }

            //Save changes to Database.
            db.SaveChanges();
        }

    /*
     * This page let's you know that the database was wiped.
     */
    [HttpGet]
        public ActionResult Complete()
        {
            return View();
        }
    }
}