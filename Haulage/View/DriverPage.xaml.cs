using Haulage.Control;
using Haulage.Model;
using Microsoft.Maui.Controls;

namespace Haulage.View;

public partial class DriverPage : ContentPage
{
    private Driver driver;
    public DriverPage(Driver driver)
    {
        InitializeComponent();
        this.driver = driver;
        UserName.Text = "Currently logged in as " + driver.Login;
        DriverController controller = new DriverController(driver.Login);
        try
        {
            List<Trip> trips = controller.GetAllTrips();
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
                ErrorMessage.Text = "Please select a trip to look in detail";
            }
            else if (Trips.SelectedItem is Trip)
            {
                Trip trip = Trips.SelectedItem as Trip;
                try
                {
                    Trip tripWithDetails = AdminController.GetTrip(trip.Id.ToString());
                    App.Current.MainPage = new NavigationPage(new TripPage(tripWithDetails, this.driver));
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

}



