﻿using MathCenter.Models;
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
                //Class Info
                sheet.Column(8).Width = 7; //CRN
                sheet.Column(9).Width = 12; //DeptPrefix
                sheet.Column(10).Width = 15; //ClassNum
                sheet.Column(11).Width = 25; //Instructor
                sheet.Column(12).Width = 12; //Days
                sheet.Column(13).Width = 10; //StartTime
                sheet.Column(14).Width = 10; //Other

                //heading of the table
                #region Table Header

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
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

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
                        cell.Value = dat.VNum;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for First Name
                        cell = sheet.Cells[rowIndex, 6];
                        cell.Value = dat.FirstName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Last Name
                        cell = sheet.Cells[rowIndex, 7];
                        cell.Value = dat.LastName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for CRN
                        cell = sheet.Cells[rowIndex, 8];
                        cell.Value = dat.SignedClass.CRN;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Class Prefix
                        cell = sheet.Cells[rowIndex, 9];
                        cell.Value = dat.SignedClass.DeptPrefix;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Class Number
                        cell = sheet.Cells[rowIndex, 10];
                        cell.Value = dat.SignedClass.ClassNum;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Instructor
                        cell = sheet.Cells[rowIndex, 11];
                        cell.Value = dat.SignedClass.Instructor;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Days
                        cell = sheet.Cells[rowIndex, 12];
                        cell.Value = dat.SignedClass.Days;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Start Time
                        cell = sheet.Cells[rowIndex, 13];
                        cell.Value = dat.SignedClass.Time;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Info for Other
                        cell = sheet.Cells[rowIndex, 14];
                        cell.Value = dat.SignedClass.Other;
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