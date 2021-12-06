using System.Collections.ObjectModel;
using Canteen.Dto;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Canteen.Management.ViewModels;

public class EmployeesViewModel : ObservableObject
{
    public ObservableCollection<EmployeeDto> Employees { get; } = new();
}