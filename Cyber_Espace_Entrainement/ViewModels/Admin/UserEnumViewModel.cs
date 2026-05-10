using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cyber_Espace_Entrainement.Models;
using Cyber_Espace_Entrainement.Models.UserEnumeration;
using Cyber_Espace_Entrainement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace Cyber_Espace_Entrainement.ViewModels.Admin
{
    public partial class UserEnumViewModel : ObservableObject
    {
        private readonly UserEnumerationService _userEnumService;

        public event Action? FormCleared;

        [ObservableProperty]
        private int id;

        [ObservableProperty]
        private int activiteId;

        [ObservableProperty]
        private bool reponse;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveUserEnumCommand))]
        private string message;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveUserEnumCommand))]
        private string libelle;

        [ObservableProperty]
        private ObservableCollection<UserEnumeration> userEnums = new();

        [ObservableProperty]
        private bool isEditMode;

        [ObservableProperty]
        private string searchText = string.Empty;

        [ObservableProperty]
        private string statusMessage = "Prêt - Aucun utilisateur sélectionné";

        [ObservableProperty]
        private string statusColor = "#2196F3";

        public UserEnumViewModel()
        {
            _userEnumService = new UserEnumerationService();
            LoadUserEnum();
        }


        private void LoadUserEnum()
        {
            var allUserEnums = _userEnumService.GetUserEnumeration();
            UserEnums.Clear();
            foreach (var userEnum in allUserEnums)
            {
                UserEnums.Add(userEnum);
            }
        }

        /// <summary>
        /// Ajouter ou modifier un utilisateur
        /// Depuis un compte admin
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanSaveUserEnum))]
        private void SaveUserEnum()
        {
            var userEnum = new UserEnumeration
            {
                Id = Id,
                ActiviteId = ActiviteId,
                Reponse = Reponse,
                Message = Message.Trim(),
                Libelle = Libelle.Trim(),

            };

            (bool success, string message) result;

            if (!isEditMode)
            {
                result = _userEnumService.AddUserEnumeration(userEnum);
            }
            else
            {
                result = _userEnumService.UpdateUserEnumeration(userEnum);
            }

            if (result.success)
            {
                ShowSuccess(result.message);
                ClearForm();
                LoadUserEnum();
            }
            else
            {
                ShowError(result.message);
            }
        }

        // Méthode de validation - INCHANGÉE
        private bool CanSaveUserEnum()
        {
            // En mode création : tout doit être rempli
            if (!IsEditMode)
            {
                bool canSave = !string.IsNullOrWhiteSpace(Message) &&
                               !string.IsNullOrWhiteSpace(Libelle);
                System.Diagnostics.Debug.WriteLine($"Mode création - CanSave: {canSave}");
                return canSave;
            }

            // En mode édition : login et email suffisent (mot de passe optionnel)
            bool canEdit = (!string.IsNullOrWhiteSpace(Message) &&
                           !string.IsNullOrWhiteSpace(Libelle));
            System.Diagnostics.Debug.WriteLine($"Mode édition - CanEdit: {canEdit}");
            return canEdit;
        }

        /// <summary>
        /// Préparer l'édition d'un utilisateur
        /// MODIFIÉ : Chargement des nouveaux champs Nom, Prenom, Section et ScoreTotal
        /// </summary>
        [RelayCommand]
        private void EditUserEnumeration(UserEnumeration? userEnum)
        {
            System.Diagnostics.Debug.WriteLine($"EditUserEnumeration appelé avec userEnum: {userEnum?.Id ?? 0}");

            if (userEnum == null) return;

            Id = userEnum.Id;
            ActiviteId = userEnum.ActiviteId;
            Reponse = userEnum.Reponse;
            Message = userEnum.Message ?? string.Empty;
            Libelle = userEnum.Libelle ?? string.Empty;

            IsEditMode = true;
            System.Diagnostics.Debug.WriteLine($"IsEditMode passé à: {IsEditMode}");

            StatusMessage = $"Mode édition : '{userEnum.Id}' - Modifiez les champs puis cliquez sur Enregistrer";
            StatusColor = "#FF9800";
        }

        /// <summary>
        /// Supprimer un utilisateur
        /// </summary>
        [RelayCommand]
        private void DeleteUserEnumeration(UserEnumeration? userEnum)
        {
            if (userEnum == null) return;

            var result = MessageBox.Show(
                $"Voulez-vous vraiment supprimer l'user enum n° '{userEnum.Id}' ?\n\nCette action est irréversible.",
                "Confirmation de suppression",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.Yes)
            {
                var deleteResult = _userEnumService.DeleteUserEnumeration(userEnum.Id);

                if (deleteResult.Success)
                {
                    ShowSuccess(deleteResult.Message);
                    LoadUserEnum();
                }
                else
                {
                    ShowError(deleteResult.Message);
                }
            }
        }

        /// <summary>
        /// Annuler l'édition - INCHANGÉE
        /// </summary>
        [RelayCommand]
        private void CancelEdit()
        {
            ClearForm();
            StatusMessage = "Édition annulée - Formulaire réinitialisé";
            StatusColor = "#2196F3";
        }

        /// <summary>
        /// Rechercher des utilisateurs
        /// </summary>
        [RelayCommand]
        private void Search()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                LoadUserEnum();
                StatusMessage = "Recherche effacée - Tous les user enums affichés";
            }
            else
            {
                var results = _userEnumService.SearchUserEnum(SearchText);
                UserEnums.Clear();
                foreach (var user in results)
                {
                    UserEnums.Add(user);
                }
                StatusMessage = $"Recherche '{SearchText}' : {results.Count} résultat(s) trouvé(s)";
                StatusColor = "#2196F3";
            }
        }

        /// <summary>
        /// Réinitialiser les filtres
        /// </summary>
        [RelayCommand]
        private void ResetFilters()
        {
            SearchText = string.Empty;
            LoadUserEnum();
            StatusMessage = "Filtres réinitialisés - Tous les utilisateurs affichés";
            StatusColor = "#2196F3";
        }



        // Méthodes d'affichage des messages - INCHANGÉES
        private void ShowSuccess(string message)
        {
            StatusMessage = "✓ " + message;
            StatusColor = "#4CAF50";
        }

        private void ShowError(string message)
        {
            StatusMessage = "✗ " + message;
            StatusColor = "#f44336";
        }

        private void ClearForm()
        {
            Id = 0;
            ActiviteId = 0;
            Reponse = false;
            Message = string.Empty;
            Libelle = string.Empty;
            IsEditMode = false;
            FormCleared?.Invoke();
        }
    }
}
