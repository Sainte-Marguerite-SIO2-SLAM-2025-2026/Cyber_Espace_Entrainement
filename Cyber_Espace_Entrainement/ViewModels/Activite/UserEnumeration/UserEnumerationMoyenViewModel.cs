using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cyber_Espace_Entrainement.Models.UserEnumeration;
using Cyber_Espace_Entrainement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Cyber_Espace_Entrainement.ViewModels.Activite
{
    public partial class UserEnumerationMoyenViewModel : ObservableObject
    {
        // Service dédié à la récupération des données et opérations métier pour l'activité.
        private readonly UserEnumerationService _service;

        [ObservableProperty]
        private string libelle = "User énumération";

        [ObservableProperty]
        private string niveau = "Niveau ";

        [ObservableProperty]
        private int? numNiveau = 1;

        [ObservableProperty]
        private string separateur = " : ";

        [ObservableProperty]
        private string libelleAct = string.Empty;

        [ObservableProperty]
        private bool jeuConnexion = false;

        [ObservableProperty]
        private bool jeuInscription = false;

        [ObservableProperty]
        private bool jeuChangementMdp = false;

        [ObservableProperty]
        private int totalPropositions;

        [ObservableProperty]
        private int score = 0;

        [ObservableProperty]
        private bool formulaireValide = false;

        [ObservableProperty]
        private bool jeuTermine = false;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string motDePasse;

        [ObservableProperty]
        private string confirmationMotDePasse;

        [ObservableProperty]
        private string messageErreur;

        private List<UserEnumeration> _activites;
        private int _indexActuel = 0;

        public UserEnumerationMoyenViewModel()
        {
            _service = new UserEnumerationService();

            _activites = _service.GetRandomOnePerLibelle();

            ChargerActivite();

            TotalPropositions = _service.GetCountRandomOnePerLibelle();
        }

        private void ChargerActivite()
        {
            if (_indexActuel >= _activites.Count)
                return;

            var item = _activites[_indexActuel];

            // reset des champs
            Email = string.Empty;
            MotDePasse = string.Empty;
            ConfirmationMotDePasse = string.Empty;
            MessageErreur = string.Empty;

            JeuConnexion = false;
            JeuInscription = false;
            JeuChangementMdp = false;

            if (item.Libelle == "formulaire de connexion")
            {
                JeuConnexion = true;
                LibelleAct = "Formulaire de connexion";
                NumNiveau = 1;
            }
            else if (item.Libelle == "formulaire de création")
            {
                JeuInscription = true;
                LibelleAct = "Formulaire d'inscription";
                NumNiveau = 2;

            }
            else if (item.Libelle == "formulaire reset mdp")
            {
                JeuChangementMdp = true;
                LibelleAct = "Formulaire de reset mdp";
                NumNiveau = 3;

            }
        }

        private void NiveauSuivant()
        {
            _indexActuel++;

            if (_indexActuel < _activites.Count)
            {
                ChargerActivite();
            }
            else
            {
                JeuTermine = true;
                LibelleAct = "Activité terminée !";
                Separateur = string.Empty;
                NumNiveau = null;
                Niveau = string.Empty;
            }
        }

        [RelayCommand(CanExecute = nameof(PeutRepondre))]
        private void ReponseOui()
        {
            Repondre(true);
        }

        [RelayCommand(CanExecute = nameof(PeutRepondre))]
        private void ReponseNon()
        {
            Repondre(false);
        }

        private void Repondre(bool reponseUtilisateur)
        {
            if (_activites[_indexActuel].Reponse == reponseUtilisateur)
                Score++;

            FormulaireValide = false;
            NiveauSuivant();
        }

        private bool PeutRepondre()
        {
            return FormulaireValide;
        }

        [RelayCommand(CanExecute = nameof(ChampsRempli))]
        private void ValiderConnexion()
        {
            FormulaireValide = true;

            ReponseOuiCommand.NotifyCanExecuteChanged();
            ReponseNonCommand.NotifyCanExecuteChanged();

            MessageErreur = _activites[_indexActuel].Message;
        }

        private bool ChampsRempli()
        {
            if (JeuConnexion)
            {
                return !string.IsNullOrWhiteSpace(Email)
                    && !string.IsNullOrWhiteSpace(MotDePasse);
            }

            if (JeuInscription)
            {
                return !string.IsNullOrWhiteSpace(Email)
                    && !string.IsNullOrWhiteSpace(MotDePasse)
                    && !string.IsNullOrWhiteSpace(ConfirmationMotDePasse);
            }

            if (JeuChangementMdp)
            {
                return !string.IsNullOrWhiteSpace(Email);
            }

            return false;
        }

        partial void OnEmailChanged(string value)
        {
            ValiderConnexionCommand.NotifyCanExecuteChanged();
        }

        partial void OnMotDePasseChanged(string value)
        {
            ValiderConnexionCommand.NotifyCanExecuteChanged();
        }

        partial void OnConfirmationMotDePasseChanged(string value)
        {
            ValiderConnexionCommand.NotifyCanExecuteChanged();
        }


    }
}
