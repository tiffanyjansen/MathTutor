using MathCenter.Models.ViewModels;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MathCenter.Excel
{
    public class DataExcel
    {
        int rowIndex = 1;
        ExcelRange cell;

        public byte[] GenerateExcel(List<Data> data)
        {
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "Math Center";
                excelPackage.Workbook.Properties.Title = "Math Center Data";
                var sheet = excelPackage.Workbook.Worksheets.Add("Data");
                sheet.Name = "Math Center Report";
                //Sign In Info
                sheet.Column(1).Width = 7; //Week
                sheet.Column(2).Width = 10; //Date
                sheet.Column(3).Width = 5; //Hour
                sheet.Column(4).Width = 7; //Min
                //Student Info
                sheet.Column(5).Width = 12; //VNum
                sheet.Column(6).Width = 15; //FirstName
                sheet.Column(7).Width = 15; //LastName
                //Class 1 Info
                sheet.Column(8).Width = 7; //CRN
                sheet.Column(9).Width = 12; //DeptPrefix
                sheet.Column(10).Width = 15; //ClassNum
                sheet.Column(11).Width = 25; //Instructor
                sheet.Column(12).Width = 5; //Days
                sheet.Column(13).Width = 10; //StartTime
                sheet.Column(14).Width = 10; //Other
                //Class 2 Info
                sheet.Column(15).Width = 7; //CRN
                sheet.Column(16).Width = 12; //DeptPrefix
                sheet.Column(17).Width = 15; //ClassNum
                sheet.Column(18).Width = 25; //Instructor
                sheet.Column(19).Width = 5; //Days
                sheet.Column(20).Width = 10; //StartTime
                sheet.Column(21).Width = 10; //Other
                //Class 3 Info
                sheet.Column(22).Width = 7; //CRN
                sheet.Column(23).Width = 12; //DeptPrefix
                sheet.Column(24).Width = 15; //ClassNum
                sheet.Column(25).Width = 25; //Instructor
                sheet.Column(26).Width = 5; //Days
                sheet.Column(27).Width = 10; //StartTime
                sheet.Column(28).Width = 10; //Other
                //Class 4 Info
                sheet.Column(29).Width = 7; //CRN
                sheet.Column(30).Width = 12; //DeptPrefix
                sheet.Column(31).Width = 15; //ClassNum
                sheet.Column(32).Width = 25; //Instructor
                sheet.Column(33).Width = 5; //Days
                sheet.Column(34).Width = 10; //StartTime
                sheet.Column(35).Width = 10; //Other

                //heading of the table
                #region Table Header

                ////Heading for SignIn Info
                //cell = sheet.Cells[rowIndex, 3];
                //cell.Value = "SignIn Info";
                //cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ////Heading for Student Info
                //cell = sheet.Cells[rowIndex, 6];
                //cell.Value = "Student Info";
                //cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ////Heading for Class 1 Info
                //cell = sheet.Cells[rowIndex, 11];
                //cell.Value = "Class 1 Info";
                //cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ////Heading for Class 2 Info
                //cell = sheet.Cells[rowIndex, 18];
                //cell.Value = "Class 2 Info";
                //cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ////Heading for Class 3 Info
                //cell = sheet.Cells[rowIndex, 25];
                //cell.Value = "Class 3 Info";
                //cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ////Heading for Class 4 Info
                //cell = sheet.Cells[rowIndex, 32];
                //cell.Value = "Class 4 Info";
                //cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ////Go to next row
                //rowIndex += 1;

                //Heading for Week Number
                cell = sheet.Cells[rowIndex, 1];
                cell.Value = "Week";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Heading for Date
                cell = sheet.Cells[rowIndex, 2];
                cell.Value = "Date";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Heading for Hour
                cell = sheet.Cells[rowIndex, 3];
                cell.Value = "Hour";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Heading for Minute
                cell = sheet.Cells[rowIndex, 4];
                cell.Value = "Minute";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Heading for V-Number
                cell = sheet.Cells[rowIndex, 5];
                cell.Value = "V-Number";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Heading for First Name
                cell = sheet.Cells[rowIndex, 6];
                cell.Value = "First Name";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Heading for Last Name
                cell = sheet.Cells[rowIndex, 7];
                cell.Value = "Last Name";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Class 1
                //Heading for CRN
                cell = sheet.Cells[rowIndex, 8];
                cell.Value = "CRN";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Heading for Class Prefix
                cell = sheet.Cells[rowIndex, 9];
                cell.Value = "Class Prefix";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                
                //Heading for Class Number
                cell = sheet.Cells[rowIndex, 10];
                cell.Value = "Class Number";
                cell.Style.Font.Bold = true;

                //Heading for Instructor
                cell = sheet.Cells[rowIndex, 11];
                cell.Value = "Instructor";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Heading for Days
                cell = sheet.Cells[rowIndex, 12];
                cell.Value = "Days";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Heading for Start Time
                cell = sheet.Cells[rowIndex, 13];
                cell.Value = "Time";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Heading for Other
                cell = sheet.Cells[rowIndex, 14];
                cell.Value = "Other";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Class 2
                //Heading for CRN
                cell = sheet.Cells[rowIndex, 15];
                cell.Value = "CRN";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Heading for Class Prefix
                cell = sheet.Cells[rowIndex, 16];
                cell.Value = "Class Prefix";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Heading for Class Number
                cell = sheet.Cells[rowIndex, 17];
                cell.Value = "Class Number";
                cell.Style.Font.Bold = true;

                //Heading for Instructor
                cell = sheet.Cells[rowIndex, 18];
                cell.Value = "Instructor";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Heading for Days
                cell = sheet.Cells[rowIndex, 19];
                cell.Value = "Days";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Heading for Start Time
                cell = sheet.Cells[rowIndex, 20];
                cell.Value = "Time";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Heading for Other
                cell = sheet.Cells[rowIndex, 21];
                cell.Value = "Other";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Class 3
                //Heading for CRN
                cell = sheet.Cells[rowIndex, 22];
                cell.Value = "CRN";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Heading for Class Prefix
                cell = sheet.Cells[rowIndex, 23];
                cell.Value = "Class Prefix";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Heading for Class Number
                cell = sheet.Cells[rowIndex, 24];
                cell.Value = "Class Number";
                cell.Style.Font.Bold = true;

                //Heading for Instructor
                cell = sheet.Cells[rowIndex, 25];
                cell.Value = "Instructor";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Heading for Days
                cell = sheet.Cells[rowIndex, 26];
                cell.Value = "Days";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Heading for Start Time
                cell = sheet.Cells[rowIndex, 27];
                cell.Value = "Time";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Heading for Other
                cell = sheet.Cells[rowIndex, 28];
                cell.Value = "Other";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Class 4
                //Heading for CRN
                cell = sheet.Cells[rowIndex, 29];
                cell.Value = "CRN";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Heading for Class Prefix
                cell = sheet.Cells[rowIndex, 30];
                cell.Value = "Class Prefix";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Heading for Class Number
                cell = sheet.Cells[rowIndex, 31];
                cell.Value = "Class Number";
                cell.Style.Font.Bold = true;

                //Heading for Instructor
                cell = sheet.Cells[rowIndex, 32];
                cell.Value = "Instructor";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Heading for Days
                cell = sheet.Cells[rowIndex, 33];
                cell.Value = "Days";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Heading for Start Time
                cell = sheet.Cells[rowIndex, 34];
                cell.Value = "Time";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Heading for Other
                cell = sheet.Cells[rowIndex, 35];
                cell.Value = "Other";
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Go to the next row.
                rowIndex = rowIndex + 1;
                #endregion

                //The Data to be added for the table.
                #region Table Body
                if (data.Count > 0)
                {
                    foreach (Data dat in data)
                    {
                        //Info for Week Number
                        cell = sheet.Cells[rowIndex, 1];
                        cell.Value = dat.Week;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Date
                        cell = sheet.Cells[rowIndex, 2];
                        cell.Value = dat.Date.ToShortDateString();
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Hour
                        cell = sheet.Cells[rowIndex, 3];
                        cell.Value = dat.Hour;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Minute
                        cell = sheet.Cells[rowIndex, 4];
                        cell.Value = dat.Min;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for V-Number
                        cell = sheet.Cells[rowIndex, 5];
                        cell.Value = Convert.ToInt32(dat.VNum);
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for First Name
                        cell = sheet.Cells[rowIndex, 6];
                        cell.Value = dat.FirstName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Last Name
                        cell = sheet.Cells[rowIndex, 7];
                        cell.Value = dat.LastName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Class 1 Info
                        //Info for CRN
                        cell = sheet.Cells[rowIndex, 8];
                        cell.Value = dat.Class1.CRN;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Class Prefix
                        cell = sheet.Cells[rowIndex, 9];
                        cell.Value = dat.Class1.DeptPrefix;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Class Number
                        cell = sheet.Cells[rowIndex, 10];
                        cell.Value = dat.Class1.ClassNum;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Instructor
                        cell = sheet.Cells[rowIndex, 11];
                        cell.Value = dat.Class1.Instructor;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Days
                        cell = sheet.Cells[rowIndex, 12];
                        cell.Value = dat.Class1.Days;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Start Time
                        cell = sheet.Cells[rowIndex, 13];
                        cell.Value = dat.Class1.Time;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Other
                        cell = sheet.Cells[rowIndex, 14];
                        cell.Value = dat.Class1.Other;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Class 2 Info
                        //Info for CRN
                        cell = sheet.Cells[rowIndex, 15];
                        if(dat.Class2 != null) { 
                            cell.Value = dat.Class2.CRN;
                        }
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Class Prefix
                        cell = sheet.Cells[rowIndex, 16];
                        if (dat.Class2 != null)
                        {
                            cell.Value = dat.Class2.DeptPrefix;
                        }
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Class Number
                        cell = sheet.Cells[rowIndex, 17];
                        if (dat.Class2 != null)
                        {
                            cell.Value = dat.Class2.ClassNum;
                        }
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Instructor
                        cell = sheet.Cells[rowIndex, 18];
                        if (dat.Class2 != null)
                        {
                            cell.Value = dat.Class2.Instructor;
                        }
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Days
                        cell = sheet.Cells[rowIndex, 19];
                        if (dat.Class2 != null)
                        {
                            cell.Value = dat.Class2.Days;
                        }
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Start Time
                        cell = sheet.Cells[rowIndex, 20];
                        if (dat.Class2 != null)
                        {
                            cell.Value = dat.Class2.Time;
                        }
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Other
                        cell = sheet.Cells[rowIndex, 21];
                        if (dat.Class2 != null)
                        {
                            cell.Value = dat.Class2.Other;
                        }
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Class 3 Info
                        //Info for CRN
                        cell = sheet.Cells[rowIndex, 22];
                        if(dat.Class3 != null)
                        { 
                            cell.Value = dat.Class3.CRN;
                        }
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Class Prefix
                        cell = sheet.Cells[rowIndex, 23];
                        if (dat.Class3 != null)
                        {
                            cell.Value = dat.Class3.DeptPrefix;
                        }
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Class Number
                        cell = sheet.Cells[rowIndex, 24];
                        if (dat.Class3 != null)
                        {
                            cell.Value = dat.Class3.ClassNum;
                        }
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Instructor
                        cell = sheet.Cells[rowIndex, 25];
                        if (dat.Class3 != null)
                        {
                            cell.Value = dat.Class3.Instructor;
                        }
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Days
                        cell = sheet.Cells[rowIndex, 26];
                        if (dat.Class3 != null)
                        {
                            cell.Value = dat.Class3.Days;
                        }
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Start Time
                        cell = sheet.Cells[rowIndex, 27];
                        if (dat.Class3 != null)
                        {
                            cell.Value = dat.Class3.Time;
                        }
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Other
                        cell = sheet.Cells[rowIndex, 28];
                        if (dat.Class3 != null)
                        {
                            cell.Value = dat.Class3.Other;
                        }
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Class 4 Info
                        //Info for CRN
                        cell = sheet.Cells[rowIndex, 29];
                        if(dat.Class4 != null)
                        {
                            cell.Value = dat.Class4.CRN;
                        }                        
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Class Prefix
                        cell = sheet.Cells[rowIndex, 30];
                        if (dat.Class4 != null)
                        {
                            cell.Value = dat.Class4.DeptPrefix;
                        }
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Class Number
                        cell = sheet.Cells[rowIndex, 31];
                        if (dat.Class4 != null)
                        {
                            cell.Value = dat.Class4.ClassNum;
                        }
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Instructor
                        cell = sheet.Cells[rowIndex, 32];
                        if (dat.Class4 != null)
                        {
                            cell.Value = dat.Class4.Instructor;
                        }
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Days
                        cell = sheet.Cells[rowIndex, 33];
                        if (dat.Class4 != null)
                        {
                            cell.Value = dat.Class4.Days;
                        }
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Start Time
                        cell = sheet.Cells[rowIndex, 34];
                        if (dat.Class4 != null)
                        {
                            cell.Value = dat.Class4.Time;
                        }
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Other
                        cell = sheet.Cells[rowIndex, 35];
                        if (dat.Class4 != null)
                        {
                            cell.Value = dat.Class4.Other;
                        }
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Go to the next row
                        rowIndex += 1;
                    }
                }
                #endregion

                //return the byte array
                return excelPackage.GetAsByteArray();
            }
        }
    }
}