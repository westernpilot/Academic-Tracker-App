using System;
using System.Collections.Generic;
using SQLite;

namespace c971.Shared
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DueDate { get; set; }
        public string InstructorName { get; set; }
        public string InstructorPhone { get; set; }
        public string InstructorEmail { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public int TermId { get; set; } // Foreign key to AcademicTerm

        // Not directly stored in the database
        [Ignore]
        public List<Assessment> Assessments { get; set; } = new List<Assessment>();
    }
}
