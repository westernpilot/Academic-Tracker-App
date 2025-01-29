using System;
using System.Collections.Generic;
using SQLite;

namespace c971.Shared
{
    public class AcademicTerm
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Not directly stored in the database
        [Ignore]
        public List<Course> Courses { get; set; } = new List<Course>();
    }
}
