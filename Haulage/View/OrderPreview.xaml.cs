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

            // Show "Confirm Pickup" button only if status is AWAITING_PICKUP
            if (orderToPreview.Status == Model.Constants.Status.AWAITING_PICKUP)
            {
                ConfirmPickup.IsVisible = true;
            }
            else
            {
                ConfirmPickup.IsVisible = false;
            }

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

            // Show "Confirm Delivery" button only if status is EXPECTED
            if (orderToPreview.Status == Model.Constants.Status.EXPECTED)
            {
                ConfirmDelivery.IsVisible = true;
            }
            else
            {
                ConfirmDelivery.IsVisible = false;
            }

        }

        private async void ConfirmPickup_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Change the status to EXPECTED
                order.Status = Model.Constants.Status.EXPECTED;
                // Update the order in the database
                DB.connection.Update(order);

                await DisplayAlert("Success", "Pickup confirmed. Status updated to EXPECTED.", "OK");

                // Refresh the page
                App.Current.MainPage = new NavigationPage(new OrderPreview(order, customer));
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
                // Change the status to COLLECTED
                order.Status = Model.Constants.Status.COLLECTED;
                // Update the order in the database
                DB.connection.Update(order);

                await DisplayAlert("Success", "Delivery confirmed. Status updated to COLLECTED.", "OK");

                // Refresh the page
                App.Current.MainPage = new NavigationPage(new OrderPreview(order, customer));
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
                    App.Current.MainPage = new NavigationPage(new OrderPreview(order, customer));
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

        private async void BackToCustomerPageBtn_Clicked(object sender, EventArgs e)
        {
            // Navigate back to the customer page
            App.Current.MainPage = new NavigationPage(new CustomerPage(customer)); // Assuming CustomerPage is the page for customer overview
        }
    }
}
