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

    }
}
