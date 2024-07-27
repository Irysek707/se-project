using Haulage.Control;
using Haulage.Model;

namespace Haulage.View;

public partial class DriverPage : ContentPage
{
	public DriverPage(Driver driver)
	{
        
		InitializeComponent();
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
}