using CommunityToolkit.Mvvm.ComponentModel;
using TicTacToe.BusinessLogic.StateHolder;

namespace TicTacToe.BusinessLogic.ViewModels
{
    public partial class GameEndViewModel : ObservableObject
    {
        [ObservableProperty]
        public string endText;
        private readonly StateHolderService _holderService;
        public GameEndViewModel(StateHolderService holderService)
        {
            _holderService = holderService;
            endText = _holderService.Winner;
        }
    }
}
