using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cyber_Espace_Entrainement.Models;
using Cyber_Espace_Entrainement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;


namespace Cyber_Espace_Entrainement.ViewModels.Users
{
        /// <summary>
        /// ViewModel pour la gestion des utilisateurs
        /// </summary>
        public partial class UserGestionViewModel : ObservableObject
        {
            private readonly UserService _userService;
        
        // Événement pour notifier la vue que le formulaire doit être vidé
        public event Action? FormCleared;

        // 
        // PROPRIÉTÉS - Formulaire
        // 

        // ObservableProperty -> permet de 'faire le boulot' à ma place
        // j'ai utilsé ici pour éviter de faire : 
        // public int userId
        // get => _userId;
        // set { _userId = value; OnPropertyChanged(nameOf(UserId));
        [ObservableProperty]
            private int userId;

            [ObservableProperty]
            [NotifyCanExecuteChangedFor(nameof(SaveUserCommand))]
            private string login = string.Empty;

            [ObservableProperty]
            [NotifyCanExecuteChangedFor(nameof(SaveUserCommand))]
            private string motPasse = string.Empty;

            [ObservableProperty]
            [NotifyCanExecuteChangedFor(nameof(SaveUserCommand))]
            private string email = string.Empty;

            [ObservableProperty]
            private UserRole selectedRole = UserRole.Prof;

            [ObservableProperty]
            private bool isEditMode = false;

            // 
            // PROPRIÉTÉS - Données
            // 

            [ObservableProperty]
            private ObservableCollection<User> users = new();

            [ObservableProperty]
            private User? selectedUser;

            [ObservableProperty]
            private string searchText = string.Empty;

            [ObservableProperty]
            private string statusMessage = "Prêt - Aucun utilisateur sélectionné";

            [ObservableProperty]
            private string statusColor = "#2196F3";


            // 
            // CONSTRUCTEUR
            // 

            public UserGestionViewModel()
            {
                _userService = new UserService();
                LoadUsers();
            }

            // 
            // COMMANDES - CRUD
            // 

            /// <summary>
            /// Ajouter ou modifier un utilisateur
            /// </summary>
            [RelayCommand(CanExecute = nameof(CanSaveUser))]
            private void SaveUser()
            {
                var user = new User
                {
                    UserId = UserId,
                    Login = Login.Trim(),
                    MotPasse = MotPasse,
                    Email = Email.Trim(),
                    Role = SelectedRole
                };

                (bool success, string message) result;

                if (IsEditMode)
                {
                    // En mode édition, si le mot de passe est vide, on ne le modifie pas
                    if (string.IsNullOrWhiteSpace(MotPasse))
                    {
                        var existingUser = _userService.GetUserById(UserId);
                        if (existingUser != null)
                        {
                            user.MotPasse = existingUser.MotPasse; // Garder l'ancien
                        }
                    }
                    result = _userService.UpdateUser(user);
                }
                else
                {
                    result = _userService.AddUser(user);
                }

                if (result.success)
                {
                    ShowSuccess(result.message);
                    ClearForm();
                    LoadUsers();
                }
                else
                {
                    ShowError(result.message);
                }
            }

            private bool CanSaveUser()
            {
                 // En mode création : tout doit être rempli
                if (!IsEditMode)
                {
                    bool canSave = !string.IsNullOrWhiteSpace(Login) &&
                                   !string.IsNullOrWhiteSpace(Email) &&
                                   !string.IsNullOrWhiteSpace(MotPasse);
                    System.Diagnostics.Debug.WriteLine($"Mode création - CanSave: {canSave}");
                    return canSave;
                }

                // En mode édition : login et email suffisent (mot de passe optionnel)
                bool canEdit = !string.IsNullOrWhiteSpace(Login) &&
                               !string.IsNullOrWhiteSpace(Email);
                System.Diagnostics.Debug.WriteLine($"Mode édition - CanEdit: {canEdit}");
                return canEdit;
            }

