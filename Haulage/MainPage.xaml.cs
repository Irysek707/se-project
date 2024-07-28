using Haulage.Control;
using Haulage.Model;
using Haulage.Model.Constants;
using Haulage.View;

namespace Haulage;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private void GoToCustomerPage(object sender, EventArgs e)
	{
        App.Current.MainPage = new NavigationPage(new CustomerPage(MockResources.mockCustomer));
    }

    private void GoToDriverPage(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new DriverPage(MockResources.mockDriver));
    }
}

