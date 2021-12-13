using System.Threading.Tasks;

namespace Canteen.Management.Services;

public interface IApiService
{
    ValueTask<bool> LoginAsync();
    Task<TDto> GetAsync<TDto>(string endpoint);

    ValueTask<string> PostAsync<T>(string endpoint,
                                   T json);
}