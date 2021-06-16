using System;
using System.Windows.Input;

namespace WpfApp {
    public class RelayCommand : ICommand {
        public RelayCommand(Action method, Func<bool> canExecuteMethod = null) {
            _method = method;
            _сanExecuteMethod = canExecuteMethod;
        }

        private readonly Action _method;
        private readonly Func<bool> _сanExecuteMethod;

        public bool CanExecute(object _ = null) =>
            _сanExecuteMethod == null || _сanExecuteMethod();

        public void Execute(object parameter) {
            if (CanExecute())
                _method();
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged() {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
