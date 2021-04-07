using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlightGearTestExec
{
    class ConnectCommand : ICommand
    {
        connectionViewModel vm_connection;
        public ConnectCommand(connectionViewModel vm_connection)
        {
            this.vm_connection = vm_connection;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            this.vm_connection.connect();
        }
    }
}
