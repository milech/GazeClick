using System;
using System.Windows.Input;
using GazeClick.ViewModels;

namespace GazeClick.Commands
{
    internal class PunchInCommand : ICommand
    {
        private readonly GazeClickViewModel _gazeClickViewModel;

        public PunchInCommand(GazeClickViewModel gazeClickViewModel)
        {
            _gazeClickViewModel = gazeClickViewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _gazeClickViewModel.CanPunchIn;
        }

        public void Execute(object parameter)
        {
            _gazeClickViewModel.PunchInRegister();
        }
    }
}
