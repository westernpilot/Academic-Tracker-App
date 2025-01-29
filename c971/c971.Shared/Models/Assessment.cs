using System;
using SQLite;

namespace c971.Shared
{
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; } // "Performance" or "Objective"
        public DateTime DueDate { get; set; }
        public DateTime StartDate { get; set; } // Anticipated start date
        public DateTime EndDate { get; set; } // Anticipated end date
        public int CourseId { get; set; } // Foreign key to Course
    }
}
