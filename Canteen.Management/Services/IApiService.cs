using System.Threading.Tasks;

namespace Canteen.Management.Services;

public interface IApiService
{
    ValueTask<bool> LoginAsync();
}