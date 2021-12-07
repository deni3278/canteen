using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Canteen.Dto;
using Canteen.Management.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Canteen.Management.ViewModels;

public class ItemsViewModel : ObservableObject
{
    private readonly IApiService _api;
    private double _idColumnWidth;
    private double _nameColumnWidth;
    private double _priceColumnWidth;
    private ItemDto _selectedItem;

    public ObservableCollection<CategoryItemsDto> Categories { get; } = new();
    public IAsyncRelayCommand RefreshCommand { get; }
    public IRelayCommand<double> ResizeCommand { get; }
    public IAsyncRelayCommand RemoveCommand { get; }

    public double IdColumnWidth
    {
        get => _idColumnWidth;
        set => SetProperty(ref _idColumnWidth, value);
    }

    public double NameColumnWidth
    {
        get => _nameColumnWidth;
        set => SetProperty(ref _nameColumnWidth, value);
    }

    public double PriceColumnWidth
    {
        get => _priceColumnWidth;
        set => SetProperty(ref _priceColumnWidth, value);
    }

    public ItemDto SelectedItem
    {
        get => _selectedItem;
        set
        {
            SetProperty(ref _selectedItem, value);
            RemoveCommand.NotifyCanExecuteChanged();
        }
    }

    public ItemsViewModel(IApiService api)
    {
        _api = api;

        RefreshCommand = new AsyncRelayCommand(Refresh);
        ResizeCommand = new RelayCommand<double>(Resize);
        RemoveCommand = new AsyncRelayCommand(async () => await _api.PostAsync("items/delete", SelectedItem), () => SelectedItem != null);
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

    private void Resize(double viewWidth)
    {
        var actualWidth = viewWidth - SystemParameters.VerticalScrollBarWidth;

        const double idColumn = 0.20;
        const double nameColumn = 0.60;
        const double priceColumn = 0.20;

        IdColumnWidth = actualWidth * idColumn;
        NameColumnWidth = actualWidth * nameColumn;
        PriceColumnWidth = actualWidth * priceColumn;
    }
}