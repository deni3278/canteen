using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Canteen.Management;

public class AuthenticationDelegatingHandler : DelegatingHandler
{
    public static string Token { get; set; } = "";
    
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);

        return await base.SendAsync(request, cancellationToken);
    }
}