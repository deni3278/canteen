using System;
using System.Windows;
using Canteen.Management.Services;
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

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        if (!await Current.Services.GetRequiredService<IApiService>().LoginAsync())
            MessageBox.Show("Cannot connect to the server.", "Canteen Management", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddApi();
        services.AddHttpClient();   // Must be registered after api
        services.AddViewModels();

        return services.BuildServiceProvider();
    }
}