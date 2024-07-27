using Haulage.Control;
using Haulage.Model;
using Haulage.Model.Users;


namespace Haulage.View;

public partial class CustomerPage : ContentPage
{
	public CustomerPage(Customer customer)
	{
		InitializeComponent();
		UserName.Text = "Currently logged in as " + customer.Login;
		CustomerController controller = new CustomerController(customer.Login);
		try
		{
			List<CustomerOrder> orders = controller.GetAllOrders();
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
		else if(Orders.SelectedItem is CustomerOrder) {
			CustomerOrder order = Orders.SelectedItem as CustomerOrder;
			try
			{
                CustomerOrder orderWithDetails = CustomerController.GetCustomerOrder(order.Id.ToString());
                App.Current.MainPage = new NavigationPage(new OrderPreview(orderWithDetails));
            }
			catch (Exception ex)
			{
				ErrorMessage.Text = ex.Message;
			}
        }
		}
}