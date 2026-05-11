using Microsoft.EntityFrameworkCore;
using Cyber_Espace_Entrainement.Data;
using Cyber_Espace_Entrainement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cyber_Espace_Entrainement.Services
{
    /// <summary>
    /// Service responsable de toutes les opérations sur les <see cref="Utilisateurs"/>.
    /// 
    /// Ce service suit le principe de responsabilité unique (SRP) :
    /// il ne contient que la logique d'accès aux données et de validation métier.
    /// L'affichage et la navigation restent dans les ViewModels et les vues.
    /// 
    /// Toutes les opérations d'écriture retournent un tuple (bool Success, string Message)
    /// pour rester MVVM-friendly (pas d'exceptions remontées, messages lisibles).
    /// </summary>
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService()
        {
            _context = new AppDbContext();
            // Crée la base de données si elle n'existe pas encore
            _context.Database.EnsureCreated();
        }

        // ====================================================================
        // LECTURE
        // ====================================================================

        /// <summary>
        /// Retourne tous les utilisateurs, triés par login alphabétiquement.
        /// </summary>
        public List<Utilisateurs> GetAllUsers()
        {
            try
            {
                return _context.Users.OrderBy(u => u.Login).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[UserService] GetAllUsers : {ex.Message}");
                return new List<Utilisateurs>();
            }
        }

        /// <summary>
        /// Retourne un utilisateur par son identifiant, ou <c>null</c> s'il est introuvable.
        /// </summary>
        /// <param name="userId">Identifiant unique de l'utilisateur.</param>
        public Utilisateurs? GetUserById(int userId)
        {
            return _context.Users.Find(userId);
        }

        /// <summary>
        /// Retourne un utilisateur par son login, ou <c>null</c> s'il est introuvable.
        /// </summary>
        /// <param name="login">Login exact de l'utilisateur.</param>
        public Utilisateurs? GetUserByLogin(string login)
        {
            return _context.Users.FirstOrDefault(u => u.Login == login);
        }

        // ====================================================================
        // ÉCRITURE — Ajout
        // ====================================================================

        /// <summary>
        /// Ajoute un nouvel utilisateur en base de données.
        /// 
        /// Règles de validation appliquées :
        /// - Login unique
        /// - Email unique
        /// - Mot de passe obligatoire (doit être renseigné pour un nouvel utilisateur)
        /// - L'ID est généré automatiquement par la BDD (auto-increment)
        /// - Le score est initialisé à 0
        /// - La date de création est remplie automatiquement
        /// </summary>
        /// <param name="user">
        /// L'entité à persister.
        /// <see cref="Utilisateurs.MotPasse"/> doit contenir le mot de passe en clair ;
        /// il sera haché par cette méthode avant insertion.
        /// </param>
        /// <returns>Tuple (succès, message lisible pour l'interface).</returns>
        public (bool Success, string Message) AddUser(Utilisateurs user)
        {
            try
            {
                // --- Validation unicité ---
                if (_context.Users.Any(u => u.Login == user.Login))
                    return (false, "Ce login est déjà utilisé par un autre compte.");

                if (_context.Users.Any(u => u.Email == user.Email))
                    return (false, "Cet email est déjà associé à un autre compte.");

                // --- Validation mot de passe ---
                // En création, un mot de passe est obligatoire
                if (string.IsNullOrWhiteSpace(user.MotPasse))
                    return (false, "Le mot de passe est obligatoire pour créer un utilisateur.");

                // --- Hachage du mot de passe avant insertion ---
                user.MotPasse = HashMDP(user.MotPasse);
                user.DateCreation = DateTime.Now;
                user.ScoreTotal = 0;

                _context.Users.Add(user);
                _context.SaveChanges();

                return (true, $"Utilisateur « {user.Login} » créé avec succès.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur lors de la création : {ex.Message}");
            }
        }

        // ====================================================================
        // ÉCRITURE — Modification
        // ====================================================================

        /// <summary>
        /// Met à jour un utilisateur existant identifié par <see cref="Utilisateurs.UserId"/>.
        /// 
        /// Règles appliquées :
        /// - L'ID n'est jamais modifié (clé primaire protégée).
        /// - Login et email doivent rester uniques (sauf pour le même utilisateur).
        /// - Si <see cref="Utilisateurs.MotPasse"/> est vide ou null, l'ancien mot de passe
        ///   haché est conservé (l'admin n'est pas obligé de resaisir le mot de passe).
        /// - Si un nouveau mot de passe est fourni, il est haché avant sauvegarde.
        /// </summary>
        /// <param name="user">
        /// L'entité avec les nouvelles valeurs.
        /// Si <see cref="Utilisateurs.MotPasse"/> est vide, l'ancien est conservé.
        /// </param>
        /// <returns>Tuple (succès, message lisible pour l'interface).</returns>
        public (bool Success, string Message) UpdateUser(Utilisateurs user)
        {
            try
            {
                // Récupération de l'enregistrement existant en BDD
                var existingUser = _context.Users.Find(user.UserId);
                if (existingUser == null)
                    return (false, "Utilisateur introuvable. Il a peut-être déjà été supprimé.");

                // --- Validation unicité login (on exclut l'utilisateur lui-même) ---
                if (_context.Users.Any(u => u.Login == user.Login && u.UserId != user.UserId))
                    return (false, "Ce login est déjà utilisé par un autre compte.");

                // --- Validation unicité email (on exclut l'utilisateur lui-même) ---
                if (_context.Users.Any(u => u.Email == user.Email && u.UserId != user.UserId))
                    return (false, "Cet email est déjà utilisé par un autre compte.");

                // --- Mise à jour des champs modifiables ---
                existingUser.Login = user.Login;
                existingUser.Email = user.Email;
                existingUser.Nom = user.Nom;
                existingUser.Prenom = user.Prenom;
                existingUser.Section = user.Section;
                existingUser.Role = user.Role;

                // ScoreTotal peut être mis à jour ici si l'admin le souhaite
                if (user.ScoreTotal.HasValue)
                    existingUser.ScoreTotal = user.ScoreTotal;

                // --- Gestion du mot de passe ---
                // Si le champ est vide : on conserve l'ancien mot de passe haché
                // Si le champ est renseigné : on hache et on met à jour
                if (!string.IsNullOrWhiteSpace(user.MotPasse))
                {
                    // On évite de re-hacher un mot de passe déjà haché
                    // BCrypt.Verify retourne true si c'est le même mot de passe qu'avant
                    bool estMemeMdp = BCrypt.Net.BCrypt.Verify(user.MotPasse, existingUser.MotPasse);

                    if (!estMemeMdp)
                        existingUser.MotPasse = HashMDP(user.MotPasse);
                    // Sinon : mot de passe inchangé, on ne retouche pas la colonne
                }
                // Si MotPasse est vide ou null → on ne modifie pas existingUser.MotPasse

                _context.SaveChanges();

                return (true, $"Utilisateur « {existingUser.Login} » modifié avec succès.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur lors de la modification : {ex.Message}");
            }
        }

        /// <summary>
        /// Met à jour uniquement le mot de passe d'un utilisateur.
        /// Utile pour une page "Changer mon mot de passe" séparée.
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur.</param>
        /// <param name="nouveauMotDePasse">Nouveau mot de passe en clair.</param>
        public (bool Success, string Message) UpdateUserPassword(int userId, string nouveauMotDePasse)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nouveauMotDePasse))
                    return (false, "Le nouveau mot de passe ne peut pas être vide.");

                var existingUser = _context.Users.Find(userId);
                if (existingUser == null)
                    return (false, "Utilisateur introuvable.");

                existingUser.MotPasse = HashMDP(nouveauMotDePasse);
                _context.SaveChanges();

                return (true, $"Mot de passe de « {existingUser.Login} » mis à jour avec succès.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur lors du changement de mot de passe : {ex.Message}");
            }
        }

        // ====================================================================
        // ÉCRITURE — Suppression
        // ====================================================================

        /// <summary>
        /// Supprime définitivement un utilisateur.
        /// Interdit la suppression du dernier administrateur pour éviter de bloquer l'accès à l'application.
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur à supprimer.</param>
        public (bool Success, string Message) DeleteUser(int userId)
        {
            try
            {
                var user = _context.Users.Find(userId);
                if (user == null)
                    return (false, "Utilisateur introuvable.");

                // Garde-fou : on ne peut pas supprimer le dernier administrateur
                if (user.Role == UserRole.Admin && _context.Users.Count(u => u.Role == UserRole.Admin) <= 1)
                    return (false, "Impossible de supprimer le dernier administrateur du système.");

                _context.Users.Remove(user);
                _context.SaveChanges();

                return (true, $"Utilisateur « {user.Login} » supprimé avec succès.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur lors de la suppression : {ex.Message}");
            }
        }

        // ====================================================================
        // AUTHENTIFICATION
        // ====================================================================

        /// <summary>
        /// Vérifie les identifiants d'un utilisateur et enregistre la connexion dans les logs.
        /// Retourne l'objet utilisateur complet en cas de succès.
        /// </summary>
        /// <param name="login">Login saisi par l'utilisateur.</param>
        /// <param name="password">Mot de passe en clair saisi par l'utilisateur.</param>
        public (bool Success, Utilisateurs? User, string Message) Authentifier(string login, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == login);

            if (user == null)
                return (false, null, "Login incorrect.");

            // BCrypt.Verify compare le mot de passe en clair avec le hash en BDD
            if (!BCrypt.Net.BCrypt.Verify(password, user.MotPasse))
                return (false, null, "Mot de passe incorrect.");

            // Enregistrement de la connexion dans le log d'historique
            _context.logConnexion.Add(new LogConnexion
            {
                UserId = user.UserId,
                derniereConnexion = DateTime.Now
            });

            // Mise à jour de la date de dernière connexion sur le profil
            user.DerniereConnexion = DateTime.Now;

            _context.SaveChanges();

            return (true, user, "Connexion réussie.");
        }

        // ====================================================================
        // RECHERCHE & FILTRES
        // ====================================================================

        /// <summary>
        /// Recherche des utilisateurs dont le login, l'email, le nom ou le prénom
        /// contient le terme de recherche (insensible à la casse).
        /// </summary>
        /// <param name="searchTerm">Terme à rechercher.</param>
        public List<Utilisateurs> SearchUsers(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return _context.Users
                .Where(u =>
                    u.Login.ToLower().Contains(searchTerm) ||
                    u.Email.ToLower().Contains(searchTerm) ||
                    (u.Nom != null && u.Nom.ToLower().Contains(searchTerm)) ||
                    (u.Prenom != null && u.Prenom.ToLower().Contains(searchTerm)))
                .OrderBy(u => u.Login)
                .ToList();
        }

        /// <summary>Retourne tous les utilisateurs ayant un rôle précis.</summary>
        public List<Utilisateurs> GetUsersByRole(UserRole role)
        {
            return _context.Users
                .Where(u => u.Role == role)
                .OrderBy(u => u.Login)
                .ToList();
        }

        /// <summary>Retourne tous les utilisateurs appartenant à une section.</summary>
        public List<Utilisateurs> GetUsersBySection(string section)
        {
            return _context.Users
                .Where(u => u.Section == section)
                .OrderBy(u => u.Login)
                .ToList();
        }

        // ====================================================================
        // SCORES
        // ====================================================================

        /// <summary>
        /// Retourne les N meilleurs utilisateurs classés par score décroissant.
        /// </summary>
        /// <param name="count">Nombre de résultats à retourner (défaut : 10).</param>
        public List<Utilisateurs> GetTopUsersByScore(int count = 10)
        {
            return _context.Users
                .Where(u => u.ScoreTotal.HasValue)
                .OrderByDescending(u => u.ScoreTotal)
                .Take(count)
                .ToList();
        }

        /// <summary>
        /// Ajoute des points au score d'un utilisateur.
        /// Si le score est null, il est initialisé à 0 avant l'ajout.
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur.</param>
        /// <param name="pointsToAdd">Nombre de points à ajouter (peut être négatif).</param>
        public (bool Success, string Message) UpdateScore(int userId, int pointsToAdd)
        {
            try
            {
                var user = _context.Users.Find(userId);
                if (user == null)
                    return (false, "Utilisateur introuvable.");

                user.ScoreTotal = (user.ScoreTotal ?? 0) + pointsToAdd;
                _context.SaveChanges();

                return (true, $"Score de « {user.Login} » mis à jour : {user.ScoreTotal} pts.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur lors de la mise à jour du score : {ex.Message}");
            }
        }

        // ====================================================================
        // LOGS DE CONNEXION
        // ====================================================================

        /// <summary>
        /// Retourne la date de la connexion précédente (avant la connexion actuelle).
        /// Utile pour afficher "Dernière connexion le XX/XX" sur le tableau de bord.
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur.</param>
        public DateTime? GetDerniereConnexionPrecedente(int userId)
        {
            return _context.logConnexion
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.derniereConnexion)
                .Skip(1) // On saute la connexion actuelle (la plus récente)
                .Select(l => l.derniereConnexion)
                .FirstOrDefault();
        }

        // ====================================================================
        // UTILITAIRES PRIVÉS
        // ====================================================================

        /// <summary>
        /// Hache un mot de passe en clair avec BCrypt (facteur de coût 12).
        /// BCrypt génère automatiquement un sel aléatoire à chaque appel.
        /// Ne jamais stocker un mot de passe en clair en base de données.
        /// </summary>
        /// <param name="password">Mot de passe en clair.</param>
        /// <returns>Hash BCrypt prêt à être stocké en BDD.</returns>
        private static string HashMDP(string password)
        {
            // workFactor: 12 = bon équilibre sécurité/performance (2^12 itérations)
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        /// <summary>
        /// Libère le contexte EF Core.
        /// À appeler si le service est instancié manuellement (hors injection de dépendances).
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}