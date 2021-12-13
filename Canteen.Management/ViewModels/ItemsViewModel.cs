﻿using System.Collections.Generic;
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
    private ItemDto _selectedItem = null!;
    private CategoryItemsDto _selectedCategory = null!;

    public ItemsViewModel(IApiService api)
    {
        _api = api;

        RefreshCommand = new AsyncRelayCommand(Refresh);
        ResizeCommand = new RelayCommand<double>(Resize);

        RemoveCommand = new AsyncRelayCommand(async () => await _api.PostAsync("items/delete",
                                                                               SelectedItem),
                                              () => SelectedItem != null!);
    }

    public ObservableCollection<CategoryItemsDto> Categories { get; } = new();
    public IAsyncRelayCommand RefreshCommand { get; }
    public IRelayCommand<double> ResizeCommand { get; }
    public IAsyncRelayCommand RemoveCommand { get; }

    public double IdColumnWidth
    {
        get => _idColumnWidth;
        set =>
            SetProperty(ref _idColumnWidth,
                        value);
    }

    public double NameColumnWidth
    {
        get => _nameColumnWidth;
        set =>
            SetProperty(ref _nameColumnWidth,
                        value);
    }

    public double PriceColumnWidth
    {
        get => _priceColumnWidth;
        set =>
            SetProperty(ref _priceColumnWidth,
                        value);
    }

    public ItemDto SelectedItem
    {
        get => _selectedItem;
        set
        {
            SetProperty(ref _selectedItem,
                        value);

            RemoveCommand.NotifyCanExecuteChanged();
        }
    }

    public CategoryItemsDto SelectedCategory
    {
        get => _selectedCategory;
        set =>
            SetProperty(ref _selectedCategory,
                        value);
    }

    public async Task AddItem(ItemDto item)
    {
        await _api.PostAsync("items/create",
                             item);

        await Refresh();
    }

    private async Task Refresh()
    {
        var categoryItems = await _api.GetAsync<IEnumerable<CategoryItemsDto>>("categories?includeItems=true");

        var categoryItemsDtos = categoryItems as CategoryItemsDto[] ?? categoryItems.ToArray();

        if (categoryItemsDtos.SequenceEqual(Categories,
                                            new CategoryItemsEqualityComparer()))
            return;

        var selectedCategory = _selectedCategory;

        Categories.Clear();

        foreach (var category in categoryItemsDtos)
        {
            category.Items = new ObservableCollection<ItemDto>(category.Items);

            Categories.Add(category);
        }

        selectedCategory = Categories.FirstOrDefault(category => category.Name.Equals(selectedCategory.Name));

        if (selectedCategory != null) SelectedCategory = selectedCategory;
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