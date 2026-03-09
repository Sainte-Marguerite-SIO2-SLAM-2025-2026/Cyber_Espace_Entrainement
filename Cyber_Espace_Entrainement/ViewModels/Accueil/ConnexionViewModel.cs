using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Cyber_Espace_Entrainement.Views.Accueil;
using Cyber_Espace_Entrainement.Services;
using Cyber_Espace_Entrainement.Models;
using Cyber_Espace_Entrainement.Views.Users;
using Cyber_Espace_Entrainement.Commands;
using Cyber_Espace_Entrainement.Views.Admin;

namespace Cyber_Espace_Entrainement.ViewModels.Accueil
{
    /// <summary>
    /// ViewModel responsable de la logique de connexion pour la vue de l'accueil.
    /// Gère les champs d'identification (login / mot de passe), les commandes associées
    /// (connexion et quitter) et l'interaction avec les services (authentification,
    /// session et boîtes de dialogue).
    /// </summary>
    public class ConnexionViewModel : INotifyPropertyChanged
    {
        #region Properties

        // Stocke la valeur saisie pour le login (nom d'utilisateur).
        private string _login;

        // Service métier gérant l'authentification et l'accès aux utilisateurs.
        // readonly car initialisé une seule fois dans le constructeur.
        private readonly UserService _userService;

        /// <summary>
        /// Login saisi par l'utilisateur.
        /// Déclenche la notification de changement pour mettre à jour la vue.
        /// </summary>
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        // Stocke la valeur saisie pour le mot de passe.
        private string _motDePasse;

        /// <summary>
        /// Mot de passe saisi par l'utilisateur.
        /// Déclenche la notification de changement pour mettre à jour la vue.
        /// </summary>
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

        /// <summary>
        /// Commande exécutée lorsque l'utilisateur tente de se connecter.
        /// Liée au bouton "Se connecter" dans la vue.
        /// </summary>
        public ICommand ConnexionCommand { get; }

        /// <summary>
        /// Commande exécutée pour quitter l'application.
        /// Liée au bouton "Quitter" dans la vue.
        /// </summary>
        public ICommand QuitterCommand { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructeur : instancie les services nécessaires et initialise les commandes.
        /// - Initialise _userService pour pouvoir appeler l'authentification.
        /// - Crée les RelayCommand pour associer l'exécution et la validation des commandes.
        /// </summary>
        public ConnexionViewModel()
        { 
            // Initialiser le service avant toute utilisation
            _userService = new UserService();

            // ConnexionCommand : exécute ExecuteConnexion si CanExecuteConnexion retourne true.
            ConnexionCommand = new RelayCommand(ExecuteConnexion, CanExecuteConnexion);

            // QuitterCommand : exécute ExecuteQuitter sans condition d'exécution.
            QuitterCommand = new RelayCommand(ExecuteQuitter);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Vérifie si la commande de connexion peut s'exécuter.
        /// Condition simple : les deux champs (Login et MotDePasse) doivent être renseignés.
        /// Ce contrôle permet d'activer/désactiver le bouton dans l'UI.
        /// </summary>
        /// <param name="parameter">Paramètre optionnel (non utilisé pour la validation).</param>
        /// <returns>True si les deux champs sont non vides ; sinon false.</returns>
        private bool CanExecuteConnexion(object parameter)
        {
            // Le bouton ne s'active que si les deux champs sont remplis
            return true; //!string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(MotDePasse);
        }

        /// <summary>
        /// Méthode exécutée lorsque l'utilisateur valide la connexion.
        /// - Appelle le service d'authentification.
        /// - En cas de succès, stocke l'utilisateur en session et ouvre la fenêtre appropriée
        ///   (interface admin ou accueil utilisateur), puis ferme la fenêtre de connexion.
        /// - En cas d'échec, affiche un message d'erreur ou d'avertissement via MessageBoxService.
        /// </summary>
        /// <param name="parameter">
        /// Attend généralement la Window courante afin de pouvoir la fermer après la connexion.
        /// Si le parameter n'est pas une Window, une erreur est affichée.
        /// </param>
        private void ExecuteConnexion(object parameter)
        {
            // Authentification via le service métier : retourne (success, user, message)
            var (success, user, message) = _userService.Authentifier(Login, MotDePasse);

            if (success)
            {
                if (parameter is Window window)
                {

                    // Sauvegarde de l'utilisateur connecté dans la session (singleton)
                    SessionService.Instance.Login(user);

                    // Selon le rôle, ouvrir la fenêtre correspondante
                    if (user.Role == UserRole.Admin)
                    {
                        var admin = new AdminAccueil();
                        window.Close(); // Ferme la fenêtre de connexion
                        admin.Show();
                    }
                    else
                    {
                        var accueil = new AccueilWindow();
                        window.Close(); // Ferme la fenêtre de connexion
                        accueil.Show();
                    }
                }
                else
                {
                    // Réception d'un paramètre inattendu : affichage d'une erreur
                    MessageBoxService.ShowError("Erreur lors de l'ouverture de la fenêtre suivante.", "Erreur");
                }
            }
            else
            {
                if (parameter is Window window)
                {
                    // TO DO : A enlever à terme
                    var accueil = new AccueilWindow();
                    window.Close(); // Ferme la fenêtre de connexion
                    accueil.Show();

                }
                //accueil.Show();
                //// Afficher le message renvoyé par le service (ou un message générique)
                //MessageBoxService.ShowWarning(
                //    string.IsNullOrWhiteSpace(message) ? "Identifiants incorrects." : message,
                //    "Attention"
                //);
            }
        }

        /// <summary>
        /// Méthode exécutée pour quitter l'application.
        /// Affiche une confirmation via MessageBoxService ; si l'utilisateur confirme,
        /// termine l'application proprement en appelant Application.Current.Shutdown().
        /// </summary>
        /// <param name="parameter">Paramètre optionnel (non utilisé).</param>
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

        /// <summary>
        /// Événement utilisé par le pattern MVVM pour notifier la vue des changements de propriété.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Méthode utilitaire pour déclencher l'événement PropertyChanged.
        /// Utilise [CallerMemberName] pour éviter de passer explicitement le nom de la propriété.
        /// </summary>
        /// <param name="propertyName">Nom de la propriété modifiée (facultatif grâce à CallerMemberName).</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}