using Microsoft.EntityFrameworkCore;
using Cyber_Espace_Entrainement.Data;
using Cyber_Espace_Entrainement.Models;

namespace Cyber_Espace_Entrainement.Services
{
    /// <summary>
    /// Service pour gérer les opérations sur les utilisateurs
    /// (un peu Comme les Models en CI4)
    /// MODIFICATION : Ajout de la gestion des nouveaux champs (Nom, Prenom, Section, ScoreTotal)
    /// </summary>
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService()
        {
            _context = new AppDbContext();
            // S'assurer que la base existe
            _context.Database.EnsureCreated();
        }

        //
        // CRUD 
        // 

        /// <summary>
        /// Récupérer tous les utilisateurs - INCHANGÉE
        /// </summary>
        public List<Utilisateurs> GetAllUsers()
        {
            return _context.Users.OrderBy(u => u.Login).ToList();
        }

        /// <summary>
        /// Récupérer un utilisateur par ID - INCHANGÉE
        /// </summary>
        public Utilisateurs? GetUserById(int userId)
        {
            return _context.Users.Find(userId);
        }

        /// <summary>
        /// Récupérer un utilisateur par login - INCHANGÉE
        /// </summary>
        public Utilisateurs? GetUserByLogin(string login)
        {
            return _context.Users.FirstOrDefault(u => u.Login == login);
        }

