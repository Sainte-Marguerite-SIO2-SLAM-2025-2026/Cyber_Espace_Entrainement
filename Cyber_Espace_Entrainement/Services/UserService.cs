using Microsoft.EntityFrameworkCore;
using Cyber_Espace_Entrainement.Data;
using Cyber_Espace_Entrainement.Models;

namespace Cyber_Espace_Entrainement.Services
{
        /// <summary>
        /// Service pour gérer les opérations sur les utilisateurs
        /// (un peu Comme les Models en CI4)
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
            /// Récupérer tous les utilisateurs
            /// </summary>
            public List<User> GetAllUsers()
            {
                return _context.Users.OrderBy(u => u.Login).ToList();
            }

            /// <summary>
            /// Récupérer un utilisateur par ID
            /// </summary>
            public User? GetUserById(int userId)
            {
                return _context.Users.Find(userId);
            }

            /// <summary>
            /// Récupérer un utilisateur par login
            /// </summary>
            public User? GetUserByLogin(string login)
            {
                return _context.Users.FirstOrDefault(u => u.Login == login);
            }

            /// <summary>
            /// Ajouter un nouvel utilisateur
            /// </summary>
            public (bool Success, string Message) AddUser(User user)
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
            /// </summary>
            public (bool Success, string Message) UpdateUser(User user)
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

                    // Mise à jour
                    existingUser.Login = user.Login;
                    existingUser.Email = user.Email;
                    existingUser.Role = user.Role;

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

            /// <summary>
            /// Supprimer un utilisateur
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
            /// Authentifier un utilisateur
            /// </summary>
            public (bool Success, User? User, string Message) Authentifier(string login, string password)
            {
                var user = _context.Users.FirstOrDefault(u => u.Login == login);

                if (user == null)
                {
                    return (false, null, "Login incorrect.");
                }

                if (user.MotPasse != HashPassword(password))
                {
                    return (false, null, "Mot de passe incorrect.");
                }

                // Mettre à jour la dernière connexion
                user.DerniereConnexion = DateTime.Now;
                _context.SaveChanges();

                return (true, user, "Connexion réussie.");
            }

            /// <summary>
            /// Rechercher des utilisateurs
            /// </summary>
            public List<User> SearchUsers(string searchTerm)
            {
                searchTerm = searchTerm.ToLower();
                return _context.Users
                    .Where(u => u.Login.ToLower().Contains(searchTerm) ||
                               u.Email.ToLower().Contains(searchTerm))
                    .OrderBy(u => u.Login)
                    .ToList();
            }

            /// <summary>
            /// Filtrer par rôle
            /// </summary>
            public List<User> GetUsersByRole(UserRole role)
            {
                return _context.Users
                    .Where(u => u.Role == role)
                    .OrderBy(u => u.Login)
                    .ToList();
            }

            // 
            // UTILITAIRES
            // 

            private static string HashPassword(string password)
            {
                // Simple hashagepour le moment 
                // à revoir :  utiliser BCrypt.Net-Next 
                return Convert.ToBase64String(
                    System.Text.Encoding.UTF8.GetBytes(password)
                );
            }

            /// <summary>
            /// Fermer la connexion (important !)
            /// </summary>
            public void Dispose()
            {
                _context.Dispose();
            }
        }
    }