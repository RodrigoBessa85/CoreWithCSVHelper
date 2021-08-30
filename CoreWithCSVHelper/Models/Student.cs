using CsvHelper.Configuration.Attributes;
using System;

namespace CoreWithCSVHelper.Models
{
    public class Student
    {
        [Index(0)]
        public string Name { get; set; } = "";
        [Index(1)]
        public string Roll { get; set; } = "";
        [Index(2)]
        public string Email { get; set; } = "";
    }
}
