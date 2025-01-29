using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
using c971.Shared.Models;

namespace c971.Pages
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            try
            {
                // Validate user inputs (name, email, etc.)
                if (string.IsNullOrEmpty(nameEntry.Text) || string.IsNullOrEmpty(emailEntry.Text))
                {
                    await DisplayAlert("Error", "Name and Email are required.", "OK");
                    return;
                }

                User user;
                // Check if the user is Graduate or Undergraduate based on user selection
                if (userTypePicker.SelectedItem.ToString() == "Graduate")
                {
                    user = new GraduateUser
                    {
                        Name = nameEntry.Text,
                        Email = emailEntry.Text
                    };
                    await App.Database.SaveGraduateUserAsync((GraduateUser)user);
                }
                else if (userTypePicker.SelectedItem.ToString() == "Undergraduate")
                {
                    user = new UndergraduateUser
                    {
                        Name = nameEntry.Text,
                        Email = emailEntry.Text
                    };
                    await App.Database.SaveUndergraduateUserAsync((UndergraduateUser)user);
                }
                else
                {
                    await DisplayAlert("Error", "Please select a valid user type.", "OK");
                    return;
                }

                await DisplayAlert("Success", "User registered successfully.", "OK");
                // Navigate to main page or other appropriate page
                await Navigation.PushAsync(new MainPage());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}
