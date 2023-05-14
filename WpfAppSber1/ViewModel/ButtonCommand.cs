using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfAppSber1.ViewModel
{
    class ButtonCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        private Action _action;
        public ButtonCommand(Action action)
        {
            _action = action;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
