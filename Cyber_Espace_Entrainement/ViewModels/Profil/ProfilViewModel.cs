using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cyber_Espace_Entrainement.Models;
using Cyber_Espace_Entrainement.Services;
using System;
using System.Windows;
using System.Windows.Input;

namespace Cyber_Espace_Entrainement.ViewModels.Profil
{
    public partial class ProfilViewModel : ObservableObject
    {
        // MODE ÉDITION
        [ObservableProperty]
        private bool isEditMode;
        [ObservableProperty]
        private bool isCollapseMode;

        [ObservableProperty]
        private bool isNewPasswordVisible;
        [ObservableProperty]
        private bool isOldPasswordVisible;
        [ObservableProperty]
        private bool isConfirmPasswordVisible;
        public Visibility EditVisibilityModify => IsEditMode ? Visibility.Hidden : Visibility.Visible;
        public Visibility EditVisibilityGeneral => IsEditMode ? Visibility.Visible : Visibility.Hidden;

        public Visibility EditCollapse => IsCollapseMode ? Visibility.Visible : Visibility.Collapsed;

        public Visibility EditVisibilityPasswordButton =>
    (!IsEditMode && !IsCollapseMode) ? Visibility.Visible : Visibility.Hidden;

        public event Action PasswordChangedSuccessfully;



