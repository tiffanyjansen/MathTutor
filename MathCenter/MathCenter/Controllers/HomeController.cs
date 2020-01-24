using MathCenter.DAL;
using MathCenter.Models;
using MathCenter.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace MathCenter.Controllers
{
    public class HomeController : Controller
    {
        private readonly MathContext db = new MathContext(); //Access to Database

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

        // "API" functions
        public JsonResult GetClasses(string ClassDeptNum, string ClassInstitution, string Other)
        {
            if(Other != null)
            {
                Class @class = new Class { Other = Other };
                db.Classes.Add(@class);
            } else
            {
                string[] classParts = ClassDeptNum.Split(' ');

                string dept = classParts[0];
                int num = 0;
                try
                {
                    num = System.Convert.ToInt32(classParts[1]);
                }
                catch (Exception)
                {
                    Debug.WriteLine("The class number wasn't a number for some reason.");
                }
                string instructor = Class.CCCollegeStrings[ClassInstitution];
                Class @class = new Class { DeptPrefix = dept, ClassNum = num, Instructor = instructor };
                db.Classes.Add(@class);
            }
            string data;
            try
            {
                db.SaveChanges();
                data = "passed";
            }
            catch(Exception)
            {
                data = "failed";
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FilterClasses(string DeptPrefix, string ClassNum, string Instructor, string Days, string Time)
        {
            var CCColleges = Class.CCCollegeStrings.Values;
            var query = db.Classes.Where(c => c.Other == null).Where(c => !CCColleges.Contains(c.Instructor));

            if(DeptPrefix != "" && DeptPrefix != null)
            {
                query = query.Where(c => c.DeptPrefix.Contains(DeptPrefix));
            }
            if (ClassNum != "" && ClassNum != null)
            {
                query = query.Where(c => c.ClassNum.ToString().Contains(ClassNum));
            }
            if (Instructor != "" && Instructor != null)
            {
                query = query.Where(c => c.Instructor.Contains(Instructor));
            }
            if (Days != "" && Days != null)
            {
                query = query.Where(c => c.Days.Contains(Days));
            }
            if (Time != "" && Time != null)
            {
                query = query.Where(c => c.Time.Contains(Time));
            }

            var classes = query.Select(c => new { c.ClassID, c.DeptPrefix, c.ClassNum, c.Instructor, c.Days, c.Time }).ToList();
            return Json(classes, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FilterOther(string Other)
        {
            var query = db.Classes.Where(c => c.Other != null);

            if(!(Other == "" || Other == null))
            {
                query = query.Where(c => c.Other.Contains(Other));
            }

            var classes = query.Select(c => new { c.ClassID, c.DeptPrefix, c.ClassNum, c.Instructor, c.Days, c.Time }).ToList();
            return Json(classes, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FilterCommunity(string DeptNum, string Institution)
        {
            var CCColleges = Class.CCCollegeStrings.Values;
            var query = db.Classes.Where(c => CCColleges.Contains(c.Instructor));

            if(DeptNum != "" && DeptNum != null)
            {
                query = query.Where(c => c.ToString().Contains(DeptNum));
            }
            if(Institution != "" && Institution != null)
            {
                string[] splitInstitution = Institution.Split(' ');
                query = query.Where(c => c.Instructor.Contains(splitInstitution[0]));
            }

            var classes = query.Select(c => new { c.ClassID, c.DeptPrefix, c.ClassNum, c.Instructor, c.Days, c.Time }).ToList();
            return Json(classes, JsonRequestBehavior.AllowGet);
        }
    }
}