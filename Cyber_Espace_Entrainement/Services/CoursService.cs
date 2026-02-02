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
    /// Service pour gérer les opérations sur les Cours
    /// </summary>
    public class CoursService
    {
        private readonly AppDbContext _context;

        public CoursService()
        {
            _context = new AppDbContext();
            // S'assurer que la base existe
            _context.Database.EnsureCreated();
        }

        //
        // CRUD
        //

        /// <summary>
        /// Récupérer toutes les Cours
        /// </summary>
        public List<Cours> GetAllCours()
        {
            try
            {
                return _context.Cours
                    .OrderBy(a => a.Titre)
                    .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur : {ex.Message}");
                return new List<Cours>();
            }
        }

        /// <summary>
        /// Récupérer une activité par son id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cours? GetCoursById(int id)
        {
            return _context.Cours.Find(id);
        }

        /// <summary>
        /// Récupérer une cour par un titre
        /// </summary>
        /// <param name="libelle"></param>
        /// <returns></returns>
        public Cours? GetCoursByLibelle(string titre)
        {
            return _context.Cours.FirstOrDefault(a => a.Titre == titre);
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
