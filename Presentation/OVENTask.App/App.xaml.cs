using NLog.Web;
using System;
using System.Windows;

namespace OVENTask.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                Program.Init();

                base.OnStartup(e);

                logger.Debug("Запуск программы");
            }
            catch (Exception ex)
            {
                logger.Fatal(ex + "ошибка при запуске");
            }
        }
    }
}
