using MathCenter.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MathCenter.Controllers
{
    public class AjaxController : Controller
    {
        private readonly MathContext db = new MathContext();

        // GET: Ajax
        public JsonResult  GetNumbers(string id)
        {
            //Find all of the distict Class Numbers in relation to the Prefix Given and use that for the drop down.
            var ClassNums = db.Classes
                .Where(c => c.DeptPrefix == id)
                .GroupBy(c => c.ClassNum)
                .Select(c => c.FirstOrDefault())
                .ToList();

            //Remove the classes with "Other" not being null
            var remClasses = ClassNums
                .Where(c => c.Other != null)
                .Select(c => c).ToList();
            foreach (var remClass in remClasses)
            {
                ClassNums.Remove(remClass);
            }

            //Convert the list into a Json Object
            string result = JsonConvert.SerializeObject(ClassNums, Formatting.None,
                    new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            //Return the Json Object
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInstructors(string id, int num)
        {
            //Find all of the distict Class Professors in relation to the previous Info and use that for the drop down.
            var Instructors = db.Classes
                .Where(c => c.DeptPrefix == id)
                .Where(c => c.ClassNum == num)
                .GroupBy(c => c.Instructor)
                .Select(c => c.FirstOrDefault())
                .ToList();

            //Remove the classes with "Other" not being null
            var remClasses = Instructors
                .Where(c => c.Other != null)
                .Select(c => c).ToList();
            foreach (var remClass in remClasses)
            {
                Instructors.Remove(remClass);
            }

            //Remove the classes with Community Colleges as an Instructor
            List<Class> ccClasses = Instructors
                .Where(c => c.Instructor == "Portland")
                .Select(c => c).ToList();
            ccClasses.Add(
                Instructors.Where(c => c.Instructor == "Chemeketa")
                .Select(c => c).FirstOrDefault());
            ccClasses.Add(
                Instructors.Where(c => c.Instructor == "Clackamas")
                .Select(c => c).FirstOrDefault());
            ccClasses.Add(
                Instructors.Where(c => c.Instructor == "Mt. Hood")
                .Select(c => c).FirstOrDefault());
            ccClasses.Add(
                Instructors.Where(c => c.Instructor == "Linn-Benton")
                .Select(c => c).FirstOrDefault());
            foreach (var ccClass in ccClasses)
            {
                Instructors.Remove(ccClass);
            }

            //Convert the list into a Json Object
            string result = JsonConvert.SerializeObject(Instructors, Formatting.None,
                    new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            //Return the Json Object
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTimes(string id, int num, string instructor)
        {
            if(instructor == "Community College")
            {
                Class PCC = db.Classes
                    .Where(c => c.DeptPrefix == id)
                    .Where(d => d.ClassNum == num)
                    .Where(n => n.Instructor == "Portland")
                    .Select(p => p).FirstOrDefault();
                Class ChCC = db.Classes
                    .Where(c => c.DeptPrefix == id)
                    .Where(d => d.ClassNum == num)
                    .Where(n => n.Instructor == "Chemeketa")
                    .Select(p => p).FirstOrDefault();
                Class ClCC = db.Classes
                    .Where(c => c.DeptPrefix == id)
                    .Where(d => d.ClassNum == num)
                    .Where(n => n.Instructor == "Clackamas")
                    .Select(p => p).FirstOrDefault();
                Class MHCC = db.Classes
                    .Where(c => c.DeptPrefix == id)
                    .Where(d => d.ClassNum == num)
                    .Where(n => n.Instructor == "Mt. Hood")
                    .Select(p => p).FirstOrDefault();
                Class LBCC = db.Classes
                    .Where(c => c.DeptPrefix == id)
                    .Where(d => d.ClassNum == num)
                    .Where(n => n.Instructor == "Linn-Benton")
                    .Select(p => p).FirstOrDefault();

                if(PCC == null || ChCC == null || ClCC == null || MHCC == null || LBCC == null)
                {
                    if (PCC == null)
                    {
                        PCC = new Class { DeptPrefix = id, ClassNum = num, Instructor = "Portland" };
                        db.Classes.Add(PCC);
                    }
                    if (ChCC == null)
                    {
                        ChCC = new Class { DeptPrefix = id, ClassNum = num, Instructor = "Chemeketa" };
                        db.Classes.Add(ChCC);
                    }
                    if (ClCC == null)
                    {
                        ClCC = new Class { DeptPrefix = id, ClassNum = num, Instructor = "Clackamas" };
                        db.Classes.Add(ClCC);
                    }
                    if (MHCC == null)
                    {
                        MHCC = new Class { DeptPrefix = id, ClassNum = num, Instructor = "Mt. Hood" };
                        db.Classes.Add(MHCC);
                    }
                    if (LBCC == null)
                    {
                        LBCC = new Class { DeptPrefix = id, ClassNum = num, Instructor = "Linn-Benton" };
                        db.Classes.Add(LBCC);
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        //There was an error.
                    }
                }
                List<Class> CCList = new List<Class>
                {
                    PCC,
                    ChCC,
                    ClCC,
                    MHCC,
                    LBCC
                };

                //Convert the list into a Json Object
                string resultCC = JsonConvert.SerializeObject(CCList, Formatting.None,
                        new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                //Return the Json Object
                return Json(resultCC, JsonRequestBehavior.AllowGet);
            }            
            //Find all possible start times
            var startTimes = db.Classes
                .Where(c => c.DeptPrefix == id)
                .Where(d => d.ClassNum == num)
                .Where(n => n.Instructor == instructor)
                .Select(p => p)
                .ToList();

            //Remove the classes with "Other" not being null
            var remClasses = startTimes
                .Where(c => c.Other != null)
                .Select(c => c).ToList();
            foreach (var remClass in remClasses)
            {
                startTimes.Remove(remClass);
            }

            //Convert the list into a Json Object
            string result = JsonConvert.SerializeObject(startTimes, Formatting.None,
                    new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            //Return the Json Object
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}  