using CommunityToolkit.Mvvm.Input;
using TicTacToe.BusinessLogic.Locator;
using TicTacToe.BusinessLogic.StateHolder;
using CommunityToolkit.Mvvm.ComponentModel;
using TicTacToe.BusinessLogic.Navigation.Interface;

namespace TicTacToe.BusinessLogic.ViewModels
{
    public partial class GameEndViewModel : ObservableObject
    {
        [ObservableProperty]
        public string endText;
        private readonly StateHolderService _holderService;
        private readonly INavigationService _navigationService;
        public RelayCommand CloseCommand { get; }
        public RelayCommand MainMenuCommand { get; }
        public RelayCommand RepeatGameCommand { get; }
        public GameEndViewModel(StateHolderService holderService,
                                INavigationService navigationService)
        {
            _holderService = holderService;
            endText = _holderService.Winner;
            _navigationService = navigationService;

            CloseCommand = new RelayCommand(OnMainMenu);
            MainMenuCommand = new RelayCommand(OnMainMenu);
            RepeatGameCommand = new RelayCommand(OnRepeat);
        }
        private void OnMainMenu()
        {
            _navigationService.CloseWindow(WindowLocator.GameEndWindow);
            _holderService.ResetStateHolderObject();
        }
        private void OnRepeat()
        {
            _navigationService.CloseWindow(WindowLocator.GameEndWindow);
            _navigationService.OpenWindow(WindowLocator.GameWindow);
        }
    }
}
