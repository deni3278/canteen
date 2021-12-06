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
}