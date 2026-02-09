using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Cyber_Espace_Entrainement.Services;
using Cyber_Espace_Entrainement.Models;
using Cyber_Espace_Entrainement.Commands;

namespace Cyber_Espace_Entrainement.ViewModels.Accueil
{
    /// <summary>
    /// ViewModel responsable de la logique d'inscription.
    /// Gère l'état du formulaire d'inscription (champs, erreurs de validation),
    /// les commandes associées et l'interaction avec le service utilisateur.
    /// Construit pour être lié à une vue WPF suivant le pattern MVVM.
    /// </summary>
    public class InscriptionViewModel : INotifyPropertyChanged
    {
        #region Properties

        // Service métier pour la gestion des utilisateurs (ajout, recherche, ...).
        private UserService _userService;

        // Propriété pour contrôler la visibilité du mot de passe dans la vue.
        //[ObservableProperty]
        //private bool isPasswordVisible;

        // Champs du formulaire d'inscription
        private string _login;
        /// <summary>
        /// Login choisi par l'utilisateur.
        /// Déclenche la validation du login à chaque modification.
        /// </summary>
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
                ValidateLogin(); // Validation immédiate pour feedback UI
            }
        }

        private string _motDePasse;
        /// <summary>
        /// Mot de passe saisi par l'utilisateur.
        /// Déclenche la validation du mot de passe à chaque modification.
        /// </summary>
        public string MotDePasse
        {
            get => _motDePasse;
            set
            {
                _motDePasse = value;
                OnPropertyChanged();
                ValidateMotDePasse();
            }
        }

        private string _nom;
        /// <summary>
        /// Nom de famille de l'utilisateur.
        /// Déclenche la validation du nom à chaque modification.
        /// </summary>
        public string Nom
        {
            get => _nom;
            set
            {
                _nom = value;
                OnPropertyChanged();
                ValidateNom();
            }
        }

        private string _prenom;
        /// <summary>
        /// Prénom de l'utilisateur.
        /// Déclenche la validation du prénom à chaque modification.
        /// </summary>
        public string Prenom
        {
            get => _prenom;
            set
            {
                _prenom = value;
                OnPropertyChanged();
                ValidatePrenom();
            }
        }

        private string _sectionSelectionnee;
        /// <summary>
        /// Section sélectionnée dans la liste déroulante (ex. "SIO1 - SLAM").
        /// Déclenche la validation de la section à chaque modification.
        /// </summary>
        public string SectionSelectionnee
        {
            get => _sectionSelectionnee;
            set
            {
                _sectionSelectionnee = value;
                OnPropertyChanged();
                ValidateSection();
            }
        }

        private string _mail;
        /// <summary>
        /// Adresse e-mail de l'utilisateur.
        /// Déclenche la validation de l'email à chaque modification.
        /// </summary>
        public string Mail
        {
            get => _mail;
            set
            {
                _mail = value;
                OnPropertyChanged();
                ValidateMail();
            }
        }

        // Propriétés contenant les messages d'erreur pour chaque champ.
        // Ces propriétés permettent l'affichage d'un retour utilisateur en temps réel.

        private string _loginError;
        /// <summary>
        /// Message d'erreur relatif au login (vide si valide).
        /// Bindable pour affichage côté UI.
        /// </summary>
        public string LoginError
        {
            get => _loginError;
            set
            {
                _loginError = value;
                OnPropertyChanged();
            }
        }

        private string _motDePasseError;
        /// <summary>
        /// Message d'erreur relatif au mot de passe (vide si valide).
        /// </summary>
        public string MotDePasseError
        {
            get => _motDePasseError;
            set
            {
                _motDePasseError = value;
                OnPropertyChanged();
            }
        }

        private string _nomError;
        /// <summary>
        /// Message d'erreur relatif au nom.
        /// </summary>
        public string NomError
        {
            get => _nomError;
            set
            {
                _nomError = value;
                OnPropertyChanged();
            }
        }

        private string _prenomError;
        /// <summary>
        /// Message d'erreur relatif au prénom.
        /// </summary>
        public string PrenomError
        {
            get => _prenomError;
            set
            {
                _prenomError = value;
                OnPropertyChanged();
            }
        }

        private string _sectionError;
        /// <summary>
        /// Message d'erreur relatif à la section sélectionnée.
        /// </summary>
        public string SectionError
        {
            get => _sectionError;
            set
            {
                _sectionError = value;
                OnPropertyChanged();
            }
        }

        private string _mailError;
        /// <summary>
        /// Message d'erreur relatif à l'email.
        /// </summary>
        public string MailError
        {
            get => _mailError;
            set
            {
                _mailError = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Collection des sections disponibles affichée dans la vue (ComboBox).
        /// ObservableCollection pour notifier automatiquement la vue en cas de modifications.
        /// </summary>
        public ObservableCollection<string> Sections { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Commande liée au bouton "Valider" qui tente d'enregistrer l'utilisateur.
        /// Le CanExecute est relié à la méthode CanValider.
        /// </summary>
        public ICommand ValiderCommand { get; }

        /// <summary>
        /// Commande liée au bouton "Quitter" qui ferme l'application après confirmation.
        /// </summary>
        public ICommand QuitterCommand { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructeur : initialise les sections proposées et les commandes.
        /// Les validations sont déclenchées par les setters des propriétés.
        /// </summary>
        public InscriptionViewModel()
        {
            // Initialisation des sections proposées dans la vue
            Sections = new ObservableCollection<string>
            {
                "SIO1 - sans spécialité",
                "SIO1 - SLAM",
                "SIO1 - SISR",
                "SIO2 - SLAM",
                "SIO2 - SISR"
            };

            // Initialisation des commandes. RelayCommand déclenche CanExecute via CommandManager.RequerySuggested.
            ValiderCommand = new RelayCommand(Valider, CanValider);
            QuitterCommand = new RelayCommand(Quitter);
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// Valide le champ Login et met à jour LoginError.
        /// Conditions :
        /// - requis
        /// - longueur minimale 3
        /// </summary>
        private void ValidateLogin()
        {
            if (string.IsNullOrWhiteSpace(Login))
            {
                LoginError = "Le login est requis";
            }
            else if (Login.Length < 3)
            {
                LoginError = "Le login doit contenir au moins 3 caractères";
            }
            else
            {
                LoginError = string.Empty;
            }
        }

        /// <summary>
        /// Valide le mot de passe.
        /// Conditions :
        /// - requis
        /// - longueur minimale 6
        /// TO DO : pour la production, envisager règles plus strictes.
        /// </summary>
        private void ValidateMotDePasse()
        {
            if (string.IsNullOrWhiteSpace(MotDePasse))
            {
                MotDePasseError = "Le mot de passe est requis";
            }
            else if (MotDePasse.Length < 6)
            {
                MotDePasseError = "Le mot de passe doit contenir au moins 6 caractères";
            }
            else
            {
                MotDePasseError = string.Empty;
            }
        }

        /// <summary>
        /// Valide le nom (requis, longueur minimale 2).
        /// </summary>
        private void ValidateNom()
        {
            if (string.IsNullOrWhiteSpace(Nom))
            {
                NomError = "Le nom est requis";
            }
            else if (Nom.Length < 2)
            {
                NomError = "Le nom doit contenir au moins 2 caractères";
            }
            else
            {
                NomError = string.Empty;
            }
        }

        /// <summary>
        /// Valide le prénom (requis, longueur minimale 2).
        /// </summary>
        private void ValidatePrenom()
        {
            if (string.IsNullOrWhiteSpace(Prenom))
            {
                PrenomError = "Le prénom est requis";
            }
            else if (Prenom.Length < 2)
            {
                PrenomError = "Le prénom doit contenir au moins 2 caractères";
            }
            else
            {
                PrenomError = string.Empty;
            }
        }

        /// <summary>
        /// Valide la sélection de section (requis).
        /// </summary>
        private void ValidateSection()
        {
            if (string.IsNullOrWhiteSpace(SectionSelectionnee))
            {
                SectionError = "La section est requise";
            }
            else
            {
                SectionError = string.Empty;
            }
        }

        /// <summary>
        /// Valide l'email via une expression régulière simple.
        /// </summary>
        private void ValidateMail()
        {
            if (string.IsNullOrWhiteSpace(Mail))
            {
                MailError = "L'email est requis";
            }
            else if (!IsValidEmail(Mail))
            {
                MailError = "L'email n'est pas valide";
            }
            else
            {
                MailError = string.Empty;
            }
        }

        /// <summary>
        /// Vérifie la validité syntaxique d'une adresse e-mail (simple regex).
        /// Pour des vérifications plus avancées, envisager des règles supplémentaires.
        /// </summary>
        /// <param name="email">Adresse à valider.</param>
        /// <returns>True si l'email correspond au pattern basique ; sinon false.</returns>
        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        /// <summary>
        /// Exécute toutes les validations et renvoie true si aucune erreur n'est présente.
        /// Utile avant l'envoi des données au service.
        /// </summary>
        /// <returns>True si le formulaire est valide.</returns>
        private bool IsFormValid()
        {
            // Forcer la réévaluation de toutes les validations pour s'assurer que les messages d'erreur sont à jour.
            ValidateLogin();
            ValidateMotDePasse();
            ValidateNom();
            ValidatePrenom();
            ValidateSection();
            ValidateMail();

            return string.IsNullOrEmpty(LoginError) &&
                   string.IsNullOrEmpty(MotDePasseError) &&
                   string.IsNullOrEmpty(NomError) &&
                   string.IsNullOrEmpty(PrenomError) &&
                   string.IsNullOrEmpty(SectionError) &&
                   string.IsNullOrEmpty(MailError);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Détermine si le bouton Valider doit être activé.
        /// Condition simple : tous les champs requis doivent contenir une valeur (non vide).
        /// </summary>
        private bool CanValider(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Login) &&
                   !string.IsNullOrWhiteSpace(MotDePasse) &&
                   !string.IsNullOrWhiteSpace(Nom) &&
                   !string.IsNullOrWhiteSpace(Prenom) &&
                   !string.IsNullOrWhiteSpace(SectionSelectionnee) &&
                   !string.IsNullOrWhiteSpace(Mail);
        }

        ///// <summary>
        ///// Toggles the visibility state of the password input field.
        ///// </summary>
        ///// <remarks>Use this method to switch between showing and hiding the password. Typically invoked
        ///// by a user action, such as clicking a visibility icon in a password entry form.</remarks>
        //[RelayCommand]
        //private void TogglePasswordVisibility()
        //{
        //    IsPasswordVisible = !IsPasswordVisible;
        //}

        /// <summary>
        /// Méthode exécutée lors de la validation du formulaire.
        /// - Vérifie la validité du formulaire.
        /// - Crée un objet Utilisateurs et appelle le UserService pour l'ajouter.
        /// - Affiche un message de succès ou d'erreur via MessageBoxService.
        /// - Navigue vers la fenêtre de connexion en cas de succès.
        /// </summary>
        /// <param name="parameter">
        /// Attendu : la Window actuelle (pour la fermeture/navigation) ; fournie par le code-behind lors de l'appel.
        /// </param>
        private void Valider(object parameter)
        {
            if (IsFormValid())
            {
                // Construction de l'entité Utilisateurs à partir des champs du formulaire
                Utilisateurs nouvelUtilisateur = new Utilisateurs
                {
                    Login = this.Login,
                    MotPasse = this.MotDePasse,
                    Nom = this.Nom,
                    Prenom = this.Prenom,
                    Section = this.SectionSelectionnee,
                    Email = this.Mail,
                    Role = UserRole.Etudiant,
                    DateCreation = DateTime.Now
                };

                // Initialiser le service ici pour l'opération d'ajout
                _userService = new UserService();
                var (succes, message) = _userService.AddUser(nouvelUtilisateur);

                if (succes)
                {
                    // Feedback à l'utilisateur : succès de l'inscription
                    MessageBoxService.ShowInformation(
                        $"Inscription réussie : {Login}",
                        "Succès"
                    );
                    NaviguerVersConnexion(parameter); // Ferme la fenêtre d'inscription et ouvre la connexion
                }
                else
                {
                    // Feedback : échec et message retourné par le service
                    MessageBoxService.ShowError(
                        $"Échec de l'inscription : {message}",
                        "Erreur"
                    );
                }
            }
            else
            {
                // Si la validation a échoué, on notifie l'utilisateur
                MessageBoxService.ShowWarning(
                    "Veuillez corriger les erreurs dans le formulaire",
                    "Erreur de validation"
                );
            }
        }

        /// <summary>
        /// Effectue la navigation vers la fenêtre de connexion (MainWindow).
        /// La méthode attend la Window courante en paramètre pour pouvoir la fermer.
        /// </summary>
        /// <param name="parameter">Window actuelle (code-behind doit passer cette référence).</param>
        private void NaviguerVersConnexion(object parameter)
        {
            // La navigation dépend d'une référence à la fenêtre ; utilisée depuis le code-behind lors de l'appel.
            if (parameter is Window window)
            {
                var connexionWindow = new MainWindow();
                window.Close();
                connexionWindow.Show();
            }
        }

        /// <summary>
        /// Commande Quitter : affiche une confirmation et ferme l'application si l'utilisateur confirme.
        /// </summary>
        private void Quitter(object parameter)
        {
            var result = MessageBoxService.ShowQuestion(
                "Voulez-vous vraiment quitter l'application ?",
                "Confirmation"
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
        /// Méthode utilitaire pour déclencher PropertyChanged.
        /// Utilise [CallerMemberName] pour éviter de fournir explicitement le nom de la propriété.
        /// </summary>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}

   