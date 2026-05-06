using Cyber_Espace_Entrainement.Data;
using Cyber_Espace_Entrainement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                return _dbContext.Phishing
                    .OrderBy(a => a.Id)
                    .ToList();
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

        public bool AjoutPhishing(Phishing phishing)
        {
            try
            {
                _dbContext.Phishing.Add(phishing);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Erreur Ajout : {e.Message}");
                return false;
            }
        }

        public bool ModifPhishing(Phishing phishing)
        {
            try
            {
                var trouver = _dbContext.Phishing.Find(phishing.Id);
                if (trouver == null) return false;

                trouver.ActiviteId = phishing.ActiviteId;
                trouver.CoursId = phishing.CoursId;
                trouver.Type = phishing.Type;
                trouver.Image = phishing.Image;
                trouver.Expediteur = phishing.Expediteur;
                trouver.Objet = phishing.Objet;
                trouver.Contenu = phishing.Contenu;

                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Erreur Modification : {e.Message}");
                return false;
            }
        }

        public bool SupprimerPhishing(int id)
        {
            try
            {
                var trouver = _dbContext.Phishing.Find(id);
                if (trouver == null) return false;

                _dbContext.Remove(trouver);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Erreur Suppression : {e.Message}");
                return false;
            }
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
