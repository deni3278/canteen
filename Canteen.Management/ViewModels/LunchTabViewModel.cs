using System;
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

public class LunchTabViewModel : ObservableObject
{
    private readonly IApiService _api;
    private ObservableCollection<ItemDto> _lunchItems = new();
    private ItemDto _selectedItemMonday;
    private ItemDto _selectedItemTuesday;
    private ItemDto _selectedItemWednesday;
    private ItemDto _selectedItemThursday;
    private ItemDto _selectedItemFriday;



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
            if (value.ItemId == 0)
            {
                
            }
        }
    }
    
    public Boolean IsNoLunchTuesday { get; set; }
    public ItemDto SelectedItemWednesday
    {
        get => _selectedItemWednesday;
        set
        {
            SetProperty( ref _selectedItemWednesday, value);

            if (value.ItemId == 0)
            {
                IsNoLunchWednesday = true;
            }
        }
    }
    
    public Boolean IsNoLunchWednesday { get; set; }
    public ItemDto SelectedItemThursday
    {
        get => _selectedItemThursday;
        set
        {
            SetProperty(ref _selectedItemThursday, value);

            if (value.ItemId == 0)
            {
                IsNoLunchThursday = true;
            }
        }
    }
    
    public Boolean IsNoLunchThursday { get; set; }

    public ItemDto SelectedItemFriday
    {
        get => _selectedItemFriday;
        set
        {
            SetProperty(ref _selectedItemFriday, value);

            if (value.ItemId == 0)
            {
                IsNoLunchFriday = true;
            }
        }
    }

    public Boolean IsNoLunchFriday { get; set; }
    public IAsyncRelayCommand RefreshCommand { get; set; }

    public LunchTabViewModel(IApiService api)
    {
        _api = api;

        RefreshCommand = new AsyncRelayCommand(Refresh);
    }

    private async Task Refresh()
    {
        var lunchItems = await _api.GetAsync<IEnumerable<ItemDto>>("items/category/4");
        
        LunchItems.Add(new ItemDto()
        {
            ItemId = 0,
            Name = "No Lunch",
            Price = 0,
            CategoryId = 4
        });
        foreach (var lunchItem in lunchItems)
        {
            LunchItems.Add(lunchItem);
        }
    }
}