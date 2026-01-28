using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cyber_Espace_Entrainement.Models;
using Cyber_Espace_Entrainement.Services;
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
            _ = LoadActivitesAsync();
        }

        
        public async Task LoadActivitesAsync()
        {

            var allActivites = _activiteService.GetAllActivites();

            MessageBox.Show("Nb activités : " + allActivites.Count);

            Activites.Clear();
            foreach (var activite in allActivites)
            {
                Activites.Add(activite);
            }
        }

    }
}
