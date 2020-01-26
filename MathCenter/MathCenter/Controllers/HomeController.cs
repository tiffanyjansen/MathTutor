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
            Dictionary<string, string> data = new Dictionary<string, string>();
            if (Other != null)
            {
                Class @class = new Class { Other = Other };
                db.Classes.Add(@class);
                data.Add("type", "Other");
                data.Add("className", @class.ToString());
            }
            else
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
                data.Add("type", "Community");
                data.Add("className", @class.ToString());
                data.Add("Institution", ClassInstitution);
            }
            try
            {
                db.SaveChanges();
                data.Add("classID", "" + db.Classes.Max(item => item.ClassID));
                data.Add("success", "true");
            }
            catch (Exception)
            {
                data.Add("success", "false");
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}