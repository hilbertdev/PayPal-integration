namespace PaypalIntegration.Api.Services;

public interface IPayPalHttpClient
{
    Task<PayPalTokenResponse?> GetAccessToken();
}