            /// <summary>
            /// Préparer l'édition d'un utilisateur
            /// </summary>
            [RelayCommand]
            private void EditUser(User? user)
        {
            System.Diagnostics.Debug.WriteLine($"EditUser appelé avec user: {user?.Login ?? "NULL"}");


            if (user == null) return;

                UserId = user.UserId;
                Login = user.Login;
                Email = user.Email;
                MotPasse = string.Empty; // Ne pas afficher le mot de passe
                SelectedRole = user.Role;
                IsEditMode = true;
            System.Diagnostics.Debug.WriteLine($"IsEditMode passé à: {IsEditMode}");

            StatusMessage = $"Mode édition : '{user.Login}' - Modifiez les champs puis cliquez sur Enregistrer";
                StatusColor = "#FF9800";
            }

            /// <summary>
            /// Supprimer un utilisateur
            /// </summary>
            [RelayCommand]
            private void DeleteUser(User? user)
            {
                if (user == null) return;

                var result = MessageBox.Show(
                    $"Voulez-vous vraiment supprimer l'utilisateur '{user.Login}' ?\n\nCette action est irréversible.",
                    "Confirmation de suppression",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result == MessageBoxResult.Yes)
                {
                    var deleteResult = _userService.DeleteUser(user.UserId);

                    if (deleteResult.Success)
                    {
                        ShowSuccess(deleteResult.Message);
                        LoadUsers();
                    }
                    else
                    {
                        ShowError(deleteResult.Message);
                    }
                }
            }

            /// <summary>
            /// Annuler l'édition
            /// </summary>
            [RelayCommand]
            private void CancelEdit()
            {
                ClearForm();
                StatusMessage = "Édition annulée - Formulaire réinitialisé";
                StatusColor = "#2196F3";
            }

            // 
            // COMMANDES - Recherche et filtres
            // 

            /// <summary>
            /// Rechercher des utilisateurs
            /// </summary>
            [RelayCommand]
            private void Search()
            {
                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    LoadUsers();
                    StatusMessage = "Recherche effacée - Tous les utilisateurs affichés";
                }
                else
                {
                    var results = _userService.SearchUsers(SearchText);
                    Users.Clear();
                    foreach (var user in results)
                    {
                        Users.Add(user);
                    }
                    StatusMessage = $"Recherche '{SearchText}' : {results.Count} résultat(s) trouvé(s)";
                    StatusColor = "#2196F3";
                }
            }

            /// <summary>
            /// Filtrer par rôle
            /// </summary>
            [RelayCommand]
            private void FilterByRole(string roleStr)
            {
                if (Enum.TryParse<UserRole>(roleStr, out var role))
                {
                    var results = _userService.GetUsersByRole(role);
                    Users.Clear();
                    foreach (var user in results)
                    {
                        Users.Add(user);
                    }
                    string roleDisplay = role switch
                    {
                        UserRole.Etudiant => "Étudiants",
                        UserRole.Prof => "Professeurs",
                        UserRole.Admin => "Administrateurs",
                        _ => role.ToString()
                    };
                    StatusMessage = $"Filtre actif : {roleDisplay} ({results.Count})";
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
                LoadUsers();
                StatusMessage = "Filtres réinitialisés - Tous les utilisateurs affichés";
                StatusColor = "#2196F3";
            }

            /// <summary>
            /// Rafraîchir la liste
            /// </summary>
            [RelayCommand]
            private void Refresh()
            {
                LoadUsers();
                ShowSuccess("Liste rafraîchie avec succès");
            }

            // 
            // MÉTHODES PRIVÉES
            // 

            private void LoadUsers()
            {
                var allUsers = _userService.GetAllUsers();
                Users.Clear();
                foreach (var user in allUsers)
                {
                    Users.Add(user);
                }
            }

            private void ClearForm()
            {
                UserId = 0;
                Login = string.Empty;
                MotPasse = string.Empty;
                Email = string.Empty;
                SelectedRole = UserRole.Prof;
                IsEditMode = false;

                // Déclencher l'événement pour que la vue vide le PasswordBox
                 FormCleared?.Invoke();
        }

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
        }
    }