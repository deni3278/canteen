using System.Windows;
using System.Windows.Controls;
using Canteen.Dto;
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

    private async void AddItem_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new AddItemWindow
        {
            Owner = App.Current.MainWindow
        };

        var itemsViewModel = DataContext as ItemsViewModel;
        var addItemViewModel = window.DataContext as AddItemViewModel;

        await itemsViewModel!.RefreshCommand.ExecuteAsync(null);

        foreach (var category in itemsViewModel.Categories)
        {
            addItemViewModel!.Categories.Add(new CategoryDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name
            });
        }

        if (!window.ShowDialog()!.Value) return;

        await itemsViewModel.AddItem(addItemViewModel!.Item);
    }

    private async void Refresh_OnClick(object sender, RoutedEventArgs e)
    {
        var itemsViewModel = DataContext as ItemsViewModel;
        await itemsViewModel!.RefreshCommand.ExecuteAsync(null);
    }

    private async void RemoveItem_OnClick(object sender, RoutedEventArgs e)
    {
        var itemsViewModel = DataContext as ItemsViewModel;
        await itemsViewModel!.RemoveCommand.ExecuteAsync(null);
    }
}