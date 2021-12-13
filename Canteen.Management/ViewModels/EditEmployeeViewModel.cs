using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Canteen.Dto;
using Canteen.Management.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Canteen.Management.ViewModels;

public class EditEmployeeViewModel : ObservableValidator
{
    private readonly IApiService _api;

    private bool _mondayChecked;
    private bool _tuesdayChecked;
    private bool _wednesdayChecked;
    private bool _thursdayChecked;
    private bool _fridayChecked;

    public EditEmployeeViewModel(IApiService api)
    {
        _api = api;
    }

    public EmployeeDto Employee { get; set; } = null!;
    public LunchMenuDto LunchMenu { get; set; } = null!;
    public EmployeeLunchDto EmployeeLunch { get; set; } = null!;
    public ItemDto MondayItem { get; set; } = null!;
    public ItemDto TuesdayItem { get; set; } = null!;
    public ItemDto WednesdayItem { get; set; } = null!;
    public ItemDto ThursdayItem { get; set; } = null!;
    public ItemDto FridayItem { get; set; } = null!;

    public bool MondayChecked
    {
        get => _mondayChecked;
        set
        {
            SetProperty(ref _mondayChecked,
                        value);

            EmployeeLunch.Monday = value;
        }
    }

    public bool TuesdayChecked
    {
        get => _tuesdayChecked;
        set
        {
            SetProperty(ref _tuesdayChecked,
                        value);

            EmployeeLunch.Tuesday = value;
        }
    }

    public bool WednesdayChecked
    {
        get => _wednesdayChecked;
        set
        {
            SetProperty(ref _wednesdayChecked,
                        value);

            EmployeeLunch.Wednesday = value;
        }
    }

    public bool ThursdayChecked
    {
        get => _thursdayChecked;
        set
        {
            SetProperty(ref _thursdayChecked,
                        value);

            EmployeeLunch.Thursday = value;
        }
    }

    public bool FridayChecked
    {
        get => _fridayChecked;
        set
        {
            SetProperty(ref _fridayChecked,
                        value);

            EmployeeLunch.Friday = value;
        }
    }

    public async Task Initialize(EmployeeDto employee)
    {
        Employee = employee;
        LunchMenu = await _api.GetAsync<LunchMenuDto>("lunchmenus/current");

        try
        {
            EmployeeLunch = await _api.GetAsync<EmployeeLunchDto>("lunchmenus/employeelunch/" + Employee.EmployeeId + "/" + LunchMenu.LunchMenuId);
        }
        catch (HttpRequestException)
        {
            EmployeeLunch = new EmployeeLunchDto
            {
                EmployeeId = Employee.EmployeeId,
                LunchMenuId = LunchMenu.LunchMenuId,
                Monday = false,
                Tuesday = false,
                Wednesday = false,
                Thursday = false,
                Friday = false
            };
        }

        OnPropertyChanged(nameof(Employee));
        OnPropertyChanged(nameof(LunchMenu));
        OnPropertyChanged(nameof(EmployeeLunch));

        MondayChecked = EmployeeLunch.Monday;
        TuesdayChecked = EmployeeLunch.Tuesday;
        WednesdayChecked = EmployeeLunch.Wednesday;
        ThursdayChecked = EmployeeLunch.Thursday;
        FridayChecked = EmployeeLunch.Friday;

        MondayItem = await _api.GetAsync<ItemDto>("items/" + LunchMenu.MondayItemId);
        TuesdayItem = await _api.GetAsync<ItemDto>("items/" + LunchMenu.TuesdayItemId);
        WednesdayItem = await _api.GetAsync<ItemDto>("items/" + LunchMenu.WednesdayItemId);
        ThursdayItem = await _api.GetAsync<ItemDto>("items/" + LunchMenu.ThursdayItemId);
        FridayItem = await _api.GetAsync<ItemDto>("items/" + LunchMenu.FridayItemId);

        OnPropertyChanged(nameof(MondayItem));
        OnPropertyChanged(nameof(TuesdayItem));
        OnPropertyChanged(nameof(WednesdayItem));
        OnPropertyChanged(nameof(ThursdayItem));
        OnPropertyChanged(nameof(FridayItem));
    }
}