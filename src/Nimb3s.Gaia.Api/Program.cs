using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Nimb3s.Gaia.Constants;
using Nimb3s.Gaia.Messages;
using Nimb3s.Gaia.Pocos.Models;
using NServiceBus;

namespace Nimb3s.Gaia.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseNServiceBus(context =>
                {
                    var endpointConfiguration = new EndpointConfiguration(typeof(ExampleModel).Assembly.GetName().Name);
                    var transport = endpointConfiguration.UseTransport<LearningTransport>();
                    transport.Routing().RouteToEndpoint(
                        assembly: typeof(ExampleMessage).Assembly,
                        @namespace: typeof(ExampleMessage).Namespace,
                        destination: GaiaConstants.MessageBus.ExampleEndpoint.ENDPOINT_NAME
                    );

                    endpointConfiguration.SendOnly();
                    endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
                        //.Settings(new JsonSerializerSettings
                        //{
                        //    TypeNameHandling = TypeNameHandling.Auto,
                        //    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full
                        //});

                    return endpointConfiguration;
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
