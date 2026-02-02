using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cyber_Espace_Entrainement.Models;
using Cyber_Espace_Entrainement.Services;
using Cyber_Espace_Entrainement.ViewModels;
using Cyber_Espace_Entrainement.Views.Accueil;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Cyber_Espace_Entrainement.ViewModels.Activite
{
    public partial class ChoixNiveauViewModel : ObservableObject
    {
        public ObservableCollection<Activites> Activites { get; }

        private readonly ActiviteNavigationService _navigationService;

        public ChoixNiveauViewModel(List<Activites> activites)
        {
            Activites = new ObservableCollection<Activites>(activites);
            _navigationService = new ActiviteNavigationService();
        }

        [RelayCommand]
        private void SelectNiveau(Activites activite)
        {
            _navigationService.OuvrirVueParActivite(activite);

            App.Current.Windows
            .OfType<ChoixNiveauWindow>()
            .FirstOrDefault()
            ?.Close();
        }
    }
}
