using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cyber_Espace_Entrainement.Models;
using Cyber_Espace_Entrainement.Services;
using System;

namespace Cyber_Espace_Entrainement.ViewModels.Profil
{
    public partial class ProfilViewModel : ObservableObject
    {
        // MODE ÉDITION
        [ObservableProperty]
        private bool isEditMode;

        // CHAMPS DU FORMULAIRE
        [ObservableProperty] private string prenom;
        [ObservableProperty] private string nom;
        [ObservableProperty] private string pseudo;
        [ObservableProperty] private string email;
        [ObservableProperty] private string section;
        [ObservableProperty] private DateTime? dateCreation;
        [ObservableProperty] private DateTime? derniereConnection;
        [ObservableProperty] private int? scoreTotal;

        // CONSTRUCTEUR
        public ProfilViewModel()
        {
            ChargerDepuisSession();
        }

        // CHARGEMENT DES DONNÉES
        private void ChargerDepuisSession()
        {
            var s = SessionService.Instance;

            Prenom = s.CurrentPrenom;
            Nom = s.CurrentNom;
            Pseudo = s.CurrentLogin;
            Email = s.CurrentEmail;
            Section = s.CurrentSection;
            DateCreation = s.CurrentDateCrea;
            DerniereConnection = s.CurrentDerniereCo;
            ScoreTotal = s.CurrentScore;

            IsEditMode = false;
        }

        /// <summary>
        /// Préparer l'édition d'un utilisateur
        /// MODIFIÉ : Chargement des nouveaux champs Nom, Prenom, Section et ScoreTotal
        /// </summary>
        [RelayCommand]
        private void EditUser()
        {
            

            IsEditMode = true;
            
        }
    }
}
