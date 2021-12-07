using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Canteen.Dto;
using Canteen.Management.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Canteen.Management.ViewModels;

public class ItemsViewModel : ObservableObject
{
    private readonly IApiService _api;
    
    public ObservableCollection<CategoryItemsDto> Categories { get; } = new();
    public IAsyncRelayCommand RefreshCommand { get; }
    public IRelayCommand<ListView> ResizeCommand { get; }

    public ItemsViewModel(IApiService api)
    {
        _api = api;

        RefreshCommand = new AsyncRelayCommand(Refresh);
        ResizeCommand = new RelayCommand<ListView>(ResizeListView);
    }

    private async Task Refresh()
    {
        var categoryItems = await _api.GetAsync<IEnumerable<CategoryItemsDto>>("categories?includeItems=true");
        
        if (categoryItems.SequenceEqual(Categories, new CategoryItemsEqualityComparer()))
            return;
        
        Categories.Clear();
        
        foreach (var category in categoryItems)
        {
            category.Items = new ObservableCollection<ItemDto>(category.Items);
            
            Categories.Add(category);
        }
    }

    private void ResizeListView(ListView list)
    {
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