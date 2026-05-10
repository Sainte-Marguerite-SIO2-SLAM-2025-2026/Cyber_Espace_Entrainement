using Cyber_Espace_Entrainement.Data;
using Cyber_Espace_Entrainement.Models;
using Cyber_Espace_Entrainement.Models.UserEnumeration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cyber_Espace_Entrainement.Services
{
    /// <summary>
    /// Service d'accès aux données pour l'activité "User Enumeration".
    /// </summary>
    class UserEnumerationService
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initialise une nouvelle instance du service et s'assure que la base de données existe.
        /// </summary>
        public UserEnumerationService()
        {
            _context = new AppDbContext();
            // S'assurer que la base existe
            _context.Database.EnsureCreated();
        }

        public List<UserEnumeration> GetUserEnumeration()
        {
            try
            {
                return _context.userEnumeration
                    .OrderBy(a => a.Id)
                    .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur : {ex.Message}");
                return new List<UserEnumeration>();
            }
        }

        /// <summary>
        /// Récupérer tout les messages de user enumeration triée par Id
        /// </summary>
        /// <returns>Liste des entités ; une liste vide est renvoyée en cas d'erreur.</returns>

        public List<UserEnumeration> GetAllUserEnumeration()
        {
            try
            {
                return _context.userEnumeration
                    .Where(a => a.ActiviteId == 1)
                    .OrderBy(a => a.Id)
                    .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur : {ex.Message}");
                return new List<UserEnumeration>();
            }
        }

        /// <summary>
        /// Retourne le nombre total d'enregistrements UserEnumeration.
        /// </summary>
        /// <returns>Nombre total d'items.</returns>
        public int GetCountUserEnumeration()
        {
            return _context.userEnumeration
                .Where(a => a.ActiviteId == 1)
                .Count();
        }

        public List<UserEnumeration> GetRandomOnePerLibelle()
        {
            try
            {
                return _context.userEnumeration
                    .Where(a => a.ActiviteId == 2)
                    .AsEnumerable() // passage en mémoire pour Random()
                    .GroupBy(u => u.Libelle)
                    .Select(g => g.OrderBy(_ => Guid.NewGuid()).First())
                    .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur : {ex.Message}");
                return new List<UserEnumeration>();
            }
        }

        public int GetCountRandomOnePerLibelle()
        {

            return _context.userEnumeration
                 .Where(a => a.ActiviteId == 2)
                 .AsEnumerable() // passage en mémoire pour Random()
                 .GroupBy(u => u.Libelle)
                 .Select(g => g.OrderBy(_ => Guid.NewGuid()).First())
                 .Count();

        }

        public (bool Success, string Message) AddUserEnumeration(UserEnumeration userEnum)
        {
            try
            {
                userEnum.CoursId = 101;
                _context.userEnumeration.Add(userEnum);
                _context.SaveChanges();
                return (true, "User Enumeration ajouté avec succès.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur : {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Inner : {ex.InnerException?.Message}");
                return (false, "Erreur lors de l'ajout de l'User Enumeration.");
            }

        }

        /// <summary>
        /// Modifier un user enum existant
        /// MODIFIÉ : Ajout de la mise à jour des nouveaux champs
        /// </summary>
        public (bool Success, string Message) UpdateUserEnumeration(UserEnumeration userEnum)
        {
            try
            {
                var existingUserEnum = _context.userEnumeration.Find(userEnum.Id);
                if (existingUserEnum == null)
                {
                    return (false, "User enumeration introuvable.");
                }

                // Vérifier unicité message (sauf pour lui-même !)
                if (_context.userEnumeration.Any(u => u.Message == userEnum.Message && u.Id != userEnum.Id))
                {
                    return (false, "Ce message est déjà utilisé.");
                }



                // Mise à jour des champs existants
                existingUserEnum.ActiviteId = userEnum.ActiviteId;
                existingUserEnum.Reponse = userEnum.Reponse;
                existingUserEnum.Message = userEnum.Message;
                existingUserEnum.Libelle = userEnum.Libelle;

                _context.SaveChanges();

                return (true, $"User enumeration '{userEnum.Id}' modifié avec succès.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur : {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Inner : {ex.InnerException?.Message}");

            }
        }
        /// <summary>
        /// supprimer un user enum existant
        /// MODIFIÉ : Ajout de la mise à jour des nouveaux champs
        /// </summary>
        public (bool Success, string Message) DeleteUserEnumeration(int id)
        {
            try
            {
                var existingUserEnum = _context.userEnumeration.Find(id);
                if (existingUserEnum == null)
                {
                    return (false, "User enumeration introuvable.");
                }
                _context.userEnumeration.Remove(existingUserEnum);
                _context.SaveChanges();
                return (true, $"User enumeration '{id}' supprimé avec succès.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur : {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Inner : {ex.InnerException?.Message}");
            }

        }

        /// <summary>
        /// Rechercher des user enum
        /// MODIFIÉ : Ajout de la recherche dans le champ Message
        /// </summary>
        public List<UserEnumeration> SearchUserEnum(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return _context.userEnumeration
                .Where(u => u.Message.ToLower().Contains(searchTerm))
                .OrderBy(u => u.Message)
                .ToList();
        }
    }
}

                        
        
