using System;
using System.Windows.Input;

namespace Cyber_Espace_Entrainement.Commands
{
    /// <summary>
    /// Implémentation réutilisable de ICommand.
    /// Utiliser dans les ViewModels pour exposer des commandes.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        // CommandManager.RequerySuggested déclenche CanExecuteChanged automatiquement
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // Vérifie si la commande peut s'exécuter
        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

        // Exécute l'action associée
        public void Execute(object? parameter) => _execute(parameter);
    }
}