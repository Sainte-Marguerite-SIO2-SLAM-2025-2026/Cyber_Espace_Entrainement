using CommunityToolkit.Mvvm.ComponentModel;
using Cyber_Espace_Entrainement.Models;
using Cyber_Espace_Entrainement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace Cyber_Espace_Entrainement.ViewModels.Admin
{
    /// <summary>
    /// ViewModel pour la page de gestion des Utilisateurs.
    /// Hérite de <see cref="AdminContenuViewModel"/> et ajoute la logique spécifique :
    /// - Champs lecture seule (Id, DateCreation, DerniereConnexion)
    /// - ComboBox pour Section et Role
    /// - Gestion du mot de passe haché (champ vide = conserver l'ancien)
    /// - Auto-increment de l'ID géré côté base de données
    /// </summary>
    public partial class AdminUtilisateursViewModel : AdminContenuViewModel
    {
        private readonly UserService _userService;

        /// <summary>
        /// Nom de la propriété clé primaire dans le modèle Utilisateurs.
        /// Doit correspondre exactement au nom de la propriété C# (pas le nom SQL).
        /// </summary>
        protected override string NomColonnePrimaire => "UserId";

        // ====================================================================
        // PROPRIÉTÉS — Listes pour les ComboBox
        // ====================================================================

        /// <summary>
        /// Liste des sections disponibles, affichée dans la ComboBox "Section".
        /// Ces valeurs sont fixes et correspondent aux promotions du lycée.
        /// </summary>
        public ObservableCollection<string> Sections { get; } = new()
        {
            "SIO1 - sans spécialité",
            "SIO1 - SLAM",
            "SIO1 - SISR",
            "SIO2 - SLAM",
            "SIO2 - SISR"
        };

        /// <summary>
        /// Liste des rôles disponibles, affichée dans la ComboBox "Role".
        /// Correspond aux valeurs de l'enum <see cref="UserRole"/>.
        /// </summary>
        public ObservableCollection<string> Roles { get; } = new()
        {
            "Etudiant",
            "Professeur",
            "Admin"
        };

        // ====================================================================
        // PROPRIÉTÉS — Champs du formulaire utilisateur
        // Ces propriétés remplacent le formulaire dynamique générique.
        // Elles permettent un binding précis (lecture seule, ComboBox, etc.)
        // ====================================================================

        [ObservableProperty] private string _login = string.Empty;
        [ObservableProperty] private string _email = string.Empty;
        [ObservableProperty] private string _nom = string.Empty;
        [ObservableProperty] private string _prenom = string.Empty;

        /// <summary>
        /// Section sélectionnée dans la ComboBox.
        /// Doit être une valeur présente dans <see cref="Sections"/>.
        /// </summary>
        [ObservableProperty]
        private string? _sectionSelectionnee;

        /// <summary>
        /// Rôle sélectionné dans la ComboBox.
        /// Doit être une valeur présente dans <see cref="Roles"/>.
        /// </summary>
        [ObservableProperty]
        private string? _roleSelectionne;

        /// <summary>
        /// Champ mot de passe.
        /// Toujours vide à l'affichage (mot de passe haché non lisible).
        /// Si vide lors de la sauvegarde → l'ancien mot de passe est conservé.
        /// Note : la vue doit gérer la PasswordBox en code-behind car WPF
        /// ne permet pas de binder directement une PasswordBox (sécurité).
        /// </summary>
        [ObservableProperty]
        private string _motPasse = string.Empty;

        // Champs en lecture seule — affichés mais non modifiables
        [ObservableProperty] private string _idAffiche = string.Empty;
        [ObservableProperty] private string _dateCreationAffichee = string.Empty;
        [ObservableProperty] private string _derniereConnexionAffichee = string.Empty;

        // ====================================================================
        // CONSTRUCTEUR
        // ====================================================================

        public AdminUtilisateursViewModel()
        {
            _userService = new UserService();

            // Chargement initial des utilisateurs au démarrage de la page
            ChargerDonnees();
        }

        // ====================================================================
        // CHARGEMENT
        // ====================================================================

        /// <summary>
        /// Récupère tous les utilisateurs via le service et les affiche dans le DataGrid.
        /// Le formulaire utilise ses propres propriétés (pas les champs dynamiques).
        /// </summary>
        protected override void ChargerDonnees()
        {
            var liste = _userService.GetAllUsers();
            Donnees = ConvertirEnDataTable(liste);

            // On n'appelle PAS InitialiserChamps() ici car le formulaire
            // est statique (propriétés dédiées, pas des ChampFormulaire dynamiques)

            AfficherStatut($"✅ {Donnees.Rows.Count} utilisateur(s) chargé(s).", estSucces: true);
        }

        // ====================================================================
        // CHARGEMENT DU FORMULAIRE LORS DE LA SÉLECTION D'UNE LIGNE
        // ====================================================================

        /// <summary>
        /// Surcharge du chargement pour alimenter les propriétés spécialisées du formulaire.
        /// - Champs lecture seule : Id, DateCreation, DerniereConnexion
        /// - ComboBox : Section, Role
        /// - MotPasse : toujours vide (mot de passe haché, non affiché)
        /// </summary>
        protected override void ChargerLigneDansFormulaire(DataRowView ligne)
        {
            IsEditMode = true;
            IdEnCours = Convert.ToInt32(ligne[NomColonnePrimaire]);

            // --- Champs lecture seule ---
            IdAffiche = ligne["UserId"]?.ToString() ?? string.Empty;
            DateCreationAffichee = ligne["DateCreation"]?.ToString() ?? string.Empty;
            DerniereConnexionAffichee = ligne["DerniereConnexion"]?.ToString() ?? string.Empty;

            // --- Champs modifiables ---
            Login = ligne["Login"]?.ToString() ?? string.Empty;
            Email = ligne["Email"]?.ToString() ?? string.Empty;
            Nom = ligne["Nom"]?.ToString() ?? string.Empty;
            Prenom = ligne["Prenom"]?.ToString() ?? string.Empty;

            // --- ComboBox Section ---
            // On cherche la valeur correspondante dans la liste (insensible à la casse)
            var sectionBDD = ligne["Section"]?.ToString() ?? string.Empty;
            SectionSelectionnee = Sections.Contains(sectionBDD) ? sectionBDD : null;

            // --- ComboBox Role ---
            var roleBDD = ligne["Role"]?.ToString() ?? string.Empty;
            RoleSelectionne = Roles.Contains(roleBDD) ? roleBDD : null;

            // --- Mot de passe toujours vide ---
            // Le mot de passe est stocké haché en BDD, on ne l'affiche JAMAIS
            MotPasse = string.Empty;

            // Déclenche FormCleared pour que la vue vide aussi la PasswordBox si elle en utilise une
            // (non nécessaire ici car MotPasse est déjà vidé, mais c'est une bonne pratique)
        }

        // ====================================================================
        // SAUVEGARDE
        // ====================================================================

        /// <summary>
        /// Construit un objet <see cref="Utilisateurs"/> depuis les propriétés du formulaire,
        /// puis appelle AddUser (mode ajout) ou UpdateUser (mode modification).
        /// 
        /// Règles appliquées :
        /// - L'ID n'est jamais envoyé en ajout (auto-increment BDD).
        /// - Le mot de passe n'est mis à jour que s'il est renseigné.
        /// - La section et le rôle proviennent des ComboBox.
        /// </summary>
        /// <param name="valeurs">Non utilisé dans cette surcharge (propriétés dédiées).</param>
        protected override (bool Success, string Message) Sauvegarder(Dictionary<string, string> valeurs)
        {
            // Validation minimale côté ViewModel
            if (string.IsNullOrWhiteSpace(Login))
                return (false, "Le champ Login est obligatoire.");

            if (string.IsNullOrWhiteSpace(Email))
                return (false, "Le champ Email est obligatoire.");

            if (string.IsNullOrWhiteSpace(Nom) || string.IsNullOrWhiteSpace(Prenom))
                return (false, "Les champs Nom et Prénom sont obligatoires.");

            if (RoleSelectionne == null)
                return (false, "Veuillez sélectionner un rôle.");

            if (!IsEditMode && string.IsNullOrWhiteSpace(MotPasse))
                return (false, "Le mot de passe est obligatoire lors de la création d'un utilisateur.");

            // En mode modification, on récupère l'utilisateur existant pour préserver ses données
            // En mode ajout, on crée une instance vide
            var user = IsEditMode && IdEnCours != null
                ? _userService.GetUserById(IdEnCours.Value) ?? new Utilisateurs()
                : new Utilisateurs();

            // ----------------------------------------------------------------
            // Mapping propriétés formulaire → modèle Utilisateurs
            // ----------------------------------------------------------------
            user.Login = Login;
            user.Email = Email;
            user.Nom = Nom;
            user.Prenom = Prenom;
            user.Section = SectionSelectionnee ?? string.Empty;

            // Conversion texte → enum UserRole (ex : "Admin" → UserRole.Admin)
            if (Enum.TryParse<UserRole>(RoleSelectionne, out var role))
                user.Role = role;
            else
                return (false, $"Rôle invalide : {RoleSelectionne}");

            // Mot de passe : on ne transmet la valeur que si elle est saisie
            // Si vide → le UserService conservera l'ancien mot de passe haché
            if (!string.IsNullOrWhiteSpace(MotPasse))
                user.MotPasse = MotPasse;

            return IsEditMode
                ? _userService.UpdateUser(user)
                : _userService.AddUser(user);
        }

        // ====================================================================
        // SUPPRESSION
        // ====================================================================

        /// <summary>
        /// Supprime un utilisateur via le service.
        /// Le service interdit lui-même la suppression du dernier administrateur.
        /// </summary>
        protected override (bool Success, string Message) Supprimer(int id)
        {
            return _userService.DeleteUser(id);
        }

        // ====================================================================
        // RÉINITIALISATION DU FORMULAIRE
        // ====================================================================

        /// <summary>
        /// Surcharge pour vider également les propriétés spécialisées du formulaire utilisateur.
        /// Appelée après une sauvegarde, une suppression ou un clic sur "Annuler".
        /// </summary>
        protected new void ReinitialiserFormulaire()
        {
            base.ReinitialiserFormulaire(); // Réinitialise IsEditMode, IdEnCours, LigneSelectionnee

            // Vide les champs du formulaire utilisateur
            IdAffiche = string.Empty;
            DateCreationAffichee = string.Empty;
            DerniereConnexionAffichee = string.Empty;
            Login = string.Empty;
            Email = string.Empty;
            Nom = string.Empty;
            Prenom = string.Empty;
            SectionSelectionnee = null;
            RoleSelectionne = null;
            MotPasse = string.Empty;
        }
    }
}