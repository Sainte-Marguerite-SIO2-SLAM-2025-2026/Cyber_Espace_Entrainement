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

        public Cours Cours { get; set; } = null!;

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
        /// Récupérer toutes les activités
        /// Joint la table Cours dans le but de trier les activités par thème 
        /// puis par ordre alphabétique
        /// </summary>
        public List<Activites> GetAllActivites()
        {
            try
            {
                return _context.Activites
                .Join(_context.Cours,
                      a => a.CoursId,
                      c => c.ID,
                      (a, c) => new { Activite = a, Theme = c.Theme })
                .AsEnumerable() 
                .GroupBy(x => x.Activite.Libelle)   
                .Select(g =>
                {
                    var act = g.First().Activite;
                    act.Theme = g.First().Theme;
                    return act;
                })
                .OrderBy(a => a.Theme)             
                .ThenBy(a => a.Libelle)               
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
