using Haulage.Control;
using Haulage.Model;

namespace Haulage.View;

public partial class DriverPage : ContentPage
{
	public DriverPage(User user)
	{
		InitializeComponent();
        UserName.Text = "Currently logged in as " + user.Login;
        try
        {
            List<Trip> trips = DriverController.GetAllTrips(user.Login);
            Trips.ItemsSource = trips;
        }
        catch (Exception ex)
        {
            ErrorMessage.Text = ex.Message;
        }
    }
}