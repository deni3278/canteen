using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Canteen.Management.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _client;
    
    public ApiService(HttpClient client)
    {
        _client = client;

        ServicePointManager.ServerCertificateValidationCallback += (_, _, _, _) => true;
    }

    public async ValueTask<bool> LoginAsync()
    {
        var response = await _client.PostAsJsonAsync("login", new {password = ""});

        if (!response.IsSuccessStatusCode)
            return false;

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();

        if (!json.TryGetProperty("token", out var tokenProperty))
            return false;

        var token = tokenProperty.GetString();

        if (token == null)
            return false;

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return true;
    }
}