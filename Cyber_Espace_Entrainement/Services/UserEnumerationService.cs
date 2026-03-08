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

        /// <summary>
        /// Récupérer tout les messages de user enumeration triée par Id
        /// </summary>
        /// <returns>Liste des entités ; une liste vide est renvoyée en cas d'erreur.</returns>

        public List<UserEnumeration> GetAllUserEnumeration()
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
        /// Retourne le nombre total d'enregistrements UserEnumeration.
        /// </summary>
        /// <returns>Nombre total d'items.</returns>
        public int GetCountUserEnumeration()
        {
            return _context.userEnumeration
                .Count();
        }

        public List<UserEnumeration> GetRandomOnePerLibelle()
        {
            try
            {
                return _context.userEnumeration
                    .Where(u => !string.IsNullOrWhiteSpace(u.Libelle))
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
                    .Where(u => !string.IsNullOrWhiteSpace(u.Libelle))
                    .AsEnumerable() // passage en mémoire pour Random()
                    .GroupBy(u => u.Libelle)
                    .Select(g => g.OrderBy(_ => Guid.NewGuid()).First())
                    .Count();
            
        }

    }
}
