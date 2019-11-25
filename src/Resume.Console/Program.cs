using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Resume.Domain.Mediators;
using Resume.Pdf;
using System;

namespace Resume.Console
{
    internal class Program
    {
        private static IServiceProvider _serviceProvider;

        private static void Main()
        {
            RegisterServices();

            var service = _serviceProvider.GetService<IProgramRunner>();
            service.Execute();

            DisposeServices();

            Environment.Exit(Environment.ExitCode);
        }

        private static void RegisterServices()
        {
            var collection = new ServiceCollection();

            collection.AddLogging(builder => { builder.AddConsole(); });

            collection.AddTransient<IProgramDescription, ProgramDescription>();
            collection.AddTransient<IProgramRunner, ProgramRunner>();
            collection.AddTransient<IResumeMediator, ResumeMediator>();
            collection.AddTransient<IResumeGenerator, ResumeGenerator>();

            _serviceProvider = collection.BuildServiceProvider();
        }

        private static void DisposeServices()
        {
            switch (_serviceProvider)
            {
                case null:
                    return;
                case IDisposable disposable:
                    disposable.Dispose();
                    break;
            }
        }
    }
}
