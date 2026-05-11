using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cyber_Espace_Entrainement.Models;
using Cyber_Espace_Entrainement.Views.Admin;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Cyber_Espace_Entrainement.ViewModels.Admin
{
    /// <summary>
    /// ViewModel de la page d'accueil administration.
    /// Affiche les cards (Utilisateurs, Cours) et ouvre la vue dédiée à chaque entité.
    /// 
    /// Modification : AdminContenu (vue générique unique) est remplacé par
    /// AdminCoursView et AdminUtilisateursView, chacune ayant son propre ViewModel.
    /// </summary>
    public partial class AdminViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Models.Admin> _admin = new();

        public AdminViewModel()
        {
            ChargerMenuAdmin();
        }

        // ====================================================================
        // INITIALISATION DU MENU
        // ====================================================================

        /// <summary>
        /// Remplit la liste des cards d'administration.
        /// Chaque entrée correspond à une page dédiée.
        /// </summary>
        private void ChargerMenuAdmin()
        {
            Admin.Clear();
            Admin.Add(new Models.Admin { Table = "Utilisateurs", Icon = "users.png" });
            Admin.Add(new Models.Admin { Table = "Cours", Icon = "cours.png" });
        }

        // ====================================================================
        // COMMANDE — Ouverture d'une page admin
        // ====================================================================

        /// <summary>
        /// Ouvre la fenêtre d'administration correspondant à la card cliquée.
        /// 
        /// Correspondances :
        /// - "Utilisateurs" → AdminUtilisateursView + AdminUtilisateursViewModel
        /// - "Cours"        → AdminCoursView        + AdminCoursViewModel
        /// 
        /// Chaque vue reçoit son ViewModel en injection de constructeur.
        /// </summary>
        /// <param name="adminEntry">La card cliquée par l'utilisateur.</param>
        [RelayCommand]
        public void OuvertureAdmin(Models.Admin adminEntry)
        {
            if (adminEntry == null) return;

            try
            {
                switch (adminEntry.Table)
                {
                    case "Utilisateurs":
                        var fenetreUtilisateurs = new AdminUtilisateurs(new AdminUtilisateursViewModel());
                        fenetreUtilisateurs.ShowDialog();
                        break;

                    case "Cours":
                        var fenetreCours = new AdminCours(new AdminCoursViewModel());
                        fenetreCours.ShowDialog();
                        break;

                    default:
                        MessageBox.Show(
                            $"Table « {adminEntry.Table} » non reconnue.",
                            "Erreur de navigation",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning
                        );
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erreur lors de l'ouverture : {ex.Message}",
                    "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }
    }
}