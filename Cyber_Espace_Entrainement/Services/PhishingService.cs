using Cyber_Espace_Entrainement.Data;
using Cyber_Espace_Entrainement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Cyber_Espace_Entrainement.Services
{
    class PhishingService
    {
        private readonly AppDbContext _dbContext;

        public Phishing Phishing { get; set; } = null!;

        public PhishingService()
        {
            _dbContext = new AppDbContext();

            _dbContext.Database.EnsureCreated();
        }


        //
        // CRUD
        //

        /// <summary>
        /// Récupérer tous les enregistrements dans Phishing
        /// </summary>
        public List<Phishing> GetAllPhishing()
        {
            try
            {
                return _dbContext.Phishing.ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur : {ex.Message}");
                return new List<Phishing>();
            }
        }

        /// <summary>
        /// Récupérer un mail par son id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Phishing GetPhishingById(int id)
        {
            return _dbContext.Phishing.Find(id);
        }

        /// <summary>
        /// Fermer la connexion (important !)
        /// </summary>
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
