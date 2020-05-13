using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Kusto.Data;
using Kusto.Data.Common;
using Kusto.Data.Net.Client;
using CseSample.Services;

[assembly: FunctionsStartup(typeof(CseSample.Startup))]
namespace CseSample
{
    public class Startup : FunctionsStartup
    {
        public IConfiguration _configuration;
        public Startup()
        {
            var configurationBuilder = new ConfigurationBuilder().AddEnvironmentVariables();
            _configuration = configurationBuilder.Build();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            // For handling connection well, we utilize Singleton life time
            // https://github.com/Azure/azure-functions-host/wiki/Managing-Connections
            builder.Services.AddSingleton((s) =>
            {
                return CreateKustoQueryClient();
            });
            builder.Services.AddScoped<ITestTableService,TestTableService>();
        }

        private ICslQueryProvider CreateKustoQueryClient()
        {
            // client code should create an object per connection string and hold on to it for as long as necessary
            // Reference https://docs.microsoft.com/en-us/azure/data-explorer/kusto/api/netfx/about-kusto-data

            string kustoUri = _configuration.GetValue<string>("KustoUri");
            string clientId = _configuration.GetValue<string>("ClientId");
            string clientSecret = _configuration.GetValue<string>("ClientSecret");
            string tenantId = _configuration.GetValue<string>("TenantId");

            var kustoConnectionStringBuilder = new KustoConnectionStringBuilder(kustoUri)
            {
                FederatedSecurity = true,
                ApplicationClientId = clientId,
                ApplicationKey = clientSecret,
                Authority = tenantId
            };
            return KustoClientFactory.CreateCslQueryProvider(kustoConnectionStringBuilder);
        }
    }
}