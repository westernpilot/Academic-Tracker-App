using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using c971.Shared;

namespace c971.Pages
{
    public partial class TermDetailPage : ContentPage
    {
        private AcademicTerm term;
        private ObservableCollection<AcademicTerm> terms;
        private ObservableCollection<Course> courses;
        private bool isEdit;

        public TermDetailPage(AcademicTerm term, ObservableCollection<AcademicTerm> terms, bool isEdit)
        {
            InitializeComponent();
            this.term = term ?? new AcademicTerm();
            this.terms = terms;
            this.isEdit = isEdit;

            BindingContext = this.term;

            // Load courses associated with this term
            LoadCoursesAsync();
        }

        private async Task LoadCoursesAsync()
        {
            var courseList = await App.Database.GetCoursesByTermAsync(term.Id);
            courses = new ObservableCollection<Course>(courseList);
            coursesView.ItemsSource = courses;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (!isEdit)
            {
                terms.Add(term);
            }

            await App.Database.SaveTermAsync(term);
            await DisplayAlert("Info", "Term details saved.", "OK");
            await Navigation.PopAsync();
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (isEdit)
            {
                terms.Remove(term);
                await App.Database.DeleteTermAsync(term);
                await DisplayAlert("Info", "Term deleted.", "OK");
                await Navigation.PopAsync();
            }
        }

        private async void OnAddCourseClicked(object sender, EventArgs e)
        {
            // Use ObservableCollection for the courses collection
            await Navigation.PushAsync(new CourseDetailPage(new Course { TermId = term.Id }, courses, false));
        }

        private async void OnCourseSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count > 0)
            {
                var selectedCourse = e.CurrentSelection[0] as Course;
                await Navigation.PushAsync(new CourseDetailPage(selectedCourse, courses, true));
            }
        }
    }
}
