using System.Windows;
using TicTacToe.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NamedServices.Microsoft.Extensions.DependencyInjection;
using TicTacToe.BusinessLogic.Locator;
using TicTacToe.BusinessLogic.Navigation.Implementations;
using TicTacToe.BusinessLogic.Navigation.Interface;
using TicTacToe.BusinessLogic.StateHolder;
using TicTacToe.BusinessLogic.ViewModels;

namespace TicTacToe
{
    public partial class App : Application
    {
        private readonly IHost _host;
        public App()
        {
            _host = new HostBuilder()
                       .ConfigureServices((context, services) =>
                       {
                           services.AddTransient<GameViewModel>();
                           services.AddTransient<GameEndViewModel>();
                           services.AddSingleton<MainViewModel>();
                           services.AddSingleton<StateHolderService>();

                           services.AddSingleton<INavigationService, NavigationService>();

                           services.AddNamedTransient<Window, MainWindow>(nameof(MainWindow), (s) => new MainWindow() { DataContext = s.GetService<MainViewModel>() });
                           services.AddNamedTransient<Window, GameWindow>(nameof(GameWindow), (s) => new GameWindow() { DataContext = s.GetService<GameViewModel>() });
                           services.AddNamedTransient<Window, GameEndWindow>(nameof(GameEndWindow), (s) => new GameEndWindow() { DataContext = s.GetService<GameEndViewModel>() });
                       }).Build();
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();
            var navigationService = _host.Services.GetService<INavigationService>();
            navigationService.OpenWindow(WindowLocator.MainWindow);
        }
    }
}
