using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace Cyber_Espace_Entrainement.ViewModels.Admin
{
    /// <summary>
    /// Classe de base abstraite pour tous les ViewModels d'administration.
    /// Fournit la logique commune : DataGrid, formulaire, recherche, statut et gestion des erreurs.
    /// 
    /// Chaque page admin (Cours, Utilisateurs) hérite de cette classe
    /// et implémente uniquement les méthodes propres à son entité.
    /// </summary>
    public abstract partial class AdminContenuViewModel : ObservableObject
    {
        /// <summary>
        /// Événement déclenché quand le formulaire est réinitialisé.
        /// Utilisé par les vues pour effectuer des actions supplémentaires (ex : vider une PasswordBox).
        /// </summary>
        public event Action? FormCleared;

        // ====================================================================
        // PROPRIÉTÉS — DataGrid
        // ====================================================================

        /// <summary>Données affichées dans le DataGrid (table générée dynamiquement).</summary>
        [ObservableProperty]
        private DataTable? _donnees;

        /// <summary>Ligne actuellement sélectionnée dans le DataGrid.</summary>
        [ObservableProperty]
        private DataRowView? _ligneSelectionnee;

        /// <summary>
        /// Appelé automatiquement par le source generator quand l'utilisateur clique sur une ligne.
        /// Charge les données de la ligne dans le formulaire.
        /// </summary>
        partial void OnLigneSelectionneeChanged(DataRowView? value)
        {
            if (value != null)
                ChargerLigneDansFormulaire(value);
        }

        // ====================================================================
        // PROPRIÉTÉS — Formulaire dynamique
        // ====================================================================

        /// <summary>
        /// Liste des champs du formulaire générés dynamiquement depuis les colonnes du DataTable.
        /// Chaque champ est lié à un TextBox (ou contrôle spécialisé) dans la vue.
        /// </summary>
        public ObservableCollection<ChampFormulaire> Champs { get; } = new();

        // ====================================================================
        // PROPRIÉTÉS — Mode édition
        // ====================================================================

        /// <summary>
        /// Indique si l'on est en mode "modification" (true) ou "ajout" (false).
        /// Modifie le libellé du bouton Sauvegarder via NotifyPropertyChangedFor.
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(LibelleBoutonSauvegarder))]
        private bool _isEditMode;

        /// <summary>Libellé dynamique du bouton principal selon le mode.</summary>
        public string LibelleBoutonSauvegarder => IsEditMode ? "💾 Enregistrer" : "➕ Ajouter";

        /// <summary>
        /// Identifiant de la ligne en cours d'édition.
        /// Vaut null si on est en mode "ajout".
        /// </summary>
        protected int? IdEnCours;

        // ====================================================================
        // PROPRIÉTÉS — Recherche
        // ====================================================================

        /// <summary>Texte saisi dans la barre de recherche.</summary>
        [ObservableProperty]
        private string _searchText = string.Empty;

        // ====================================================================
        // PROPRIÉTÉS — Statut & Erreurs (MVVM-friendly, pas de MessageBox ici)
        // ====================================================================

        /// <summary>
        /// Message de statut ou d'erreur affiché dans l'interface.
        /// Peut contenir un message de succès (vert), d'erreur (rouge) ou être vide.
        /// La vue se charge de l'afficher dans un TextBlock ou Border dédié.
        /// </summary>
        [ObservableProperty]
        private string _statusMessage = string.Empty;

        /// <summary>
        /// Couleur du message de statut.
        /// Vert = succès, Rouge = erreur, Transparent = aucun message.
        /// </summary>
        [ObservableProperty]
        private Brush _statusColor = Brushes.Transparent;

        /// <summary>
        /// Indique si un message de statut est actuellement visible.
        /// Utilisé pour masquer/afficher la zone d'erreur via un DataTrigger.
        /// </summary>
        public bool HasStatusMessage => !string.IsNullOrEmpty(StatusMessage);

        // ====================================================================
        // MÉTHODES ABSTRAITES — À implémenter dans chaque sous-classe
        // ====================================================================

        /// <summary>Nom de la colonne clé primaire dans le DataTable (ex : "UserId", "ID").</summary>
        protected abstract string NomColonnePrimaire { get; }

        /// <summary>Charge les données depuis la base et les place dans <see cref="Donnees"/>.</summary>
        protected abstract void ChargerDonnees();

        /// <summary>
        /// Sauvegarde une ligne (ajout ou modification) selon <see cref="IsEditMode"/>.
        /// </summary>
        /// <param name="valeurs">Dictionnaire clé=NomColonne, valeur=texte saisi.</param>
        /// <returns>Tuple (succès, message lisible).</returns>
        protected abstract (bool Success, string Message) Sauvegarder(Dictionary<string, string> valeurs);

        /// <summary>Supprime la ligne dont l'identifiant est passé en paramètre.</summary>
        /// <param name="id">Identifiant de la ligne à supprimer.</param>
        /// <returns>Tuple (succès, message lisible).</returns>
        protected abstract (bool Success, string Message) Supprimer(int id);

        // ====================================================================
        // INITIALISATION DU FORMULAIRE
        // ====================================================================

        /// <summary>
        /// Crée les champs du formulaire depuis les colonnes du DataTable.
        /// La colonne clé primaire est ignorée (non modifiable par l'utilisateur).
        /// Les sous-classes peuvent surcharger cette méthode pour ajouter des champs spécialisés
        /// (ComboBox, champs lecture seule, etc.).
        /// </summary>
        protected virtual void InitialiserChamps()
        {
            if (Donnees == null) return;

            Champs.Clear();

            foreach (DataColumn col in Donnees.Columns)
            {
                // On n'affiche pas la clé primaire dans le formulaire (auto-increment BDD)
                if (col.ColumnName == NomColonnePrimaire) continue;

                Champs.Add(new ChampFormulaire(col.ColumnName, col.ColumnName));
            }
        }

        // ====================================================================
        // COMMANDES — Recherche
        // ====================================================================

        /// <summary>Filtre le DataGrid selon le texte de recherche sur toutes les colonnes.</summary>
        [RelayCommand]
        private void Search()
        {
            if (Donnees == null) return;

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // Texte vide : on recharge tout sans filtre
                ChargerDonnees();
                return;
            }

            // Filtrage en mémoire avec la syntaxe DataView.RowFilter
            var conditions = Donnees.Columns
                .Cast<DataColumn>()
                .Select(c => $"CONVERT([{c.ColumnName}], System.String) LIKE '%{SearchText}%'");

            Donnees.DefaultView.RowFilter = string.Join(" OR ", conditions);
            AfficherStatut($"🔍 {Donnees.DefaultView.Count} résultat(s) trouvé(s).", estSucces: true);
        }

        /// <summary>Supprime le filtre de recherche et recharge toutes les données.</summary>
        [RelayCommand]
        private void ResetFilters()
        {
            SearchText = string.Empty;

            if (Donnees != null)
                Donnees.DefaultView.RowFilter = string.Empty;

            ChargerDonnees();
        }

        // ====================================================================
        // COMMANDES — CRUD
        // ====================================================================

        /// <summary>
        /// Déclenche la sauvegarde (ajout ou modification) selon le mode en cours.
        /// Délègue la logique métier à la sous-classe via <see cref="Sauvegarder"/>.
        /// </summary>
        [RelayCommand]
        private void SaveLigne()
        {
            // Vide le message précédent avant toute action
            EffacerStatut();

            // Récupère toutes les valeurs saisies dans le formulaire
            var valeurs = Champs.ToDictionary(c => c.NomColonne, c => c.Valeur);

            var resultat = Sauvegarder(valeurs);

            AfficherStatut(
                resultat.Success ? $"✅ {resultat.Message}" : $"❌ {resultat.Message}",
                estSucces: resultat.Success
            );

            if (resultat.Success)
            {
                ReinitialiserFormulaire();
                ChargerDonnees();
            }
        }

        /// <summary>
        /// Supprime la ligne sélectionnée après confirmation de l'utilisateur.
        /// La confirmation (MessageBox) est ici dans la base car c'est une interaction UI,
        /// acceptable dans une classe abstraite parente. Peut être déplacée dans le code-behind
        /// si l'on souhaite une séparation stricte.
        /// </summary>
        [RelayCommand]
        private void DeleteLigne(DataRowView? ligne)
        {
            if (ligne == null) return;

            int id = Convert.ToInt32(ligne[NomColonnePrimaire]);

            // Confirmation avant suppression définitive
            var confirmation = MessageBox.Show(
                $"Supprimer cet enregistrement (ID : {id}) ?\n\nCette action est irréversible.",
                "Confirmation de suppression",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (confirmation != MessageBoxResult.Yes) return;

            var resultat = Supprimer(id);

            AfficherStatut(
                resultat.Success ? $"✅ {resultat.Message}" : $"❌ {resultat.Message}",
                estSucces: resultat.Success
            );

            if (resultat.Success)
            {
                ReinitialiserFormulaire();
                ChargerDonnees();
            }
        }

        /// <summary>Annule l'édition en cours et réinitialise le formulaire.</summary>
        [RelayCommand]
        private void CancelEdit()
        {
            ReinitialiserFormulaire();
            EffacerStatut();
        }

        // ====================================================================
        // HELPERS — Formulaire
        // ====================================================================

        /// <summary>
        /// Remplit le formulaire avec les données de la ligne sélectionnée.
        /// Passe automatiquement en mode édition et mémorise l'ID.
        /// </summary>
        protected virtual void ChargerLigneDansFormulaire(DataRowView ligne)
        {
            IsEditMode = true;
            IdEnCours = Convert.ToInt32(ligne[NomColonnePrimaire]);

            foreach (var champ in Champs)
            {
                if (Donnees!.Columns.Contains(champ.NomColonne))
                    champ.Valeur = ligne[champ.NomColonne]?.ToString() ?? string.Empty;
            }
        }

        /// <summary>
        /// Vide tous les champs, repasse en mode ajout et réinitialise la sélection.
        /// Déclenche l'événement <see cref="FormCleared"/> (utile pour vider les PasswordBox).
        /// </summary>
        protected void ReinitialiserFormulaire()
        {
            IsEditMode = false;
            IdEnCours = null;

            foreach (var champ in Champs)
                champ.Valeur = string.Empty;

            LigneSelectionnee = null;
            FormCleared?.Invoke();
        }

        // ====================================================================
        // HELPERS — Statut & Erreurs
        // ====================================================================

        /// <summary>
        /// Affiche un message de statut dans la zone dédiée de l'interface.
        /// Vert pour le succès, rouge pour l'erreur.
        /// </summary>
        /// <param name="message">Texte à afficher.</param>
        /// <param name="estSucces">true = couleur verte, false = couleur rouge.</param>
        protected void AfficherStatut(string message, bool estSucces)
        {
            StatusMessage = message;
            StatusColor = estSucces
                ? new SolidColorBrush(Color.FromRgb(56, 142, 60))   // Vert matériel
                : new SolidColorBrush(Color.FromRgb(211, 47, 47));   // Rouge matériel

            // Notifie la vue que la visibilité du bloc d'erreur a changé
            OnPropertyChanged(nameof(HasStatusMessage));
        }

        /// <summary>Efface le message de statut (masque la zone d'erreur).</summary>
        protected void EffacerStatut()
        {
            StatusMessage = string.Empty;
            StatusColor = Brushes.Transparent;
            OnPropertyChanged(nameof(HasStatusMessage));
        }

        // ====================================================================
        // HELPER — Conversion List<T> → DataTable (utilisé par les sous-classes)
        // ====================================================================

        /// <summary>
        /// Convertit une liste d'objets typés en DataTable pour l'affichage dans le DataGrid.
        /// Utilise la réflexion pour lire automatiquement les propriétés du modèle.
        /// 
        /// Règles appliquées :
        /// - Les propriétés [NotMapped] sont ignorées (ex : RoleDisplay, propriétés de navigation EF).
        /// - Les enums sont convertis en texte lisible (ex : UserRole.Admin → "Admin").
        /// - Les valeurs null sont remplacées par DBNull.Value pour le DataTable.
        /// </summary>
        /// <typeparam name="T">Type du modèle (ex : Cours, Utilisateurs).</typeparam>
        /// <param name="liste">Liste d'objets à convertir.</param>
        /// <returns>DataTable prêt pour l'affichage.</returns>
        protected static DataTable ConvertirEnDataTable<T>(List<T> liste)
        {
            var table = new DataTable();

            // Propriétés publiques lisibles, hors [NotMapped]
            var proprietes = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && !p.IsDefined(typeof(NotMappedAttribute), false))
                .ToArray();

            // Création des colonnes — les enums sont stockés en string
            foreach (var prop in proprietes)
            {
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                if (type.IsEnum) type = typeof(string);
                table.Columns.Add(prop.Name, type);
            }

            // Remplissage des lignes
            foreach (var item in liste)
            {
                var row = table.NewRow();

                foreach (var prop in proprietes)
                {
                    var valeur = prop.GetValue(item);

                    // Enum → texte pour l'affichage
                    if (valeur != null && valeur.GetType().IsEnum)
                        valeur = valeur.ToString();

                    row[prop.Name] = valeur ?? DBNull.Value;
                }

                table.Rows.Add(row);
            }

            return table;
        }
    }

    // ========================================================================
    // CHAMP DE FORMULAIRE DYNAMIQUE
    // ========================================================================

    /// <summary>
    /// Représente un champ du formulaire dynamique.
    /// Exposé dans <see cref="AdminContenuViewModel.Champs"/> et lié aux contrôles de la vue.
    /// </summary>
    public class ChampFormulaire : ObservableObject
    {
        /// <summary>Nom technique de la colonne (correspond à la propriété du modèle).</summary>
        public string NomColonne { get; }

        /// <summary>Libellé affiché au-dessus du champ dans l'interface.</summary>
        public string Libelle { get; }

        private string _valeur = string.Empty;

        /// <summary>Valeur saisie par l'utilisateur dans le champ.</summary>
        public string Valeur
        {
            get => _valeur;
            set => SetProperty(ref _valeur, value);
        }

        /// <param name="nomColonne">Nom technique de la colonne.</param>
        /// <param name="libelle">Libellé affiché dans le formulaire.</param>
        public ChampFormulaire(string nomColonne, string libelle)
        {
            NomColonne = nomColonne;
            Libelle = libelle;
        }
    }
}