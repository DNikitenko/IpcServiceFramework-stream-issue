using JKang.IpcServiceFramework;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace IpcServiceFrameworkTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // starting a server
            new Thread(StartServer).Start();

            // sending requests to the server
            IpcServiceClient<IComputingService> client = new IpcServiceClientBuilder<IComputingService>()
            .UseTcp(IPAddress.Loopback, 45684)
            .Build();

            float result = await client.InvokeAsync(x => x.AddFloat(1.23f, 4.56f));
            Console.WriteLine($"AddFloat result is {result}");

            var stream = await client.InvokeAsync(x => x.GetMemoryStream());
            Console.WriteLine($"Length of stream is {stream.Length}");

            Console.ReadKey();
        }

        private static void StartServer()
        {
            // configure DI
            IServiceCollection services = ConfigureServices(new ServiceCollection());

            // build and run service host
            new IpcServiceHostBuilder(services.BuildServiceProvider())
                .AddNamedPipeEndpoint<IComputingService>(name: "endpoint1", pipeName: "pipeName")
                .AddTcpEndpoint<IComputingService>(name: "endpoint2", ipEndpoint: IPAddress.Loopback, port: 45684)
                .Build()
                .Run();
        }

        private static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddIpc(builder =>
                {
                    builder
                        .AddNamedPipe(options =>
                        {
                            options.ThreadCount = 2;
                        })
                        .AddService<IComputingService, ComputingService>();
                });
        }
    }
}
