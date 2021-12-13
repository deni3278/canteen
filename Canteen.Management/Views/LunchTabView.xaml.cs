using System.Windows.Controls;
using Canteen.Management.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Canteen.Management.Views;

public partial class LunchTabView : UserControl
{
    public LunchTabView()
    {
        InitializeComponent();

        DataContext = App.Current.Services.GetService<LunchTabViewModel>();
    }
}