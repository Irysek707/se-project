using Haulage.Control;
using Haulage.Model;


namespace Haulage.View;

public partial class CustomerPage : ContentPage
{
	public CustomerPage(User user)
	{
		InitializeComponent();
		UserName.Text = "Currently logged in as " + user.Login;
		try
		{
			List<CustomerOrder> orders = CustomerController.GetAllOrders(user.Login);
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