        // CHAMPS DU FORMULAIRE
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveUserCommand))]
        private string prenom;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveUserCommand))]
        private string nom;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveUserCommand))] 
        private string pseudo;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveUserCommand))] 
        private string email;

        [ObservableProperty] private string section;
        [ObservableProperty] private DateTime? dateCreation;
        [ObservableProperty] private DateTime? derniereConnection;
        [ObservableProperty] private int? scoreTotal;

        [ObservableProperty] private string messageInfos;
        [ObservableProperty] private string messageMdp;


        [ObservableProperty] private string oldPassword;
        [ObservableProperty] private string newPassword;
        [ObservableProperty] private string confirmPassword;

        private UserService _userService;



        public bool IsErrorInfos => !string.IsNullOrEmpty(MessageInfos) && (!MessageInfos.Contains("réussie"));
        public bool IsSuccessInfos => !string.IsNullOrEmpty(MessageInfos) && (MessageInfos.Contains("réussie")|| messageInfos.Contains("succès"));

        public bool IsErrorMdp => !string.IsNullOrEmpty(MessageMdp) && !MessageMdp.Contains("succès");
        public bool IsSuccessMdp => !string.IsNullOrEmpty(MessageMdp) && MessageMdp.Contains("succès");



        [RelayCommand(CanExecute = nameof(CanChangePassword))]
        private void ChangePassword()
        {
            MessageMdp = string.Empty; // Vérifications simples
            if (string.IsNullOrWhiteSpace(OldPassword) ||
                string.IsNullOrWhiteSpace(NewPassword) ||
                string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                MessageMdp = "Tous les champs doivent être remplis.";
                OnPropertyChanged(nameof(IsErrorMdp));
                return;
            }
            if (NewPassword.Length < 6)
            {
                MessageMdp = "Le nouveau mot de passe doit contenir au moins 6 caractères.";
                OnPropertyChanged(nameof(IsErrorMdp));
                return;
            }
            if (NewPassword != ConfirmPassword)
            {
                MessageMdp = "Les mots de passe ne correspondent pas.";
                OnPropertyChanged(nameof(IsErrorMdp));
                return;
            }
            if (NewPassword == OldPassword)
            {
                MessageMdp = "Le nouveau mot de passe doit être différent de l'ancien";
                OnPropertyChanged(nameof(IsErrorMdp));
                return;
            }
            _userService = new UserService();
            var (success, user, message) = _userService.Authentifier(SessionService.Instance.CurrentLogin, OldPassword);

            if (!success)
            {
                MessageMdp = "L'ancien mot de passe n'est pas bon";
                return;
            }
            SessionService.Instance.CurrentUser.MotPasse = NewPassword;
            (success, message) = _userService.UpdateUserPassword(SessionService.Instance.CurrentUser);
            if (!success)
            {
                MessageMdp = message;
                OnPropertyChanged(nameof(IsErrorMdp));
                return;
            }
            MessageInfos = "Mot de passe modifié avec succès!";
            OnPropertyChanged(nameof(IsSuccessInfos));
            PasswordChangedSuccessfully?.Invoke();
            ChargerDepuisSession();
        }

        [RelayCommand]
        private void ToggleNewPasswordVisibility()
        {
            IsNewPasswordVisible = !IsNewPasswordVisible;
        }

        [RelayCommand]
        private void ToggleOldPasswordVisibility()
        {
            IsOldPasswordVisible = !IsOldPasswordVisible;
        }

        [RelayCommand]
        private void ToggleConfirmPasswordVisibility()
        {
            IsConfirmPasswordVisible = !IsConfirmPasswordVisible;
        }

        private bool CanChangePassword()
        {
            // Le bouton n'est actif que si on est en mode édition
            // ET si Email ou Pseudo ont changé
            return !string.IsNullOrWhiteSpace(OldPassword)
                && !string.IsNullOrWhiteSpace(NewPassword)
                && !string.IsNullOrWhiteSpace(ConfirmPassword);
        }

        partial void OnOldPasswordChanged(string value)
        {
            ChangePasswordCommand.NotifyCanExecuteChanged();
        }

        partial void OnNewPasswordChanged(string value)
        {
            ChangePasswordCommand.NotifyCanExecuteChanged();
        }

        partial void OnConfirmPasswordChanged(string value)
        {
            ChangePasswordCommand.NotifyCanExecuteChanged();
        }
    

        // CONSTRUCTEUR
        public ProfilViewModel()
        {
            ChargerDepuisSession();
        }


        partial void OnIsEditModeChanged(bool value)
        {
            OnPropertyChanged(nameof(EditVisibilityModify));
            OnPropertyChanged(nameof(EditVisibilityGeneral));
            OnPropertyChanged(nameof(EditVisibilityPasswordButton));
        }

        partial void OnIsCollapseModeChanged(bool value)
        {
            OnPropertyChanged(nameof(EditCollapse));
            OnPropertyChanged(nameof(EditVisibilityPasswordButton));
        }

        partial void OnMessageInfosChanged(string value)
        {
            OnPropertyChanged(nameof(IsErrorInfos));
            OnPropertyChanged(nameof(IsSuccessInfos));
        }

        partial void OnMessageMdpChanged(string value)
        {
            OnPropertyChanged(nameof(IsErrorMdp));
            OnPropertyChanged(nameof(IsSuccessMdp));
        }


        [RelayCommand(CanExecute = nameof(CanSaveUser))]
        private void SaveUser()
        {
            var userService = new UserService();

            // 1. Récupérer l'utilisateur depuis la BDD
            var user = userService.GetUserById(SessionService.Instance.CurrentUser.UserId);

            if (user == null)
            {
                MessageInfos = "Utilisateur introuvable.";
                OnPropertyChanged(nameof(IsErrorInfos));
                return;
            }

            // 2. Modifier les champs
            user.Login = Pseudo;
            user.Email = Email;
            user.Nom = Nom;
            user.Prenom = Prenom;
            user.Section = Section;
            user.ScoreTotal = ScoreTotal;

            // 3. Sauvegarder
            var (succes, message) = userService.UpdateUser(user);

            if (!succes)
            {
                MessageInfos = message;
                OnPropertyChanged(nameof(IsErrorInfos));

                return;
            }

            // 4. Mettre à jour la session
            user = userService.GetUserById(user.UserId); // Recharger pour avoir les données à jour
            SessionService.Instance.UpdateSessionUser(user);
            ChargerDepuisSession();


            MessageInfos = "Mise à jour réussie.";
            OnPropertyChanged(nameof(IsSuccessInfos));

            IsEditMode = false;
        }



        private bool CanSaveUser()
        {
            // Le bouton n'est actif que si on est en mode édition
            // ET si Email ou Pseudo ont changé
            return IsEditMode
                && !string.IsNullOrWhiteSpace(Pseudo)
                && !string.IsNullOrWhiteSpace(Email)
                && !string.IsNullOrWhiteSpace(Prenom)
                && !string.IsNullOrWhiteSpace(Nom)
                && (Pseudo != SessionService.Instance.CurrentLogin
                    || Email != SessionService.Instance.CurrentEmail
                    || Prenom != SessionService.Instance.CurrentPrenom
                    || Nom != SessionService.Instance.CurrentNom);
        }

        // CHARGEMENT DES DONNÉES
        private void ChargerDepuisSession()
        {
            var s = SessionService.Instance;
            var u = new UserService();

            Prenom = s.CurrentPrenom;
            Nom = s.CurrentNom;
            Pseudo = s.CurrentLogin;
            Email = s.CurrentEmail;
            Section = s.CurrentSection;
            DateCreation = s.CurrentDateCrea;
            DerniereConnection = u.GetDerniereConnexionPrecedente(s.CurrentUser.UserId);
            ScoreTotal = s.CurrentScore;

            IsEditMode = false;
            IsCollapseMode = false;
            MessageMdp = String.Empty;
            IsNewPasswordVisible = false;
            IsOldPasswordVisible = false;
            IsConfirmPasswordVisible = false;


        }

        /// <summary>
        /// Préparer l'édition d'un utilisateur
        /// MODIFIÉ : Chargement des nouveaux champs Nom, Prenom, Section et ScoreTotal
        /// </summary>
        [RelayCommand]
        private void EditUser()
        {
            IsEditMode = true;
            SaveUserCommand.NotifyCanExecuteChanged();
            MessageInfos = "";
        }

        [RelayCommand]
        private void EditPassword()
        {
            IsCollapseMode = true;
            MessageInfos = "";
        }

        [RelayCommand]
        private void CancelEdit()
        {
            ChargerDepuisSession();
        }


    }
}
