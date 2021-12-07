using System.Windows;
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

    private void ListView_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        var list = sender as ListView;
        var grid = list.View as GridView;
        var actualWidth = list.ActualWidth - SystemParameters.VerticalScrollBarWidth;
        
        const double first = 0.20;
        const double second = 0.60;
        const double third = 0.20;

        grid.Columns[0].Width = actualWidth * first;
        grid.Columns[1].Width = actualWidth * second;
        grid.Columns[2].Width = actualWidth * third;
    }
}