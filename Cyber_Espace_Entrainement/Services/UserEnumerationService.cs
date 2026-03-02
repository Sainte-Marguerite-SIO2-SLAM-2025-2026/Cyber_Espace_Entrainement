using Cyber_Espace_Entrainement.Data;
using Cyber_Espace_Entrainement.Models;
using Cyber_Espace_Entrainement.Models.UserEnumeration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cyber_Espace_Entrainement.Services
{
    class UserEnumerationService
    {
        private readonly AppDbContext _context;

        public UserEnumerationService()
        {
            _context = new AppDbContext();
            // S'assurer que la base existe
            _context.Database.EnsureCreated();
        }

        /// <summary>
        /// Récupérer tout les messages de user enumeration
        /// </summary>
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
    }
}