        /// <summary>
        /// Ajouter un nouvel utilisateur - INCHANGÉE
        /// Les nouveaux champs (Nom, Prenom, Section, ScoreTotal) sont gérés automatiquement
        /// car ils sont passés dans l'objet User
        /// </summary>
        public (bool Success, string Message) AddUser(Utilisateurs user)
        {
            try
            {
                // Vérifier si le login existe déjà
                if (_context.Users.Any(u => u.Login == user.Login))
                {
                    return (false, "Ce login existe déjà.");
                }

                // Vérifier si l'email existe déjà
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    return (false, "Cet email existe déjà.");
                }

                // Hasher le mot de passe (à complexifier avec Bcrypt plus tard)
                user.MotPasse = HashPassword(user.MotPasse);
                user.DateCreation = DateTime.Now;
                user.ScoreTotal = 0; // Initialiser le score à 0

                // REMARQUE : Les champs Nom, Prenom, Section, ScoreTotal sont déjà 
                // présents dans l'objet user et seront sauvegardés automatiquement
                _context.Users.Add(user);
                _context.SaveChanges();

                return (true, $"Utilisateur '{user.Login}' créé avec succès.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur : {ex.Message}");
            }
        }

        /// <summary>
        /// Modifier un utilisateur existant
        /// MODIFIÉ : Ajout de la mise à jour des nouveaux champs
        /// </summary>
        public (bool Success, string Message) UpdateUser(Utilisateurs user)
        {
            try
            {
                var existingUser = _context.Users.Find(user.UserId);
                if (existingUser == null)
                {
                    return (false, "Utilisateur introuvable.");
                }

                // Vérifier unicité login (sauf pour lui-même !)
                if (_context.Users.Any(u => u.Login == user.Login && u.UserId != user.UserId))
                {
                    return (false, "Ce login est déjà utilisé.");
                }

                // Vérifier unicité email (sauf pour lui-même !)
                if (_context.Users.Any(u => u.Email == user.Email && u.UserId != user.UserId))
                {
                    return (false, "Cet email est déjà utilisé.");
                }

                // Mise à jour des champs existants
                existingUser.Login = user.Login;
                existingUser.Email = user.Email;
                existingUser.Role = user.Role;

                // AJOUTÉ : Mise à jour des nouveaux champs
                existingUser.Nom = user.Nom;
                existingUser.Prenom = user.Prenom;
                existingUser.Section = user.Section;
                existingUser.ScoreTotal = user.ScoreTotal;

                // Ne modifier le mot de passe que s'il a changé
                if (!string.IsNullOrEmpty(user.MotPasse) && user.MotPasse != existingUser.MotPasse)
                {
                    existingUser.MotPasse = HashPassword(user.MotPasse);
                }

                _context.SaveChanges();

                return (true, $"Utilisateur '{user.Login}' modifié avec succès.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur : {ex.Message}");
            }
        }

        public (bool Success, string Message) UpdateUserPassword(Utilisateurs user)
        {
            try
            {
                var existingUser = _context.Users.Find(user.UserId);
                if (existingUser == null)
                {
                    return (false, "Utilisateur introuvable.");
                }

                    existingUser.MotPasse = HashPassword(user.MotPasse);

                _context.SaveChanges();

                return (true, $"Mot de passe de l'utilisateur '{user.Login}' modifié avec succès.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur : {ex.Message}");
            }
        }

        /// <summary>
        /// Supprimer un utilisateur - INCHANGÉE
        /// </summary>
        public (bool Success, string Message) DeleteUser(int userId)
        {
            try
            {
                var user = _context.Users.Find(userId);
                if (user == null)
                {
                    return (false, "Utilisateur introuvable.");
                }

                // Empêcher la suppression du dernier admin
                if (user.Role == UserRole.Admin && _context.Users.Count(u => u.Role == UserRole.Admin) <= 1)
                {
                    return (false, "Impossible de supprimer le dernier administrateur.");
                }

                _context.Users.Remove(user);
                _context.SaveChanges();

                return (true, $"Utilisateur '{user.Login}' supprimé.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur : {ex.Message}");
            }
        }

        // 
        // MÉTHODES SPÉCIFIQUES
        // 

        /// <summary>
        /// Authentifier un utilisateur - INCHANGÉE
        /// </summary>
        public (bool Success, Utilisateurs? User, string Message) Authentifier(string login, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == login);

            if (user == null)
                return (false, null, "Login incorrect.");

            if (user.MotPasse != HashPassword(password))
                return (false, null, "Mot de passe incorrect.");

            // Ajouter une entrée dans le log
            _context.logConnexion.Add(new LogConnexion
            {
                UserId = user.UserId,
                derniereConnexion = DateTime.Now
            });

            // Mettre à jour la dernière connexion actuelle
            user.DerniereConnexion = DateTime.Now;

            _context.SaveChanges();

            return (true, user, "Connexion réussie.");
        }


        /// <summary>
        /// Rechercher des utilisateurs
        /// MODIFIÉ : Ajout de la recherche dans les champs Nom et Prenom
        /// </summary>
        public List<Utilisateurs> SearchUsers(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return _context.Users
                .Where(u => u.Login.ToLower().Contains(searchTerm) ||
                           u.Email.ToLower().Contains(searchTerm) ||
                           // AJOUTÉ : Recherche dans Nom et Prenom (en tenant compte des valeurs null)
                           (u.Nom != null && u.Nom.ToLower().Contains(searchTerm)) ||
                           (u.Prenom != null && u.Prenom.ToLower().Contains(searchTerm)))
                .OrderBy(u => u.Login)
                .ToList();
        }

        /// <summary>
        /// Filtrer par rôle - INCHANGÉE
        /// </summary>
        public List<Utilisateurs> GetUsersByRole(UserRole role)
        {
            return _context.Users
                .Where(u => u.Role == role)
                .OrderBy(u => u.Login)
                .ToList();
        }

        // AJOUTÉ : Nouvelle méthode pour filtrer par section
        /// <summary>
        /// Filtrer les utilisateurs par section
        /// </summary>
        public List<Utilisateurs> GetUsersBySection(string section)
        {
            return _context.Users
                .Where(u => u.Section == section)
                .OrderBy(u => u.Login)
                .ToList();
        }

        // AJOUTÉ : Nouvelle méthode pour obtenir le top des utilisateurs par score
        /// <summary>
        /// Obtenir les meilleurs utilisateurs par score (classement)
        /// </summary>
        public List<Utilisateurs> GetTopUsersByScore(int count = 10)
        {
            return _context.Users
                .Where(u => u.ScoreTotal.HasValue)
                .OrderByDescending(u => u.ScoreTotal)
                .Take(count)
                .ToList();
        }

        // AJOUTÉ : Nouvelle méthode pour mettre à jour le score
        /// <summary>
        /// Mettre à jour le score d'un utilisateur
        /// </summary>
        public (bool Success, string Message) UpdateScore(int userId, int pointsToAdd)
        {
            try
            {
                var user = _context.Users.Find(userId);
                if (user == null)
                {
                    return (false, "Utilisateur introuvable.");
                }

                // Initialiser le score à 0 s'il est null
                if (!user.ScoreTotal.HasValue)
                {
                    user.ScoreTotal = 0;
                }

                // Ajouter les points
                user.ScoreTotal += pointsToAdd;

                _context.SaveChanges();

                return (true, $"Score de '{user.Login}' mis à jour : {user.ScoreTotal} points.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur : {ex.Message}");
            }
        }

        /// <summary>
        /// Récupérer la dernière connexion précédente d’un utilisateur
        /// (celle AVANT la connexion actuelle)
        /// </summary>
        public DateTime? GetDerniereConnexionPrecedente(int userId)
        {
            return _context.logConnexion
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.derniereConnexion)
                .Skip(1) // on saute la plus récente
                .Select(l => l.derniereConnexion)
                .FirstOrDefault();
        }



        // 
        // UTILITAIRES
        // 

        private static string HashPassword(string password)
        {
            // Simple hashage pour le moment 
            // à revoir :  utiliser BCrypt.Net-Next 
            return Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes(password)
            );
        }

        /// <summary>
        /// Fermer la connexion (important !) - INCHANGÉE
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}