using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HeboTech.Wiper
{
    public class AsyncCommand : IAsyncCommand

    {
        protected readonly Predicate<object> _canExecute;
        protected Func<object, Task> _asyncExecute;
        public bool Running { get; protected set; }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public AsyncCommand(Func<object, Task> execute)
            : this(execute, null)
        {
        }

        public AsyncCommand(Func<object, Task> asyncExecute, Predicate<object> canExecute)
        {
            _asyncExecute = asyncExecute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (Running)
                return false;
            if (_canExecute == null)
            {
                return true;
            }
            return _canExecute(parameter);
        }

        public async void Execute(object parameter)
        {
            Running = true;
            await ExecuteAsync(parameter);
            Running = false;
        }

        protected virtual async Task ExecuteAsync(object parameter)
        {
            await _asyncExecute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}