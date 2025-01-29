using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using c971.Shared.Models; 

namespace c971.Shared
{
    public class Database
    {
        private readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<AcademicTerm>().Wait();
            _database.CreateTableAsync<Course>().Wait();
            _database.CreateTableAsync<Assessment>().Wait();
            _database.CreateTableAsync<GraduateUser>().Wait();
            _database.CreateTableAsync<UndergraduateUser>().Wait();
        }

        // Method to get all terms
        public Task<List<AcademicTerm>> GetTermsAsync()
        {
            return _database.Table<AcademicTerm>().ToListAsync();
        }

        // Method to save a term
        public async Task<int> SaveTermAsync(AcademicTerm term)
        {
            if (term.Id != 0)
            {
                return await _database.UpdateAsync(term);
            }
            else
            {
                await _database.InsertAsync(term);
                return term.Id; // Get the auto-incremented Id after insertion
            }
        }

        // Method to delete a term
        public Task<int> DeleteTermAsync(AcademicTerm term)
        {
            return _database.DeleteAsync(term);
        }

        // Method to save a course
        public async Task<int> SaveCourseAsync(Course course)
        {
            if (course.Id != 0)
            {
                return await _database.UpdateAsync(course);
            }
            else
            {
                await _database.InsertAsync(course);
                return course.Id;
            }
        }

        // Method to delete a course
        public Task<int> DeleteCourseAsync(Course course)
        {
            return _database.DeleteAsync(course);
        }

        // Method to get all assessments
        public Task<List<Assessment>> GetAssessmentsAsync()
        {
            return _database.Table<Assessment>().ToListAsync();
        }

        // Method to save an assessment
        public async Task<int> SaveAssessmentAsync(Assessment assessment)
        {
            if (assessment.Id != 0)
            {
                return await _database.UpdateAsync(assessment);
            }
            else
            {
                await _database.InsertAsync(assessment);
                return assessment.Id;
            }
        }

        // Method to delete an assessment
        public Task<int> DeleteAssessmentAsync(Assessment assessment)
        {
            return _database.DeleteAsync(assessment);
        }

        // Method to get assessments by course ID
        public Task<List<Assessment>> GetAssessmentsByCourseAsync(int courseId)
        {
            return _database.Table<Assessment>()
                            .Where(a => a.CourseId == courseId)
                            .ToListAsync();
        }

        // Method to get courses by term ID
        public Task<List<Course>> GetCoursesByTermAsync(int termId)
        {
            return _database.Table<Course>()
                            .Where(c => c.TermId == termId)
                            .ToListAsync();
        }

        // Method to save a graduate user
        public Task<int> SaveGraduateUserAsync(GraduateUser user)
        {
            if (user.Id != 0)
            {
                return _database.UpdateAsync(user);
            }
            else
            {
                return _database.InsertAsync(user);
            }
        }

        // Method to save an undergraduate user
        public Task<int> SaveUndergraduateUserAsync(UndergraduateUser user)
        {
            if (user.Id != 0)
            {
                return _database.UpdateAsync(user);
            }
            else
            {
                return _database.InsertAsync(user);
            }
        }

        // Method to seed initial data
        public async Task SeedDataAsync()
        {
            var terms = await GetTermsAsync();
            if (terms.Count > 0)
            {
                return; // Data already seeded
            }

            // Seed Spring 2024 term
            var term1 = new AcademicTerm
            {
                Title = "Spring 2024",
                StartDate = new DateTime(2024, 1, 1),
                EndDate = new DateTime(2024, 6, 30)
            };
            await SaveTermAsync(term1);

            var course1 = new Course
            {
                Title = "Introduction to Computer Science",
                StartDate = new DateTime(2024, 1, 1),
                EndDate = new DateTime(2024, 6, 30),
                DueDate = new DateTime(2024, 6, 30),
                InstructorName = "Anika Patel",
                InstructorPhone = "555-123-4567",
                InstructorEmail = "anika.patel@strimeuniversity.edu",
                Status = "In Progress",
                Notes = "This is a sample course.",
                TermId = term1.Id
            };
            await SaveCourseAsync(course1);

            var assessment1 = new Assessment
            {
                Title = "Performance Assessment",
                Type = "Performance",
                DueDate = new DateTime(2024, 6, 15),
                CourseId = course1.Id
            };
            var assessment2 = new Assessment
            {
                Title = "Objective Assessment",
                Type = "Objective",
                DueDate = new DateTime(2024, 6, 30),
                CourseId = course1.Id
            };
            await SaveAssessmentAsync(assessment1);
            await SaveAssessmentAsync(assessment2);

            // Seed Summer 2024 term
            var term2 = new AcademicTerm
            {
                Title = "Summer 2024",
                StartDate = new DateTime(2024, 7, 1),
                EndDate = new DateTime(2024, 12, 31)
            };
            await SaveTermAsync(term2);

            var course2 = new Course
            {
                Title = "Data Structures",
                StartDate = new DateTime(2024, 7, 1),
                EndDate = new DateTime(2024, 12, 31),
                DueDate = new DateTime(2024, 12, 20),
                InstructorName = "John Doe",
                InstructorPhone = "555-987-6543",
                InstructorEmail = "john.doe@strimeuniversity.edu",
                Status = "Planned",
                Notes = "Focus on algorithms.",
                TermId = term2.Id
            };
            await SaveCourseAsync(course2);

            var assessment3 = new Assessment
            {
                Title = "Final Exam",
                Type = "Objective",
                DueDate = new DateTime(2024, 12, 15),
                CourseId = course2.Id
            };
            await SaveAssessmentAsync(assessment3);

            // Seed Fall 2024 term
            var term3 = new AcademicTerm
            {
                Title = "Fall 2024",
                StartDate = new DateTime(2024, 9, 1),
                EndDate = new DateTime(2024, 12, 31)
            };
            await SaveTermAsync(term3);

            var course3 = new Course
            {
                Title = "Algorithms",
                StartDate = new DateTime(2024, 9, 1),
                EndDate = new DateTime(2024, 12, 31),
                DueDate = new DateTime(2024, 12, 25),
                InstructorName = "Jane Smith",
                InstructorPhone = "555-321-1234",
                InstructorEmail = "jane.smith@strimeuniversity.edu",
                Status = "In Progress",
                Notes = "Work on practical examples.",
                TermId = term3.Id
            };
            await SaveCourseAsync(course3);

            var assessment4 = new Assessment
            {
                Title = "Project Submission",
                Type = "Performance",
                DueDate = new DateTime(2024, 12, 10),
                CourseId = course3.Id
            };
            await SaveAssessmentAsync(assessment4);
        }
    }
}
