using System.Windows;
using System.Windows.Controls;
using Canteen.Management.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Canteen.Management.Views;

public partial class EmployeesView : UserControl
{
    public EmployeesView()
    {
        InitializeComponent();

        DataContext = App.Current.Services.GetService<EmployeesViewModel>();
    }

    private async void EditEmployee_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new EditEmployeeWindow
        {
            Owner = App.Current.MainWindow
        };

        var employeesViewModel = DataContext as EmployeesViewModel;
        var editEmployeeViewModel = window.DataContext as EditEmployeeViewModel;

        await editEmployeeViewModel.Initialize(employeesViewModel.SelectedEmployee);

        if (!window.ShowDialog().Value) return;

        await employeesViewModel.EditEmployee(editEmployeeViewModel.EmployeeLunch);
    }

    private async void Refresh_OnClick(object sender, RoutedEventArgs e)
    {
        var employeesViewModel = DataContext as EmployeesViewModel;
        await employeesViewModel.RefreshCommand.ExecuteAsync(null);
    }
}