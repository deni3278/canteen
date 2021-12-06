using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace Canteen.Management;

public partial class App : Application
{
    public new static App Current => (App) Application.Current;
    
    public IServiceProvider Services { get; }
    
    public App()
    {
        InitializeComponent();

        Services = ConfigureServices();
    }
    
    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddHttpClient();
        services.AddManagementServices();
        services.AddViewModels();

        return services.BuildServiceProvider();
    }
}