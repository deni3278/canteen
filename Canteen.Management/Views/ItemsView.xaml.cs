using System.Windows.Controls;
using Canteen.Management.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Canteen.Management.Views;

public partial class ItemsView : UserControl
{
    public ItemsView()
    {
        InitializeComponent();
        
        DataContext = App.Current.Services.GetService<ItemsViewModel>();
    }
}