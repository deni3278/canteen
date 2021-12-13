using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Canteen.Dto;
using Canteen.Management.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Canteen.Management.ViewModels;

public class EmployeesViewModel : ObservableObject
{
    private readonly IApiService _api;
    private double _idColumnWidth;
    private double _firstNameColumnWidth;
    private double _lastNameColumnWidth;
    private EmployeeDto _selectedEmployee = null!;
    private bool _hasSelectedItem;

    public ObservableCollection<EmployeeDto> Employees { get; } = new();
    public IAsyncRelayCommand RefreshCommand { get; }
    public IAsyncRelayCommand EditCommand { get; } = null!;
    public IRelayCommand<double> ResizeCommand { get; }

    public EmployeeDto SelectedEmployee
    {
        get => _selectedEmployee;
        set
        {
            SetProperty(ref _selectedEmployee, value);
            HasSelectedItem = SelectedEmployee != null!;
        }
    }

    public bool HasSelectedItem
    {
        get => _hasSelectedItem;
        set => SetProperty(ref _hasSelectedItem, value);
    }

    public double IdColumnWidth
    {
        get => _idColumnWidth;
        set => SetProperty(ref _idColumnWidth, value);
    }

    public double FirstNameColumnWidth
    {
        get => _firstNameColumnWidth;
        set => SetProperty(ref _firstNameColumnWidth, value);
    }

    public double LastNameColumnWidth
    {
        get => _lastNameColumnWidth;
        set => SetProperty(ref _lastNameColumnWidth, value);
    }

    public EmployeesViewModel(IApiService api)
    {
        _api = api;

        RefreshCommand = new AsyncRelayCommand(Refresh);
        ResizeCommand = new RelayCommand<double>(Resize);
    }

    private async Task Refresh()
    {
        var employees = await _api.GetAsync<IEnumerable<EmployeeDto>>("employees");

        Employees.Clear();

        foreach (var employee in employees)
        {
            Employees.Add(employee);
        }
    }

    public async Task EditEmployee(EmployeeLunchDto employeeLunch)
    {
        await _api.PostAsync("lunchmenus/employeelunch", employeeLunch);
    }

    private void Resize(double viewWidth)
    {
        var actualWidth = viewWidth - SystemParameters.VerticalScrollBarWidth;

        const double idColumn = 0.20;
        const double firstNameColumn = 0.60;
        const double lastNameColumn = 0.20;

        IdColumnWidth = actualWidth * idColumn;
        FirstNameColumnWidth = actualWidth * firstNameColumn;
        LastNameColumnWidth = actualWidth * lastNameColumn;
    }
}