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
            if (!EmailValide(Email))
            {
                MessageErreur = "Adresse email invalide.";
                return;
            }

            if (JeuInscription && !ConfirmationValide(MotDePasse, ConfirmationMotDePasse))
            {
                MessageErreur = "Les mots de passe ne correspondent pas.";
                return;
            }

            if (!MotDePasseValide(MotDePasse) && (JeuConnexion || JeuInscription))
            {
                MessageErreur = "Le mot de passe doit contenir au moins 6 caractères.";
                return;
            }

            FormulaireValide = true;

            ReponseOuiCommand.NotifyCanExecuteChanged();
            ReponseNonCommand.NotifyCanExecuteChanged();

            MessageErreur = _activites[_indexActuel].Message;
        }

        private bool ChampsRempli()
        {
            // Email obligatoire + format valide
            if (!EmailValide(Email))
                return false;

            if (JeuConnexion)
            {
                return MotDePasseValide(MotDePasse);
            }

            if (JeuInscription)
            {
                return MotDePasseValide(MotDePasse)
                    && ConfirmationValide(MotDePasse, ConfirmationMotDePasse);
            }

            if (JeuChangementMdp)
            {
                return true; // seul l'email est requis, déjà validé plus haut
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

        [RelayCommand]
        private void Recommencer()
        {
            // Réinitialisation des variables de progression
            Score = 0;
            _indexActuel = 0;
            JeuTermine = false;

            // Réinitialisation des champs utilisateur
            Email = string.Empty;
            MotDePasse = string.Empty;
            ConfirmationMotDePasse = string.Empty;
            MessageErreur = string.Empty;

            // Réinitialisation des flags d’affichage
            JeuConnexion = false;
            JeuInscription = false;
            JeuChangementMdp = false;
            FormulaireValide = false;

            // Recharger une nouvelle série d’activités
            _activites = _service.GetRandomOnePerLibelle();
            TotalPropositions = _service.GetCountRandomOnePerLibelle();

            // Recharger la première activité
            ChargerActivite();

            // Mise à jour des commandes
            ReponseOuiCommand.NotifyCanExecuteChanged();
            ReponseNonCommand.NotifyCanExecuteChanged();
            ValiderConnexionCommand.NotifyCanExecuteChanged();
        }

        private bool EmailValide(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Regex simple et efficace
            return System.Text.RegularExpressions.Regex.IsMatch(
                email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
            );
        }

        private bool MotDePasseValide(string mdp)
        {
            return !string.IsNullOrWhiteSpace(mdp) && mdp.Length >= 6;
        }


        private bool ConfirmationValide(string mdp, string confirm)
        {
            return mdp == confirm;
        }


    }
}
