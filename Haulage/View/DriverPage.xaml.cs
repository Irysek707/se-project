using Haulage.Control;
using Haulage.Model;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;

namespace Haulage.View
{
    public partial class DriverPage : ContentPage
    {
        private Driver driver;
        private TripStop selectedStop;  // Correct type for selected stop
        private Trip selectedTrip;  // Correct type for selected trip

        public DriverPage(Driver driver)
        {
            InitializeComponent();
            this.driver = driver;
            UserName.Text = "Currently logged in as " + driver.Login;

            // Load the trips when the page initializes
            LoadTrips();
        }

        private void LoadTrips
            ()
        {
            try
            {
                List<Trip> trips = TripController.GetAllTrips();
                Trips.ItemsSource = trips;
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
                ErrorMessage.Text = "Please select a trip to look in detail.";
            }
            else if (Trips.SelectedItem is Trip trip)
            {
                try
                {
                    // Use TripController to get the selected trip with all details
                    selectedTrip = TripController.GetTripWithDetails(trip.Id.ToString());
                    StopsListView.ItemsSource = selectedTrip.Stops;  // Load stops for the selected trip
                    ErrorMessage.Text = "";  // Clear any previous error
                }
                catch (Exception ex)
                {
                    ErrorMessage.Text = ex.Message;
                }
            }
        }

        private void Stops_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (StopsListView.SelectedItem is TripStop stop)
            {
                selectedStop = stop;
                ErrorMessage.Text = "";  // Clear any previous error
            }
        }

        private async void ConfirmPickupDelivery_Clicked(object sender, EventArgs e)
        {
            if (selectedTrip == null)
            {
                ErrorMessage.Text = "Please select a trip.";
                return;
            }

            if (selectedStop == null)
            {
                ErrorMessage.Text = "Please select a stop.";
                return;
            }

            try
            {
                // Confirm the pickup or delivery using the selected trip and stop
                bool confirmationSuccess = selectedTrip.ConfirmPickupDelivery(selectedStop.Id);

                if (confirmationSuccess)
                {
                    await DisplayAlert("Confirmation", "Pickup/Delivery confirmed successfully.", "OK");
                    StopsListView.ItemsSource = selectedTrip.Stops;  // Refresh the stops list
                    ErrorMessage.Text = "";  // Clear any previous error
                }
                else
                {
                    ErrorMessage.Text = "Failed to confirm pickup/delivery. It may already be completed.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message;
            }
        }

        private void BackToMainPage(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new MainPage());
        }
    }
}
