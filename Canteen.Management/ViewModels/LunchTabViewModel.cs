using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Canteen.Dto;
using Canteen.Management.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Canteen.Management.ViewModels;

public class LunchTabViewModel : ObservableObject
{
    private readonly IApiService _api;
    private ObservableCollection<ItemDto> _lunchItems = null!;
    private ItemDto _selectedItemMonday = null!;
    private ItemDto _selectedItemTuesday = null!;
    private ItemDto _selectedItemWednesday = null!;
    private ItemDto _selectedItemThursday = null!;
    private ItemDto _selectedItemFriday = null!;

    public LunchTabViewModel(IApiService api)
    {
        _api = api;

        RefreshCommand = new AsyncRelayCommand(Refresh);
    }

    public IAsyncRelayCommand RefreshCommand { get; set; }

    public ObservableCollection<ItemDto> LunchItems
    {
        get => _lunchItems;
        set => SetProperty(ref _lunchItems, value);
    }

    public ItemDto SelectedItemMonday
    {
        get => _selectedItemMonday;
        set => SetProperty(ref _selectedItemMonday, value);
    }

    public ItemDto SelectedItemTuesday
    {
        get => _selectedItemTuesday;
        set
        {
            SetProperty(ref _selectedItemTuesday, value);

            if (value.ItemId == 0) { }
        }
    }

    public ItemDto SelectedItemWednesday
    {
        get => _selectedItemWednesday;
        set => SetProperty(ref _selectedItemWednesday, value);
    }

    public ItemDto SelectedItemThursday
    {
        get => _selectedItemThursday;
        set => SetProperty(ref _selectedItemThursday, value);
    }

    public ItemDto SelectedItemFriday
    {
        get => _selectedItemFriday;
        set => SetProperty(ref _selectedItemFriday, value);
    }

    private async Task Refresh()
    {
        var lunchItems = new List<ItemDto>
        {
            null!
        };

        lunchItems.AddRange(await _api.GetAsync<List<ItemDto>>("items/category/4"));

        LunchItems = new ObservableCollection<ItemDto>(lunchItems);
    }
}