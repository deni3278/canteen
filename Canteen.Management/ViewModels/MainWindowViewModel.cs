using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Canteen.Management.ViewModels;

public class MainWindowViewModel : ObservableObject
{
    private ObservableObject _currentViewModel;

    public MainWindowViewModel()
    {
        var services = App.Current.Services;

        _currentViewModel = services.GetRequiredService<DashboardViewModel>();

        DashboardCommand = new RelayCommand(() => CurrentViewModel = services.GetRequiredService<DashboardViewModel>());
        EmployeesCommand = new RelayCommand(() => CurrentViewModel = services.GetRequiredService<EmployeesViewModel>());
        ItemsCommand = new RelayCommand(() => CurrentViewModel = services.GetRequiredService<ItemsViewModel>());
        MenuCommand = new RelayCommand(() => CurrentViewModel = services.GetRequiredService<MenuViewModel>());
    }

    public ObservableObject CurrentViewModel
    {
        get => _currentViewModel;
        set =>
            SetProperty(ref _currentViewModel,
                        value);
    }

    public IRelayCommand DashboardCommand { get; }
    public IRelayCommand EmployeesCommand { get; }
    public IRelayCommand ItemsCommand { get; }
    public IRelayCommand MenuCommand { get; }
}