using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Cyber_Espace_Entrainement.Views.Accueil;
using Cyber_Espace_Entrainement.Services;
using Cyber_Espace_Entrainement.Models;
using Cyber_Espace_Entrainement.Views.Users;

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
            // Authentification
            var (success, user, message) = _userService.Authentifier(Login, MotDePasse);

            if (success)
            {
                if (parameter is Window window)
                {

                    SessionService.Instance.Login(user); // Garde en memoire l'utilisateur connecté                    

                    // afficher la fenêtre d'accueil si utilisateur ou admin si admin
                    if (user.Role == UserRole.Admin)
                    {
                        var admin = new UserGestion();
                        window.Close(); // Ferme la fenêtre de connexion
                        admin.Show();
                    }
                    else {
                        var accueil = new AccueilWindow();
                        window.Close(); // Ferme la fenêtre de connexion
                        accueil.Show(); 
                    }
                }
                else
                {
                    MessageBoxService.ShowError("Erreur lors de l'ouverture de la fenêtre suivante.", "Erreur");
                }
            }
            else
            {
                // Afficher le message renvoyé par le service (ou un message générique)
                MessageBoxService.ShowWarning(string.IsNullOrWhiteSpace(message) ? "Identifiants incorrects." : message, "Attention");
            }
        }

        private void ExecuteQuitter(object parameter)
        {
            var result = MessageBoxService.ShowQuestion(
                "Voulez-vous vraiment quitter l'application ?",
                "Quitter"
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