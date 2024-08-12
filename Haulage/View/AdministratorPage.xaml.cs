using Haulage.Control;
using Haulage.Model;
using Haulage.Model.Users;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;

namespace Haulage.View
{
    public partial class AdministratorPage : ContentPage
    {
        private Admin admin;
        private AdminController adminController = new AdminController();

        public AdministratorPage(Admin admin)
        {
            InitializeComponent();
            this.admin = admin;
            UserName.Text = "Currently logged in as " + admin.Login;
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<Trip> trips = adminController.GetAllTrips();
                Trips.ItemsSource = trips;

                List<Driver> drivers = AdminController.GetAllDrivers();
                DriversListView.ItemsSource = drivers;
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message;
            }
        }

        private void Trips_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (Trips.SelectedItem == null)
            {
                ErrorMessage.Text = "Please select a trip to look in detail";
            }
            else if (Trips.SelectedItem is Trip trip)
            {
                try
                {
                    Trip tripWithDetails = AdminController.GetTrip(trip.Id.ToString());
                    App.Current.MainPage = new NavigationPage(new TripPage(tripWithDetails, admin));
                }
                catch (Exception ex)
                {
                    ErrorMessage.Text = ex.Message;
                }
            }
        }

        private void BackToMainPage(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new MainPage());
        }

        private async void EditDriver_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var driver = button.CommandParameter as Driver;

            // Prompt for new Name and Surname
            string newName = await DisplayPromptAsync("Edit Driver", "Enter new name:", initialValue: driver.Name);
            string newSurname = await DisplayPromptAsync("Edit Driver", "Enter new surname:", initialValue: driver.Surname);
            if (!string.IsNullOrWhiteSpace(newName) && !string.IsNullOrWhiteSpace(newSurname))
            {
                try
                {
                    driver.Name = newName;
                    driver.Surname = newSurname;
                    AdminController.UpdateDriver(driver);
                    LoadData();
                }
                catch (Exception ex)
                {
                    ErrorMessage.Text = ex.Message;
                }
            }
        }

        private async void DeleteDriver_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var driver = button.CommandParameter as Driver;
            bool confirm = await DisplayAlert("Delete Driver", $"Are you sure you want to delete {driver.Name} {driver.Surname}?", "Yes", "No");
            if (confirm)
            {
                try
                {
                    AdminController.DeleteDriver(driver);
                    LoadData();
                }
                catch (Exception ex)
                {
                    ErrorMessage.Text = ex.Message;
                }
            }
        }

        private async void AddNewDriver_Clicked(object sender, EventArgs e)
        {
            // Prompt for Login, Name and Surname of a new Driver
            string newLogin = await DisplayPromptAsync("Add New Driver", "Enter driver login:");
            string newName = await DisplayPromptAsync("Add New Driver", "Enter driver name:");
            string newSurname = await DisplayPromptAsync("Add New Driver", "Enter driver surname:");
            if (!string.IsNullOrWhiteSpace(newName) && !string.IsNullOrWhiteSpace(newSurname))
            {
                try
                {
                    var newDriver = new Driver
                    {
                        Login = newLogin,
                        Name = newName,
                        Surname = newSurname
                    };
                    AdminController.AddDriver(newDriver);
                    LoadData();
                }
                catch (Exception ex)
                {
                    ErrorMessage.Text = ex.Message;
                }
            }
        }
    }
}
