
namespace TicTacToe.BusinessLogic.Navigation.Interface
{
    public interface INavigationService
    {
        void OpenWindow(string windowName);
        void CloseWindow(string windowName);
        public string? CurrentWindowKey { get; }
        void CloseAllWindow();
    }
}
