using GalaSoft.MvvmLight.Command;
using System;
using System.Threading.Tasks;

namespace HeboTech.Wiper
{
    public class AsyncRelayCommand : RelayCommand, IAsyncCommand
    {
        public AsyncRelayCommand(Action execute)
            : base(execute)
        { }

        public AsyncRelayCommand(Action execute, Func<bool> canExecute)
            : base(execute, canExecute)
        {
        }

        public bool Running { get; protected set; }

        public async override void Execute(object parameter)
        {
            Running = true;
            await Task.Factory.StartNew(() => base.Execute(parameter));
            Running = false;
        }
    }
}
