using Cyber_Espace_Entrainement.Models;

namespace Cyber_Espace_Entrainement.Services
{
    /// <summary>
    /// Service pour gérer la session de l'utilisateur connecté
    /// </summary>
    public class SessionService
    {
        // Singleton
        private static SessionService? _instance;
        public static SessionService Instance => _instance ??= new SessionService();

        // Utilisateur actuellement connecté (privé)
        private Utilisateurs? _currentUser;

        // Propriétés publiques en lecture seule
        public Utilisateurs? CurrentUser => _currentUser;
        public bool IsAuthenticated => _currentUser != null;
        public string? CurrentLogin => _currentUser?.Login;
        public UserRole? CurrentRole => _currentUser?.Role;
        public string? CurrentNom => _currentUser?.Nom;
        public string? CurrentPrenom => _currentUser?.Prenom;
        public string? CurrentEmail => _currentUser?.Email;
        public int? CurrentUserId => _currentUser?.UserId;
        public int? CurrentScore => _currentUser?.ScoreTotal;
        public string? CurrentSection => _currentUser?.Section;
        public string? CurrentMotPasse => _currentUser?.MotPasse;

        // Constructeur privé pour le singleton
        private SessionService() { }

        /// <summary>
        /// Enregistrer l'utilisateur connecté
        /// </summary>
        public void Login(Utilisateurs user)
        {
            _currentUser = user;
        }

        /// <summary>
        /// Déconnecter l'utilisateur
        /// </summary>
        public void Logout()
        {
            _currentUser = null;
        }

        /// <summary>
        /// Vérifier si l'utilisateur a un rôle spécifique
        /// </summary>
        public bool HasRole(UserRole role)
        {
            return _currentUser?.Role == role;
        }

        /// <summary>
        /// Obtenir le nom complet de l'utilisateur
        /// </summary>
        public string GetFullName()
        {
            if (_currentUser == null)
                return "Invité";

            if (!string.IsNullOrEmpty(_currentUser.Prenom) && !string.IsNullOrEmpty(_currentUser.Nom))
                return $"{_currentUser.Prenom} {_currentUser.Nom}";

            return _currentUser.Login;
        }
    }
}