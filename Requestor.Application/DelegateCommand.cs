using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Requestor
{
    public class DelegateCommand : ICommand    {
        Func<object, Task> execute;
        Func<object, bool> canExecute;


        #region Constructors
        public DelegateCommand(Func<object, Task> execute, Func<object, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }


        public DelegateCommand(Func<object, Task> execute)
        {
            this.execute = execute;
            this.canExecute = this.AlwaysCanExecute;
        }
        #endregion

        #region IDelegateCommand
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }


        public bool CanExecute(object parameter)
        {
            return canExecute(parameter);
        }


        public event EventHandler CanExecuteChanged;


        public async void Execute(object parameter)
        {
            await execute(parameter);
        }
        #endregion


        bool AlwaysCanExecute(object param)
        {
            return true;
        }
    } 
}
