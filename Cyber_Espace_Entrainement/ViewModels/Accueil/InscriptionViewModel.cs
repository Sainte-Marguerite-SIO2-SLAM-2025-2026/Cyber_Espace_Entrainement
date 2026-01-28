using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Cyber_Espace_Entrainement.Services;
using Cyber_Espace_Entrainement.Models;

namespace Cyber_Espace_Entrainement.ViewModels.Accueil
{
    public class InscriptionViewModel : INotifyPropertyChanged
    {
        #region Properties

        private UserService _userService;
        private string _login;
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
                ValidateLogin();
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
                ValidateMotDePasse();
            }
        }

        private string _nom;
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

        // Erreurs de validation
        private string _loginError;
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
        public string MailError
        {
            get => _mailError;
            set
            {
                _mailError = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Sections { get; set; }

        #endregion

        #region Commands

        public ICommand ValiderCommand { get; }
        public ICommand QuitterCommand { get; }

        #endregion

        #region Constructor

        public InscriptionViewModel()
        {
            // Initialisation des sections
            Sections = new ObservableCollection<string>
            {
                "SIO1 - sans spécialité",
                "SIO1 - SLAM",
                "SIO1 - SISR",
                "SIO2 - SLAM",
                "SIO2 - SISR"
            };

            // Initialisation des commandes
            ValiderCommand = new RelayCommand(Valider, CanValider);
            QuitterCommand = new RelayCommand(Quitter);
        }

        #endregion

        #region Validation Methods

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

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        private bool IsFormValid()
        {
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

        private bool CanValider(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Login) &&
                   !string.IsNullOrWhiteSpace(MotDePasse) &&
                   !string.IsNullOrWhiteSpace(Nom) &&
                   !string.IsNullOrWhiteSpace(Prenom) &&
                   !string.IsNullOrWhiteSpace(SectionSelectionnee) &&
                   !string.IsNullOrWhiteSpace(Mail);
        }

        private void Valider(object parameter)
        {
            if (IsFormValid())
            {
                MessageBoxService.ShowInformation(
                    $"Inscription réussie : {Login}",
                    "Succès"
                );


                // Création de l'objet Utilisateurs
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

                // Ajout de l'utilisateur à la base de données
                _userService = new UserService();
                _userService.AddUser(nouvelUtilisateur);

                NaviguerVersConnexion(parameter);
            }
            else
            {
                MessageBoxService.ShowWarning(
                    "Veuillez corriger les erreurs dans le formulaire",
                    "Erreur de validation"
                );
            }
        }

        private void NaviguerVersConnexion(object parameter)
        {
            // Cette méthode sera appelée depuis le code-behind
            // car la navigation nécessite une référence à la fenêtre
            if (parameter is Window window)
            {
                var connexionWindow = new MainWindow();
                window.Close();
                connexionWindow.Show();
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    #region RelayCommand

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }

    #endregion
}