using Haulage.Control;
using Haulage.Model;
using Haulage.Model.Constants;
using Haulage.Model.Users;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Haulage.View
{
    public partial class CustomerPage : ContentPage
    {
        private Customer customer;
        private CustomerController controller;
        private ObservableCollection<CustomerOrder> orders;

        public CustomerPage(Customer customer)
        {
            InitializeComponent();
            this.customer = customer;
            this.controller = new CustomerController(customer.Login);
            UserName.Text = "Currently logged in as " + customer.Login;

            ConfirmPickupCommand = new Command<CustomerOrder>(ConfirmPickup);
            LoadOrders();

            BindingContext = this;
        }

        public ICommand ConfirmPickupCommand { get; }

        private void LoadOrders()
        {
            try
            {
                var orderList = controller.GetAllOrders();
                orders = new ObservableCollection<CustomerOrder>(orderList);
                Orders.ItemsSource = orders;
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message;
            }
        }

        private void Orders_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (Orders.SelectedItem == null)
            {
                ErrorMessage.Text = "Please select an order to look in detail";
            }
            else if (Orders.SelectedItem is CustomerOrder order)
            {
                try
                {
                    CustomerOrder orderWithDetails = controller.GetCustomerOrder(order.Id.ToString());
                    App.Current.MainPage = new NavigationPage(new OrderPreview(orderWithDetails, customer));
                }
                catch (Exception ex)
                {
                    ErrorMessage.Text = ex.Message;
                }
            }
        }

        private async void ConfirmPickup(CustomerOrder order)
        {
            try
            {
                // Change the status to EXPECTED
                order.Status = Status.EXPECTED;
                // Update the order in the database
                DB.connection.Update(order);

                await DisplayAlert("Success", "Pickup confirmed. Status updated to EXPECTED.", "OK");

                // Refresh the list
                LoadOrders();
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
