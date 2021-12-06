using System;
using System.Net.Http.Headers;
using Canteen.Management.Services;
using Canteen.Management.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Canteen.Management;

public static class ServiceCollectionExtensions
{
    public static void AddHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient<IApiService, ApiService>(client =>
        {
            client.BaseAddress = new Uri("");
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });
    }
    
    public static void AddManagementServices(this IServiceCollection services)
    {
        services.AddScoped<IApiService, ApiService>();
    }
    
    public static void AddViewModels(this IServiceCollection services)
    {
        services.AddScoped<DashboardViewModel>();
        services.AddScoped<EmployeesViewModel>();
        services.AddScoped<ItemsViewModel>();
        services.AddTransient<MainWindowViewModel>();
        services.AddScoped<MenuViewModel>();
    }
}