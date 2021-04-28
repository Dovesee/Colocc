using Colocc.ViewModels;
using Colocc.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Reflection;
using System.Windows;

namespace Colocc
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<MessageDialog, ViewModels.MessageDialogViewModel>();
            containerRegistry.RegisterInstance<ILog>(new LogTextFileApplication(Assembly.GetExecutingAssembly().GetName().Name+
               Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version) { InfoIsEnable = true });
        }
    }
}
