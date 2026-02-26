using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Windows;
using TestSmsWpfApp.Services;
using TestSmsWpfApp.ViewModels;

namespace TestSmsWpfApp
{
    public partial class App : Application
    {
        private IHost _host = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(cfg =>
                {
                    cfg.AddJsonFile("appsettings.json", false);
                })
                .ConfigureServices((ctx, services) =>
                {
                    services.AddSingleton<IEnvironmentService, EnvironmentService>();
                    services.AddSingleton<MainViewModel>();
                    services.AddSingleton<MainWindow>();
                })
                .UseSerilog((ctx, cfg) =>
                {
                    cfg.WriteTo.File(
                        "test-sms-wpf-app-.log",
                        rollingInterval: RollingInterval.Day);
                })
                .Build();

            var window = _host.Services.GetRequiredService<MainWindow>();
            window.Show();
        }
    }

}
