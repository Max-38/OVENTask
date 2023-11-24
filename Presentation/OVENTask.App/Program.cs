using DevExpress.Mvvm.POCO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OVENTask.App.Models.Interfaces;
using OVENTask.App.Models.Services;
using OVENTask.App.ViewModels;
using OVENTask.Application.Services;

namespace OVENTask.App
{
    public class Program
    {
        private static IHost host;

        public static void Init()
        {
            host = Host.CreateDefaultBuilder().ConfigureServices(services =>
            {
                services.AddSingleton<MainWindowVM>();
                services.AddTransient<FileService>();
                services.AddTransient<IDialogService, DefaultDialogService>();

            }).Build();
        }
        public MainWindowVM MainWindowVM => host.Services.GetRequiredService<MainWindowVM>();
    }
}
