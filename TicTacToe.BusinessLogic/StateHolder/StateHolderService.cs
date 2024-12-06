
namespace TicTacToe.BusinessLogic.StateHolder
{
    public class StateHolderService
    {
        public string Winner { get; set; } = string.Empty;
        public bool GameWithBot { get; set; } = false;
        public void ResetStateHolderObject()
        {
            GameWithBot = false;
            Winner = string.Empty;
        }
    }
}
