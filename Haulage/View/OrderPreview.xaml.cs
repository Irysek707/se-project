using Haulage.Control;
using Haulage.Model;
using Microsoft.Maui.Controls;

namespace Haulage.View;

public partial class OrderPreview : ContentPage
{
    CustomerOrder order;
	public OrderPreview(CustomerOrder orderToPreview)
	{
        InitializeComponent();
        OrderId.Text = "Currently viewing Order " + orderToPreview.Id;
        order = orderToPreview;
        Items.ItemsSource = orderToPreview.Manifest.Items;
        Total.Text = "Total for this order is " + orderToPreview.Manifest.Total;
        if(orderToPreview.Handover != null)
        {
            string pickup = String.Format("Your {0} is scheduled for {1}", "pickup", orderToPreview.Handover.ExpectedHandover);
            string deliver = String.Format("Your {0} is scheduled for {1}", "delivery", orderToPreview.Handover.ExpectedHandover);
            HandoverDate.Text = orderToPreview.Handover.Pickup ? pickup : deliver;
        }
        if(orderToPreview.Status == Model.Constants.Status.PENDING)
        {
            PickupDate.IsVisible = true;
            DatePicker.IsVisible = true;
            DatePicker.IsEnabled = true;
            TimePicker.IsEnabled = true;
            TimePicker.IsVisible = true;
            PickupBtn.IsEnabled = true;
            PickupBtn.IsVisible = true;
        }
    }

    private async void PickupBtn_Clicked(object sender, EventArgs e)
    {
        if (DatePicker.Date > DateTime.Today || (DatePicker.Date == DateTime.Today && TimePicker.Time >= DateTime.Now.TimeOfDay))
        {
            try
            {
                Handover handover = CustomerController.ScheduleHandover(order, DatePicker.Date+TimePicker.Time, true);
                await DisplayAlert("Pickup scheduled", handover.ExpectedHandover.ToString(),"Accept");
                order.AddHandover(handover);
                App.Current.MainPage = new NavigationPage(new OrderPreview(order));
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message;
            }
        }
        else
        {
            ErrorMessage.Text = "Please select a valid date to pickup";
        }
    }
}