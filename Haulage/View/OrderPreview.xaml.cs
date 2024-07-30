using Haulage.Control;
using Haulage.Model;
using Haulage.Model.Users;
using Microsoft.Maui.Controls;

namespace Haulage.View
{
    public partial class OrderPreview : ContentPage
    {
        private CustomerOrder order;
        private Customer customer;
        private CustomerController controller;

        public OrderPreview(CustomerOrder orderToPreview, Customer customer)
        {
            InitializeComponent();
            this.customer = customer;
            this.controller = new CustomerController(customer.Login);
            UserName.Text = "Currently logged in as " + customer.Login;
            OrderId.Text = "Currently viewing Order " + orderToPreview.Id;
            order = orderToPreview;
            Items.ItemsSource = orderToPreview.Manifest.Items;
            Total.Text = "Total for this order is " + orderToPreview.Manifest.Total;

            if (orderToPreview.Handover != null)
            {
                string pickup = String.Format("Your {0} is scheduled for {1}", "pickup", orderToPreview.Handover.ExpectedHandover);
                string deliver = String.Format("Your {0} is scheduled for {1}", "delivery", orderToPreview.Handover.ExpectedHandover);
                HandoverDate.Text = orderToPreview.Handover.Pickup ? pickup : deliver;
            }

            Status.Text = "Current order status " + orderToPreview.Status;

            // Set button visibility based on order status
            ConfirmPickup.IsVisible = (orderToPreview.Status == Model.Constants.Status.AWAITING_PICKUP);
            ConfirmDelivery.IsVisible = (orderToPreview.Status == Model.Constants.Status.EXPECTED);

            if (orderToPreview.Status == Model.Constants.Status.PENDING)
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

        private async void ConfirmPickup_Clicked(object sender, EventArgs e)
        {
            try
            {
                order.ConfirmPickup();
                await DisplayAlert("Success", "Pickup confirmed. Status updated to EXPECTED.", "OK");
                RefreshPage();
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message;
            }
        }

        private async void ConfirmDelivery_Clicked(object sender, EventArgs e)
        {
            try
            {
                order.ConfirmDelivery();
                await DisplayAlert("Success", "Delivery confirmed. Status updated to COLLECTED.", "OK");
                RefreshPage();
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message;
            }
        }

        private async void PickupBtn_Clicked(object sender, EventArgs e)
        {
            if (DatePicker.Date > DateTime.Today || (DatePicker.Date == DateTime.Today && TimePicker.Time >= DateTime.Now.TimeOfDay))
            {
                try
                {
                    Handover handover = controller.ScheduleHandover(order, DatePicker.Date + TimePicker.Time, true);
                    await DisplayAlert("Pickup scheduled", handover.ExpectedHandover.ToString(), "Accept");
                    order.AddHandover(handover);
                    RefreshPage();
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

        private void RefreshPage()
        {
            App.Current.MainPage = new NavigationPage(new OrderPreview(order, customer));
        }

        private async void BackToCustomerPageBtn_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new CustomerPage(customer));
        }
    }
}
