using CommunityToolkit.Mvvm.ComponentModel;

namespace TicTacToe.BusinessLogic.Models
{
    public partial class Cell : ObservableObject
    {
        [ObservableProperty]
        public string value;
        public int Row { get; set; }
        public int Column { get; set; }
        public Cell(int row, int column)
        {
            Row = row;
            Column = column;
            Value = string.Empty;
        }
    }
}
