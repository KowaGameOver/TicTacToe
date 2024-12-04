using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using TicTacToe.BusinessLogic.Navigation.Interface;
using TicTacToe.BusinessLogic.Locator;
using TicTacToe.BusinessLogic.StateHolder;

namespace TicTacToe.BusinessLogic.ViewModels
{
    public partial class GameViewModel : ObservableObject
    {
        private int moveCounter = 0;
        private bool gameEnd = false;
        private string previousSymbol = "X";
        private readonly StateHolderService _holderService;
        private readonly INavigationService _navigationService;
        private Dictionary<Tuple<int,int>, List<string>> buttonRowContent = new Dictionary<Tuple<int,int>, List<string>>();
        public RelayCommand<Button> MakeMoveCommand { get; }
        public GameViewModel(INavigationService navigationService,
                             StateHolderService holderService)
        {
            _holderService = holderService;
            _navigationService = navigationService;
            MakeMoveCommand = new RelayCommand<Button>(MakeMove);
        }
        public void MakeMove(Button button)
        {
            if (button.Content != null) 
                return;
            var symbol = previousSymbol switch
            {
                "X" => "O", 
                "O" => "X", 
            };
            previousSymbol = symbol;
            button.Content = new TextBlock { Text = symbol };

            string cleanName = button.Name.TrimStart('_');
            string[] parts = cleanName.Split('_');

            int row = int.Parse(parts[0]);
            int column = int.Parse(parts[1]);

            var key = Tuple.Create(row, column);

            if (buttonRowContent.ContainsKey(key))
                buttonRowContent[key].Add(symbol);
            else
                buttonRowContent[key] = new List<string> { symbol };

            gameEnd = CheckGameResult(symbol);
            if (gameEnd)
            {
                _navigationService.CloseWindow(WindowLocator.GameWindow);
                _navigationService.OpenWindow(WindowLocator.GameEndWindow);
            }
        }
        private bool CheckGameResult(string symbol)
        {
            for (int row = 0; row < 3; row++) 
            {
                if (buttonRowContent.Where(k => k.Key.Item1 == row).Count() < 3)
                    continue;
                var rowValues = buttonRowContent
                                            .Where(k => k.Key.Item1 == row)
                                            .Select(k => k.Value.First());
                if (rowValues.All(x => x == rowValues.First()))
                {
                    _holderService.Winner = "The winner is: " + symbol;
                    return true;
                }
            }

            for (int col = 0; col < 3; col++) 
            {
                if (buttonRowContent.Where(k => k.Key.Item2 == col).Count() < 3)
                    continue;
                var columnValues = buttonRowContent
                                            .Where(k => k.Key.Item2 == col)
                                            .Select(k => k.Value.First());
                if (columnValues.All(x => x == columnValues.First()))
                {
                    _holderService.Winner = "The winner is: " + symbol;
                    return true;
                }
            }

            var mainDiagonalValues = buttonRowContent
                                        .Where(k => k.Key.Item1 == k.Key.Item2)
                                        .Select(k => k.Value.First());

            if (mainDiagonalValues.Count() == 3 && mainDiagonalValues.All(x => x == mainDiagonalValues.First()))
            {
                _holderService.Winner = "The winner is: " + symbol;
                return true;
            }

            var antiDiagonalValues = buttonRowContent
                                        .Where(k => k.Key.Item1 + k.Key.Item2 == 3 - 1) 
                                        .Select(k => k.Value.First());

            if (antiDiagonalValues.Count() == 3 && antiDiagonalValues.All(x => x == antiDiagonalValues.First()))
            {
                _holderService.Winner = "The winner is: " + symbol;
                return true;
            }

            moveCounter++;
            if (moveCounter == 9)
            {
                _holderService.Winner = "Draw!:)";
                return true;
            }

            return false;
        }
    }
}
