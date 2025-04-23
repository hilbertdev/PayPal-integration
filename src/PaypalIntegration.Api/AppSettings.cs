namespace PaypalIntegration.Api;

public sealed class AppSettings
{
    public PayPalSettings? PayPalSettings { get; set; }
}

public sealed record PayPalSettings
{
    public string? ClientId { get; init; }
    public string? ClientSecret { get; init; }
}