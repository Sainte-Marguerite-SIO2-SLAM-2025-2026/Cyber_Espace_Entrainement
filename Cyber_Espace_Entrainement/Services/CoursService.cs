using Cyber_Espace_Entrainement.Data;
using Cyber_Espace_Entrainement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cyber_Espace_Entrainement.Services
{
    /// <summary>
    /// Service responsable de toutes les opérations sur les <see cref="Cours"/>.
    /// 
    /// Suit le même contrat que <see cref="UserService"/> :
    /// les opérations d'écriture retournent un tuple (bool Success, string Message)
    /// pour rester MVVM-friendly (pas d'exceptions remontées vers la vue).
    /// </summary>
    public class CoursService
    {
        private readonly AppDbContext _context;

        public CoursService()
        {
            _context = new AppDbContext();
            // Crée la base de données si elle n'existe pas encore
            _context.Database.EnsureCreated();
        }

        // ====================================================================
        // LECTURE
        // ====================================================================

        /// <summary>
        /// Retourne tous les cours triés par titre alphabétiquement.
        /// En cas d'erreur de connexion, retourne une liste vide et journalise l'erreur.
        /// </summary>
        public List<Cours> GetAllCours()
        {
            try
            {
                return _context.Cours
                    .OrderBy(c => c.Titre)
                    .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CoursService] GetAllCours : {ex.Message}");
                return new List<Cours>();
            }
        }

        /// <summary>
        /// Retourne un cours par son identifiant, ou <c>null</c> s'il est introuvable.
        /// </summary>
        /// <param name="id">Identifiant unique du cours.</param>
        public Cours? GetCoursById(int id)
        {
            return _context.Cours.Find(id);
        }

        /// <summary>
        /// Retourne le premier cours dont le titre correspond exactement, ou <c>null</c>.
        /// Utile pour vérifier l'existence avant un ajout.
        /// </summary>
        /// <param name="titre">Titre exact du cours.</param>
        public Cours? GetCoursByTitre(string titre)
        {
            return _context.Cours.FirstOrDefault(c => c.Titre == titre);
        }

        // ====================================================================
        // ÉCRITURE — Ajout
        // ====================================================================

        /// <summary>
        /// Ajoute un nouveau cours en base de données.
        /// 
        /// Règles appliquées :
        /// - Le titre est obligatoire.
        /// - L'ID est généré automatiquement par la BDD (auto-increment).
        /// </summary>
        /// <param name="cours">L'entité à persister (sans ID : il est généré par la BDD).</param>
        /// <returns>Tuple (succès, message lisible pour l'interface).</returns>
        public (bool Success, string Message) AddCours(Cours cours)
        {
            try
            {
                // Validation : le titre est le champ minimal obligatoire
                if (string.IsNullOrWhiteSpace(cours.Titre))
                    return (false, "Le titre du cours est obligatoire.");

                _context.Cours.Add(cours);
                _context.SaveChanges();

                return (true, $"Cours « {cours.Titre} » ajouté avec succès.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur lors de l'ajout : {ex.Message}");
            }
        }

        // ====================================================================
        // ÉCRITURE — Modification
        // ====================================================================

        /// <summary>
        /// Met à jour un cours existant identifié par <see cref="Cours.ID"/>.
        /// Seuls les champs métier sont mis à jour (l'ID est protégé).
        /// 
        /// Note : le champ <see cref="Cours.Exemple"/> était manquant dans l'ancienne version.
        /// Il est désormais correctement mis à jour.
        /// </summary>
        /// <param name="cours">L'entité avec les nouvelles valeurs.</param>
        /// <returns>Tuple (succès, message lisible pour l'interface).</returns>
        public (bool Success, string Message) UpdateCours(Cours cours)
        {
            try
            {
                var existant = _context.Cours.Find(cours.ID);
                if (existant == null)
                    return (false, "Cours introuvable. Il a peut-être déjà été supprimé.");

                // --- Mise à jour de tous les champs métier ---
                // L'ID n'est jamais modifié (clé primaire protégée)
                existant.Titre = cours.Titre;
                existant.Definition = cours.Definition;
                existant.Explication = cours.Explication;
                existant.Exemple = cours.Exemple;       // ← corrigé : manquait dans l'ancienne version
                existant.Image1 = cours.Image1;
                existant.Image2 = cours.Image2;
                existant.Image3 = cours.Image3;
                existant.Lien = cours.Lien;
                existant.Theme = cours.Theme;
                existant.ImageBouton = cours.ImageBouton;

                _context.SaveChanges();

                return (true, $"Cours « {cours.Titre} » modifié avec succès.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur lors de la modification : {ex.Message}");
            }
        }

        // ====================================================================
        // ÉCRITURE — Suppression
        // ====================================================================

        /// <summary>
        /// Supprime définitivement un cours par son identifiant.
        /// La confirmation utilisateur est gérée côté ViewModel (non ici).
        /// </summary>
        /// <param name="coursId">Identifiant du cours à supprimer.</param>
        /// <returns>Tuple (succès, message lisible pour l'interface).</returns>
        public (bool Success, string Message) DeleteCours(int coursId)
        {
            try
            {
                var cours = _context.Cours.Find(coursId);
                if (cours == null)
                    return (false, "Cours introuvable.");

                _context.Cours.Remove(cours);
                _context.SaveChanges();

                return (true, $"Cours « {cours.Titre} » supprimé avec succès.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur lors de la suppression : {ex.Message}");
            }
        }

        // ====================================================================
        // UTILITAIRES
        // ====================================================================

        /// <summary>
        /// Libère le contexte EF Core.
        /// À appeler si le service est instancié manuellement (hors injection de dépendances).
        /// </summary>
        public void Dispose() => _context.Dispose();
    }
}