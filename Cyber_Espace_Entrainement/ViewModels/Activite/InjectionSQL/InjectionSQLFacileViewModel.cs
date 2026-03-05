using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cyber_Espace_Entrainement.Models.InjectionSQL;
using Cyber_Espace_Entrainement.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Cyber_Espace_Entrainement.ViewModels.Activite
{
    public partial class InjectionSQLFacileViewModel : ObservableObject
    {
        // ──────────────────────────────────────────────────────────────
        // Dépendance
        // ──────────────────────────────────────────────────────────────
        private readonly InjectionSQLFacileService _service;

        // Liste des 3 utilisateurs chargés depuis la BDD (kept in memory)
        private IReadOnlyList<InjectionSQL> _loadedUsers = Array.Empty<InjectionSQL>();

        // ──────────────────────────────────────────────────────────────
        // Propriétés bindées — formulaire
        // ──────────────────────────────────────────────────────────────

        /// <summary>Login de la cible, pré-rempli et visible mais non éditable.</summary>
        [ObservableProperty]
        private string _targetLogin = string.Empty;

        /// <summary>
        /// Valeur saisie dans le champ MOT DE PASSE.
        /// Mise à jour depuis le code-behind (PasswordBox non bindable nativement).
        /// Déclenche le recalcul de SqlQuery.
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SqlQuery))]
        private string _passwordInput = string.Empty;

        /// <summary>Requête SQL reconstruite en temps réel (affichage pédagogique).</summary>
        public string SqlQuery => _service.BuildSqlQuery(TargetLogin, PasswordInput);

        // ──────────────────────────────────────────────────────────────
        // Propriétés bindées — état UI
        // ──────────────────────────────────────────────────────────────

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private Visibility _loadingErrorVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private string _loadingError = string.Empty;

        // ──────────────────────────────────────────────────────────────
        // Propriétés bindées — résultat de connexion
        // ──────────────────────────────────────────────────────────────

        [ObservableProperty]
        private bool _loginSuccess;

        [ObservableProperty]
        private string _resultIcon = string.Empty;

        [ObservableProperty]
        private string _resultMessage = string.Empty;

        [ObservableProperty]
        private Visibility _resultVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private Visibility _userInfoVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private InjectionSQL? _connectedUser;

        // ──────────────────────────────────────────────────────────────
        // Constructeur
        // ──────────────────────────────────────────────────────────────
        public InjectionSQLFacileViewModel()
        {
            _service = new InjectionSQLFacileService();
            _ = LoadAsync();
        }

        // ──────────────────────────────────────────────────────────────
        // Chargement initial : 3 utilisateurs aléatoires depuis la BDD
        // ──────────────────────────────────────────────────────────────
        private async Task LoadAsync()
        {
            IsLoading = true;
            LoadingErrorVisibility = Visibility.Collapsed;

            try
            {
                _loadedUsers = await Task.Run(() => _service.GetRandomUsers());

                if (_loadedUsers.Count == 0)
                    throw new InvalidOperationException("Aucun utilisateur trouvé pour cette activité.");

                // Sélectionne aléatoirement l'un des 3 comme cible du scénario
                var rnd = new Random();
                var target = _loadedUsers[rnd.Next(_loadedUsers.Count)];
                TargetLogin = target.Login;
            }
            catch (Exception ex)
            {
                LoadingError = $"Impossible de charger l'activité : {ex.Message}";
                LoadingErrorVisibility = Visibility.Visible;
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ──────────────────────────────────────────────────────────────
        // Commande : tentative de connexion
        // Le PasswordBox est passé en CommandParameter pour lire sa valeur.
        // ──────────────────────────────────────────────────────────────
        [RelayCommand]
        private void Login(object? parameter)
        {
            // Récupère le mot de passe saisi
            string password = parameter is PasswordBox pb ? pb.Password : string.Empty;

            // Met à jour PasswordInput pour rafraîchir SqlQuery
            PasswordInput = password;

            // Simulation (aucune requête réelle envoyée à la BDD)
            InjectionSQL? user = _service.SimulateLogin(_loadedUsers, TargetLogin, password);

            if (user is not null)
            {
                LoginSuccess = true;
                ConnectedUser = user;
                ResultIcon = "✅";
                ResultMessage = $"Accès accordé ! Vous êtes connecté en tant que {user.Prenom} {user.Nom}.";
                UserInfoVisibility = Visibility.Visible;
            }
            else
            {
                LoginSuccess = false;
                ConnectedUser = null;
                ResultIcon = "❌";
                ResultMessage = "Mot de passe incorrect. Réessayez.";
                UserInfoVisibility = Visibility.Collapsed;
            }

            ResultVisibility = Visibility.Visible;
        }
    }
}