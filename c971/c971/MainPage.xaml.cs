using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using c971.Shared.Models; // Ensure this matches the new location of the AcademicTerm model
using c971.Shared; // Ensure this matches the new location of the Database class

namespace c971.Pages
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<AcademicTerm> Terms { get; set; }
        public ObservableCollection<AcademicTerm> FilteredTerms { get; set; }

        public MainPage()
        {
            InitializeComponent();
            LoadTerms();
        }

        private async void LoadTerms()
        {
            var termList = await App.Database.GetTermsAsync();

            // Debugging output to verify all terms are being retrieved
            Console.WriteLine($"Number of terms retrieved: {termList.Count}");
            foreach (var term in termList)
            {
                Console.WriteLine($"Term: {term.Title}, Start: {term.StartDate}, End: {term.EndDate}");
            }

            Terms = new ObservableCollection<AcademicTerm>(termList);
            FilteredTerms = new ObservableCollection<AcademicTerm>(Terms);

            BindingContext = this;
            termsCollectionView.ItemsSource = FilteredTerms;
        }

        private async void OnAddTermClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TermDetailPage(new AcademicTerm(), Terms, false));
        }

        private async void OnTermSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count > 0)
            {
                var selectedTerm = e.CurrentSelection[0] as AcademicTerm;
                await Navigation.PushAsync(new TermDetailPage(selectedTerm, Terms, true));
            }
        }

        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = e.NewTextValue.ToLower();

            // Filter terms based on search text
            var filteredList = Terms.Where(t => t.Title.ToLower().Contains(searchText)).ToList();

            FilteredTerms.Clear();
            foreach (var term in filteredList)
            {
                FilteredTerms.Add(term);
            }
        }
    }
}
