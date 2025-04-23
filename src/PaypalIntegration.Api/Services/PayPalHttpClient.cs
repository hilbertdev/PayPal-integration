using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace PaypalIntegration.Api.Services;

public class PayPalHttpClient(IOptions<AppSettings> appSettings, HttpClient client) : IPayPalHttpClient
{
    private readonly PayPalSettings _payPalSettings = appSettings.Value.PayPalSettings!;

    public async Task<PayPalTokenResponse?> GetAccessToken()
    {
        try
        {
            // Set basic auth header
            var byteArray = Encoding.ASCII.GetBytes($"{_payPalSettings.ClientId}:{_payPalSettings.ClientSecret}");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            // Prepare the request
            var request = new HttpRequestMessage(HttpMethod.Post, "/v1/oauth2/token");
            request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8,
                "application/x-www-form-urlencoded");

            // Send the request
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // Read the response
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PayPalTokenResponse>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception e)
        {
            return null;
        }
    }
}