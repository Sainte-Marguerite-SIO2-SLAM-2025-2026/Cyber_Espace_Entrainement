using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Cyber_Espace_Entrainement.ViewModels.Accueil
{
    public class ConnexionViewModel : INotifyPropertyChanged
    {
        #region Properties

        private string _login;
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        private string _motDePasse;
        public string MotDePasse
        {
            get => _motDePasse;
            set
            {
                _motDePasse = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand ConnexionCommand { get; }
        public ICommand QuitterCommand { get; }

        #endregion

        #region Constructor

        public ConnexionViewModel()
        {
            ConnexionCommand = new RelayCommand(ExecuteConnexion, CanExecuteConnexion);
            QuitterCommand = new RelayCommand(ExecuteQuitter);
        }

        #endregion

        #region Command Methods

        private bool CanExecuteConnexion(object parameter)
        {
            // Le bouton ne s'active que si les deux champs sont remplis
            return !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(MotDePasse);
        }

        private void ExecuteConnexion(object parameter)
        {
            // Simulation de connexion
            if (Login == "admin" && MotDePasse == "1234")
            {
                MessageBox.Show($"Bienvenue, {Login} !", "Connexion réussie", MessageBoxButton.OK, MessageBoxImage.Information);
                // Ici, vous pourriez ouvrir la fenêtre suivante
            }
            else
            {
                MessageBox.Show("Identifiants incorrects.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteQuitter(object parameter)
        {
            var result = MessageBox.Show(
                "Voulez-vous vraiment quitter l'application ?",
                "Quitter",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}