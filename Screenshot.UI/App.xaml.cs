using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SelectArea
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;
        public App()
        {
            _serviceProvider = new ServiceCollection()
                .AddSingleton<MainWindow>()
                .BuildServiceProvider();
        }
        
        private void OnStartup(object sender, StartupEventArgs e)
        {
            // TODO: Correctly implement DI
            _serviceProvider
                .GetService<MainWindow>()
                ?.Show();
        }
        
    }
}