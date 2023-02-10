using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using Autofac.Features.ResolveAnything;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SelectArea.Common;
using SelectArea.Helpers;

namespace SelectArea
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
        }
        
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var builder = new ContainerBuilder();
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
            
            builder.RegisterType<SettingsAPI>().As<SettingsAPI>().SingleInstance();

            ContainerManager.Container = builder.Build();
        }
        
    }
}