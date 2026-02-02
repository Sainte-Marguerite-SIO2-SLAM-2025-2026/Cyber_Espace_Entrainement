using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cyber_Espace_Entrainement.Models;
using Cyber_Espace_Entrainement.Services;
using Cyber_Espace_Entrainement.Views.Accueil;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace Cyber_Espace_Entrainement.ViewModels.Activite
{
    public partial class AccueilTestViewModel : ObservableObject
    {
        private readonly ActiviteService _activiteService;
        private readonly ActiviteNavigationService _navigationService;

        [ObservableProperty]
        private string libelle;

        [ObservableProperty]
        private ObservableCollection<Activites> activites = new();

        /// <summary>
        /// CONSTRUCTEUR qui construit
        /// </summary>
        public AccueilTestViewModel()
        {
            _activiteService = new ActiviteService();
            _navigationService = new ActiviteNavigationService();
            // On initialise la collection
            Activites = new ObservableCollection<Activites>();

            // Chargement immédiat et bloquant
            LoadActivites();
        }

        private void LoadActivites()
        {
            try
            {
                var data = _activiteService.GetAllActivites();

                Activites.Clear();
                foreach (var item in data)
                {
                    Activites.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur de chargement : {ex.Message}");
            }
        }

        [RelayCommand]
        private void SelectActivite(Activites activite)
        {
            var activitesMemeLibelle = _activiteService.GetActiviteByLibelle(activite.Libelle);

            if (activitesMemeLibelle.Count > 1)
            {
                new ChoixNiveauWindow(activitesMemeLibelle).ShowDialog();
            }
            else
            {
                _navigationService.OuvrirVueParActivite(activitesMemeLibelle.First());
            }
        }

    }
}
