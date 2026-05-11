using Cyber_Espace_Entrainement.Models;
using Cyber_Espace_Entrainement.Services;
using System.Collections.Generic;

namespace Cyber_Espace_Entrainement.ViewModels.Admin
{
    /// <summary>
    /// ViewModel pour la page de gestion des Cours.
    /// Hérite de <see cref="AdminContenuViewModel"/> et implémente les opérations CRUD
    /// spécifiques au modèle <see cref="Cours"/> via <see cref="CoursService"/>.
    /// 
    /// Ce ViewModel ne contient que la logique propre aux cours.
    /// Toute la logique commune (DataGrid, formulaire, statut) est dans la classe parente.
    /// </summary>
    public class AdminCoursViewModel : AdminContenuViewModel
    {
        private readonly CoursService _coursService;

        /// <summary>
        /// Nom de la propriété clé primaire dans le modèle Cours.
        /// Doit correspondre exactement au nom de la propriété C# (pas le nom SQL).
        /// </summary>
        protected override string NomColonnePrimaire => "ID";

        public AdminCoursViewModel()
        {
            _coursService = new CoursService();

            // Chargement initial des données au démarrage de la page
            ChargerDonnees();
        }

        // ====================================================================
        // CHARGEMENT
        // ====================================================================

        /// <summary>
        /// Récupère tous les cours via le service et les affiche dans le DataGrid.
        /// Réinitialise aussi les champs du formulaire en fonction des colonnes disponibles.
        /// </summary>
        protected override void ChargerDonnees()
        {
            var liste = _coursService.GetAllCours();
            Donnees = ConvertirEnDataTable(liste);
            InitialiserChamps();
            AfficherStatut($"✅ {Donnees.Rows.Count} cours chargé(s).", estSucces: true);
        }

        // ====================================================================
        // SAUVEGARDE
        // ====================================================================

        /// <summary>
        /// Construit un objet <see cref="Cours"/> depuis les valeurs du formulaire,
        /// puis appelle AddCours (mode ajout) ou UpdateCours (mode modification).
        /// 
        /// En mode modification, on charge le cours existant depuis la BDD pour
        /// ne pas écraser des champs non présents dans le formulaire.
        /// </summary>
        /// <param name="valeurs">Dictionnaire clé=NomColonne, valeur=texte saisi.</param>
        protected override (bool Success, string Message) Sauvegarder(Dictionary<string, string> valeurs)
        {
            // En mode édition, on récupère le cours existant pour préserver les champs non affichés
            // En mode ajout, on crée une instance vide
            var cours = IsEditMode && IdEnCours != null
                ? _coursService.GetCoursById(IdEnCours.Value) ?? new Models.Cours()
                : new Models.Cours();

            // ----------------------------------------------------------------
            // Mapping formulaire → modèle Cours
            // Chaque clé correspond au nom d'une propriété de Cours.cs
            // ----------------------------------------------------------------
            cours.Titre = valeurs.GetValueOrDefault("Titre", string.Empty);
            cours.Definition = valeurs.GetValueOrDefault("Definition", string.Empty);
            cours.Explication = valeurs.GetValueOrDefault("Explication", string.Empty);
            cours.Exemple = valeurs.GetValueOrDefault("Exemple", string.Empty);
            cours.Image1 = valeurs.GetValueOrDefault("Image1", string.Empty);
            cours.Image2 = valeurs.GetValueOrDefault("Image2", string.Empty);
            cours.Image3 = valeurs.GetValueOrDefault("Image3", string.Empty);
            cours.Lien = valeurs.GetValueOrDefault("Lien", string.Empty);
            cours.Theme = valeurs.GetValueOrDefault("Theme", string.Empty);
            cours.ImageBouton = valeurs.GetValueOrDefault("ImageBouton", string.Empty);

            // Délégation au service selon le mode
            return IsEditMode
                ? _coursService.UpdateCours(cours)
                : _coursService.AddCours(cours);
        }

        // ====================================================================
        // SUPPRESSION
        // ====================================================================

        /// <summary>
        /// Supprime un cours via le service.
        /// La confirmation utilisateur est gérée dans la classe parente.
        /// </summary>
        protected override (bool Success, string Message) Supprimer(int id)
        {
            return _coursService.DeleteCours(id);
        }
    }
}