using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TicTacToe.BusinessLogic.Models;
using TicTacToe.BusinessLogic.Locator;
using TicTacToe.BusinessLogic.StateHolder;
using CommunityToolkit.Mvvm.ComponentModel;
using TicTacToe.BusinessLogic.Navigation.Interface;

namespace TicTacToe.BusinessLogic.ViewModels
{
    public partial class GameViewModel : ObservableObject
    {
        private int moveCounter = 0;
        private string previousSymbol = "X";
        private readonly StateHolderService _holderService;
        private readonly INavigationService _navigationService;
        public RelayCommand<Cell> MakeMoveCommand { get; }
        public RelayCommand CloseCommand {  get; }
        public ObservableCollection<Cell> Board { get; } = new ObservableCollection<Cell>();
        public GameViewModel(INavigationService navigationService,
                             StateHolderService holderService)
        {
            _holderService = holderService;
            _navigationService = navigationService;

            CloseCommand = new RelayCommand(() => _navigationService.CloseWindow(WindowLocator.GameWindow));
            MakeMoveCommand = new RelayCommand<Cell>(MakeMove);
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                    Board.Add(new Cell(row, col));
            }
        }
        private async void MakeMove(Cell currentCell)
        {
            if (!string.IsNullOrEmpty(currentCell.Value))
                return;

            var currentSymbol = previousSymbol switch
            {
                "X" => "O",
                "O" => "X"
            };

            previousSymbol = currentSymbol;
            currentCell.Value = currentSymbol;

            if (IsGameEnd(currentSymbol).ended)
            {
                _navigationService.CloseWindow(WindowLocator.GameWindow);
                _navigationService.OpenWindow(WindowLocator.GameEndWindow);
            }

            if (_holderService.GameWithBot)
            {
                currentSymbol = previousSymbol switch
                {
                    "X" => "O",
                    "O" => "X"
                };

                previousSymbol = currentSymbol;

                await BotMoveAsync(currentSymbol);

                if (IsGameEnd(currentSymbol).ended)
                {
                    _navigationService.CloseWindow(WindowLocator.GameWindow);
                    _navigationService.OpenWindow(WindowLocator.GameEndWindow);
                }
            }
        }
        private (bool ended,int minMaxMetric) IsGameEnd(string symbol)
        {
            for (int row = 0; row < 3; row++)
                if (Board.Where(c => c.Row == row).All(c => c.Value == symbol))
                {
                    _holderService.Winner = $"Winner is {symbol}";
                    return (true, symbol == "X" ? 10 : -10);
                }

            for (int col = 0; col < 3; col++)
                if (Board.Where(c => c.Column == col).All(c => c.Value == symbol))
                {
                    _holderService.Winner = $"Winner is {symbol}";
                    return (true, symbol == "X" ? 10 : -10);
                }

            if (Board.Where(c => c.Row == c.Column).All(c => c.Value == symbol))
            {
                _holderService.Winner = $"Winner is {symbol}";
                return (true, symbol == "X" ? 10 : -10);
            }

            if (Board.Where(c => c.Row + c.Column == 2).All(c => c.Value == symbol))
            {
                _holderService.Winner = $"Winner is {symbol}";
                return (true, symbol == "X" ? 10 : -10);
            }

            if (Board.All(c => !string.IsNullOrEmpty(c.Value)))
            {
                _holderService.Winner = "Draw!";
                return (true, 0); 
            }

            return (false, 0); 
        }
        private async Task BotMoveAsync(string currentSymbol)
        {
            await Task.Delay(200);

            int bestScore = int.MinValue;
            Cell bestMove = null;

            foreach (var cell in Board)
            {
                if (string.IsNullOrEmpty(cell.Value))
                {
                    cell.Value = currentSymbol; 
                    int score = Minimax(Board, 0, false, currentSymbol, currentSymbol);
                    cell.Value = ""; 

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = cell;
                    }
                }
            }
            if (bestMove != null)
                bestMove.Value = currentSymbol;
        }
        private int Minimax(ObservableCollection<Cell> board, int depth, bool isMaximizing, string botSymbol, string playerSymbol)
        {
            var (ended, score) = IsGameEnd(botSymbol);
            if (ended)
                return score - depth; 

            if (isMaximizing) 
            {
                int bestScore = int.MinValue;

                foreach (var cell in board)
                {
                    if (string.IsNullOrEmpty(cell.Value))
                    {
                        cell.Value = botSymbol;
                        score = Minimax(board, depth + 1, false, botSymbol, playerSymbol);
                        cell.Value = ""; 
                        bestScore = Math.Max(bestScore, score);
                    }
                }
                return bestScore;
            }
            else 
            {
                int bestScore = int.MaxValue;

                foreach (var cell in board)
                {
                    if (string.IsNullOrEmpty(cell.Value))
                    {
                        cell.Value = playerSymbol; 
                        score = Minimax(board, depth + 1, true, botSymbol, playerSymbol);
                        cell.Value = ""; 
                        bestScore = Math.Min(bestScore, score);
                    }
                }
                return bestScore;
            }
        }
    }
}
