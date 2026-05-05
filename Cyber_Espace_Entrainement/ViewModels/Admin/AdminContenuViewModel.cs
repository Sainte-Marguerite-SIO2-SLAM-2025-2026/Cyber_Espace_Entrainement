using Cyber_Espace_Entrainement.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Cyber_Espace_Entrainement.ViewModels.Admin
{
    public partial class AdminContenuViewModel : ObservableObject
    {
        private readonly AdminService _adminService;
        private readonly Models.Admin _tableActuelle;

        public event Action? FormCleared;

        // ====================================================================
        // TITRE
        // ====================================================================

        public string TitreTable => _tableActuelle.Table ?? string.Empty;

        // ====================================================================
        // DONNÉES DU DATAGRID
        // ====================================================================

        [ObservableProperty]
        private DataTable? _donnees;

        [ObservableProperty]
        private DataRowView? _ligneSelectionnee;

        partial void OnLigneSelectionneeChanged(DataRowView? value)
        {
            if (value != null)
                ChargerLigneDansFormulaire(value);
        }

        // ====================================================================
        // FORMULAIRE DYNAMIQUE
        // ====================================================================

        public ObservableCollection<ChampFormulaire> Champs { get; } = new();

        // ====================================================================
        // MODE ÉDITION
        // ====================================================================

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(LibelleBoutonSauvegarder))]
        private bool _isEditMode;

        public string LibelleBoutonSauvegarder => IsEditMode ? "💾 Enregistrer" : "➕ Ajouter";

        private object? _cléPrimaireEnCours;
        private string? _nomColonnePrimaire;

        // ====================================================================
        // RECHERCHE
        // ====================================================================

        [ObservableProperty]
        private string _searchText = string.Empty;

        // ====================================================================
        // STATUT
        // ====================================================================

        [ObservableProperty]
        private string _statusMessage = string.Empty;

        [ObservableProperty]
        private Brush _statusColor = Brushes.Transparent;

        // ====================================================================
        // CONSTRUCTEUR
        // ====================================================================

        public AdminContenuViewModel(Models.Admin admin)
        {
            _adminService = new AdminService();
            _tableActuelle = admin;

            ChargerDonnees();
        }

        // ====================================================================
        // CHARGEMENT
        // ====================================================================

        private void ChargerDonnees()
        {
            try
            {
                var result = _adminService.ChargerTable(_tableActuelle.Table);
                if (result == null)
                {
                    AfficherStatut("❌ Impossible de charger les données.", estSucces: false);
                    return;
                }

                Donnees = result;
                InitialiserChamps();
                AfficherStatut($"✅ {Donnees.Rows.Count} enregistrement(s) chargé(s).", estSucces: true);
            }
            catch (Exception ex)
            {
                AfficherStatut($"❌ Erreur de chargement : {ex.Message}", estSucces: false);
            }
        }

        /// <summary>
        /// Génère les champs du formulaire depuis les colonnes du DataTable.
        /// La première colonne est supposée être la clé primaire.
        /// </summary>
        private void InitialiserChamps()
        {
            if (Donnees == null) return;

            Champs.Clear();
            bool estPremiere = true;

            foreach (DataColumn col in Donnees.Columns)
            {
                if (estPremiere)
                {
                    // La première colonne = clé primaire : on la mémorise mais on ne l'affiche pas
                    _nomColonnePrimaire = col.ColumnName;
                    estPremiere = false;
                    continue;
                }

                Champs.Add(new ChampFormulaire(col.ColumnName, col.ColumnName));
            }
        }

        // ====================================================================
        // COMMANDES
        // ====================================================================

        [RelayCommand]
        private void Search()
        {
            if (Donnees == null) return;

            try
            {
                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    ChargerDonnees();
                    return;
                }

                // Filtrage en mémoire sur toutes les colonnes
                var vue = Donnees.DefaultView;
                var conditions = Donnees.Columns
                    .Cast<DataColumn>()
                    .Select(c => $"CONVERT([{c.ColumnName}], System.String) LIKE '%{SearchText}%'");
                vue.RowFilter = string.Join(" OR ", conditions);

                AfficherStatut($"🔍 {vue.Count} résultat(s) pour « {SearchText} ».", estSucces: true);
            }
            catch (Exception ex)
            {
                AfficherStatut($"❌ {ex.Message}", estSucces: false);
            }
        }

        [RelayCommand]
        private void ResetFilters()
        {
            SearchText = string.Empty;
            if (Donnees != null)
                Donnees.DefaultView.RowFilter = string.Empty;
            ChargerDonnees();
        }

        [RelayCommand]
        private void SaveLigne()
        {
            try
            {
                var valeurs = Champs.ToDictionary(c => c.NomColonne, c => (object?)c.Valeur);

                if (IsEditMode && _cléPrimaireEnCours != null && _nomColonnePrimaire != null)
                {
                    _adminService.ModifierLigne(_tableActuelle.Table, _nomColonnePrimaire, _cléPrimaireEnCours, valeurs);
                    AfficherStatut("✅ Enregistrement modifié avec succès.", estSucces: true);
                }
                else
                {
                    _adminService.AjouterLigne(_tableActuelle.Table, valeurs);
                    AfficherStatut("✅ Enregistrement ajouté avec succès.", estSucces: true);
                }

                ReinitialiserFormulaire();
                ChargerDonnees();
            }
            catch (Exception ex)
            {
                AfficherStatut($"❌ Erreur : {ex.Message}", estSucces: false);
            }
        }

        [RelayCommand]
        private void DeleteLigne(DataRowView? ligne)
        {
            if (ligne == null || _nomColonnePrimaire == null) return;

            var clé = ligne[_nomColonnePrimaire];

            var confirm = MessageBox.Show(
                $"Supprimer cet enregistrement (ID : {clé}) ?\n\nCette action est irréversible.",
                "Confirmation de suppression",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (confirm != MessageBoxResult.Yes) return;

            try
            {
                _adminService.SupprimerLigne(_tableActuelle.Table, _nomColonnePrimaire, clé);
                AfficherStatut("✅ Enregistrement supprimé.", estSucces: true);
                ReinitialiserFormulaire();
                ChargerDonnees();
            }
            catch (Exception ex)
            {
                AfficherStatut($"❌ Erreur : {ex.Message}", estSucces: false);
            }
        }

        [RelayCommand]
        private void CancelEdit()
        {
            ReinitialiserFormulaire();
            AfficherStatut(string.Empty, estSucces: true);
        }

        // ====================================================================
        // HELPERS
        // ====================================================================

        private void ChargerLigneDansFormulaire(DataRowView ligne)
        {
            IsEditMode = true;

            if (_nomColonnePrimaire != null)
                _cléPrimaireEnCours = ligne[_nomColonnePrimaire];

            foreach (var champ in Champs)
            {
                if (ligne.Row.Table.Columns.Contains(champ.NomColonne))
                    champ.Valeur = ligne[champ.NomColonne]?.ToString() ?? string.Empty;
            }
        }

        private void ReinitialiserFormulaire()
        {
            IsEditMode = false;
            _cléPrimaireEnCours = null;
            foreach (var champ in Champs)
                champ.Valeur = string.Empty;

            LigneSelectionnee = null;
            FormCleared?.Invoke();
        }

        private void AfficherStatut(string message, bool estSucces)
        {
            StatusMessage = message;
            StatusColor = estSucces
                ? new SolidColorBrush(Color.FromRgb(56, 142, 60))
                : new SolidColorBrush(Color.FromRgb(211, 47, 47));
        }
    }

    // ========================================================================
    // CHAMP DE FORMULAIRE DYNAMIQUE
    // ========================================================================

    public class ChampFormulaire : ObservableObject
    {
        public string NomColonne { get; }
        public string Libelle { get; }

        private string _valeur = string.Empty;
        public string Valeur
        {
            get => _valeur;
            set => SetProperty(ref _valeur, value);
        }

        public ChampFormulaire(string nomColonne, string libelle)
        {
            NomColonne = nomColonne;
            Libelle = libelle;
        }
    }
}