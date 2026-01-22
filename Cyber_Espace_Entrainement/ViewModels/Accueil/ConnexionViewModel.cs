using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Cyber_Espace_Entrainement.Views.Accueil;
using Cyber_Espace_Entrainement.Services;


namespace Cyber_Espace_Entrainement.ViewModels.Accueil
{
    public class ConnexionViewModel : INotifyPropertyChanged
    {
        #region Properties

        private string _login;
        private readonly UserService _userService;
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
            // Initialiser le service avant toute utilisation
            _userService = new UserService();

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
            // Utiliser le résultat réel de l'authentification
            var (success, user, message) = _userService.Authentifier(Login, MotDePasse);

            if (success)
            {
                if (parameter is Window window)
                {
                    SessionService.Instance.Login(user);
                    var accueil = new AccueilWindow();
                    MessageBox.Show($"Bienvenue, {Login} !", "Connexion réussie", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Cacher la fenêtre de connexion puis afficher la fenêtre d'accueil
                    window.Hide();
                    accueil.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ouverture de la fenêtre suivante.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // Afficher le message renvoyé par le service (ou un message générique)
                MessageBox.Show(string.IsNullOrWhiteSpace(message) ? "Identifiants incorrects." : message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
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