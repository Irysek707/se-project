using Haulage.Control;
using Haulage.Model;
using Haulage.Model.Constants;
using Microsoft.Maui.Controls;

namespace Haulage.View;

public partial class TripPage : ContentPage
{
    private User user;
    private Trip trip;
    private string driver;
    private string vehicle;

    public TripPage(Trip trip, User user)
    {
        InitializeComponent();
        this.user = user;
        this.trip = trip;
        UserName.Text = "Currently logged in as " + user.Login;
        TripId.Text = "Currently viewing trip " + trip.Id;
        StopCount.Text = "Number of stops: " + trip.NumberOfStops;
        Stops.ItemsSource = trip.Stops;
        Status.Text = "Current trip status " + trip.TripStatus;
        ScheduledDuration.Text = "Scheduled duration for the trip " + trip.ScheduledDuration;

        // Check if the trip is delayed and set the visibility of the button
        DelayedTripBtn.IsVisible = trip.TripStatus != TripStatus.DELAYED;

        // Check if the trip is delayed or scheduled and set the visibility of the button
        OnTimeTripBtn.IsVisible = trip.TripStatus == TripStatus.DELAYED || trip.TripStatus == TripStatus.SCHEDULED;

        if (user.Role == Model.Constants.Role.ADMIN)
        {
            AllocateDriverBtn.IsEnabled = true;
            AllocateDriverBtn.IsVisible = true;
            AllocateVehicleBtn.IsEnabled = true;
            AllocateVehicleBtn.IsVisible = true;
            Vehicles.ItemsSource = AdminController.GetAllVehicles();
        }
    }

    private async void AllocateDriverBtn_Clicked(object sender, EventArgs e)
    {
        AdminController.DeallocateDriver(this.trip);
        if (Drivers.SelectedItem != null)
        {
            Driver driver = Drivers.SelectedItem as Driver;
            try
            {
                AdminController.AllocateDriver(driver, trip);
                this.driver = driver.Login;
                await DisplayAlert("Driver Assigned", this.driver, "Accept");
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message;
            }
        }
        else
        {
            ErrorMessage.Text = "No driver selected";
        }
    }

    private async void AllocateVehicleBtn_Clicked(object sender, EventArgs e)
    {
        AdminController.DeallocateVehicle(this.trip);
        if (Vehicles.SelectedItem != null)
        {
            Transport vehicle = Vehicles.SelectedItem as Transport;
            try
            {
                AdminController.AllocateVehicle(vehicle, trip);
                this.vehicle = vehicle.Name;
                await DisplayAlert("Vehicle Assigned", this.vehicle, "Accept");
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message;
            }
        }
        else
        {
            ErrorMessage.Text = "No vehicle selected";
        }
    }

    private async void BackToUserPageBtn_Clicked(object sender, EventArgs e)
    {
        if (user.Role == Model.Constants.Role.DRIVER)
        {
            App.Current.MainPage = new NavigationPage(new DriverPage(MockResources.mockDriver));
        }
        else if (user.Role == Model.Constants.Role.ADMIN)
        {
            App.Current.MainPage = new NavigationPage(new AdministratorPage(MockResources.mockAdmin));
        }
    }

    // Event handler for the "Delay the trip" button
    private async void DelayedTripBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            trip.DelayTrip();
            Status.Text = "Current trip status " + trip.TripStatus;
            await DisplayAlert("Trip Delayed", "The trip has been delayed.", "OK");
            DelayedTripBtn.IsVisible = false; // Hide the button after delaying the trip
        }
        catch (Exception ex)
        {
            ErrorMessage.Text = ex.Message;
        }
    }

    // Event handler for the "OnTime trip" button
    private async void OnTimeTripBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            trip.OnTimeTrip();
            Status.Text = "Current trip status " + trip.TripStatus;
            await DisplayAlert("Trip On Time", "The trip has been set as On Time.", "OK");
            OnTimeTripBtn.IsVisible = false; // Hide the button after adding On Time to the trip
        }
        catch (Exception ex)
        {
            ErrorMessage.Text = ex.Message;
        }
    }
}
