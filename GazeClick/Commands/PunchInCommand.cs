// -----------------------------------------------------------
// GazeClick - App for controlling mouse cursor and emulating
// mouse clicks with gaze tracked using Tobii eye-trackers
// - command for Punch-in register button, binding in View
// (C) 2022 Michal Lech, Gdynia, Poland
// Released under GNU General Public License v3.0 (GPL-3.0)
// email: mlech.ksm@gmail.com
//-----------------------------------------------------------

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
