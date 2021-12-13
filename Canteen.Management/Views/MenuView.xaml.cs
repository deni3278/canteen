using System.Windows.Controls;
using Canteen.Management.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Canteen.Management.Views;

public partial class MenuView : UserControl
{
    public MenuView()
    {
        InitializeComponent();

        DataContext = App.Current.Services.GetService<MenuViewModel>();
    }
}