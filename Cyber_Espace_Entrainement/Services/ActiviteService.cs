using Cyber_Espace_Entrainement.Data;
using Cyber_Espace_Entrainement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Cyber_Espace_Entrainement.Services
{
    /// <summary>
    /// Service pour gérer les opérations sur les activités
    /// </summary>
    public class ActiviteService
    {
        private readonly AppDbContext _context;

        public ActiviteService() 
        {
            _context = new AppDbContext();
            // S'assurer que la base existe
            _context.Database.EnsureCreated();
        }

        //
        // CRUD
        //

        /// <summary>
        /// Récupérer toutes les activités (version async)
        /// </summary>
        public List<Activites> GetAllActivites()
        {
            try
            {
                // Utilisation de .ToList() synchrone
                return _context.Activites
                    .OrderBy(a => a.Libelle)
                    .ToList()
                    .GroupBy(a => a.Libelle)
                    .Select(g => g.First())
                    .ToList();
                     
                    
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur : {ex.Message}");
                return new List<Activites>();
            }
        }

        /// <summary>
        /// Récupérer une activité par son id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Activites? GetActiviteById(int id)
        {
            return _context.Activites.Find(id);
        }

        /// <summary>
        /// Récupérer une activité par un libellé
        /// </summary>
        /// <param name="libelle"></param>
        /// <returns></returns>
        public List<Activites> GetActiviteByLibelle(string libelle)
        {
            return _context.Activites
                .Where(a => a.Libelle == libelle)
                .OrderBy(a => a.Niveau)
                .ToList();
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
