using Amazon.Extensions.Configuration.SystemsManager;
using Amazon.Extensions.NETCore.Setup;
using PaypalIntegration.Api.Services;
using Amazon.SimpleSystemsManagement;
using PaypalIntegration.Api;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonSimpleSystemsManagement>();

if (!builder.Environment.IsDevelopment())
{
    builder.Configuration.AddSystemsManager(configureSource =>
    {
        // The prefix path in Parameter Store (e.g., /myapplication/dev)
        configureSource.Path = "/paypayIntegration";

        // Automatically reload configuration every 5 minutes
        configureSource.ReloadAfter = TimeSpan.FromMinutes(5);

        // Use specific AWS options (e.g., region, credentials)
        configureSource.AwsOptions = new AWSOptions
        {
            Region = Amazon.RegionEndpoint.USEast1,
        };

        // Set to true if it's okay to continue even if the parameters aren't found
        configureSource.Optional = true;

        // Handle any load exceptions (e.g., log or decide whether to reload)
        configureSource.OnLoadException += exceptionContext =>
        {
            Console.WriteLine($"SSM load exception: {exceptionContext.Exception.Message}");
            exceptionContext.Ignore = true; // Optional: ignore and continue app startup
        };

        // (Optional) Custom parameter processing if needed, e.g., remove path prefixes
        configureSource.ParameterProcessor = new DefaultParameterProcessor();
    });
}

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddHttpClient<IPayPalHttpClient, PayPalHttpClient>(config =>
{
    config.BaseAddress = new Uri("PayPalSettings:BaseUrl");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();