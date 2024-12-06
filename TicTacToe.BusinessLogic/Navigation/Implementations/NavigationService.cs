using System.Windows;
using System.Windows.Controls;
using TicTacToe.BusinessLogic.Helpers;
using TicTacToe.BusinessLogic.Navigation.Interface;
using NamedServices.Microsoft.Extensions.DependencyInjection;
using TicTacToe.BusinessLogic.Locator;

namespace TicTacToe.BusinessLogic.Navigation.Implementations
{
    public class NavigationService : INavigationService
    {
        private string? _currentWindowKey;
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, Window> _openedWindows;

        public string? CurrentWindowKey => _currentWindowKey;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _openedWindows = new Dictionary<string, Window>();
        }

        public void CloseAllWindow()
        {
            foreach (var windowState in _openedWindows.Values)
                windowState.Close();
            _openedWindows.Clear();
        }

        public void OpenWindow(string windowName)
        {
            try
            {
                if (_openedWindows.ContainsKey(windowName))
                    return;

                Window window = null;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    window = _serviceProvider.GetRequiredNamedService<Window>(windowName);
                });

                _currentWindowKey = windowName;
                OpenWindow(window);
                _openedWindows.Add(windowName, window);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void CloseWindow(string windowName)
        {
            if (_openedWindows.ContainsKey(windowName))
            {
                try
                {
                    _openedWindows[windowName].Close();
                    var res = _openedWindows.Remove(windowName);
                    _currentWindowKey = null;
                }
                catch (Exception ex)
                {
                    var res = _openedWindows.Remove(windowName);
                    _currentWindowKey = null;
                }
            }
        }

        private void OpenWindow<T>(T window)
            where T : Window
        {
            Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        window.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                    }
                });
            });
        }
    }
}
