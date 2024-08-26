using Haulage.Control;
using Haulage.Model;
using Haulage.Model.Helpers;
using Haulage.Model.Users;
using Microsoft.Maui.Controls;
using static Haulage.Model.Helpers.OtherHelpers;

namespace Haulage.View;

public partial class AdministratorPage : ContentPage
{
    private Admin admin;
    private AdminController adminController = new AdminController();
    public AdministratorPage(Admin admin)
    {
        InitializeComponent();
        this.admin = admin;
        UserName.Text = "Currently logged in as " + admin.Login;
        try
        {
            List<Trip> trips = adminController.GetAllTrips();
            Trips.ItemsSource = trips;
        }
        catch (Exception ex)
        {
            ErrorMessage.Text = ex.Message;
        }
    }

    private void Trips_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if(Trips.SelectedItem == null)
        {
            ErrorMessage.Text = "Please select a trip to look in detail";
        }
        else if(Trips.SelectedItem is Trip)
        {
            Trip trip = Trips.SelectedItem as Trip;
            try
            {
                Trip tripWithDetails = AdminController.GetTrip(trip.Id.ToString());
                App.Current.MainPage = new NavigationPage(new TripPage(tripWithDetails, admin));
            }
            catch (Exception ex) {
                ErrorMessage.Text = ex.Message;
            }
        }


    }

    private void BackToMainPage(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new MainPage());
    }


    // Method to plan trip routes
    private void PlanTripRoutesButton_Click(object sender, EventArgs e)
    {
        PlanTripRoutes();
    }

    private void PlanTripRoutes()
    {
        // Example placeholder data: list of destinations
        List<string> destinations = new List<string> { "DestinationA", "DestinationB", "DestinationC" };

        // Call the RouteHelper's CalculateRoute method
        List<string> plannedRoute = RouteHelper.CalculateRoute(destinations);

        // Combine the planned route into a single string for display
        string plannedRouteString = string.Join(" -> ", plannedRoute);

        // Display the planned route on a label
        PlannedRouteLabel.Text = "Planned Route: " + plannedRouteString;
    }
}