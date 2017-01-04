using System.Windows.Input;

namespace HeboTech.Wiper
{
    public interface IAsyncCommand : ICommand
    {
        bool Running { get; }
        void RaiseCanExecuteChanged();
    }
}