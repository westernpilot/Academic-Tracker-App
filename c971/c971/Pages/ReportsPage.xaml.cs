using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace c971.Pages
{
    public partial class ReportsPage : ContentPage
    {
        private ObservableCollection<LogEntry> logs;
        private ObservableCollection<LogEntry> filteredLogs;

        public ReportsPage()
        {
            InitializeComponent();
            logs = new ObservableCollection<LogEntry>();
            filteredLogs = new ObservableCollection<LogEntry>(logs);
            logsCollectionView.ItemsSource = filteredLogs;
        }

        private async void OnGenerateLogsClicked(object sender, EventArgs e)
        {
            try
            {
                // Generate log files with multiple rows
                var logDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Logs");
                Directory.CreateDirectory(logDirectory);

                for (int i = 1; i <= 5; i++) // Generate 5 log files as an example
                {
                    var logFileName = $"log_{DateTime.Now:yyyyMMdd_HHmmss}_{i}.txt";
                    var logFilePath = Path.Combine(logDirectory, logFileName);

                    using (var writer = new StreamWriter(logFilePath))
                    {
                        writer.WriteLine($"Log File: {logFileName}");
                        for (int j = 1; j <= 10; j++) // Generate 10 rows of logs per file
                        {
                            writer.WriteLine($"Log Entry {j} - {DateTime.Now}");
                        }
                    }

                    // Add log entry to the collection
                    logs.Add(new LogEntry
                    {
                        LogDate = DateTime.Now.ToString("g"),
                        LogContent = $"Generated {logFileName} with 10 entries"
                    });
                }

                // Update the filtered logs
                FilterLogs(searchBar.Text);

                await DisplayAlert("Success", "Logs generated successfully.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to generate logs: {ex.Message}", "OK");
            }
        }

        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            FilterLogs(e.NewTextValue);
        }

        private void FilterLogs(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                filteredLogs.Clear();
                foreach (var log in logs)
                {
                    filteredLogs.Add(log);
                }
            }
            else
            {
                var lowerSearchText = searchText.ToLower();
                var searchResults = logs.Where(log => log.LogContent.ToLower().Contains(lowerSearchText)).ToList();

                filteredLogs.Clear();
                foreach (var log in searchResults)
                {
                    filteredLogs.Add(log);
                }
            }
        }

        public class LogEntry
        {
            public string LogDate { get; set; }
            public string LogContent { get; set; }
        }
    }
}
