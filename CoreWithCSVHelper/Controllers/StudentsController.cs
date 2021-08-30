using CoreWithCSVHelper.Models;
using CsvHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace CoreWithCSVHelper.Controllers
{
    public class StudentsController : Controller
    {
        [HttpGet]
        public IActionResult Index(List<Student> students = null)
        {
            students = students == null ? new List<Student>() : students;
            return View(students);
        }

        [HttpPost]
        public IActionResult Index(IFormFile file, [FromServices] IHostingEnvironment hostingEnvironment)
        {
            #region Upload CSV
            string fileName = $"{hostingEnvironment.WebRootPath}\\files\\{file.FileName}";

            using (FileStream fileStream = System.IO.File.Create(fileName))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }
            #endregion

            var students = this.GetStudentList(file.FileName);
            return Index(students);
        }

        private List<Student> GetStudentList(string fileName)
        {
            List<Student> students = new List<Student>();

            #region Read CSV
            var path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + fileName;
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var student = csv.GetRecord<Student>();
                    students.Add(student);
                }
            }
            #endregion

            #region Create CSV
            path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\FilesTo"}";
            using (var write = new StreamWriter(path + "\\NewFile.csv"))
            using (var csv = new CsvWriter(write, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(students);
            }
            #endregion

            return students;
        }
    }
}
