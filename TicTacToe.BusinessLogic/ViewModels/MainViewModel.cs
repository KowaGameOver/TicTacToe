using CommunityToolkit.Mvvm.Input;
using TicTacToe.BusinessLogic.Locator;
using TicTacToe.BusinessLogic.StateHolder;
using CommunityToolkit.Mvvm.ComponentModel;
using TicTacToe.BusinessLogic.Navigation.Interface;

namespace TicTacToe.BusinessLogic.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly StateHolderService _stateHolderService;
        public RelayCommand NewGameCommand { get; }
        public RelayCommand NewGameWithBotCommand { get; }
        public MainViewModel(INavigationService navigationService,
                             StateHolderService stateHolderService)
        {
            _navigationService = navigationService;
            _stateHolderService = stateHolderService;

            NewGameCommand = new RelayCommand(OnNewGameClick);
            NewGameWithBotCommand = new RelayCommand(OnNewGameWithBotClick);
        }
        private void OnNewGameClick()
            => _navigationService.OpenWindow(WindowLocator.GameWindow);
        private void OnNewGameWithBotClick()
        {
            _stateHolderService.GameWithBot = true;
            _navigationService.OpenWindow(WindowLocator.GameWindow);
        }
    }
}
