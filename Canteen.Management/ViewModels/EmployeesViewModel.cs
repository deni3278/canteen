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
    
    public ObservableCollection<EmployeeDto> Employees { get; } = new();
    public IAsyncRelayCommand RefreshCommand { get; }

    public EmployeesViewModel(IApiService api)
    {
        _api = api;
        
        RefreshCommand = new AsyncRelayCommand(Refresh);
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
}