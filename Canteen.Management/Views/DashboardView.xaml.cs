using System.Windows.Controls;
using Canteen.Management.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Canteen.Management.Views;

public partial class DashboardView : UserControl
{
    public DashboardView()
    {
        InitializeComponent();

        DataContext = App.Current.Services.GetService<DashboardViewModel>();
    }
}