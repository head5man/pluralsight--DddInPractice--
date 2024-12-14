using System;
using System.Windows.Input;

namespace DddInPractice.UI.Common
{
    public class Command<T> : ICommand
    {
        private readonly Func<T, bool> canExecute;
        private readonly Action<T> execute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        public Command(Action<T> execute)
            : this(execute, _ => true)
        {
        }


        public Command(Action<T> execute, Func<T, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }


        public bool CanExecute(object parameter)
        {
            if (parameter == null && typeof(T).IsValueType)
                return false;

            return canExecute((T)parameter);
        }


        public void Execute(object parameter)
        {
            execute((T)parameter);
        }
    }


    public class Command : Command<object>
    {
        public Command(Action execute, Func<bool> canExecute)
            : base(_ => execute(), _ => canExecute())
        {
        }


        public Command(Action execute)
            : this(execute, () => true)
        {
        }
    }
}
