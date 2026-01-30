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
        private bool isVisibleMode;

        public Visibility EditVisibilityModify => IsEditMode ? Visibility.Hidden : Visibility.Visible;
        public Visibility EditVisibilityGeneral => IsEditMode ? Visibility.Visible : Visibility.Hidden;


        // CHAMPS DU FORMULAIRE
        [ObservableProperty] private string prenom;
        [ObservableProperty] private string nom;
        [ObservableProperty] private string pseudo;
        [ObservableProperty] private string email;
        [ObservableProperty] private string section;
        [ObservableProperty] private DateTime? dateCreation;
        [ObservableProperty] private DateTime? derniereConnection;
        [ObservableProperty] private int? scoreTotal;

        [ObservableProperty] private string message;


        // CONSTRUCTEUR
        public ProfilViewModel()
        {
            ChargerDepuisSession();
        }

       
        partial void OnIsEditModeChanged(bool value) { 
            OnPropertyChanged(nameof(EditVisibilityModify));
            OnPropertyChanged(nameof(EditVisibilityGeneral));
        }

        [RelayCommand(CanExecute = nameof(CanSaveUser))]
        private void SaveUser()
        {
            var userService = new UserService();

            // 1. Récupérer l'utilisateur depuis la BDD
            var user = userService.GetUserById(SessionService.Instance.CurrentUser.UserId);

            if (user == null)
            {
                Message = "Utilisateur introuvable.";
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
                Message = message;
                return;
            }

            // 4. Mettre à jour la session
            SessionService.Instance.UpdateSessionUser(user);

            Message = "Mise à jour réussie.";
            IsEditMode = false;
        }



        private bool CanSaveUser()
        {
            // Le bouton n'est actif que si on est en mode édition
            // ET si Email ou Pseudo ont changé
            return IsEditMode
                && !string.IsNullOrWhiteSpace(Pseudo)
                && !string.IsNullOrWhiteSpace(Email)
                && (Pseudo != SessionService.Instance.CurrentLogin
                    || Email != SessionService.Instance.CurrentEmail);
        }

        partial void OnPseudoChanged(string value)
        {
            SaveUserCommand.NotifyCanExecuteChanged();
        }

        partial void OnEmailChanged(string value)
        {
            SaveUserCommand.NotifyCanExecuteChanged();
        }


        // CHARGEMENT DES DONNÉES
        private void ChargerDepuisSession()
        {
            var s = SessionService.Instance;

            Prenom = s.CurrentPrenom;
            Nom = s.CurrentNom;
            Pseudo = s.CurrentLogin;
            Email = s.CurrentEmail;
            Section = s.CurrentSection;
            DateCreation = s.CurrentDateCrea;
            DerniereConnection = s.CurrentDerniereCo;
            ScoreTotal = s.CurrentScore;
            Message = "";

            IsEditMode = false;
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
            Message = "";
        }

        [RelayCommand]
        private void CancelEdit()
        {
            ChargerDepuisSession();
        }
    }
}
