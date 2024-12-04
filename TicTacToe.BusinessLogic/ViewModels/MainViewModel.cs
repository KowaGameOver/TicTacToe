using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using TicTacToe.BusinessLogic.Locator;
using TicTacToe.BusinessLogic.Navigation.Interface;

namespace TicTacToe.BusinessLogic.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        public RelayCommand NewGameCommand { get; }
        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            NewGameCommand = new RelayCommand(OnNewGameClick);
        }

        public void OnNewGameClick()
        {
            _navigationService.OpenWindow(WindowLocator.GameWindow);
        }
    }
}
