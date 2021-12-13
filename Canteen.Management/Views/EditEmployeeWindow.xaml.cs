using System.Windows;
using Canteen.Management.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Canteen.Management.Views;

public partial class EditEmployeeWindow : Window
{
    public EditEmployeeWindow()
    {
        InitializeComponent();

        DataContext = App.Current.Services.GetService<EditEmployeeViewModel>();
    }

    private void Close_OnClick(object sender,
                               RoutedEventArgs e)
    {
        DialogResult = false;

        Close();
    }

    private void EditEmployee_OnClick(object sender,
                                      RoutedEventArgs e)
    {
        DialogResult = true;

        Close();
    }
}