using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Canteen.Dto;

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
        var response = await _client.PostAsJsonAsync("login", new PasswordDto {Password = ""});

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();

        if (!json.TryGetProperty("token", out var tokenProperty))
            return false;

        var token = tokenProperty.GetString();

        if (token == null)
            return false;

        AuthenticationDelegatingHandler.Token = token;

        return true;
    }

    public async Task<T> GetAsync<T>(string endpoint)
    {
        var response = await _client.GetAsync(endpoint);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<T>();

        if (json == null)
            throw new JsonException("Cannot deserialize to the specified type");

        return json;
    }

    public async ValueTask<string> PostAsync<T>(string endpoint, T json)
    {
        var response = await _client.PostAsJsonAsync(endpoint, json);

        return await response.Content.ReadAsStringAsync();
    }
}