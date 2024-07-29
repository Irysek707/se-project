using Haulage.Control;
using Haulage.Model;

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
		if(user.Role == Model.Constants.Role.ADMIN)
		{
			
			AllocateDriverBtn.IsEnabled = true;
			AllocateDriverBtn.IsVisible = true;
			AllocateVehicleBtn.IsEnabled = true;
			AllocateVehicleBtn.IsVisible = true;
			Drivers.ItemsSource = AdminController.GetAllDriver();
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
			catch (Exception ex) { 
				ErrorMessage.Text = ex.Message;
			}
		}
		else
		{
			ErrorMessage.Text = "No driver selected";
		}
    }

    private async  void AllocateVehicleBtn_Clicked(object sender, EventArgs e)
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
}