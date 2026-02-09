using CommunityToolkit.Mvvm.ComponentModel;
using Cyber_Espace_Entrainement.Models;
using Cyber_Espace_Entrainement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace Cyber_Espace_Entrainement.ViewModels.Cours
{
    public partial class CoursAccueilViewModel : ObservableObject
    {
        private readonly CoursService _coursService;
        private readonly CoursNavigationService _navigationService;

        [ObservableProperty]
        private ObservableCollection<Models.Cours> _cours = new();

        public CoursAccueilViewModel()
        {
            _coursService = new CoursService();
            _navigationService = new CoursNavigationService();

            Cours = new ObservableCollection<Models.Cours>();

            LoadCours();
        }

        private void LoadCours()
        {
            try
            {
                var data = _coursService.GetAllCours();

                Cours.Clear();
                foreach (var item in data)
                {
                    Cours.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur de chargement : {ex.Message}");
            }
        }
    }
}
