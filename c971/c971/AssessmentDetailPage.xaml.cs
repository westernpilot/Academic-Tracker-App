using Microsoft.Maui.Controls;
using Plugin.LocalNotification;
using System;
using System.Collections.ObjectModel;
using c971.Shared.Models; 
using c971.Shared; 
namespace c971 
{
    public partial class AssessmentDetailPage : ContentPage
    {
        private Assessment assessment;
        private ObservableCollection<Assessment> assessments;
        private bool isEdit;

        public AssessmentDetailPage(Assessment assessment, ObservableCollection<Assessment> assessments, bool isEdit)
        {
            InitializeComponent();
            this.assessment = assessment ?? new Assessment();
            this.assessments = assessments;
            this.isEdit = isEdit;

            BindingContext = this.assessment;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (!isEdit)
            {
                assessments.Add(assessment);
            }
            else
            {
                var index = assessments.IndexOf(assessment);
                assessments[index] = assessment;
            }

            await App.Database.SaveAssessmentAsync(assessment); // Ensure that App.Database is accessible
            ScheduleNotifications(assessment);

            await DisplayAlert("Info", "Assessment details saved.", "OK");
            await Navigation.PopAsync();
        }

        private void ScheduleNotifications(Assessment assessment)
        {
            var startNotification = new NotificationRequest
            {
                NotificationId = new Random().Next(),
                Title = "Assessment Start",
                Description = $"Assessment {assessment.Title} starts today!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = assessment.StartDate
                }
            };

            var endNotification = new NotificationRequest
            {
                NotificationId = new Random().Next(),
                Title = "Assessment End",
                Description = $"Assessment {assessment.Title} ends today!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = assessment.EndDate
                }
            };

            LocalNotificationCenter.Current.Show(startNotification);
            LocalNotificationCenter.Current.Show(endNotification);
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (isEdit)
            {
                assessments.Remove(assessment);
                await App.Database.DeleteAssessmentAsync(assessment);
                await DisplayAlert("Info", "Assessment deleted.", "OK");
                await Navigation.PopAsync();
            }
        }
    }
}
