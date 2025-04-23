using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;

namespace PaypalIntegration.Api;

public class PayPalSettingsProvider
{
    private readonly IAmazonSimpleSystemsManagement _ssm;

    public PayPalSettingsProvider(IAmazonSimpleSystemsManagement ssm)
    {
        _ssm = ssm;
    }

    public async Task<PayPalSettings> GetPayPalSettingsAsync()
    {
        var clientIdParam = await _ssm.GetParameterAsync(new GetParameterRequest
        {
            Name = "/paypal/client-id",
            WithDecryption = true
        });

        var clientSecretParam = await _ssm.GetParameterAsync(new GetParameterRequest
        {
            Name = "/paypal/client-secret",
            WithDecryption = true
        });

        return new PayPalSettings
        {
            ClientId = clientIdParam.Parameter.Value,
            ClientSecret = clientSecretParam.Parameter.Value
        };
    }
}
