﻿using System.Windows;

namespace OVENTask.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Program.Init();

            base.OnStartup(e);
        }
    }
}
