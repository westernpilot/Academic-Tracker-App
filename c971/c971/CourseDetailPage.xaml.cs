using Microsoft.Maui.Controls;
using Plugin.LocalNotification;
using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using System.Globalization;
using c971.Shared.Models; // Ensure this matches the new location of the Course and Assessment models
using c971.Shared; // Ensure this matches the new location of the Database class

namespace c971
{
    public partial class CourseDetailPage : ContentPage
    {
        private Course course;
        private ObservableCollection<Course> courses;
        private ObservableCollection<Assessment> assessments;
        private bool isEdit;

        public CourseDetailPage(Course course, ObservableCollection<Course> courses, bool isEdit)
        {
            InitializeComponent();
            this.course = course ?? new Course();
            this.courses = courses;
            this.isEdit = isEdit;

            LoadAssessments();

            BindingContext = this.course;
        }

        private async void LoadAssessments()
        {
            if (isEdit)
            {
                var assessmentList = await App.Database.GetAssessmentsByCourseAsync(course.Id);
                assessments = new ObservableCollection<Assessment>(assessmentList);
            }
            else
            {
                assessments = new ObservableCollection<Assessment>();
            }

            course.Assessments = new List<Assessment>(assessments);
            assessmentsView.ItemsSource = assessments; // Reference the CollectionView
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            if (!isEdit)
            {
                courses.Add(course);
            }
            else
            {
                var index = courses.IndexOf(course);
                courses[index] = course; // Update the course in the collection
            }

            await App.Database.SaveCourseAsync(course);

            foreach (var assessment in assessments)
            {
                assessment.CourseId = course.Id;
                await App.Database.SaveAssessmentAsync(assessment);
            }

            ScheduleNotifications(course);

            await DisplayAlert("Info", "Course details saved.", "OK");
            await Navigation.PopAsync();
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(course.InstructorEmail) || !IsValidEmail(course.InstructorEmail))
            {
                DisplayAlert("Validation Error", "Please enter a valid instructor email address.", "OK");
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Use IdnMapping class to convert Unicode domain names.
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private void ScheduleNotifications(Course course)
        {
            var startNotification = new NotificationRequest
            {
                NotificationId = new Random().Next(),
                Title = "Course Start",
                Description = $"Course {course.Title} starts today!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = course.StartDate
                }
            };

            var endNotification = new NotificationRequest
            {
                NotificationId = new Random().Next(),
                Title = "Course End",
                Description = $"Course {course.Title} ends today!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = course.EndDate
                }
            };

            LocalNotificationCenter.Current.Show(startNotification);
            LocalNotificationCenter.Current.Show(endNotification);
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (isEdit)
            {
                courses.Remove(course);
                await App.Database.DeleteCourseAsync(course);
                await DisplayAlert("Info", "Course deleted.", "OK");
                await Navigation.PopAsync();
            }
        }

        private async void OnAddAssessmentClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AssessmentDetailPage(new Assessment(), assessments, false));
        }

        private async void OnAssessmentSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count > 0)
            {
                var selectedAssessment = e.CurrentSelection[0] as Assessment;
                await Navigation.PushAsync(new AssessmentDetailPage(selectedAssessment, assessments, true));
                ((CollectionView)sender).SelectedItem = null; // Deselect the item
            }
        }

        private async void OnShareNotesClicked(object sender, EventArgs e)
        {
            await ShareNotes(course.Notes);
        }

        private async Task ShareNotes(string notes)
        {
            if (string.IsNullOrEmpty(notes))
            {
                await DisplayAlert("Error", "No notes to share.", "OK");
                return;
            }

            await Share.RequestAsync(new ShareTextRequest
            {
                Title = "Share Course Notes",
                Text = notes
            });
        }
    }
}
