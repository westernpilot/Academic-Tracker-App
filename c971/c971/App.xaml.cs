using Microsoft.Maui.Controls;
using System;
using System.IO;
using c971.Shared; // Ensure this matches the new location of the Database class

namespace c971
{
    public partial class App : Application
    {
        public static Database Database { get; private set; }

        public App()
        {
            InitializeComponent();

            // Path to the SQLite database file
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "c971.db3");
            Database = new Database(dbPath);

            // Seed the database with initial data
            SeedDatabase();

            MainPage = new AppShell();
        }

        private async void SeedDatabase()
        {
            await Database.SeedDataAsync();
        }
    }
}
