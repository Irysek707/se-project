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
            string newName = await DisplayPromptAsync("Edit Driver", "Enter new name:", initialValue: driver.Login);
            if (!string.IsNullOrWhiteSpace(newName))
            {
                try
                {
                    driver.Login = newName;
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
            bool confirm = await DisplayAlert("Delete Driver", $"Are you sure you want to delete {driver.Login}?", "Yes", "No");
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
            string newDriverName = await DisplayPromptAsync("Add New Driver", "Enter driver name:");
            if (!string.IsNullOrWhiteSpace(newDriverName))
            {
                try
                {
                    var newDriver = new Driver { Login = newDriverName };
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
